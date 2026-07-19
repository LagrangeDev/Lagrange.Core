using System.Reflection;

namespace Lagrange.Milky;

public static class Constants
{
    public static readonly string GitHash = GetGitHash(9);
    public static readonly string MilkyVersion = "1.2";

    private static string GetGitHash(int length)
    {
        var assembly = Assembly.GetAssembly(typeof(Program));
        if (assembly == null) return "unknown";

        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute == null) return "unknown";

        string version = attribute.InformationalVersion;

        int plusIndex = version.LastIndexOf('+');
        if (plusIndex < 0) return "unknown";

        return version.Substring(plusIndex + 1, length);
    }
}