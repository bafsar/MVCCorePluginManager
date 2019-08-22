﻿using System;
using System.Reflection;
using MVCCorePluginManager.Abstract;

namespace Plugins.TestPlugin2
{
    public class TestPlugin2Module : IModule
    {
        public string Title
        {
            get { return "TestPlugin2 Index Page"; }
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
            get { return "TestPlugin2"; }
        }
    }
}