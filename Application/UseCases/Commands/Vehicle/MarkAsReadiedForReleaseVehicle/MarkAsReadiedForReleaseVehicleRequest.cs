using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.MarkAsReadiedForReleaseVehicle;

public record MarkAsReadiedForReleaseVehicleRequest(Guid VehicleId) : IRequest<Result>;