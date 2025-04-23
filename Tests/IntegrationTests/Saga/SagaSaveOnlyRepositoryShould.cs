using Domain.SharedKernel.ValueObjects;
using Domain.VehicleAddingSaga;
using Domain.VehicleAggregate;
using Infrastructure.Adapters.Postgres.Saga;
using JetBrains.Annotations;
using JsonNet.ContractResolvers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests.Saga;

[TestSubject(typeof(VehicleAddingSagaSaveOnlyRepository))]
public class SagaSaveOnlyRepositoryShould : IntegrationTestBase
{
    private readonly VehicleAddingSaga _saga;

    public SagaSaveOnlyRepositoryShould()
    {
        var (saga, _) = Vehicle.Create(Guid.NewGuid(),
            PlateNumber.Create("К333ОТ77"), Color.White,
            Vin.Create("SALYA2BN2KA791786"), Domain.SharedKernel.ValueObjects.Location.Create(10.0, 10.0));

        _saga = saga;
    }
    
    [Fact]
    public async Task Add()
    {
        // Arrange
        
        var repository = new VehicleAddingSagaSaveOnlyRepository(Context);
        var uow = new Infrastructure.Adapters.Postgres.UnitOfWork(Context);

        // Act
        await repository.Add(_saga);
        await uow.Commit();

        // Assert
        // Context.ChangeTracker.Clear();
        var sagaFromDb =
            await Context.Saga.FirstOrDefaultAsync(x => x.SagaId == _saga.SagaId, TestContext.Current.CancellationToken);
        var deserializedSaga = JsonConvert.DeserializeObject<VehicleAddingSaga>(sagaFromDb!.Content,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All, 
                ContractResolver = new PrivateSetterAndCtorContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
        Assert.Equivalent(_saga, deserializedSaga);
    }
}