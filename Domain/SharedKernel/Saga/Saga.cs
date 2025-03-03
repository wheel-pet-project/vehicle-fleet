namespace Domain.SharedKernel.Saga;

public abstract class Saga
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}