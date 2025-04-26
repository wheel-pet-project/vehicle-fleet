using Domain.VehicleAddingSaga;

namespace Application.Ports.Postgres;

public interface IVehicleAddingSagaRepository
{
    Task<VehicleAddingSaga?> GetById(Guid id);
    
    Task Add(VehicleAddingSaga saga);

    void Update(VehicleAddingSaga saga);
}