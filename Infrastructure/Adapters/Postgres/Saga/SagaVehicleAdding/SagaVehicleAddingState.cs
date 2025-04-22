using Domain.SharedKernel.Exceptions.ArgumentException;
using Infrastructure.Adapters.Postgres.Saga.SagaSharedKernel;

namespace Infrastructure.Adapters.Postgres.Saga.SagaVehicleAdding;

public class SagaVehicleAddingState : ISagaState<ProcessState>
{
    internal SagaVehicleAddingState()
    {
        States = [..SagaVehicleAddingService.All().Select(x => new ProcessState(x)).ToList()];
    }
    
    public List<IProcessState> States { get; init; }
    public bool IsCompleted => States.All(x => x.IsCompleted);
    public bool IsFaulted => States.Any(x => x.IsFaulted);
    
    public void UpdateSagaState(ISagaEvent sagaEvent)
    {
        if (States.All(x => x.Service != sagaEvent.Service))
            throw new ValueOutOfRangeException("Service is unknown for saga");
        
        States
            .First(x => x.Service == sagaEvent.Service)
            .UpdateProcessState(sagaEvent.IsSuccess);
    }
}

public class ProcessState(SagaVehicleAddingService service) : IProcessState
{
    public SagaService Service { get; set; } = service;
    public bool IsFaulted { get; set; }
    public bool IsCompleted { get; set; }

    public void UpdateProcessState(bool processingResult)
    {
        if (processingResult)
        {
            IsCompleted = true;
            return;
        }

        IsFaulted = true;
    }
}