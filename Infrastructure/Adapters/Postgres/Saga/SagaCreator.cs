using Application.Ports.Postgres.Saga;
using Domain.VehicleAggregate;

namespace Infrastructure.Adapters.Postgres.Saga;

public class SagaCreator : ISagaCreator
{
    public ISaga CreateSagaVehicleAdding(Vehicle vehicle)
    {
        return new SagaVehicleAdding.SagaVehicleAdding(vehicle.Id);
    }
}