using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MVCCorePluginManager
{
    public static class ApplicationManager
    {
        #region Fields

        private static IApplicationLifetime _applicationLifetime;

        #endregion


        #region Private Methods

        private static async Task RunWebHostAsync(Func<string[], IWebHostBuilder> webHostBuilderCreater, string[] args)
        {
            var webHost = webHostBuilderCreater(args).Build();
            _applicationLifetime = webHost.Services.GetService<IApplicationLifetime>();
            await webHost.RunAsync();
        }

        #endregion


        #region Public Methods

        public static async Task RunAsync(Func<string[], IWebHostBuilder> webHostBuilderCreater, string[] args)
        {
            var isCancelKeyPressed = false;

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                isCancelKeyPressed = true;
                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };

            while (true)
            {
#if DEBUG
                Console.WriteLine("Application is starting");
#endif
                await RunWebHostAsync(webHostBuilderCreater, args);
#if DEBUG
                Console.WriteLine("Application has been terminated");
#endif
                if (isCancelKeyPressed)
                    break;
            }
        }

        public static void Stop()
        {
            _applicationLifetime?.StopApplication();
        }

        public static void Restart()
        {
            Stop();
        }

        #endregion
    }
}