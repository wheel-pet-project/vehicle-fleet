using Domain.VehicleAggregate;

namespace Application.Ports.Postgres;

public interface IVehicleRepository
{
    Task Add(Vehicle vehicle);

    void Update(Vehicle vehicle);

    Task<Vehicle?> GetById(Guid id);

    Task<IReadOnlyList<Vehicle>> GetAll(int? page = null, int? pageSize = null);
}