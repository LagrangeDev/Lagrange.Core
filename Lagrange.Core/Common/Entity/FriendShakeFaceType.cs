﻿namespace Lagrange.Core.Common.Entity;

public enum FriendShakeFaceType
{
    Poke = 1,
    Heart = 2,
    Like = 3,
    HeartBroken = 4,
    SixSixSix = 5,
    Kamehameha = 6
}

public enum FriendSpecialShakeFaceType
{
    Like = 1,
    Hear = 2,
    Haha = 3,
    Pig = 4,
    Bomb = 5,
    Poop = 6,
    Mua = 7,
    Pill = 8,
    Durian = 9,
    Lololo = 10,
    Pan = 11,
    Cash = 12,
}

public static class FriendShakeFaceTypeExt
{
    private static string[] nameMapping = new string[]
    {
        string.Empty, "戳一戳", "比心", "点赞", "心碎", "666", "放大招"
    };
    private static string[] specialNameMapping = new string[]
    {
        string.Empty, "点赞", "爱心", "哈哈", "猪头", "炸弹", "便便", "亲亲", "药丸", "榴莲", "略略略", "平底锅", "钞票"
    };

    public static string ToName(this FriendShakeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= nameMapping.Length)
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        return nameMapping[v];
    }

    public static string? TryGetName(this FriendShakeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= nameMapping.Length)
            return null;
        return nameMapping[v];
    }

    public static string ToName(this FriendSpecialShakeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= specialNameMapping.Length)
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        return specialNameMapping[v];
    }

    public static string? TryGetName(this FriendSpecialShakeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= specialNameMapping.Length)
            return null;
        return specialNameMapping[v];
    }
}