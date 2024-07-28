using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchUserInfoEvent : ProtocolEvent
{
    public BotUserInfo UserInfo { get; }
    
    public uint Uin { get; }
    
    public string? Uid { get; }

    private FetchUserInfoEvent(uint uin, string? uid) : base(true)
    {
        Uin = uin;
        Uid = uid;
    }

    private FetchUserInfoEvent(int resultCode, BotUserInfo userInfo) : base(resultCode)
    {
        UserInfo = userInfo;
    }

    public static FetchUserInfoEvent Create(uint uid) => new(uid, null);
    
    public static FetchUserInfoEvent Create(string uid) => new(0, uid);

    public static FetchUserInfoEvent Result(int resultCode, BotUserInfo userInfo) =>
        new(resultCode, userInfo);
}