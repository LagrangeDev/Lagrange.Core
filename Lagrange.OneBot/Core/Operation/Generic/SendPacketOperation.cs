using System.Text.Json;
using System.Text.Json.Nodes;
using Lagrange.Core;
using Lagrange.Core.Internal.Packets;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Extension;
using Lagrange.OneBot.Core.Entity.Action;
using Lagrange.OneBot.Core.Entity.Action.Response;

namespace Lagrange.OneBot.Core.Operation.Generic;

[Operation(".send_packet")]
public class SendPacketOperation : IOperation
{
    public async Task<OneBotResult> HandleOperation(BotContext context, JsonNode? payload)
    {
        if (payload.Deserialize<OneBotSendPacket>() is { } send)
        {
            int sequence = context.ContextCollection.Service.GetNewSequence();
            var ssoPacket = new SsoPacket(send.Type, send.Command, (uint)sequence, new BinaryPacket(send.Data.UnHex()));
            var task = await context.ContextCollection.Packet.SendPacket(ssoPacket);

            return new OneBotResult(new OneBotSendPacketResponse
            {
                Sequence = sequence,
                RetCode = task.RetCode,
                Extra = task.Extra,
                Result = task.Payload.ReadBytes(Prefix.Uint32 | Prefix.WithPrefix).Hex()
            }, 0, "ok");
        }

        throw new Exception();
    }
}