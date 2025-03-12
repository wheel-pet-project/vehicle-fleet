using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.DeleteVehicle;

public record DeleteVehicleCommand(Guid VehicleId) : IRequest<Result>;