using Application.Ports.Postgres.Saga;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.VehicleAddingSaga;
using Newtonsoft.Json;

namespace Infrastructure.Adapters.Postgres.Saga;

public class VehicleAddingSagaSaveOnlyRepository(DataContext context) : IVehicleAddingSagaSaveOnlyRepository
{
    public async Task Add(VehicleAddingSaga saga)
    {
        var sagaModel = MapToSagaModel(saga);
        
        await context.Saga.AddAsync(sagaModel);
    }

    private SagaModel MapToSagaModel(VehicleAddingSaga saga)
    {
        return new SagaModel
        {
            SagaId = saga.SagaId,
            Type = saga.GetType().Name,
            Version = saga.Version,
            Content = JsonConvert.SerializeObject(saga,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }),
            IsCompleted = saga.SagaState.IsCompleted,
            IsFaulted = saga.SagaState.IsFaulted
        };
    }
}