using Application.Ports.Postgres.Saga;
using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres.Saga;
using Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;
using JetBrains.Annotations;
using JsonNet.ContractResolvers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;
using Location = Application.UseCases.Commands.Vehicle.AddVehicle.Location;

namespace IntegrationTests.Saga;

[TestSubject(typeof(SagaSaveOnlyRepository))]
public class SagaSaveOnlyRepositoryShould : IntegrationTestBase
{
    private readonly Vehicle _vehicle =
        Vehicle.Create(Guid.NewGuid(),
            PlateNumber.Create("К333ОТ77"), Color.White,
            Vin.Create("SALYA2BN2KA791786"), Domain.SharedKernel.ValueObjects.Location.Create(10.0, 10.0));
    
    [Fact]
    public async Task Add()
    {
        // Arrange
        var sagaCreator = new SagaCreator();
        var saga = sagaCreator.CreateSagaVehicleAdding(_vehicle);
        var repository = new SagaSaveOnlyRepository(Context);
        var uow = new Infrastructure.Adapters.Postgres.UnitOfWork(Context);

        // Act
        await repository.Add(saga);
        await uow.Commit();

        // Assert
        // Context.ChangeTracker.Clear();
        var sagaFromDb =
            await Context.Saga.FirstOrDefaultAsync(x => x.SagaId == saga.SagaId, TestContext.Current.CancellationToken);
        var deserializedSaga = JsonConvert.DeserializeObject<Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding.SagaVehicleAdding>(sagaFromDb!.Content,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All, 
                ContractResolver = new PrivateSetterAndCtorContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
        Assert.Equivalent(saga, deserializedSaga);
    }
}