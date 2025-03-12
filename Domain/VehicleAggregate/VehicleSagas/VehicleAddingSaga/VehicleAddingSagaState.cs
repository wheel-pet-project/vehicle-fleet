using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.VehicleAggregate.VehicleSagas.VehicleAddingSaga;

public sealed class VehicleAddingSagaState
{
    public static readonly VehicleAddingSagaState Added = new(1, nameof(Added).ToLowerInvariant());

    public static readonly VehicleAddingSagaState WaitingOtherServices =
        new(2, nameof(WaitingOtherServices).ToLowerInvariant());

    public static readonly VehicleAddingSagaState ReadiedForRelease =
        new(3, nameof(ReadiedForRelease).ToLowerInvariant());

    private VehicleAddingSagaState()
    {
    }

    private VehicleAddingSagaState(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public bool AddedInDocuments { get; private set; }
    public bool AddedInBooking { get; private set; }
    public bool AddedInRental { get; private set; }

    public static IEnumerable<VehicleAddingSagaState> All()
    {
        return
        [
            Added,
            WaitingOtherServices,
            ReadiedForRelease
        ];
    }

    public static VehicleAddingSagaState FromName(string name)
    {
        var state = All().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (state == null) throw new ValueOutOfRangeException($"{nameof(name)} unknown state or null");
        return state;
    }

    public static VehicleAddingSagaState FromId(int id)
    {
        var state = All().SingleOrDefault(s => s.Id == id);
        if (state == null) throw new ValueOutOfRangeException($"{nameof(id)} unknown state or null");
        return state;
    }

    public static bool operator ==(VehicleAddingSagaState? a, VehicleAddingSagaState? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Id == b.Id;
    }

    public static bool operator !=(VehicleAddingSagaState a, VehicleAddingSagaState b)
    {
        return !(a == b);
    }
}