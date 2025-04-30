using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAddingSaga;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres.Repositories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.Saga;

[TestSubject(typeof(VehicleAddingSagaRepository))]
public class SagaRepositoryShould : IntegrationTestBase
{
    private readonly VehicleAddingSaga _saga;

    public SagaRepositoryShould()
    {
        var (saga, _) = Vehicle.Create(Guid.NewGuid(),
            PlateNumber.Create("К333ОТ77"), Color.White,
            Vin.Create("SALYA2BN2KA791786"), Location.Create(10.0, 10.0));

        _saga = saga;
    }

    [Fact]
    public async Task Add()
    {
        // Arrange
        var repository = new VehicleAddingSagaRepository(Context);
        var uow = new Infrastructure.Adapters.Postgres.UnitOfWork(Context);

        // Act
        await repository.Add(_saga);
        await uow.Commit();

        // Assert
        Context.ChangeTracker.Clear();
        var sagaFromDb = await Context.VehicleAddingSaga.FirstOrDefaultAsync(
            x => x.SagaId == _saga.SagaId, TestContext.Current.CancellationToken);
        Assert.Equivalent(_saga, sagaFromDb);
    }

    [Fact]
    public async Task Update()
    {
        // Arrange
        await AddSagaToDb();
        var repository = new VehicleAddingSagaRepository(Context);
        var uow = new Infrastructure.Adapters.Postgres.UnitOfWork(Context);

        ChangeSagaToFaultedState();

        // Act
        repository.Update(_saga);
        await uow.Commit();

        // Assert
        Context.ChangeTracker.Clear();
        var sagaFromDb = await Context.VehicleAddingSaga.FirstOrDefaultAsync(
            x => x.SagaId == _saga.SagaId, TestContext.Current.CancellationToken);
        Assert.Equivalent(_saga, sagaFromDb);
    }

    [Fact]
    public async Task GetById()
    {
        // Arrange
        await AddSagaToDb();
        var repository = new VehicleAddingSagaRepository(Context);

        // Act
        var actual = await repository.GetById(_saga.SagaId);

        // Assert
        Context.ChangeTracker.Clear();
        var sagaFromDb = await Context.VehicleAddingSaga.FirstOrDefaultAsync(
            x => x.SagaId == _saga.SagaId, TestContext.Current.CancellationToken);
        Assert.Equivalent(_saga, sagaFromDb);
    }

    private void ChangeSagaToFaultedState()
    {
        var sagaEvent =
            new VehicleAddingSagaEvent(_saga.SagaId, _saga.VehicleId, false, VehicleAddingSagaMicroservice.Booking);

        // Act
        _saga.State.UpdateSagaState(sagaEvent);
    }

    private async Task AddSagaToDb()
    {
        var repository = new VehicleAddingSagaRepository(Context);
        var uow = new Infrastructure.Adapters.Postgres.UnitOfWork(Context);
        await repository.Add(_saga);
        await uow.Commit();
    }
}