using Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace PatchLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args[0].ToString() != "--official")
            {
                Process _process = new();
                _process.StartInfo.FileName = "Updater.exe";
                _process.StartInfo.WorkingDirectory = Application.StartupPath;
                _process.Start();

                Application.Exit();
            }
            else
            {
                ApplicationConfiguration.Initialize();

                if (RegistryService.ReadRegKey("path") == "ValueNotFound" || !Directory.Exists(RegistryService.ReadRegKey("path")))
                {
                    Properties.Settings.Default.IsGameInstalled = false;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.IsGameInstalled = true;
                    Properties.Settings.Default.GameInstallPath = RegistryService.ReadRegKey("path");
                    Properties.Settings.Default.Save();
                }

                Application.Run(new BFME1());
            }
        }
    }
}