using System.IO.Compression;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Logic;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Message;
using Lagrange.Core.Services;
using Lagrange.Core.Utility;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Internal.Services.Message;

[EventSubscribe<LongMsgRecvEventReq>(Protocols.All)]
[Service("trpc.group.long_msg_interface.MsgService.SsoRecvLongMsg")]
internal class LongMsgRecvService : BaseService<LongMsgRecvEventReq, LongMsgRecvEventResp>
{
    protected override ValueTask<ReadOnlyMemory<byte>> Build(LongMsgRecvEventReq input, BotContext context)
    {
        var packet = new LongMsgInterfaceReq
        {
            RecvReq = new LongMsgRecvReq
            {
                PeerInfo = new LongMsgPeerInfo { PeerUid = context.Keystore.Uid },
                ResId = input.ResId,
                MsgType = input.IsGroup ? 1u : 3u, // 4 for wpamsg, 5 for grpmsg temp
            },
            Attr = new LongMsgAttr
            {
                SubCmd = context.Config.Protocol.IsAndroid() ? 3u : 2u, // 1 -> Android 2 -> NTPC 0 -> Undefined
                ClientType = context.Config.Protocol switch
                {
                    Protocols.Windows or Protocols.MacOs or Protocols.Linux => 1u,
                    Protocols.AndroidPhone => 2u,
                    // Protocols.IOS => 3u,
                    // Protocols.IPad => 4u,
                    Protocols.AndroidPad => 5u,
                    _ => 0u
                },
                Platform = context.Config.Protocol switch
                {
                    Protocols.Windows => 3u,
                    Protocols.Linux => 6u,
                    Protocols.MacOs => 7u,
                    // Protocols.IOS => 8u,
                    Protocols.AndroidPad or Protocols.AndroidPhone => 9u,
                    _ => 0u
                },
                ProxyType = 0
            }
        };
        
        return ValueTask.FromResult(ProtoHelper.Serialize(packet));
    }

    protected override async ValueTask<LongMsgRecvEventResp> Parse(ReadOnlyMemory<byte> input, BotContext context)
    {
        var rsp = ProtoHelper.Deserialize<LongMsgInterfaceRsp>(input.Span);
        
        await using var src = new MemoryStream(rsp.RecvRsp.Payload);
        await using var dest = new MemoryStream();
        await using var gzip = new GZipStream(src, CompressionMode.Decompress);
        await gzip.CopyToAsync(dest);
        var decompressedContent = dest.ToArray();

        var logic = context.EventContext.GetLogic<MessagingLogic>();
        var content = ProtoHelper.Deserialize<PbMultiMsgTransmit>(decompressedContent.AsSpan());
        var multiMsg = content.Items.First(x => x.FileName == "MultiMsg").Buffer.Msg;
        
        var messages = new List<BotMessage>(multiMsg.Count);
        foreach (var msg in multiMsg)
        {
            var message = await logic.Parse(msg);
            messages.Add(message);
        }

        return new LongMsgRecvEventResp(messages);
    }
}