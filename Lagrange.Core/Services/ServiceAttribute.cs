using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Services;

/// <summary>
/// Describes the SSO transport options for a protocol service.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ServiceAttribute(
    string command,
    RequestType requestType = RequestType.D2Auth,
    EncryptType encryptType = EncryptType.EncryptD2Key) : Attribute
{
    public string Command { get; } = command;

    public RequestType RequestType { get; } = requestType;

    public EncryptType EncryptType { get; } = encryptType;

    public bool DisableLog { get; init; }
}
