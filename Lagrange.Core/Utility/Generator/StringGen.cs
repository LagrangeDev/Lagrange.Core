using System.Text;

namespace Lagrange.Core.Utility.Generator;

internal static class StringGen
{
    private const string Hex = "1234567890abcdef";

    public static string GenerateTrace()
    {
        var sb = new StringBuilder(55);

        sb.Append(0.ToString("X2")); // 2 chars
        sb.Append('-'); // 1 char

        for (var i = 0; i < 32; i++) sb.Append(Hex[Random.Shared.Next(0, Hex.Length)]); // 32 chars
        sb.Append('-'); // 1 char

        for (var i = 0; i < 16; i++) sb.Append(Hex[Random.Shared.Next(0, Hex.Length)]); // 16 chars
        sb.Append('-'); // 1 char

        sb.Append(01.ToString("X2")); // 2 chars

        return sb.ToString();
    }

    public static string GenerateHex(int length) => RandomGenerate(Hex, length);

    public static string RandomGenerate(string table, int length)
    {
        var sb = new StringBuilder(length);
        for (var i = 0; i < length; i++) sb.Append(table[Random.Shared.Next(0, Hex.Length)]);

        return sb.ToString();
    }
}