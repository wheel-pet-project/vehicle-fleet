using Api.Adapters.Grpc.EnumMappers;
using Application.UseCases.Commands.Model.AddModel;
using Application.UseCases.Commands.Model.UpdateModelCategory;
using Application.UseCases.Commands.Model.UpdateModelTariff;
using Application.UseCases.Queries.Model.GetAllModels;
using Application.UseCases.Queries.Model.GetModelById;
using Domain.SharedKernel.Errors;
using Domain.SharedKernel.Exceptions.ArgumentException;
using FluentResults;
using Grpc.Core;
using MediatR;
using Proto.VehicleFleetV1;
using Status = Grpc.Core.Status;

namespace Api.Adapters.Grpc;

public partial class VehicleFleetV1(
    IMediator mediator,
    ColorMapper colorMapper,
    StatusMapper statusMapper) : VehicleFleet.VehicleFleetBase
{
    public override async Task<AddModelRes> AddModel(AddModelReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new AddModelCommand(
            request.Brand,
            request.CarModel,
            request.Category[0],
            request.PricePerMin,
            request.PricePerHour,
            request.PricePerDay));

        return result.IsSuccess
            ? new AddModelRes { ModelId = result.Value.ModelId.ToString() }
            : ParseErrorToRpcException<AddModelRes>(result.Errors);
    }

    public override async Task<UpdateModelCategoryRes> UpdateModelCategory(
        UpdateModelCategoryReq request,
        ServerCallContext context)
    {
        var result = await mediator.Send(new UpdateModelCategoryCommand(
            ParseGuidOrThrow(request.ModelId),
            request.Category[0]));

        return result.IsSuccess
            ? new UpdateModelCategoryRes()
            : ParseErrorToRpcException<UpdateModelCategoryRes>(result.Errors);
    }

    public override async Task<UpdateModelTariffRes> UpdateModelTariff(
        UpdateModelTariffReq request,
        ServerCallContext context)
    {
        var result = await mediator.Send(new UpdateModelTariffCommand(ParseGuidOrThrow(request.ModelId),
            request.PricePerMin,
            request.PricePerHour,
            request.PricePerDay));

        return result.IsSuccess
            ? new UpdateModelTariffRes()
            : ParseErrorToRpcException<UpdateModelTariffRes>(result.Errors);
    }

    public override async Task<GetAllModelsRes> GetAllModels(GetAllModelsReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetAllModelsQuery(
                request.Page,
                request.PageSize),
            context.CancellationToken);

        if (result.IsFailed) return ParseErrorToRpcException<GetAllModelsRes>(result.Errors);

        var response = new GetAllModelsRes();
        response.Models.AddRange(result.Value.Models.Select(x => new GetAllModelsRes.Types.ModelShortView
            { ModelId = x.Id.ToString(), Brand = x.Brand, CarModel = x.CarModel }));

        return response;
    }

    public override async Task<GetModelByIdRes> GetModelById(GetModelByIdReq request, ServerCallContext context)
    {
        var result = await mediator.Send(new GetModelByIdQuery(ParseGuidOrThrow(request.ModelId)),
            context.CancellationToken);

        return result.IsSuccess
            ? new GetModelByIdRes
            {
                ModelId = result.Value.Id.ToString(),
                Brand = result.Value.Brand,
                CarModel = result.Value.CarModel,
                Category = new string(result.Value.Category, 1),
                PricePerMin = result.Value.PricePerMinute,
                PricePerHour = result.Value.PricePerHour,
                PricePerDay = result.Value.PricePerDay
            }
            : ParseErrorToRpcException<GetModelByIdRes>(result.Errors);
    }

    private T ParseErrorToRpcException<T>(List<IError> errors)
    {
        if (errors.Exists(x => x is NotFound))
            throw new RpcException(new Status(StatusCode.NotFound, string.Join(' ', errors.Select(x => x.Message))));

        if (errors.Exists(x => x is CommitFail))
            throw new RpcException(new Status(StatusCode.Unavailable, string.Join(' ', errors.Select(x => x.Message))));

        throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(' ', errors.Select(x => x.Message))));
    }

    private Guid ParseGuidOrThrow(string potentialId)
    {
        return Guid.TryParse(potentialId, out var id)
            ? id
            : throw new ValueOutOfRangeException($"{nameof(potentialId)} is invalid uuid");
    }
}