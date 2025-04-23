using Domain.SharedKernel.Exceptions.ArgumentException;
using Domain.SharedKernel.Saga;

namespace Domain.VehicleAddingSaga;

public class VehicleAddingState : ISagaState<VehicleAddingProcess>
{
    internal VehicleAddingState()
    {
        States = [..VehicleAddingSagaMicroservice.All().Select(x => new VehicleAddingProcess(x)).ToList()];
    }
    
    public List<IProcessState> States { get; init; }
    public bool IsCompleted => States.All(x => x.IsCompleted);
    public bool IsFaulted => States.Any(x => x.IsFaulted);
    
    public void UpdateSagaState(ISagaEvent sagaEvent)
    {
        if (States.All(x => x.Microservice != sagaEvent.Service))
            throw new ValueOutOfRangeException("Service is unknown for saga");
        
        States
            .First(x => x.Microservice == sagaEvent.Service)
            .UpdateProcessState(sagaEvent.IsSuccess);
    }
}