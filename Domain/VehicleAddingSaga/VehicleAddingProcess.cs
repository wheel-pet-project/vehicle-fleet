using Domain.SharedKernel.Saga;

namespace Domain.VehicleAddingSaga;

public class VehicleAddingProcess(VehicleAddingSagaMicroservice microservice) : IProcessState
{
    public SagaMicroservice Microservice { get; set; } = microservice;
    public bool IsFaulted { get; set; }
    public bool IsCompleted { get; set; }

    public void UpdateProcessState(bool processingResult)
    {
        if (processingResult) IsCompleted = true;
        else IsFaulted = true;
    }
}