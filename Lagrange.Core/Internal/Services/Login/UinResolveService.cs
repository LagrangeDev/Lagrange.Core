using System.Diagnostics;
using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Login;
using Lagrange.Core.Internal.Packets.Login;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Services.Login;

[EventSubscribe<UinResolveEventReq>(Protocols.Android)]
[Service("wtlogin.name2uin", RequestType.D2Auth, EncryptType.EncryptEmpty)]
internal class UinResolveService : BaseService<UinResolveEventReq, UinResolveEventResp>
{
    private Lazy<WtLogin> _packet = new();

    protected override async ValueTask<ReadOnlyMemory<byte>> Build(UinResolveEventReq input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));

        return await _packet.Value.BuildOicq04Android(input.Qid);
    }

    protected override ValueTask<UinResolveEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        if (!_packet.IsValueCreated) _packet = new Lazy<WtLogin>(() => new WtLogin(context));
        
        var payload = _packet.Value.Parse(input.Span, out ushort command);
        var reader = new BinaryPacket(payload);
        Debug.Assert(command == 0x810);
        
        ushort internalCmd = reader.Read<ushort>();
        byte state = reader.Read<byte>();
        var tlvs = ProtocolHelper.TlvUnPack(ref reader);
        
        if (tlvs.TryGetValue(0x146, out var error))
        {
            var errorReader = new BinaryPacket(error.AsSpan());
            uint _ = errorReader.Read<uint>(); // error code
            string errorTitle = errorReader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            string errorMessage = errorReader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            
            return new ValueTask<UinResolveEventResp>(new UinResolveEventResp(state, (errorTitle, errorMessage)));
        }

        if (state == 0)
        {
            var tlv104 = tlvs[0x104];
            var tlv113 = tlvs[0x113];
            
            var tlv113Reader = new BinaryPacket(tlv113.AsSpan());
            long uin = tlv113Reader.Read<uint>();
            short _ = tlv113Reader.Read<short>();
            string qid = tlv113Reader.ReadString(Prefix.Int16 | Prefix.LengthOnly);
            
            return new ValueTask<UinResolveEventResp>(new UinResolveEventResp(state, (uin, qid), tlv104));
        }
        
        return new ValueTask<UinResolveEventResp>(new UinResolveEventResp(state, null));
    }
}