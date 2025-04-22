using Application.Ports.Postgres.Saga;
using Domain.SharedKernel.Exceptions.ArgumentException;
using Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;
using Newtonsoft.Json;

namespace Infrastructure.Adapters.Postgres.Saga;

public class SagaSaveOnlyRepository(DataContext context) : ISagaSaveOnlyRepository
{
    public async Task Add(ISaga saga)
    {
        _ = saga switch
        {
            SagaVehicleAdding.SagaVehicleAdding sagaVehicleAdding => await AddVehicleAddingSaga(sagaVehicleAdding),
            _ => throw new ValueOutOfRangeException("Unknown saga type for saving")
        };
    }

    private async Task<bool> AddVehicleAddingSaga(SagaVehicleAdding.SagaVehicleAdding saga)
    {
        var sagaModel = MapToSagaModel(saga);
        
        await context.Saga.AddAsync(sagaModel);
        return true;

        SagaModel MapToSagaModel(SagaVehicleAdding.SagaVehicleAdding s)
        { 
            return new SagaModel
            {
                SagaId = s.SagaId,
                Type = s.GetType().Name,
                Version = s.Version,
                Content = JsonConvert.SerializeObject(s,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }),
                IsCompleted = s.SagaState.IsCompleted,
                IsFaulted = s.SagaState.IsFaulted
            };
        }
    }
}