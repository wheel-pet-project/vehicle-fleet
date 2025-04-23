using Domain.VehicleAddingSaga;

namespace Application.Ports.Postgres.Saga;

public interface IVehicleAddingSagaSaveOnlyRepository
{
    public Task Add(VehicleAddingSaga saga);
}