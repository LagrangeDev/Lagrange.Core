using System.Text;
using Lagrange.OneBot.Utility;
using Microsoft.Extensions.Hosting;

namespace Lagrange.OneBot;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        QrCodeHelper.Output("https://txz.qq.com/p?k=1GF4r45MPwoy6ZSJSuRdlwyKlKOleKyi&f=1600001615");

        var hostBuilder = new LagrangeAppBuilder(args)
            .ConfigureConfiguration("appsettings.json", false, true);
        
        hostBuilder.Build().Run();
    }
}