namespace Domain.SharedKernel.Saga;

public abstract class SagaMicroservice(int number, string name)
{
    public int Number { get; } = number;
    public string Name { get; } = name;


    public static bool operator ==(SagaMicroservice a, SagaMicroservice b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Number == b.Number;
    }

    public static bool operator !=(SagaMicroservice a, SagaMicroservice b)
    {
        return !(a == b);
    }

    protected bool Equals(SagaMicroservice other)
    {
        return Number == other.Number && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SagaMicroservice)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Number, Name);
    }
}