namespace Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

public abstract class SagaService(int number, string name)
{
    public int Number { get; } = number;
    public string Name { get; } = name;

    
    public static bool operator ==(SagaService a, SagaService b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Number == b.Number;
    }

    public static bool operator !=(SagaService a, SagaService b)
    {
        return !(a == b);
    }
    
    protected bool Equals(SagaService other)
    {
        return Number == other.Number && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SagaService)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Number, Name);
    }
}