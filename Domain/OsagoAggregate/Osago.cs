namespace Domain.OsagoAggregate;

/// <summary>
/// ОСАГО 
/// </summary>
public class Osago
{
    public Guid Id { get; private set; }
    public Guid VehicleId { get; private set; }
    public string PhotoStorageBucketAndKey { get; private set; }
    public DateOnly DateOfIssue { get; private set; }
    public DateOnly DateOfExpiry  { get; private set; }
    public bool IsOutdated { get; private set; }
}