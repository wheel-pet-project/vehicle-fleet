namespace Domain.SharedKernel.Saga;

public interface IProcessState
{
    public bool IsFaulted { get; private protected set; }
    public bool IsCompleted { get; private protected set; }
    public SagaMicroservice Microservice { get; private protected set; }

    void UpdateProcessState(bool processingResult);
}