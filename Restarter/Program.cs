﻿using System.Diagnostics;
using System.Reflection;
using System.Text;
using Helper;
using System.IO;

namespace Restarter
{
    internal class Program
    {
        static readonly Mutex _mutex = new(true, ConstStrings.C_MUTEX_NAME_RESTARTER);
        static readonly string programPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        static void Main(string[] args)
        {
            File.AppendAllText("log.txt", "Main started" + Environment.NewLine);
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                if (_mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Application.Run(new UpdaterDialog());

                    if (UpdateIsDownloaded.LauncherUpdateIsDownloaded)
                        return;
                }
                else
                {
                    File.AppendAllText("log.txt", $"aborting {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
                    Application.Exit();
                }

                if (args.Length == 0)
                {
                    if (GetLastSelectedGameLauncher() == 1)
                    {
                        StartBFME1Launcher("--official");
                    }

                    else if (GetLastSelectedGameLauncher() == 2)
                    {
                        StartBFME2Launcher("--official");
                    }

                    else if (GetLastSelectedGameLauncher() == 25)
                    {
                        StartBFME25Launcher("--official");
                    }
                }
                else
                {
                    switch (args[0])
                    {
                        case "--restart":

                            if (args[1] is not null)
                            {
                                if (args[1] == "--BFME1Launcher")
                                {
                                    SetLastSelectedGameLauncher(1);
                                    StartBFME1Launcher("--official");
                                }

                                if (args[1] == "--BFME2Launcher")
                                {
                                    SetLastSelectedGameLauncher(2);
                                    StartBFME2Launcher("--official");
                                }

                                if (args[1] == "--BFME25Launcher")
                                {
                                    SetLastSelectedGameLauncher(25);
                                    StartBFME25Launcher("--official");
                                }
                            }
                            break;

                        case "--showLauncherUpdateLog":

                            if (GetLastSelectedGameLauncher() == 1)
                            {
                                SetLastSelectedGameLauncher(1);
                                StartBFME1Launcher("--showLauncherUpdateLog");
                            }

                            else if (GetLastSelectedGameLauncher() == 2)
                            {
                                SetLastSelectedGameLauncher(2);
                                StartBFME2Launcher("--showLauncherUpdateLog");
                            }

                            else if (GetLastSelectedGameLauncher() == 25)
                            {
                                SetLastSelectedGameLauncher(25);
                                StartBFME25Launcher("--showLauncherUpdateLog");
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        private static void StartBFME1Launcher(string argument)
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                Thread.Sleep(1000);
                Process _restarterProcess = new();
                _restarterProcess.StartInfo.FileName = ConstStrings.C_LAUNCHEREXE_BFME1_FILENAME;
                _restarterProcess.StartInfo.WorkingDirectory = programPath;
                _restarterProcess.StartInfo.UseShellExecute = true;
                _restarterProcess.StartInfo.Arguments = argument;
                _restarterProcess.Start();
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        private static void StartBFME2Launcher(string argument)
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                Thread.Sleep(1000);
                Process _restarterProcess = new();
                _restarterProcess.StartInfo.FileName = ConstStrings.C_LAUNCHEREXE_BFME2_FILENAME;
                _restarterProcess.StartInfo.WorkingDirectory = programPath;
                _restarterProcess.StartInfo.UseShellExecute = true;
                _restarterProcess.StartInfo.Arguments = argument;
                _restarterProcess.Start();
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        private static void StartBFME25Launcher(string argument)
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                Thread.Sleep(1000);
                Process _restarterProcess = new();
                _restarterProcess.StartInfo.FileName = ConstStrings.C_LAUNCHEREXE_BFME25_FILENAME;
                _restarterProcess.StartInfo.WorkingDirectory = programPath;
                _restarterProcess.StartInfo.UseShellExecute = true;
                _restarterProcess.StartInfo.Arguments = argument;
                _restarterProcess.Start();
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        /// <summary>
        /// Selected Games:
        /// 1 = BFME1
        /// 2 = BFME2
        /// 25 = ROTWK
        /// </summary>
        /// <returns>Returns the selected Game Launcher as an integer value</returns>
        private static int GetLastSelectedGameLauncher()
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                if (File.Exists(Path.Combine(programPath, ConstStrings.C_LAUNCHERSELECTEDINFOFILE)))
                {
                    int selectedGame = Convert.ToInt32(File.ReadAllText(@Path.Combine(programPath, ConstStrings.C_LAUNCHERSELECTEDINFOFILE), Encoding.UTF8));
                    return selectedGame;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            return 1;
        }

        private static void SetLastSelectedGameLauncher(int selectedGame)
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                File.WriteAllText(Path.Combine(programPath, ConstStrings.C_LAUNCHERSELECTEDINFOFILE), selectedGame.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }
    }
}