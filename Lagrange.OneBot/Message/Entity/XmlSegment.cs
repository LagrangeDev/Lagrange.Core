using System.Text.Json.Serialization;
using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;

namespace Lagrange.OneBot.Message.Entity;

[Serializable]
public partial class XmlSegment(string xml, int serviceid)
{
    public XmlSegment() : this("", 35) { }

    [JsonPropertyName("data")] [CQProperty] public string Xml { get; set; } = xml;

    [JsonPropertyName("service_id")] [CQProperty] public int ServiceId { get; set; } = serviceid;
}

[SegmentSubscriber(typeof(XmlEntity), "xml")]
public partial class XmlSegment : SegmentBase
{
    public override void Build(MessageBuilder builder, SegmentBase segment)
    {
        if (segment is XmlSegment xml) builder.Xml(xml.Xml, xml.ServiceId);
    }

    public override SegmentBase FromEntity(MessageChain chain, IMessageEntity entity)
    {
        if (entity is not XmlEntity xmlEntity) throw new ArgumentException("Invalid entity type.");

        return new XmlSegment(xmlEntity.Xml, xmlEntity.ServiceId);
    }
}