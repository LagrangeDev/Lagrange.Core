using Lagrange.Core.Core.Packets.Message.Routing;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message;

[ProtoContract]
internal class RoutingHead
{
    [ProtoMember(1)] public C2C.C2C? C2C { get; set; }

    [ProtoMember(2)] public Grp? Grp { get; set; }

    [ProtoMember(3)] public GrpTmp? GrpTmp { get; set; }

    [ProtoMember(6)] public WPATmp? WpaTmp { get; set; }

    [ProtoMember(15)] public Trans0X211? Trans0X211 { get; set; }
}