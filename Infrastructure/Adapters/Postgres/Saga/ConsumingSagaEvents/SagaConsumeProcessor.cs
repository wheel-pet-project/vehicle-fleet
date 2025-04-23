using Dapper;
using Domain.SharedKernel.Exceptions.DataConsistencyViolationException;
using Domain.VehicleAddingSaga;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using Npgsql;

namespace Infrastructure.Adapters.Postgres.Saga.ConsumingSagaEvents;

public class SagaConsumeProcessor(NpgsqlDataSource dataSource) : ISagaConsumeProcessor
{
    private readonly JsonSerializerSettings _jsonSettings = new()
        { TypeNameHandling = TypeNameHandling.All, ContractResolver = new PrivateSetterContractResolver() };
    
    public async Task<bool> SagaVehicleAddingProcess(VehicleAddingSagaEvent @event)
    {
        await using var connection = new NpgsqlConnection(dataSource.ConnectionString);
        return await Process(async () =>
        {
            var saga = await GetSagaById(@event.SagaId);
            if (saga == null) throw new DataConsistencyViolationException(
                $"Saga with id: {@event.SagaId} in inconsistent state");

            saga.ProcessSagaEvent(@event);

            await Update(saga);
        });
        
        async Task<VehicleAddingSaga?> GetSagaById(Guid sagaId) 
        {
            var saga = await connection.QueryFirstOrDefaultAsync<SagaDapperModel>(SqlSelect, new { SagaId = sagaId });
            if (saga == null) throw new DataConsistencyViolationException(
                $"Saga with id {sagaId} not found for update");

            var deserializedSaga = JsonConvert.DeserializeObject<VehicleAddingSaga>(saga.Content, _jsonSettings);
            return deserializedSaga;
        }
        
        async Task Update(VehicleAddingSaga vehicleAddingSaga) 
        {
            await connection.ExecuteAsync(SqlUpdate, new
            {    
                SagaId = vehicleAddingSaga.SagaId,
                Content = JsonConvert.SerializeObject(vehicleAddingSaga, _jsonSettings),
                IsCompleted = vehicleAddingSaga.SagaState.IsCompleted,
                IsFaulted = vehicleAddingSaga.SagaState.IsFaulted
            });
        }
    }
    
    private async Task<bool> Process(Func<Task> concreteSagaProcess)
    {
        try
        {
            await concreteSagaProcess();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
    
    private class SagaDapperModel
    {
        public required Guid SagaId { get; init; }
        public required int Version { get; init; }
        public required string Content { get; init; }
        public required bool IsCompleted { get; init; }
        public required bool IsFaulted { get; init; }
    }
    
    private const string SqlSelect =
        """
        SELECT saga_id,
               content,
               is_completed,
               is_faulted
        FROM saga
        WHERE saga_id = @SagaId
        """;
    
    private const string SqlUpdate =
        """
        UPDATE saga
        SET content = @Content,
            is_completed = @IsCompleted,
            is_faulted = @IsFaulted
        WHERE saga_id = @SagaId
        """;
}