using HAClimateDeskbandInstaller.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace HAClimateDeskbandInstaller
{
    class Program
    {
        private const string InstallerExecutableName = "HAClimateDeskbandInstaller.exe";
        private const string DllName = "HAClimateDeskband.dll";
        static Guid UninstallGuid = new Guid(@"2d0e746f-e2ae-4c2c-9040-5c5a715e7a8a");

        class InstallInfo
        {
            public List<string> FilesToCopy { get; set; }
            public List<string> FilesToRegister { get; set; }
            public string TargetPath { get; set; }
        }

        static void Main(string[] args)
        {
            Console.Title = "HA Climate Deskband Installer";

            InstallInfo info = new InstallInfo
            {
                FilesToCopy = new List<string> { DllName },
                FilesToRegister = new List<string> { DllName },
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

            Console.WriteLine("Press any key to close this window..");
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
            Console.WriteLine("Installing HA Climate Deskband on your computer, please wait.");
            RestartExplorer restartExplorer = new RestartExplorer();

            try
            {
                // Create directory
                if (!Directory.Exists(info.TargetPath))
                {
                    Console.Write("Creating target directory.. ");
                    Directory.CreateDirectory(info.TargetPath);
                    Console.WriteLine("OK.");
                    CopyFiles(info);

                    // Copy the uninstaller too
                    File.Copy(Assembly.GetExecutingAssembly().Location, Path.Combine(info.TargetPath, InstallerExecutableName), true);
                }
                else
                {
                    restartExplorer.Execute(() =>
                    {
                        CopyFiles(info);

                        // Copy the uninstaller too
                        File.Copy(Assembly.GetExecutingAssembly().Location, Path.Combine(info.TargetPath, InstallerExecutableName), true);
                    });
                }

                // Register assemblies
                foreach (string filename in info.FilesToRegister)
                {
                    string targetFilePath = Path.Combine(info.TargetPath, filename);
                    Console.Write($"Registering {filename}.. ");
                    RegisterDLL(targetFilePath, true, false);
                    Console.WriteLine("OK.");
                }

                Console.Write("Registering uninstaller.. ");
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(info.TargetPath, DllName));
                CreateUninstaller(Path.Combine(info.TargetPath, InstallerExecutableName), Version.Parse(fileVersionInfo.FileVersion));
                Console.WriteLine("OK.");

                // Remove pending delete operations
                Console.Write("Cleaning up previous pending uninstalls.. ");

                if (CleanUpPendingDeleteOperations(info.TargetPath, out string errorMessage))
                {
                    Console.WriteLine("OK.");
                }
                else
                {
                    Console.WriteLine($"ERROR: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while installing..");
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CopyFiles(InstallInfo info)
        {
            foreach (string filename in info.FilesToCopy)
            {
                string targetFilePath = Path.Combine(info.TargetPath, filename);
                Console.Write($"Copying {filename}.. ");
                WriteEmbeddedResourceToFile(filename, targetFilePath);
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
                // Remove files
                foreach (string filename in info.FilesToCopy)
                {
                    string targetFilePath = Path.Combine(info.TargetPath, filename);

                    if (File.Exists(targetFilePath))
                    {
                        Console.Write($"Deleting {filename}.. ");
                        File.Delete(targetFilePath);
                        Console.WriteLine("OK.");
                    }
                }
            });

            Console.Write($"Deleting {InstallerExecutableName}.. ");

            try
            {
                if (Win32Api.DeleteFile(Path.Combine(info.TargetPath, InstallerExecutableName)))
                {
                    Console.WriteLine("OK.");
                }
                else
                {
                    Win32Api.MoveFileEx(Path.Combine(info.TargetPath, InstallerExecutableName), null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
                    Console.WriteLine("Scheduled for deletion after next reboot.");
                }
            }
            catch
            {
                Win32Api.MoveFileEx(Path.Combine(info.TargetPath, InstallerExecutableName), null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
                Console.WriteLine("Scheduled for deletion after next reboot.");
            }

            if (Directory.Exists(info.TargetPath))
            {
                Console.Write("Deleting target directory.. ");

                try
                {
                    Directory.Delete(info.TargetPath);
                    Console.WriteLine("OK.");
                }
                catch
                {
                    Win32Api.MoveFileEx(info.TargetPath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
                    Console.WriteLine("Scheduled for deletion after next reboot.");
                }
            }

            Console.Write("Removing uninstall info from registry.. ");
            DeleteUninstaller();
            Console.WriteLine("OK.");

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

        static private void CreateUninstaller(string pathToUninstaller, Version version)
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }

                try
                {
                    RegistryKey registryKey = null;

                    try
                    {
                        string guidText = UninstallGuid.ToString("B");
                        registryKey = parent.OpenSubKey(guidText, true) ?? parent.CreateSubKey(guidText);

                        if (registryKey == null)
                        {
                            throw new Exception($"Unable to create uninstaller '{@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"}\\{guidText}'");
                        }

                        string exe = pathToUninstaller;

                        registryKey.SetValue("DisplayName", "HA Climate DeskBand");
                        registryKey.SetValue("ApplicationVersion", version.ToString());
                        registryKey.SetValue("Publisher", "KoalaBear84");
                        registryKey.SetValue("DisplayIcon", exe);
                        registryKey.SetValue("DisplayVersion", version.ToString(3));
                        registryKey.SetValue("URLInfoAbout", "https://github.com/KoalaBear84/HAClimateDeskband");
                        registryKey.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        registryKey.SetValue("UninstallString", exe + " /uninstall");
                    }
                    finally
                    {
                        if (registryKey != null)
                        {
                            registryKey.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred writing uninstall information to the registry.  The service is fully installed but can only be uninstalled manually through the command line.", ex);
                }
            }
        }

        static private void DeleteUninstaller()
        {
            using (RegistryKey parent = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }

                string guidText = UninstallGuid.ToString("B");
                parent.DeleteSubKeyTree(guidText, false);
            }
        }

        static bool CleanUpPendingDeleteOperations(string basepath, out string errorMessage)
        {
            // Check the registry for pending operations on the program files (previous pending uninstall)

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\", true))
                {
                    if (key != null)
                    {
                        object o = key.GetValue("PendingFileRenameOperations");

                        if (o != null)
                        {
                            string[] values = o as string[];
                            List<string> dest = new List<string>();

                            for (int i = 0; i < values.Length; i += 2)
                            {
                                if (!values[i].Contains(basepath))
                                {
                                    dest.Add(values[i]);
                                    dest.Add(values[i + 1]);
                                }
                            }
                            //if (dest.Count > 0)
                            key.SetValue("PendingFileRenameOperations", dest.ToArray());
                            //else
                            //key.DeleteValue("PendingFileRenameOperations");
                        }
                    }
                }

                errorMessage = "";

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                errorMessage = "An error occurred cleaning up previous uninstall information to the registry. The program might be partially uninstalled on the next reboot.";
                return false;
            }
        }
    }
}
