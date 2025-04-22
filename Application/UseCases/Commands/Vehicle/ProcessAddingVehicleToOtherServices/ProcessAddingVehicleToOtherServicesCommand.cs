using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ProcessAddingVehicleToOtherServices;

public record ProcessAddingVehicleToOtherServicesCommand(
    Guid VehicleId,
    bool IsCompleted) : IRequest<Result>;