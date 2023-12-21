namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchAvatarEvent : ProtocolEvent
{
    public uint Uin { get; }
    public string AvatarUrl { get; }
    public string Uid { get; }

    private FetchAvatarEvent(string uid) : base(true)
    {
        Uid = uid;
    }

    private FetchAvatarEvent(int resultCode, uint uin, string avatarUrl) : base(resultCode)
    {
        Uin = uin;
        AvatarUrl = avatarUrl;
    }

    public static FetchAvatarEvent Create(string uid) => new(uid);

    public static FetchAvatarEvent Result(int resultCode, uint uin, string avatarUrl) =>
        new(resultCode, uin, avatarUrl);
}