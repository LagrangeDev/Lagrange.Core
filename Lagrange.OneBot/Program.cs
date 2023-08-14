using System.Text;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        
        var hostBuilder = new LagrangeAppBuilder(args)
            .ConfigureConfiguration("appsettings.json", false, true)
            .ConfigureBots();
        
        hostBuilder.Build().Run();
    }
}