using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Internal.Packets.Message.Component.Extra;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

public class KeyboardEntity : IMessageEntity
{
    public KeyboardData Data { get; set; }

    internal KeyboardEntity() => Data = new KeyboardData();

    public KeyboardEntity(KeyboardData data) => Data = data;
    
    public KeyboardEntity(string data) => Data = JsonSerializer.Deserialize<KeyboardData>(data) ?? throw new Exception();

    IEnumerable<Elem> IMessageEntity.PackElement() => new Elem[]
    {
        new()
        {
            CommonElem = new CommonElem
            {
                ServiceType = 46,
                PbElem = new ButtonExtra { Data = Data }.Serialize().ToArray(),
                BusinessType = 1
            }
        }
    };

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem) => null;

    public string ToPreviewString() => throw new NotImplementedException();
}

# region Json & Protobuf

[ProtoContract]
public class KeyboardData
{
    [JsonPropertyName("rows")] [ProtoMember(1)]
    public List<Row> Rows { get; set; } = new();
}

[ProtoContract]
public class Row
{
    [JsonPropertyName("buttons")] [ProtoMember(1)]
    public List<Button> Buttons { get; set; } = new();
}

[ProtoContract]
public class Button
{
    [JsonPropertyName("id")] [ProtoMember(1)]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("render_data")] [ProtoMember(2)]
    public RenderData RenderData { get; set; } = new();

    [JsonPropertyName("action")] [ProtoMember(3)]
    public Action Action { get; set; } = new();
}

[ProtoContract]
public class Action
{
    [JsonPropertyName("type")] [ProtoMember(1)]
    public int Type { get; set; }

    [JsonPropertyName("permission")] [ProtoMember(2)]
    public Permission Permission { get; set; } = new();

    // [JsonPropertyName("click_limit")] 
    // public long ClickLimit { get; set; }

    [JsonPropertyName("unsupport_tips")] [ProtoMember(4)]
    public string UnsupportTips { get; set; } = string.Empty;

    [JsonPropertyName("data")] [ProtoMember(5)]
    public string Data { get; set; } = string.Empty;

    [JsonPropertyName("reply")] [ProtoMember(7)]
    public bool? Reply { get; set; }
    
    [JsonPropertyName("enter")] [ProtoMember(8)]
    public bool? Enter { get; set; }
}

[ProtoContract]
public class Permission
{
    [JsonPropertyName("type")] [ProtoMember(1)]
    public int Type { get; set; }

    [JsonPropertyName("specify_role_ids")] [ProtoMember(2)]
    public List<string> SpecifyRoleIds { get; set; } = new();
    
    [JsonPropertyName("specify_user_ids")] [ProtoMember(3)]
    public List<string> SpecifyUserIds { get; set; } = new();
}

[ProtoContract]
public class RenderData
{
    [JsonPropertyName("label")] [ProtoMember(1)]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("visited_label")] [ProtoMember(2)]
    public string VisitedLabel { get; set; } = string.Empty;
    
    [JsonPropertyName("style")] [ProtoMember(3)]
    public int Style { get; set; }
}

#endregion