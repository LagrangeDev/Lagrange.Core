using System.Reflection;
using System.Runtime;
using System.Text;
using Lagrange.OneBot.Extensions;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        string? version = Assembly.GetAssembly(typeof(Program))?
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion;

        Console.ForegroundColor = ConsoleColor.Magenta;
        if (Console.BufferWidth >= 45)
        {
            Console.WriteLine($$"""
               __
              / / ___ ____ ________ ____  ___ ____
             / /_/ _ `/ _ `/ __/ _ `/ _ \/ _ `/ -_)
            /____|_,_/\_, /_/  \_,_/_//_/\_, /\__/
                     /___/   ____       /___/__       __
                            / __ \___  ___ / _ )___  / /_
                           / /_/ / _ \/ -_) _  / _ \/ __/
                           \____/_//_/\__/____/\___/\__/
            """);
        }
        else Console.WriteLine("Lagrange.OneBot");
        Console.ResetColor();

        Console.WriteLine($"Version: {version?[^40..] ?? "unknown"}\n");

        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        GCSettings.LatencyMode = GCLatencyMode.Batch;

        if (!File.Exists("appsettings.json"))
        {
            Console.WriteLine("No exist config file, create it now...");

            var assm = Assembly.GetExecutingAssembly();
            using var istr = assm.GetManifestResourceStream("Lagrange.OneBot.Resources.appsettings.json")!;
            using var temp = File.Create("appsettings.json");
            istr.CopyTo(temp);

            istr.Close();
            temp.Close();

            Console.WriteLine("Please Edit the appsettings.json to set configs and press any key to continue");
            Console.ReadKey(true);
        }

        Host.CreateApplicationBuilder()
            .ConfigureLagrangeCore()
            .ConfigureOneBot()
            .Build()
            .InitializeMusicSigner() // Very ugly (
            .Run();
    }
}