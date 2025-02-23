using Domain.ModelAggregate;

namespace Application.Ports.Postgres;

public interface IModelRepository
{
    Task Add(Model model);

    void Update(Model model);

    Task<Model?> GetById(Guid id);
}