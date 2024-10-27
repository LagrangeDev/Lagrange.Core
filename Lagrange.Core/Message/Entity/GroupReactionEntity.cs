using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class GroupReactionEntity : IMessageEntity
{
    private readonly IEnumerable<BotGroupReaction> _reactions;

    public GroupReactionEntity() { _reactions = Array.Empty<BotGroupReaction>(); }

    public GroupReactionEntity(IEnumerable<BotGroupReaction> reactions)
    {
        _reactions = reactions;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        var infos = _reactions
            .Select(reaction => new GroupReactionExtraFaceInfo()
            {
                FaceId = reaction.FaceId,
                Type = reaction.Type,
                Count = reaction.Count,
                IsAdded = reaction.IsAdded ? 1u : 0u,
            })
            .ToArray();

        var extra = new GroupReactionExtra
        {
            Body = new()
            {
                Field1 = new()
                {
                    Field1 = 0,
                    Field2 = 7240
                },
                FaceInfos = infos,
            }
        };
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, extra);

        return new Elem[] {
            new() { CommonElem = new() {
                ServiceType = 38,
                PbElem = stream.ToArray(),
                BusinessType = 1
            } }
        };
    }

    string IMessageEntity.ToPreviewString()
    {
        return "";
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        // TODO Parase
        return null;
    }
}