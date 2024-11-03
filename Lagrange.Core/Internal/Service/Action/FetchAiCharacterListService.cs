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

[EventSubscribe(typeof(FetchAiCharacterListEvent))]
[Service("OidbSvcTrpcTcp.0x929d_0")]
internal class FetchAiCharacterListService : BaseService<FetchAiCharacterListEvent>
{
    protected override bool Build(FetchAiCharacterListEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device,
        out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929D_0>(new OidbSvcTrpcTcp0x929D_0()
        {
            Group = input.GroupUin,
            ChatType = input.ChatType,
        });
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out FetchAiCharacterListEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var payload = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x929D_0Response>>(input);
        extraEvents = null;
        if (payload.ErrorCode == 0)
        {

            output = FetchAiCharacterListEvent.Result((int)payload.ErrorCode,payload.Body.Property
                .Select(x => new AiCharacterList(x.Type,
                    x.Vulue.Select(x => new AiCharacter(x.CharacterId, x.CharacterName, x.CharacterVoiceUrl)).ToList())
                ).ToList(),string.Empty);            
        }
        else
        {
            output = FetchAiCharacterListEvent.Result((int)payload.ErrorCode, null,payload.ErrorMsg);
        }

        return true;
    }
}