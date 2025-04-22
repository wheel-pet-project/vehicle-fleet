namespace Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

public interface ISagaEvent
{
    public Guid SagaId { get; } 
    public bool IsSuccess { get; }
    public SagaService Service { get; }
}