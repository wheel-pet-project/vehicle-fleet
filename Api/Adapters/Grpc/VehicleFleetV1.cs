using Grpc.Core;
using MediatR;
using Proto.VehicleFleetV1;

namespace Api.Adapters.Grpc;

public class VehicleFleetV1(IMediator mediator) : VehicleFleet.VehicleFleetBase
{
    
}