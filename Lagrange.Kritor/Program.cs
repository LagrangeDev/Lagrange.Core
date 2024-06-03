using System.Reflection;

namespace Lagrange.Kritor;

internal static class Program
{
    public static void Main(string[] args)
    {
        if (!File.Exists("appsettings.json"))
        {
            Console.WriteLine("No exist config file, create it now...");

            var assm = Assembly.GetExecutingAssembly();
            using var istr = assm.GetManifestResourceStream("Lagrange.Kritor.Resources.appsettings.json")!;
            using var temp = File.Create("appsettings.json");
            istr.CopyTo(temp);
            
            istr.Close();
            temp.Close();

            Console.WriteLine("Please Edit the appsettings.json to set configs and press any key to continue");
            Console.ReadLine();
        }
        
        var hostBuilder = new LagrangeAppBuilder(args)
            .ConfigureConfiguration("appsettings.json", false, true)
            .ConfigureKritorGrpcService();
        
        hostBuilder.Build().Run();

    }
}