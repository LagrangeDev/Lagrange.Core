using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.System;

/// <summary>
/// wtlogin.trans_emp and wtlogin.login packet, has tag of C201 in ushort and length of 0x35 in byte
/// <para>没错 这个就是臭名昭著的0C03算法 每一个包都有这个几把</para>
/// </summary>
[ProtoContract]
internal class Sign
{
    [ProtoMember(1)] public byte[] Signature { get; set; }
    
    [ProtoMember(3)] public Software S { get; set; }
}