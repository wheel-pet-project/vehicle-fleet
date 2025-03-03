using Domain.SharedKernel.ValueObjects;
using FluentResults;
using MediatR;

namespace Application.UseCases.Commands.Vehicle.AddVehicle;

public record AddVehicleRequest(
    Guid ModelId,
    string PlateNumber,
    Color Color,
    string Vin,
    Location? Location = null) : IRequest<Result<AddVehicleResponse>>;

public record Location(double Latitude, double Longitude);