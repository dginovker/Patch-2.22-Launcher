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
          Console.WriteLine("What is the current line number?");
          // Write the current line number to a file named "line.txt"
          File.WriteAllText("line.txt", ("Hello World! "));
            Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
            ApplicationConfiguration.Initialize();
            Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());

            string configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
            if (!File.Exists(configPath))
            {
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                Settings.Default.Save();
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
            }

            try {
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
              using Mutex mutex = new(false, Process.GetCurrentProcess().ProcessName);
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
              if (!mutex.WaitOne(0, false))
              {
                  Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
                  MessageBox.Show("Launcher is already running! Please Check TrayIcon or Taskmanager first.", "Launcher already running!");
                  return;
              }
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
            } catch (Exception ex) {
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
              MessageBox.Show("Error: " + ex.Message, "Error");
              // Log the exception
              Console.WriteLine(ex.Message);
            }

              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
            if (RegistryService.ReadRegKey("path") == "ValueNotFound" || !Directory.Exists(RegistryService.ReadRegKey("path")))
            {
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
                Settings.Default.IsGameInstalled = false;
                Settings.Default.Save();
            }
            else
            {
                Settings.Default.IsGameInstalled = true;
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
                Settings.Default.GameInstallPath = RegistryService.ReadRegKey("path");
                Settings.Default.Save();
            }
            Application.Run(new BFME1());
              Console.WriteLine((new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber());
        }
    }
}