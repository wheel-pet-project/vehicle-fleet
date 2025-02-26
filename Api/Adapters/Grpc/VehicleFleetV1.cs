using Grpc.Core;

namespace Api.Adapters.Grpc;

public class VehicleFleetV1 : Greeter.GreeterBase
{
    private readonly ILogger<VehicleFleetV1> _logger;

    public VehicleFleetV1(ILogger<VehicleFleetV1> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        context.Status = new Status(StatusCode.Internal, "Internal Server Error");
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}