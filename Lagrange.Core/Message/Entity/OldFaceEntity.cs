using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(OldFace))]
public class OldFaceEntity : IMessageEntity
{
    public uint FaceId { get; set; }

    public uint Type { get; set; }
    
    public OldFaceEntity() {}
    
    public OldFaceEntity(uint faceid , uint type = 1)
    {
        FaceId = faceid;
        Type = type;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new() { OldFace = new OldFace { Index = FaceId, Type = Type} },
            new() { Text = new Text { Str = FaceId.ToString(), } } // 34字段的face会强行抢走一个 1字段的text 故出此下策
        };
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        return elems.OldFace is { Index: not null }
            ? new OldFaceEntity(elems.OldFace.Index.Value) 
            : null;
    }

    public string ToPreviewString()
    {
        return $"[OldFace]: {FaceId}";
    }

}