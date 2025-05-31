namespace Domain.SharedKernel.Saga;

public interface ISagaState<out TSagaState>
    where TSagaState : IProcessState
{
    protected List<IProcessState> States { get; init; }

    public IReadOnlyList<TSagaState> ProcessStates => States.OfType<TSagaState>().ToList();
    public bool IsCompleted { get; }
    public bool IsFaulted { get; }

    public void UpdateSagaState(ISagaEvent sagaEvent);
}