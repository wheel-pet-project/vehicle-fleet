using Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

namespace Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;

public record SagaVehicleAddingConsumerEvent(
    Guid SagaId, 
    Guid VehicleId,
    bool IsSuccess,
    SagaService Service) : ISagaEvent;

public class SagaVehicleAddingService : SagaService
{
    public static readonly SagaVehicleAddingService Booking = new(1, nameof(Booking)); 
    public static readonly SagaVehicleAddingService Rent = new(2, nameof(Rent));
    public static readonly SagaVehicleAddingService VehicleDocuments = new(3, nameof(VehicleDocuments));
    
    public SagaVehicleAddingService(int number, string name) : base(number, name)
    {
    }

    public static IEnumerable<SagaVehicleAddingService> All()
    {
        return
        [
            Booking,
            Rent,
            VehicleDocuments
        ];
    }
}