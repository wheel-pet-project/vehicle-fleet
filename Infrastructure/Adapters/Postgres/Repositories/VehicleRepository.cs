using Application.Ports.Postgres;
using Domain.VehicleAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Adapters.Postgres.Repositories;

public sealed class VehicleRepository(DataContext context) : IVehicleRepository
{
    public async Task Add(Vehicle vehicle)
    {
        context.Attach(vehicle.Status);
        await context.Vehicles.AddAsync(vehicle);
    }

    public void Update(Vehicle vehicle)
    {
        context.Attach(vehicle.Status);
        context.Vehicles.Update(vehicle);
    }

    public async Task<Vehicle?> GetById(Guid id)
    {
        return await context.Vehicles
            .Include(x => x.Status)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Vehicle>> GetAll(int? page = null, int? pageSize = null)
    {
        var queryable = context.Vehicles
            .Include(x => x.Status)
            .AsQueryable()
            .AsNoTracking();

        if (page == null || pageSize == null) return await queryable.ToListAsync();

        return await queryable
            .Skip((page.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();
    }
}