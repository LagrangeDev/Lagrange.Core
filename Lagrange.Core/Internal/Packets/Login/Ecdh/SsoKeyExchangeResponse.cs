using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Login.Ecdh;

/// <summary>
/// Response for trpc.login.ecdh.EcdhService.SsoKeyExchange
/// </summary>
[ProtoContract]
internal class SsoKeyExchangeResponse
{
    [ProtoMember(1)] public byte[] GcmEncrypted { get; set; }

    [ProtoMember(2)] public byte[] Body { get; set; } // 腾讯你个傻逼这什么东西啊怎么还带Tag是0的Member

    [ProtoMember(3)] public byte[] PublicKey { get; set; }
}