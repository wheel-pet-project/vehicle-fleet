using Domain.VehicleAggregate;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.AddVehicle;

public record AddVehicleRequest(
    Guid CorrelationId,
    Guid ModelId,
    Status Status,
    string PlateNumber,
    string Vin,
    int FuelLevelPercents) : BaseRequest(CorrelationId), IRequest<Result>;