namespace Domain.PtsAggregate;

/// <summary>
/// Паспорт ТС 
/// </summary>
public class Pts
{
    private Pts() { }
    
    // private Pts() { }
    
    public Guid VehicleId { get; private set; }
    public string FrontPhotoStorageBucketAndKey { get; private set; }
    public string BackPhotoStorageBucketAndKey { get; private set; }
    public DateOnly YearOfManufacture { get; private set; }
    public string Color { get; private set; }
    public long Vin { get; private set; }
}