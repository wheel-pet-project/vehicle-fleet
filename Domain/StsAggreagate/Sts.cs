using Domain.SharedKernel.ValueObjects;

namespace Domain.StsAggreagate;

/// <summary>
/// Свидетельство о регистрации ТС 
/// </summary>
public class Sts
{
    public Guid Id { get; private set; }
    public string FrontPhotoStorageBucketAndKey { get; private set; }
    public string BackPhotoStorageBucketAndKey { get; private set; }
}