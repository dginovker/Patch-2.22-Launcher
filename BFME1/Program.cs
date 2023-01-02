using Helper;
using PatchLauncher.Properties;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace PatchLauncher
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            string configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            if (!File.Exists(configPath))
            {
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                Settings.Default.Save();
            }

            try {
              using Mutex mutex = new(false, Process.GetCurrentProcess().ProcessName);
              if (!mutex.WaitOne(0, false))
              {
                  MessageBox.Show("Launcher is already running! Please Check TrayIcon or Taskmanager first.", "Launcher already running!");
                  return;
              }
            } catch (Exception ex) {
              MessageBox.Show("Error: " + ex.Message, "Error");
              // Log the exception
              Console.WriteLine(ex.Message);
            }

            if (RegistryService.ReadRegKey("path") == "ValueNotFound" || !Directory.Exists(RegistryService.ReadRegKey("path")))
            {
                Settings.Default.IsGameInstalled = false;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.IsGameInstalled = true;
                Settings.Default.GameInstallPath = RegistryService.ReadRegKey("path");
                Settings.Default.Save();
            }
            Application.Run(new BFME1());
        }
    }
}