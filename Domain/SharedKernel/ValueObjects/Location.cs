using CSharpFunctionalExtensions;
using Domain.SharedKernel.Exceptions.ArgumentException;

namespace Domain.SharedKernel.ValueObjects;

public class Location : ValueObject
{
    private Location()
    {
    }

    private Location(double latitude, double longitude) : this()
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; }
    public double Longitude { get; }

    public bool InSquare(double sizeInDegree, Location center)
    {
        if (sizeInDegree <= 0)
            throw new ValueOutOfRangeException($"{nameof(sizeInDegree)} must be greater than 0");

        return Latitude <= center.Latitude + sizeInDegree
               && Latitude >= center.Latitude - sizeInDegree
               && Longitude <= center.Longitude + sizeInDegree
               && Longitude >= center.Longitude - sizeInDegree;
    }

    public bool InSquare(Location upperLeftLocation, Location lowerRightLocation)
    {
        return Latitude <= upperLeftLocation.Latitude && Latitude >= lowerRightLocation.Latitude &&
               Longitude <= lowerRightLocation.Longitude &&
               Longitude >= upperLeftLocation.Longitude;
    }

    public static Location Create(double latitude, double longitude)
    {
        if (latitude is < 0 or > 90)
            throw new ValueOutOfRangeException($"{nameof(latitude)} must be between 0 and 90");
        if (longitude is < 0 or > 180)
            throw new ValueOutOfRangeException($"{nameof(longitude)} must be between 0 and 180");

        return new Location(latitude, longitude);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Math.Round(Latitude, 5);
        yield return Math.Round(Longitude, 5);
    }
}