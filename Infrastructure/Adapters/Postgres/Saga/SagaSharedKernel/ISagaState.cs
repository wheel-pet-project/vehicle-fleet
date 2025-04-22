namespace Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

public interface ISagaState<out TSagaState> 
    where TSagaState : IProcessState 
{
    protected List<IProcessState> States { get; init; }
    
    public IReadOnlyList<TSagaState> ProcessStates => States.OfType<TSagaState>().ToList();
    public abstract bool IsCompleted { get; }
    public abstract bool IsFaulted { get; }

    public void UpdateSagaState(ISagaEvent sagaEvent);
}