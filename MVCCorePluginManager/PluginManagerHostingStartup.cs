using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MVCCorePluginManager.Abstract;
using MVCCorePluginManager.Internal;

namespace MVCCorePluginManager
{
    public class PluginManagerHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    _hostingEnvironment = webHostBuilderContext.HostingEnvironment;
                })
                .ConfigureServices(services =>
                {
                    _mvcBuilder = services.AddMvc();

                    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                    SetPluginDirectory();
                    RegisterPlugins();

                    _pluginChangingWatcher = new PluginChangingWatcher(_pluginFolder.FullName);
                    _pluginChangingWatcher.PluginsChanged += Watcher_PluginsChanged;
                    _pluginChangingWatcher.Start();
                });
        }


        ~PluginManagerHostingStartup()
        {
            _pluginChangingWatcher?.Stop();
        }


        #region EventHandler Methods

        private void Watcher_PluginsChanged()
        {
            _pluginChangingWatcher.Stop();
            PluginManager.ResetModules();
            ApplicationManager.Restart();
        }


        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);

            var assemblyFileName = $"{assemblyName.Name}.dll";
            var assemblyPath = _pluginAddressManager?.Get(assemblyFileName);

            if (string.IsNullOrWhiteSpace(assemblyPath) || !File.Exists(assemblyPath))
                return null;

            var assemblyAsByte = File.ReadAllBytes(assemblyPath);
            return Assembly.Load(assemblyAsByte);
        }

        #endregion


        #region Methods

        private void SetPluginDirectory()
        {
            var pluginsPath = Path.Combine(_hostingEnvironment.ContentRootPath, Plugins);

            if (!Directory.Exists(pluginsPath))
                Directory.CreateDirectory(pluginsPath);

            _pluginFolder = new DirectoryInfo(pluginsPath);
        }


        /// <summary>
        ///     Initialize method that registers all plugins and triggers the starter
        /// </summary>
        private void RegisterPlugins()
        {
            _pluginAddressManager = new PluginAddressManager();

            var assemblies = _pluginFolder.GetFiles(FileNamePatternToRun, SearchOption.AllDirectories)
                .Select(x =>
                {
                    _pluginAddressManager?.Add(x.FullName);
                    return AssemblyName.GetAssemblyName(x.FullName);
                })
                .Select(x => Assembly.Load(x.FullName));

            foreach (var assembly in assemblies)
            {
                //Add the plugin as a reference to the application
                Extensions.AddApplicationPart(_mvcBuilder, assembly);

                var type = assembly.GetTypes().FirstOrDefault(t => t.GetInterface(typeof(IModule).Name) != null);

                if (type != null)
                {
                    //Add the modules to the PluginManager to manage them later
                    var module = (IModule) Activator.CreateInstance(type);
                    PluginManager.Modules.Add(module, assembly);
                }
            }

            _pluginAddressManager = null;
        }

        #endregion


        #region Members

        private IHostingEnvironment _hostingEnvironment;
        private IMvcBuilder _mvcBuilder;

        private const string Plugins = nameof(Plugins);
        private const string FileNamePatternToRun = "*.dll";

        private PluginChangingWatcher _pluginChangingWatcher;
        private PluginAddressManager _pluginAddressManager;


        /// <summary>
        ///     The source plugin folder from which to copy to memory from
        /// </summary>
        /// <remarks>
        ///     This folder can't contain sub folders to organize plugin types
        /// </remarks>
        private static DirectoryInfo _pluginFolder;

        #endregion
    }
}