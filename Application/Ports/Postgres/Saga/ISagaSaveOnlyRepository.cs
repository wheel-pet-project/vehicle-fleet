namespace Application.Ports.Postgres.Saga;

public interface ISagaSaveOnlyRepository
{
    public Task Add(ISaga saga);
}