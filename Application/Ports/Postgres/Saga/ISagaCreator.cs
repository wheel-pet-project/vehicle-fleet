using Domain.VehicleAggregate;

namespace Application.Ports.Postgres.Saga;

public interface ISagaCreator
{
    public ISaga CreateSagaVehicleAdding(Vehicle vehicle);
}