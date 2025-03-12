using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.MarkAsReadiedForReleaseVehicle;

public record MarkAsReadiedForReleaseVehicleCommand(Guid VehicleId) : IRequest<Result>;