using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;

namespace Lagrange.Core.Message.Entity;

public class GreyTipEntity : IMessageEntity
{
    public string GreyTip { get; }
    public uint ObjectType { get; } = 3;
    public uint SubType { get; } = 2;
    public uint Type { get; } = 4;
    
    public GreyTipEntity(string greyTip , uint objectType, uint subType, uint type)
    {
        GreyTip = greyTip;
        ObjectType = objectType;
        SubType = subType;
        Type = type;
    }
    
    public GreyTipEntity() => GreyTip = string.Empty;
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        var content = new GreyTipContent
        {
            GrayTip = GreyTip,
            ObjectType = ObjectType,
            SubType = SubType,
            Type = Type
        };
        
        var extra = new GreyTipExtra
        {
            Layer1 = new GreyTipExtraLayer1
            {
                Info = new GreyTipExtraInfo { Content =  JsonSerializer.Serialize(content), Type = 1 }
            }
        };
        
        return new Elem[]
        {
            new()
            {
                GeneralFlags = new GeneralFlags
                {
                    PbReserve = extra.Serialize().ToArray(),
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        throw new NotImplementedException();
    }

    public string ToPreviewString()
    {
        throw new NotImplementedException();
    }
    
    private class GreyTipContent
    {
        [JsonPropertyName("gray_tip")] public string GrayTip { get; set; } = string.Empty;
        
        [JsonPropertyName("object_type")] public uint ObjectType { get; set; }
        
        [JsonPropertyName("sub_type")] public uint SubType { get; set; }
        
        [JsonPropertyName("type")] public uint Type { get; set; }
    }
}