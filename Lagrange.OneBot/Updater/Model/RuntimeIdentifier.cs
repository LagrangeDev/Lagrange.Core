namespace Lagrange.OneBot.Updater.Model
{
    public enum RuntimeIdentifier
    {
        WinX64,
        WinX86,
        LinuxX64,
        LinuxArm64,
        LinuxArm32,
        MacX64,
        MacArm64,
    }
    
    //extension method
    
    public static class RuntimeIdentifierExtensions
    {
        public static string GetRuntimeIdentifier(this RuntimeIdentifier runtimeIdentifier)
        {
            return runtimeIdentifier switch
            {
                RuntimeIdentifier.WinX64 => "win-x64",
                RuntimeIdentifier.WinX86 => "win-x86",
                RuntimeIdentifier.LinuxX64 => "linux-x64",
                RuntimeIdentifier.LinuxArm64 => "linux-arm64",
                RuntimeIdentifier.LinuxArm32 => "linux-arm",
                RuntimeIdentifier.MacX64 => "osx-x64",
                RuntimeIdentifier.MacArm64 => "osx-arm64",
                _ => throw new ArgumentOutOfRangeException(nameof(runtimeIdentifier), runtimeIdentifier, null)
            };
        }
        
        public static bool IsLinux(this RuntimeIdentifier runtimeIdentifier)
        {
            return runtimeIdentifier switch
            {
                RuntimeIdentifier.LinuxX64 => true,
                RuntimeIdentifier.LinuxArm64 => true,
                RuntimeIdentifier.LinuxArm32 => true,
                _ => false
            };
        }
        
        public static bool IsWindows(this RuntimeIdentifier runtimeIdentifier)
        {
            return runtimeIdentifier switch
            {
                RuntimeIdentifier.WinX64 => true,
                RuntimeIdentifier.WinX86 => true,
                _ => false
            };
        }
        
        public static bool IsMac(this RuntimeIdentifier runtimeIdentifier)
        {
            return runtimeIdentifier switch
            {
                RuntimeIdentifier.MacX64 => true,
                RuntimeIdentifier.MacArm64 => true,
                _ => false
            };
        }
    }
}