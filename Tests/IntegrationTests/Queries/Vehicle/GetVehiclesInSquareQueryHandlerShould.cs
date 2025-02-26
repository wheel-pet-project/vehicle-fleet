using Application.UseCases.Queries.Vehicle.GetVehiclesInSquare;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using JetBrains.Annotations;
using Xunit;

namespace IntegrationTests.Queries.Vehicle;

[TestSubject(typeof(GetVehiclesInSquareQueryHandler))]
public class GetVehiclesInSquareQueryHandlerShould : IntegrationTestBase
{
    [Fact]
    public async Task ReturnVehicleWithCorrectValues()
    {
        // Arrange
        var (expectedModel, expectedVehicles) = await AddModelAndVehicles(1);
        var queryHandler = new GetVehiclesInSquareQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(
            new GetVehiclesInSquareQuery(Status.Added,
                new LocationDto(expectedVehicles[0].Location.Latitude + 1, expectedVehicles[0].Location.Longitude + 1),
                new LocationDto(expectedVehicles[0].Location.Latitude - 1, expectedVehicles[0].Location.Longitude - 1)),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedVehicles[0].Id, actual.Value.Vehicles[0].Id);
        Assert.Equal(expectedVehicles[0].Color, actual.Value.Vehicles[0].Color);
        Assert.Equal(expectedVehicles[0].Location.Longitude, actual.Value.Vehicles[0].Longitude);
        Assert.Equal(expectedVehicles[0].Location.Latitude, actual.Value.Vehicles[0].Latitude);
        Assert.Equal(expectedModel.Brand.Name, actual.Value.Vehicles[0].Brand);
        Assert.Equal(expectedModel.CarModel.Name, actual.Value.Vehicles[0].CarModel);
    }

    [Fact]
    public async Task ReturnVehiclesInSquare()
    {
        // TODO: решить проблему
        // Arrange
        var (expectedModel, expectedVehicles) = await AddModelAndVehicles(1);
        var queryHandler = new GetVehiclesInSquareQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(
            new GetVehiclesInSquareQuery(Status.Added,
                new LocationDto(expectedVehicles[0].Location.Latitude - 1, expectedVehicles[0].Location.Longitude + 1),
                new LocationDto(expectedVehicles[0].Location.Latitude + 1, expectedVehicles[0].Location.Longitude - 1)),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEmpty(actual.Value.Vehicles);
        Assert.Equal(expectedVehicles.Count, actual.Value.Vehicles.Count);
    }

    [Fact]
    public async Task ReturnEmptyListWhenNoVehicles()
    {
        var queryHandler = new GetVehiclesInSquareQueryHandler(DataSource);

        // Act
        var actual =
            await queryHandler.Handle(
                new GetVehiclesInSquareQuery(Status.Added, new LocationDto(1, 1), new LocationDto(2, 2)),
                TestContext.Current.CancellationToken);

        // Assert
        Assert.Empty(actual.Value.Vehicles);
    }
    
    private async Task<(Domain.ModelAggregate.Model, List<Domain.VehicleAggregate.Vehicle>)> AddModelAndVehicles(
        int vehiclesCount)
    {
        var model = Domain.ModelAggregate.Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
            Category.Create('B'), Tariff.Create(1, 2, 3));
        
        await Context.Models.AddAsync(model);
        await Context.SaveChangesAsync();
        
        var vehicles = Enumerable.Range(0, vehiclesCount)
            .Select(_ => Domain.VehicleAggregate.Vehicle.Create(model.Id, PlateNumber.Create("К333ОТ77"), Color.Red,
                Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0), FuelLevel.Create()))
            .ToList();
        
        Context.AttachRange(vehicles.Select(x => x.Status).ToList());
        await Context.Vehicles.AddRangeAsync(vehicles);
        await Context.SaveChangesAsync();
        
        return (model, vehicles);
    }
}