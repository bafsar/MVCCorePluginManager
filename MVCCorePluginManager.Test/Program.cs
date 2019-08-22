using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MVCCorePluginManager.Test
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await ApplicationManager.RunAsync(CreateWebHostBuilder, args);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}