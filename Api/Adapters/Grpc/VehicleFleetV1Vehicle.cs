using Application.UseCases.Commands.Vehicle.AddVehicle;
using Application.UseCases.Commands.Vehicle.DeleteVehicle;
using Application.UseCases.Commands.Vehicle.MarkAsReadiedForReleaseVehicle;
using Application.UseCases.Commands.Vehicle.ReleaseVehicle;
using Application.UseCases.Commands.Vehicle.SendToServiceVehicle;
using Application.UseCases.Queries.Vehicle.GetAllVehicles;
using Application.UseCases.Queries.Vehicle.GetVehicleById;
using Grpc.Core;
using Proto.VehicleFleetV1;
using Location = Application.UseCases.Commands.Vehicle.AddVehicle.Location;

namespace Api.Adapters.Grpc;

/// <summary>
/// Часть класса gRPC API, работающая с сущностью ТС
/// </summary>
public partial class VehicleFleetV1 : VehicleFleet.VehicleFleetBase
{
    public override async Task<AddVehicleRes> AddVehicle(AddVehicleReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new AddVehicleRequest(
            Guid.Parse(request.ModelId), 
            request.PlateNumber,
            colorMapper.ProtoToDomain(request.Color), 
            request.Vin,
            request.Location is not null ? new Location(request.Location.Latitude, request.Location.Longitude) : null));
        
        return result.IsSuccess 
            ? new AddVehicleRes { VehicleId = result.Value.VehicleId.ToString() }
            : ParseErrorToRpcException<AddVehicleRes>(result.Errors);
    }

    public override async Task<DeleteVehicleRes> DeleteVehicle(DeleteVehicleReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new DeleteVehicleRequest(Guid.Parse(request.VehicleId)));

        return result.IsSuccess
            ? new DeleteVehicleRes()
            : ParseErrorToRpcException<DeleteVehicleRes>(result.Errors);
    }

    public override async Task<MarkAsReadiedForReleaseVehicleRes> MarkAsReadiedForReleaseVehicle(MarkAsReadiedForReleaseVehicleReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new MarkAsReadiedForReleaseVehicleRequest(Guid.Parse(request.VehicleId)));
        
        return result.IsSuccess
            ? new MarkAsReadiedForReleaseVehicleRes()
            : ParseErrorToRpcException<MarkAsReadiedForReleaseVehicleRes>(result.Errors);
    }

    public override async Task<ReleaseVehicleRes> ReleaseVehicle(ReleaseVehicleReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new ReleaseVehicleRequest(Guid.Parse(request.VehicleId)));

        return result.IsSuccess
            ? new ReleaseVehicleRes()
            : ParseErrorToRpcException<ReleaseVehicleRes>(result.Errors);
    }

    public override async Task<SendToServiceVehicleRes> SendToServiceVehicle(SendToServiceVehicleReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new SendToServiceVehicleRequest(Guid.Parse(request.VehicleId)));

        return result.IsSuccess
            ? new SendToServiceVehicleRes()
            : ParseErrorToRpcException<SendToServiceVehicleRes>(result.Errors);
    }

    public override async Task<GetAllVehiclesRes> GetAllVehicles(GetAllVehiclesReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetAllVehiclesQuery(
            statusMapper.ProtoToDomain(request.FilteringStatus),
            request.Page, 
            request.PageSize));

        if (result.IsFailed) ParseErrorToRpcException<GetAllModelsRes>(result.Errors);
        
        var response = new GetAllVehiclesRes();
        response.Vehicles.AddRange(result.Value.Vehicles.Select(x => new GetAllVehiclesRes.Types.VehicleShortView
        {
            VehicleId = x.Id.ToString(), 
            Brand = x.Brand, 
            CarModel = x.CarModel,
            Color = colorMapper.DomainToProto(x.Color),
            Location = new Proto.VehicleFleetV1.Location
            {
                Latitude = x.Latitude, 
                Longitude = x.Longitude
            }
        }));
        
        return response;
    }

    public override async Task<GetVehicleByIdRes> GetVehicleById(GetVehicleByIdReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetVehicleByIdQuery(Guid.Parse(request.VehicleId)));

        throw new NotImplementedException();
        // return result.IsSuccess
        //     ? new GetVehicleByIdRes()
        //     {
        //         Brand = 
        //     }
    }

    public override async Task<GetVehicleDetailsByIdRes> GetVehicleDetailsById(GetVehicleDetailsByIdReq request, ServerCallContext context)
    {
        throw new NotImplementedException();
        // return base.GetVehicleDetailsById(request, context);
    }

    public override async Task<GetVehiclesInSquareRes> GetVehiclesInSquare(GetVehiclesInSquareReq request, ServerCallContext context)
    {
        throw new NotImplementedException();
        // return base.GetVehiclesInSquare(request, context);
    }
}