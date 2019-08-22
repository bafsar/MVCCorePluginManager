using System;
using System.Reflection;
using MVCCorePluginManager.Abstract;

namespace Plugins.TestPlugin1
{
    public class TestPlugin1Module : IModule
    {
        public string Title
        {
            get { return "TestPlugin1 Index Page"; }
        }

        public string Name
        {
            get { return Assembly.GetAssembly(GetType()).GetName().Name; }
        }

        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }

        public string EntryControllerName
        {
            get { return "TestPlugin1"; }
        }
    }
}