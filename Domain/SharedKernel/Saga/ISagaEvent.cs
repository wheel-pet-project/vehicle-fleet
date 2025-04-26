namespace Domain.SharedKernel.Saga;

public interface ISagaEvent
{
    public Guid SagaId { get; }
    public bool IsSuccess { get; }
    public SagaMicroservice Microservice { get; }
}