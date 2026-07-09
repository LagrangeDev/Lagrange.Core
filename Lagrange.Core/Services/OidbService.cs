using Lagrange.Core.Events;
using Lagrange.Core.Exceptions;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Utility;
using Lagrange.Proto;

namespace Lagrange.Core.Services;

/// <summary>
/// Base class for an OIDB protocol service with protobuf payload marshalling and error handling.
/// </summary>
public abstract class OidbService<TEventRequest, TEventResponse, TRequest, TResponse>
    : BaseService<TEventRequest, TEventResponse>
    where TEventRequest : ProtocolEvent
    where TEventResponse : ProtocolEvent
    where TRequest : IProtoSerializable<TRequest>
    where TResponse : IProtoSerializable<TResponse>
{
    private readonly uint? _command;
    private readonly uint? _service;
    private readonly uint _reserved;

    private protected OidbService() { }

    protected OidbService(
        uint command,
        uint service,
        uint reserved = 0)
    {
        _command = command;
        _service = service;
        _reserved = reserved;
    }

    protected virtual uint Command => _command ?? throw new InvalidOperationException("OIDB command is not configured.");

    protected virtual uint Service => _service ?? throw new InvalidOperationException("OIDB service is not configured.");

    protected virtual uint Reserved => _reserved;

    protected abstract Task<TRequest> ProcessRequest(TEventRequest request, BotContext context);

    protected abstract Task<TEventResponse> ProcessResponse(TResponse response, BotContext context);

    protected override async ValueTask<TEventResponse> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var oidb = ProtoHelper.Deserialize<Oidb>(input.Span);
        if (oidb.Result != 0)
        {
            string tag = $"OidbSvcTrpcTcp.0x{Command:X}_{Service}";
            context.LogWarning(tag, "Error: {0}, Message: {1}", null, oidb.Result, oidb.Message);
            throw new OperationException((int)oidb.Result, oidb.Message);
        }

        return await ProcessResponse(ProtoHelper.Deserialize<TResponse>(oidb.Body.Span), context);
    }

    protected override async ValueTask<ReadOnlyMemory<byte>> Build(TEventRequest input, BotContext context)
    {
        var request = await ProcessRequest(input, context);
        return ProtoHelper.Serialize(new Oidb
        {
            Command = Command,
            Service = Service,
            Body = ProtoHelper.Serialize(request),
            Reserved = Reserved
        });
    }
}
