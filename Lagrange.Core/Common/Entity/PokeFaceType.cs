namespace Lagrange.Core.Common.Entity;

public enum PokeFaceType
{
    Poke = 1,
    Heart = 2,
    Like = 3,
    HeartBroken = 4,
    SixSixSix = 5,
    Kamehameha = 6
}

public enum SpecialPokeFaceType
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

public static class PokeFaceTypeExt
{
    private static readonly string[] nameMapping =
    {
        string.Empty, "戳一戳", "比心", "点赞", "心碎", "666", "放大招"
    };
    private static readonly string[] specialNameMapping =
    {
        string.Empty, "点赞", "爱心", "哈哈", "猪头", "炸弹", "便便", "亲亲", "药丸", "榴莲", "略略略", "平底锅", "钞票"
    };

    /// <summary>
    /// Return the name of the shake face type.
    /// A <see cref="ArgumentOutOfRangeException"/> will be thrown if the type is not valid.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the type is not valid.</exception>
    public static string ToName(this PokeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= nameMapping.Length)
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        return nameMapping[v];
    }


    /// <summary>
    /// Tries to get the name of the shake face type.
    /// Returns null if the type is not valid.
    /// </summary>
    /// <param name="type">The shake face type.</param>
    /// <returns>The name of the shake face type, or null if the type is not valid.</returns>
    public static string? TryGetName(this PokeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= nameMapping.Length)
            return null;
        return nameMapping[v];
    }

    /// <summary>
    /// Return the name of the shake face type.
    /// A <see cref="ArgumentOutOfRangeException"/> will be thrown if the type is not valid.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the type is not valid.</exception>
    public static string ToName(this SpecialPokeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= specialNameMapping.Length)
            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        return specialNameMapping[v];
    }

    /// <summary>
    /// Tries to get the name of the shake face type.
    /// Returns null if the type is not valid.
    /// </summary>
    /// <param name="type">The shake face type.</param>
    /// <returns>The name of the shake face type, or null if the type is not valid.</returns>
    public static string? TryGetName(this SpecialPokeFaceType type)
    {
        int v = (int)type;
        if (v <= 0 || v >= specialNameMapping.Length)
            return null;
        return specialNameMapping[v];
    }
}