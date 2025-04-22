namespace Application.Ports.Postgres.Saga;

public interface ISaga
{
    public Guid SagaId { get; }
}