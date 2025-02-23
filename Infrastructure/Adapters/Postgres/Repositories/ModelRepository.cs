using Application.Ports.Postgres;
using Domain.ModelAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Postgres.Repositories;

public sealed class ModelRepository(DataContext context) : IModelRepository
{
    public async Task Add(Model model)
    {
        await context.AddAsync(model);
    }

    public void Update(Model model)
    {
        context.Update(model);
    }

    public async Task<Model?> GetById(Guid id)
    {
        return await context.Models.FirstOrDefaultAsync(x => x.Id == id);
    }
}