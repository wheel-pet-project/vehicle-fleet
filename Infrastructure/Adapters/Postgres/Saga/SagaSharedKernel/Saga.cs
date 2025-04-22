namespace Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

public abstract class Saga
{ 
    internal Saga(int version, ISagaState<IProcessState> sagaState)
    {
        Version = version;
        SagaState = sagaState;
    }
    
    public Guid SagaId { get; private set; } = Guid.NewGuid();
    public int Version { get; private set; }
    public ISagaState<IProcessState> SagaState { get; private set; } = null!;
}