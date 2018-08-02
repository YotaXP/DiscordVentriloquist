using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordVentriloquist
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private void Application_Startup(object sender, StartupEventArgs e) {
        //    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Resolver);
        //}

        //static System.Reflection.Assembly Resolver(object sender, ResolveEventArgs args) {
        //    if (args.Name.StartsWith("Newtonsoft.Json,")) {
        //        Assembly a1 = Assembly.GetExecutingAssembly();
        //        Stream s = a1.GetManifestResourceStream("DiscordVentriloquist.Newtonsoft.Json.dll");
        //        byte[] block = new byte[s.Length];
        //        s.Read(block, 0, block.Length);
        //        Assembly a2 = Assembly.Load(block);
        //        return a2;
        //    }
        //    else
        //        return null;
        //}
    }
}
