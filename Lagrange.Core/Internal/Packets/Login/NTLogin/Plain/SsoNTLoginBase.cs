using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain;

// Resharper disable InconsistentNaming

/// <summary>
/// <para>Base for trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLogin and trpc.login.ecdh.EcdhService.SsoNTLoginEasyLogin</para>
/// <para>Should be encrypted and put into <see cref="SsoNTLoginEncryptedData"/></para>
/// </summary>
/// <typeparam name="T">Body Type</typeparam>
[ProtoContract]
internal class SsoNTLoginBase<T> where T : class
{
    [ProtoMember(1)] public SsoNTLoginHeader? Header { get; set; }

    [ProtoMember(2)] public T? Body { get; set; }
}