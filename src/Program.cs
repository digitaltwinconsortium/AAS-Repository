
namespace AdminShell
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Opc.Ua.Configuration;

    public class Program
    {
        public static IHost AppHost { get; private set; }

        public static ApplicationInstance App = new();
        public static ProductCarbonFootprintService PCFService;

        public static void Main(string[] args)
        {
            AppHost = CreateHostBuilder(args).Build();
            AppHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
