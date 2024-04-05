using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(SrcMsg))]
public class ForwardEntity : IMessageEntity
{
    public DateTime Time { get; set; }
    
    public ulong MessageId { get; set; }
    
    public uint Sequence { get; set; }
    
    public string? Uid { get; set; }

    public uint TargetUin { get; set; }

    internal List<Elem> Elements { get; }

    private string? SelfUid { get; set; }
    
    public ForwardEntity()
    {
        Sequence = 0;
        Uid = null;
        Elements = new List<Elem>();
    }

    public ForwardEntity(MessageChain chain)
    {
        Time = chain.Time;
        Sequence = chain.Sequence;
        Uid = chain.Uid;
        Elements = chain.Elements;
        TargetUin = chain.FriendUin;
        MessageId = chain.MessageId;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        var reserve = new SrcMsg.Preserve
        {
            MessageId = MessageId,
            ReceiverUid = SelfUid,
            SenderUid = Uid,
            ClientSequence = 0
        };
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, reserve);
        
        return new Elem[]
        {
            new()
            {
                SrcMsg = new SrcMsg
                {
                    OrigSeqs = new List<uint> { Sequence },
                    SenderUin = TargetUin,
                    Time = (int)(Time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
                    Elems = Elements,
                    PbReserve = stream.ToArray(),
                    ToUin = 0
                }
            }
        };
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.SrcMsg is { } srcMsg)
        {
            var reserve = Serializer.Deserialize<SrcMsg.Preserve>(srcMsg.PbReserve.AsSpan());
            return new ForwardEntity
            {
                Sequence = srcMsg.OrigSeqs?[0] ?? 0,
                TargetUin = (uint)srcMsg.SenderUin,
                MessageId = reserve.MessageId
            };
        }

        return null;
    }

    public void SetSelfUid(string selfUid) => SelfUid = selfUid;

    public string ToPreviewString() => $"[Forward]: Sequence: {Sequence}";

    public string ToPreviewText() => string.Empty;
}
