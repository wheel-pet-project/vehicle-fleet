using Domain.ModelAggregate;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres;
using Infrastructure.Adapters.Postgres.Repositories;
using JetBrains.Annotations;
using Xunit;

namespace IntegrationTests.Repositories;

[TestSubject(typeof(VehicleRepository))]
public class VehicleRepositoryShould : IntegrationTestBase
{
    private readonly Guid _sagaId = Guid.NewGuid();
    private readonly Model _model = Model.Create(Brand.Create("Kia"), CarModel.Create("Rio"),
        Category.Create(Category.BCategory), Tariff.Create(10.0M, 300.0M, 4000.0M));
    private readonly PlateNumber _plateNumber = PlateNumber.Create("К333ОТ77");
    private readonly Color _color = Color.Red;
    private readonly Vin _vin = Vin.Create("SALYA2BN2KA791786");
    private readonly FuelLevel _fuelLevel = FuelLevel.Create();
    private readonly Location _location = Location.Create(10.0, 10.0);

    [Fact]
    public async Task Add()
    {
        // Arrange
        await AddModel(_model);
        var (_, vehicle) = Vehicle.Create(_model.Id, _plateNumber, _color, _vin, _location, _fuelLevel);
        var repositoryAnUowBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repository, uow) = repositoryAnUowBuilder.Build(Context);

        // Act
        await repository.Add(vehicle);
        await uow.Commit();

        // Assert
        vehicle.ClearDomainEvents();
        var vehicleFromDb = await repository.GetById(vehicle.Id);
        Assert.Equivalent(vehicle, vehicleFromDb);
    }

    [Fact]
    public async Task Update()
    {
        // Arrange
        await AddModel(_model);
        var (_, vehicle) = Vehicle.Create(_model.Id, _plateNumber, _color, _vin, _location, _fuelLevel);
        var repositoryAnUowBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repositoryForArrange, uowForArrange) = repositoryAnUowBuilder.Build(Context);

        await repositoryForArrange.Add(vehicle);
        await uowForArrange.Commit();

        var (repository, uow) = repositoryAnUowBuilder.Build(Context);
        vehicle.MarkAsAdded();

        // Act
        repository.Update(vehicle);
        await uow.Commit();

        // Assert
        vehicle.ClearDomainEvents();
        var vehicleFromDb = await repository.GetById(vehicle.Id);
        Assert.Equivalent(vehicle, vehicleFromDb);
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        await AddModel(_model);
        var (_, vehicle) = Vehicle.Create(_model.Id, _plateNumber, _color, _vin, _location, _fuelLevel);
        var repositoryAnUowBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repository, uow) = repositoryAnUowBuilder.Build(Context);
        await repository.Add(vehicle);
        await uow.Commit();

        // Act
        var actual = await repository.GetById(vehicle.Id);

        // Assert
        vehicle.ClearDomainEvents();
        Assert.Equivalent(vehicle, actual);
    }

    [Fact]
    public async Task GetAll()
    {
        // Arrange
        await AddModel(_model);
        var (_, vehicle) = Vehicle.Create(_model.Id, _plateNumber, _color, _vin, _location, _fuelLevel);
        var repositoryAnUowBuilder = new RepositoryAndUnitOfWorkBuilder();
        var (repository, uow) = repositoryAnUowBuilder.Build(Context);
        await repository.Add(vehicle);
        await uow.Commit();

        // Act
        var vehicles = await repository.GetAll();

        // Assert
        vehicle.ClearDomainEvents();
        Assert.NotEmpty(vehicles);
        Assert.Equivalent(vehicle, vehicles[0]);
    }

    private async Task AddModel(Model model)
    {
        await Context.Models.AddAsync(model);
        await Context.SaveChangesAsync();
    }

    private class RepositoryAndUnitOfWorkBuilder
    {
        public (VehicleRepository, Infrastructure.Adapters.Postgres.UnitOfWork) Build(
            DataContext context)
        {
            return (new VehicleRepository(context),
                new Infrastructure.Adapters.Postgres.UnitOfWork(context));
        }
    }
}