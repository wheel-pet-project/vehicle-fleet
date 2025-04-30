using Domain.SharedKernel.Exceptions.PublicExceptions;
using Proto.VehicleFleetV1;
using DomainStatus = Domain.VehicleAggregate.Status;

namespace Api.Adapters.Grpc.EnumMappers;

public class StatusMapper
{
    public DomainStatus ProtoToDomain(Status protoStatus)
    {
        return protoStatus switch
        {
            Status.Added => DomainStatus.Added,
            Status.ReadiedForRelease => DomainStatus.ReadiedForRelease,
            Status.ReleasedUnspecified => DomainStatus.Released,
            Status.Occupied => DomainStatus.Occupied,
            Status.Serviced => DomainStatus.Serviced,
            Status.Deleted => DomainStatus.Deleted,
            _ => throw new ValueIsUnsupportedException($"{nameof(protoStatus)} is unknown status")
        };
    }

    public Status DomainToProto(DomainStatus domainStatus)
    {
        return domainStatus switch
        {
            _ when domainStatus == DomainStatus.Added => Status.Added,
            _ when domainStatus == DomainStatus.ReadiedForRelease => Status.ReadiedForRelease,
            _ when domainStatus == DomainStatus.Released => Status.ReleasedUnspecified,
            _ when domainStatus == DomainStatus.Occupied => Status.Occupied,
            _ when domainStatus == DomainStatus.Serviced => Status.Serviced,
            _ when domainStatus == DomainStatus.Deleted => Status.Deleted,
            _ => throw new ValueIsUnsupportedException($"{nameof(domainStatus)} is unknown status")
        };
    }
}