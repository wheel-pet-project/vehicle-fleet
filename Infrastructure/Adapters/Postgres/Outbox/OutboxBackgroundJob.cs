using Dapper;
using Domain.SharedKernel;
using JsonNet.ContractResolvers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using Quartz;

namespace Infrastructure.Adapters.Postgres.Outbox;

[DisallowConcurrentExecution]
public class OutboxBackgroundJob(
    NpgsqlDataSource dataSource,
    IMediator mediator,
    ILogger<OutboxBackgroundJob> logger) : IJob
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ContractResolver = new PrivateSetterContractResolver()
    };

    public async Task Execute(IJobExecutionContext jobExecutionContext)
    {
        await using var connection = await dataSource.OpenConnectionAsync(jobExecutionContext.CancellationToken);
        var outboxEnumerable = await connection.QueryAsync<OutboxEvent>(QuerySql);
        var outboxEvents = outboxEnumerable.AsList();

        if (outboxEvents.Count > 0)
        {
            var domainEvents = outboxEvents.Select(x => JsonConvert
                    .DeserializeObject<DomainEvent>(x.Content, _jsonSerializerSettings))
                .OfType<DomainEvent>()
                .ToList()
                .AsReadOnly();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                foreach (var domainEvent in domainEvents)
                {
                    await mediator.Publish(domainEvent, jobExecutionContext.CancellationToken);
                    var commandForUpdate = new CommandDefinition(UpdateSql,
                        new { ProcessedOnUtc = DateTime.UtcNow, domainEvent.EventId }, transaction);

                    await connection.ExecuteAsync(commandForUpdate);
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                logger.LogError("Failed of processing outbox events and save updates, exception: {e}", e);
                await transaction.RollbackAsync();
            }
        }
    }

    private const string QuerySql =
        """
        SELECT event_id AS EventId,
               type AS Type,
               content AS Content,
               occurred_on_utc AS OccurredOnUtc,
               processed_on_utc AS ProcessedOnUtc
        FROM outbox
        WHERE processed_on_utc IS NULL
        ORDER BY occurred_on_utc
        LIMIT 30
        FOR UPDATE SKIP LOCKED 
        """;

    private const string UpdateSql =
        """
        UPDATE outbox
        SET processed_on_utc = @ProcessedOnUtc
        WHERE event_id = @EventId
        """;
}