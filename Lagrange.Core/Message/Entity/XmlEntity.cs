using System.Text;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary.Compression;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(RichMsg))]
public class XmlEntity : IMessageEntity
{
    public string Xml { get; set; }

    public int ServiceId {get;set;}
    
    public XmlEntity()
    {
        Xml = "";
        ServiceId = 35;
    }
    
    public XmlEntity(string xml,int serviceid = 35)
    {
        Xml = xml;
        ServiceId = serviceid;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                RichMsg = new RichMsg
                {
                    ServiceId = ServiceId,
                    Template1 = ZCompression.ZCompress(Xml, new byte[] { 0x01 }),
                }
            }
        };
    }   
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.RichMsg is { ServiceId: 35, Template1: not null } richMsg)
        {
            var xml = ZCompression.ZDecompress(richMsg.Template1.Skip(1).ToArray());
            return new XmlEntity(Encoding.UTF8.GetString(xml));
        }

        return null;
    }
    
    public string ToPreviewString() => $"[Xml]: {Xml}";
}