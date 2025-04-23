using Application.UseCases.Queries.Vehicle.GetVehicleById;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.ValueObjects;
using JetBrains.Annotations;
using Xunit;

namespace IntegrationTests.Queries.Vehicle;

[TestSubject(typeof(GetVehicleByIdQueryHandler))]
public class GetVehicleByIdQueryHandlerShould : IntegrationTestBase
{
    [Fact]
    public async Task ReturnModelById()
    {
        // Arrange
        var (expectedModel, expectedVehicle) = await AddVehicle();
        var queryHandler = new GetVehicleByIdQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetVehicleByIdQuery(expectedVehicle.Id),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(expectedVehicle.Id, actual.Value.Id);
        Assert.Equal(expectedModel.Brand.Name, actual.Value.Brand);
        Assert.Equal(expectedModel.CarModel.Name, actual.Value.CarModel);
        Assert.Equal((double)expectedModel.Tariff.PricePerMinute, actual.Value.PricePerMinute);
        Assert.Equal((double)expectedModel.Tariff.PricePerHour, actual.Value.PricePerHour);
        Assert.Equal(expectedVehicle.Color, actual.Value.Color);
        Assert.Equal(expectedVehicle.FuelLevel.Percents, actual.Value.FuelLevelPercents);
        Assert.Equal(expectedVehicle.Location.Latitude, actual.Value.Latitude);
        Assert.Equal(expectedVehicle.Location.Longitude, actual.Value.Longitude);
        Assert.Equal(expectedVehicle.PlateNumber.Value, actual.Value.PlateNumber);
        Assert.Equal(expectedVehicle.Status, actual.Value.Status);
    }

    [Fact]
    public async Task ReturnFailIfNotFound()
    {
        // Arrange
        var queryHandler = new GetVehicleByIdQueryHandler(DataSource);

        // Act
        var actual = await queryHandler.Handle(new GetVehicleByIdQuery(Guid.NewGuid()),
            TestContext.Current.CancellationToken);

        // Assert
        Assert.True(actual.IsFailed);
        Assert.IsType<NotFound>(actual.Errors[0]);
    }

    private async Task<(Domain.ModelAggregate.Model, Domain.VehicleAggregate.Vehicle)> AddVehicle()
    {
        var model = Domain.ModelAggregate.Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
            Category.Create('B'), Tariff.Create(1, 2, 3));

        await Context.Models.AddAsync(model);
        await Context.SaveChangesAsync();

        var (_, vehicle) = Domain.VehicleAggregate.Vehicle.Create(model.Id,
            PlateNumber.Create("К333ОТ77"), Color.Red,
            Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0), FuelLevel.Create());

        Context.Attach(vehicle.Status);
        await Context.Vehicles.AddAsync(vehicle);
        await Context.SaveChangesAsync();

        return (model, vehicle);
    }
}