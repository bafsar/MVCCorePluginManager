# MVCCorePluginManager

Simple Plugin Manager for ASP.NET Core v2.2

It presents a simple way to develop projects that works with plugins on ASP.NET Core v2.2

Usage:

Section 1:
To develop a plugin, first, we have to implement IModule interface from MVCCorePluginManager project.

Example:

    public class TestPluginModule : IModule
    {
        public string Title
        {
            get { return "TestPlugin Index Page"; }
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
            get { return "TestPlugin"; }
        }
    }

The most important part in this interface is EntryControllerName. We are specify the main controller for this plugin. This is the plugin's entry point. After create this controller, you can define anything you want. It will run like from inside of the main project.

Already Core v2.x compiles the views, you will get two dlls. If you don't have any view, then you will get only one.

Section 2:

Now, we have to add this code to the main project to include plugin manager:

[assembly: HostingStartup(typeof(PluginManagerHostingStartup))]

Simply, we can add inside of Program.cs or Startup.cs

Also, we have to change our Program.cs little bit like this:

        public static async Task Main(string[] args)
        {
            await ApplicationManager.RunAsync(CreateWebHostBuilder, args);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }

We are doing this because, after every plugin added or removed, the application should be restart. With this code, it restarts automatically.

If we don't change the code like this, after any plugin changing process, the application stops and when we do any request, we'll see an error page.

Section 3:

Now, it's time to use the plugin in any page.

To do that, we request the plugin with it's name in view's code block and then adding in html block

    @{
        var testPluginModule = PluginManager.GetModule("Plugins.TestPlugin"); // Your plugin assembly name without extension.
    }
    
    @if (testPluginModule != null)
    {
        @Html.ActionLink(testPluginModule.Title, "Index", testPluginModule.EntryControllerName)
    }

On code, all we have to do are these.

Section 4:

Finally, we can add or remove plugins. Just you have to do, adding assemblies in Plugins folder or removing from it. Plugins folder is on the root of the project.

Also, if you want you can add plugins in their own folders. With this way, your code can be more organized...

Remember to give write permission for Plugins folder!

Also, you can look to example on https://test3.bilalafsar.com
