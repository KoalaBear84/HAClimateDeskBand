using HAClimateDeskbandInstaller.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HAClimateDeskbandInstaller
{
    class Program
    {
        class InstallInfo
        {
            public List<string> FilesToCopy { get; set; }
            public List<string> FilesToRegister { get; set; }
            public string TargetPath { get; set; }
        }

        static void Main(string[] args)
        {
            Console.Title = "HAClimateDeskband Installer";

            InstallInfo info = new InstallInfo
            {
                FilesToCopy = new List<string> { "HAClimateDeskband.dll" },
                FilesToRegister = new List<string> { "HAClimateDeskband.dll" },
                TargetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "HAClimateDeskband")
            };

            if (args.Length > 0 && args[0].ToLower() == "/uninstall")
            {
                RollBack(info);
            }
            else
            {
                Install(info);
            }

            // pause
            Console.WriteLine("Press a key to close this window..");
            Console.ReadKey();
        }

        public static void WriteEmbeddedResourceToFile(string resourceName, string fileName)
        {
            string fullResourceName = $"{Assembly.GetExecutingAssembly().GetName().Name}.Resources.{resourceName}";

            using (Stream manifestResourceSTream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullResourceName))
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    manifestResourceSTream.CopyTo(fileStream);
                }
            }
        }

        static void Install(InstallInfo info)
        {
            ProgressBar progressBar = new ProgressBar();

            RestartExplorer restartExplorer = new RestartExplorer();
            restartExplorer.ReportProgress += Console.WriteLine;
            restartExplorer.ReportPercentage += (percentage) =>
            {
                progressBar.Report(percentage);
            };
            //Console.WriteLine($"Percentage: {percentage}");

            string[] manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();

            foreach (string manifestResourceName in manifestResourceNames)
            {
                Console.WriteLine(manifestResourceName);
            }

            // Create directory
            if (!Directory.Exists(info.TargetPath))
            {
                Console.Write("Creating target directory.. ");
                Directory.CreateDirectory(info.TargetPath);
                Console.WriteLine("OK.");
                CopyFiles(info);
            }
            else
            {
                //foreach (var item in info.FilesToRegister)
                //{
                //    var targetFilePath = System.IO.Path.Combine(info.TargetPath, item);                    
                //    RegisterDLL(targetFilePath, false, true);
                //    RegisterDLL(targetFilePath, true, true);
                //    Console.WriteLine("OK.");

                //}

                restartExplorer.Execute(() =>
                {
                    CopyFiles(info);
                });
            }

            // Register assemblies
            foreach (string item in info.FilesToRegister)
            {
                string targetFilePath = Path.Combine(info.TargetPath, item);
                Console.Write($"Registering {item}.. ");
                RegisterDLL(targetFilePath, true, false);
                Console.WriteLine("OK.");
            }
        }

        private static void CopyFiles(InstallInfo info)
        {
            foreach (string item in info.FilesToCopy)
            {
                string targetFilePath = Path.Combine(info.TargetPath, item);
                Console.Write($"Copying {item}.. ");
                WriteEmbeddedResourceToFile(item, targetFilePath);
                Console.WriteLine("OK.");
            }
        }

        static bool RegisterDLL(string target, bool bit64 = false, bool unregister = false)
        {
            string args = unregister ? "/unregister" : "/nologo /codebase";

            string regAsmPath = bit64 ?
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework64\v4.0.30319\regasm.exe") :
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\v4.0.30319\regasm.exe");

            RunProgram(regAsmPath, $@"{args} ""{target}""");

            return true;
        }

        static bool RollBack(InstallInfo info)
        {
            // Unregister assembly
            foreach (string item in info.FilesToRegister)
            {
                string targetFilePath = Path.Combine(info.TargetPath, item);
                RegisterDLL(targetFilePath, false, true);
                RegisterDLL(targetFilePath, true, true);
            }

            // Delete files
            RestartExplorer restartExplorer = new RestartExplorer();
            restartExplorer.Execute(() =>
            {
                // First copy files to program files folder          
                foreach (string item in info.FilesToCopy)
                {
                    string targetFilePath = Path.Combine(info.TargetPath, item);

                    if (File.Exists(targetFilePath))
                    {
                        Console.Write($"Deleting {item}... ");
                        File.Delete(targetFilePath);
                        Console.WriteLine("OK.");
                    }
                }
            });

            if (Directory.Exists(info.TargetPath))
            {
                Console.Write("Deleting target directory... ");
                Directory.Delete(info.TargetPath);
                Console.WriteLine("OK.");
            }

            return true;
        }

        static string RunProgram(string path, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                
                process.WaitForExit();
                
                return output;
            }
        }
    }
}
