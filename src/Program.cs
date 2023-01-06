
namespace AdminShell
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class Program
    {
        public static List<AdminShellPackageEnv> env = new List<AdminShellPackageEnv>();
        public static List<string> envFileName = new List<string>();
        public static bool isLoading = true;
        public static object changeAasxFile = new object();


        public static void Main(string[] args)
        {
            Task.Run(() => LoadLocalAASXFiles()).ConfigureAwait(false);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void LoadLocalAASXFiles()
        {
            string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.aasx");
            Console.WriteLine("Found " + fileNames.Length.ToString() + " AAS in directory " + Directory.GetCurrentDirectory());
            Array.Sort(fileNames);

            // load all local AASX files
            foreach (string fn in fileNames)
            {
                Console.WriteLine("Loading {0}...", fn);
                envFileName.Add(fn);
                env.Add(new AdminShellPackageEnv(fn, true));
            }

            // set all parents
            foreach (var e in env)
            {
                if (e?.AasEnv?.Submodels != null)
                {
                    foreach (var sm in e.AasEnv.Submodels)
                    {
                        if (sm != null)
                        {
                            sm.SetSMEsParent();
                        }
                    }
                }
            }

            isLoading = false;

            TreeBuilder.SignalNewData(TreeBuilder.TreeUpdateMode.RebuildAndCollapse);
        }
    }
}
