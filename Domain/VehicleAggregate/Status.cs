using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.Exceptions.DomainRulesViolationException;

namespace Domain.VehicleAggregate;

public sealed class Status : Entity<int>
{
    public static readonly Status Added = new(1, nameof(Added).ToLowerInvariant());
    public static readonly Status ReadiedForRelease = new(2, nameof(ReadiedForRelease).ToLowerInvariant());
    public static readonly Status Released = new(3, nameof(Released).ToLowerInvariant());
    public static readonly Status Occupied = new(4, nameof(Occupied).ToLowerInvariant());
    public static readonly Status Serviced = new(5, nameof(Serviced).ToLowerInvariant());
    public static readonly Status Deleted = new(6, nameof(Deleted).ToLowerInvariant());

    private Status()
    {
    }

    public Status(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    public string Name { get; private set; }

    public bool CanBeChangedToThisStatus(Status potentialStatus)
    {
        if (potentialStatus is null) throw new ValueIsRequiredException($"{nameof(potentialStatus)} cannot be null");
        if (!All().Contains(potentialStatus))
            throw new ValueOutOfRangeException($"{nameof(potentialStatus)} cannot be unsupported");

        return potentialStatus switch
        {
            _ when this == potentialStatus => throw new DomainRulesViolationException(
                "Vehicle already have this status", isAlreadyInThisState: true),
            _ when potentialStatus == Serviced &&
                   (this == Serviced || this == Occupied || this == Deleted) is false => true,
            _ when potentialStatus == ReadiedForRelease && this == Added => true,
            _ when potentialStatus == Deleted && this == Added => true,
            _ when potentialStatus == Released && this == ReadiedForRelease => true,
            _ when potentialStatus == Deleted && this == ReadiedForRelease => true,
            _ when potentialStatus == Occupied && this == Released => true,
            _ when potentialStatus == Released && this == Occupied => true,
            _ when potentialStatus == ReadiedForRelease && this == Serviced => true,
            _ when potentialStatus == Deleted && this == Serviced => true,
            _ => false
        };
    }

    public static IEnumerable<Status> All()
    {
        return
        [
            Added,
            ReadiedForRelease,
            Released,
            Occupied,
            Serviced,
            Deleted
        ];
    }

    public static Status FromName(string name)
    {
        var status = All().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (status == null) throw new ValueOutOfRangeException($"{nameof(name)} unknown status or null");
        return status;
    }

    public static Status FromId(int id)
    {
        var status = All().SingleOrDefault(s => s.Id == id);
        if (status == null) throw new ValueOutOfRangeException($"{nameof(id)} unknown status or null");
        return status;
    }

    public static bool operator ==(Status? a, Status? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Id == b.Id;
    }

    public static bool operator !=(Status a, Status b)
    {
        return !(a == b);
    }
}