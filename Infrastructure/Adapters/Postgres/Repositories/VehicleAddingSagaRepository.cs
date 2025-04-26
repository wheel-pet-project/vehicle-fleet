using Application.Ports.Postgres;
using Domain.VehicleAddingSaga;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Postgres.Saga;

public class VehicleAddingSagaRepository(DataContext context) : IVehicleAddingSagaRepository
{
    public async Task<VehicleAddingSaga?> GetById(Guid id)
    {
        return await context.VehicleAddingSaga.FirstOrDefaultAsync(x => x.SagaId == id);
    }

    public async Task Add(VehicleAddingSaga saga)
    {
        await context.VehicleAddingSaga.AddAsync(saga);
    }

    public void Update(VehicleAddingSaga saga)
    {
        context.VehicleAddingSaga.Update(saga);
    }
}