using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.ReleaseVehicle;

public record ReleaseVehicleCommand(Guid VehicleId) : IRequest<Result>;