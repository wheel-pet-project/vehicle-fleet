using Domain.SharedKernel.Saga;

namespace Domain.VehicleAddingSaga;

public class VehicleAddingSagaMicroservice : SagaMicroservice
{
    public static readonly VehicleAddingSagaMicroservice Booking = new(1, nameof(Booking)); 
    public static readonly VehicleAddingSagaMicroservice Rent = new(2, nameof(Rent));
    public static readonly VehicleAddingSagaMicroservice VehicleDocuments = new(3, nameof(VehicleDocuments));
    
    private VehicleAddingSagaMicroservice(int number, string name) : base(number, name)
    {
    }

    public static IEnumerable<VehicleAddingSagaMicroservice> All()
    {
        return
        [
            Booking,
            Rent,
            VehicleDocuments
        ];
    }
}