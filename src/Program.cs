using AdminShell;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;

namespace AdminShell
{
    public class Program
    {
        public static List<AdminShellPackageEnv> env = new List<AdminShellPackageEnv>();
        public static List<string> envFileName = new List<string>();
        public static bool isLoading = true;
        public static object changeAasxFile = new object();
        public static event EventHandler NewDataAvailable;

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

        public class NewDataAvailableArgs : EventArgs
        {
            public int signalNewDataMode;

            public NewDataAvailableArgs(int mode = 2)
            {
                signalNewDataMode = mode;
            }
        }

        public enum TreeUpdateMode
        {
            ValuesOnly = 0,
            Rebuild,
            RebuildAndCollapse
        }

        // 0 == same tree, only values changed
        // 1 == same tree, structure may change
        // 2 == build new tree, keep open nodes
        // 3 == build new tree, all nodes closed
        public static void signalNewData(int mode)
        {
            NewDataAvailable?.Invoke(null, new NewDataAvailableArgs(mode));
        }

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
            if (env != null)
            {
                foreach (var e in env)
                {
                    if (e?.AasEnv?.Submodels != null)
                    {
                        foreach (var sm in e.AasEnv.Submodels)
                        {
                            if (sm != null)
                            {
                                sm.SetAllParents();
                            }
                        }
                    }
                }
            }

            isLoading = false;
        }

        public static string ShortLocation(Exception ex)
        {
            if (ex == null || ex.StackTrace == null)
                return "";

            string[] lines = ex.StackTrace.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            if (lines.Length < 1)
                return "";

            // search for " in "
            // as the most actual stacktrace might be a built-in function, this might not work and therefore
            // go down in the stack
            int currLine = 0;
            while (true)
            {
                // nothing found at all
                if (currLine >= lines.Length)
                    return "";

                // access current line
                /* TODO (MIHO, 2020-11-12): replace with Regex for multi language. Ideally have Exception messages
                   always as English. */
                var p = lines[currLine].IndexOf(" in ", StringComparison.Ordinal);

                if (p < 0)
                    p = lines[currLine].IndexOf(" bei ", StringComparison.Ordinal);

                if (p < 0)
                {
                    // advance to next oldest line
                    currLine++;
                    continue;
                }

                // search last "\" or "/", to get only filename portion and position
                p = lines[currLine].LastIndexOfAny(new[] { '\\', '/' });
                if (p < 0)
                {
                    // advance to next oldest line
                    currLine++;
                    continue;
                }

                // return this
                return lines[currLine].Substring(p);
            }
        }
    }
}
