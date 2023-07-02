using System.Text;
using System.Xml;
using Lagrange.Core.Core.Packets.Message.Element;
using Lagrange.Core.Core.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary.Compression;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(RichMsg))]
public class XmlEntity : IMessageEntity
{
    public string Xml { get; set; }
    
    public XmlEntity() => Xml = "";
    
    public XmlEntity(string xml) => Xml = xml;
    
    public XmlEntity(XmlNode xml) => Xml = xml.OuterXml;

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                RichMsg = new RichMsg
                {
                    ServiceId = 35,
                    Template1 = ZCompression.ZCompress(Xml, new byte[] { 0x01 }),
                }
            }
        };
    }   
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.RichMsg is { ServiceId: 35, Template1: not null })
        {
            var xml = ZCompression.ZDecompress(elems.RichMsg.Template1.Skip(1).ToArray());
            return new XmlEntity(Encoding.UTF8.GetString(xml));
        }

        return null;
    }
    
    public string ToPreviewString() => $"[Xml]: {Xml}";
}