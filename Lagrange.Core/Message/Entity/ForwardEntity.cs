using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Message.Component;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using Lagrange.Core.Internal.Packets.Message.Routing;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(SrcMsg))]
public class ForwardEntity : IMessageEntity
{
    public DateTime Time { get => Chain.Time; }

    public ulong MessageId { get => Chain.MessageId; }

    public uint Sequence { get => Chain.Sequence; }

    public uint ClientSequence { get => Chain.ClientSequence; }

    public string? Uid { get => Chain.Uid; set => Chain.Uid = value; }

    public uint TargetUin { get => Chain.FriendUin; }

    private string _selfUid = string.Empty;

    public MessageChain Chain { get; internal set; }

    public ForwardEntity()
    {
        Chain = MessageBuilder.Friend(0).Build();
    }

    public ForwardEntity(MessageChain chain)
    {
        Chain = chain;
    }

    IEnumerable<Elem> IMessageEntity.PackElement() => PackElement(false);

    IEnumerable<Elem> IMessageEntity.PackFakeElement() => PackElement(true);

    IEnumerable<Elem> PackElement(bool fake)
    {
        var result = new List<Elem> {
            new() {
                SrcMsg = new SrcMsg {
                    OrigSeqs = new List<uint> { ClientSequence != 0 ? ClientSequence : Sequence },
                    SenderUin = TargetUin,
                    Time = (int?)new DateTimeOffset(Time).ToUnixTimeSeconds(),
                    Elems = Chain.Elements,
                    PbReserve = ProtoExt.SerializeToBytes(new SrcMsg.Preserve
                    {
                        MessageId = MessageId,
                        SenderUid = Uid,
                    }),
                    SourceMsg = fake ? ProtoExt.SerializeToBytes(MessagePacker.BuildFake(Chain, _selfUid)) : null,
                    ToUin = 0
                }
            },
        };

        if (!fake && ClientSequence == 0)
        {
            result.Add(new Elem
            {
                Text = new Text
                {
                    Str = "not null",
                    PbReserve = ProtoExt.SerializeToBytes(new MentionExtra
                    {
                        Type = 2,
                        Uid = Chain.Uid!,
                    })
                }
            });
        }

        return result;
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.SrcMsg is not { } src) return null;

        if (src.SourceMsg != null)
        {
            var chain = MessagePacker.Parse(Serializer.Deserialize<PushMsgBody>(src.SourceMsg.AsSpan()), true);
            return new ForwardEntity(chain);
        }

        if (src.PbReserve != null)
        {
            var reserve = Serializer.Deserialize<SrcMsg.Preserve>(src.PbReserve.AsSpan());
            return new ForwardEntity(MessagePacker.Parse(new PushMsgBody
            {
                ResponseHead = new ResponseHead
                {
                    FromUin = (uint)src.SenderUin,
                    FromUid = reserve.SenderUid,
                    Grp = reserve.ReceiverUid != null ? null : new ResponseGrp { }
                },
                ContentHead = new ContentHead
                {
                    Random = (long?)(reserve.MessageId & 0xFFFFFFFF),
                    Sequence = (src.OrigSeqs != null && src.OrigSeqs.Count > 0) ? src.OrigSeqs[0] : 0,
                    Timestamp = src.Time,
                },
                Body = new MessageBody
                {
                    RichText = new RichText
                    {
                        Elems = src.Elems ?? new List<Elem>(),
                    }
                }
            }, true));
        }

        return new ForwardEntity(new MessageChain(0, 0, (src.OrigSeqs != null && src.OrigSeqs.Count > 0 ? src.OrigSeqs[0] : 0), 0));
    }

    void IMessageEntity.SetSelfUid(string selfUid) => _selfUid = selfUid;

    string IMessageEntity.ToPreviewString() => $"[Forward] {{ {Chain.Sequence} }}";

    string IMessageEntity.ToPreviewText() => "";
}
