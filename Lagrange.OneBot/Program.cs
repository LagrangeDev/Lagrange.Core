using System.Runtime;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        GCSettings.LatencyMode = GCLatencyMode.Batch;

        if (!File.Exists("appsettings.json"))
        {
            Console.WriteLine("No exist config file, create it now...");
            File.WriteAllBytes("appsettings.json", Resource1.appsettings);
        }

        var hostBuilder = new LagrangeAppBuilder(args)
            .ConfigureConfiguration("appsettings.json", false, true)
            .ConfigureBots()
            .ConfigureOneBot();

        hostBuilder.Build().Run();
    }
}