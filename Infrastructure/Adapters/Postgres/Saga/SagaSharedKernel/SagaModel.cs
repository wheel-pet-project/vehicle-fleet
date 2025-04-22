namespace Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

public class SagaModel
{
    public required Guid SagaId { get; init; }
    public required string Type { get; init; }
    public required int Version { get; init; }
    public required string Content { get; init; }
    public required bool IsCompleted { get; init; }
    public required bool IsFaulted { get; init; }
}