using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Internal.Event.System;

#pragma warning disable CS8618

internal class FetchUserInfoEvent : ProtocolEvent
{
    public BotUserInfo UserInfo { get; }
    
    public string Uid { get; }

    private FetchUserInfoEvent(string uid) : base(true)
    {
        Uid = uid;
    }

    private FetchUserInfoEvent(int resultCode, BotUserInfo userInfo) : base(resultCode)
    {
        UserInfo = userInfo;
    }

    public static FetchUserInfoEvent Create(string uid) => new(uid);

    public static FetchUserInfoEvent Result(int resultCode, BotUserInfo userInfo) =>
        new(resultCode, userInfo);
}