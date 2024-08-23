using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group Clock In
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xEB7, 1)]
internal class OidbSvcTrpcTcp0xEB7_1
{
    [ProtoMember(2)] public BodyClass Body { get; set; }

    [ProtoContract]
    internal class BodyClass
    {
        [ProtoMember(1)] public string Uin { get; set; }

        [ProtoMember(2)] public string GroupUin { get; set; }

        // 不确定要不要加，测试过没有这个参数也是可以的
        [ProtoMember(3)] public string AppVersion { get; set; }
    }
}
