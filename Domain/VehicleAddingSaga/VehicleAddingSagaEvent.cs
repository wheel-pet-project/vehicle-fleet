using Domain.SharedKernel.Saga;

namespace Domain.VehicleAddingSaga;

public record VehicleAddingSagaEvent(
    Guid SagaId,
    Guid VehicleId,
    bool IsSuccess,
    SagaMicroservice Microservice) : ISagaEvent;