namespace Domain.SharedKernel.Saga;

public abstract class Saga
{
    internal Saga(int version, ISagaState<IProcessState> state)
    {
        Version = version;
        State = state;
    }

    public Guid SagaId { get; private set; } = Guid.NewGuid();
    public int Version { get; private set; }
    public ISagaState<IProcessState> State { get; private set; }
}