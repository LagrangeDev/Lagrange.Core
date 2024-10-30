using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;
#pragma warning disable CS8601 
[EventSubscribe(typeof(FetchAiCharacterListEvent))]
[Service("OidbSvcTrpcTcp.0x929c_0")]
internal class FetchAiRecordListService : BaseService<FetchAiCharacterListEvent>
{
    protected override bool Build(FetchAiCharacterListEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929C_0>(new OidbSvcTrpcTcp0x929C_0()
        {
            Group = input.GroupUin
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchAiCharacterListEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929C_0Response>>(input);
        output = FetchAiCharacterListEvent.Result(payload.Body.Property
            .Select(x => new AiCharacter(x.CharacterId, x.CharacterName, x.CharacterVoiceUrl)).ToList()
        );
        extraEvents = null;
        return true;
    }
}