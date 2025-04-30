using Application.UseCases.Commands.Vehicle.OccupyVehicle;
using FluentResults;
using MediatR;

namespace Infrastructure.Adapters.Postgres.Inbox.InputConsumerEvents;

public class BookingCreatedConsumerEvent : IConvertibleToCommand
{
    public BookingCreatedConsumerEvent(Guid eventId, Guid bookingId, Guid vehicleId)
    {
        EventId = eventId;
        BookingId = bookingId;
        VehicleId = vehicleId;
    }

    public Guid EventId { get; }
    public Guid BookingId { get; }
    public Guid VehicleId { get; }

    public IRequest<Result> ToCommand()
    {
        return new OccupyVehicleCommand(VehicleId, BookingId);
    }
}