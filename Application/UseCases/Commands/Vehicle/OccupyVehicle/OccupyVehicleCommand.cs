using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.OccupyVehicle;

public record OccupyVehicleCommand(Guid VehicleId, Guid BookingId) : IRequest<Result>;