namespace Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

public interface IProcessState
{
    public bool IsFaulted { get; private protected set; }
    public bool IsCompleted { get; private protected set; }
    public SagaService Service { get; private protected set; }

    void UpdateProcessState(bool processingResult);
}