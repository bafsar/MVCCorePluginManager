using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVCCorePluginManager.Abstract;

namespace MVCCorePluginManager
{
    public static class PluginManager
    {
        static PluginManager()
        {
            ResetModules();
        }

        internal static Dictionary<IModule, Assembly> Modules { get; set; }

        public static IEnumerable<IModule> GetModules()
        {
            return Modules.Select(m => m.Key).ToList();
        }

        public static IModule GetModule(string name)
        {
            return GetModules().FirstOrDefault(m => m.Name == name);
        }

        internal static void ResetModules()
        {
            Modules = new Dictionary<IModule, Assembly>();
        }
    }
}