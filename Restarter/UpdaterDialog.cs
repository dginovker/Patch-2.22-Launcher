﻿using Helper;
using AutoUpdaterDotNET;
using System.Net;
using System.Net.Mime;
using System.Reflection;

namespace Restarter
{
    public partial class UpdaterDialog : Form
    {
        public UpdaterDialog()
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            InitializeComponent();
            BackgroundImage = Helper.Properties.Resources.UpdaterSplash;
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        private void UpdaterDialog_Shown(object sender, EventArgs e)
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            try
            {
                AutoUpdater.Start("https://ravo92.github.io/LauncherUpdater_Test.xml");
                AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
                AutoUpdater.Synchronous = true;
                AutoUpdater.Mandatory = true;
                AutoUpdater.UpdateMode = Mode.ForcedDownload;
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.ShowRemindLaterButton = false;
                AutoUpdater.LetUserSelectRemindLater = false;
                AutoUpdater.HttpUserAgent = "BFME Patch 2.22 Launcher Updater";
                AutoUpdater.AppTitle = "Patch 2.22 Launcher";
                AutoUpdater.RunUpdateAsAdmin = true;
                AutoUpdater.DownloadPath = Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME_LAUNCHER);
                AutoUpdater.ClearAppDirectory = false;
                AutoUpdater.ReportErrors = false;

                string jsonPath = Path.Combine(Environment.CurrentDirectory, "AutoUpdaterSettings.json");
                AutoUpdater.PersistenceProvider = new JsonFilePersistenceProvider(jsonPath);
            }
            catch (Exception ex)
            {
                LogHelper.LoggerRestarter.Error(ex, "");
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            if (args.Error == null)
            {
                if (args.IsUpdateAvailable)
                {
                    try
                    {
                        if (AutoUpdater.DownloadUpdate(args))
                        {
                            UpdateIsDownloaded.LauncherUpdateIsDownloaded = true;
                            Close();
                        }
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }

                }
                else
                {
                    Thread.Sleep(1000);
                    Close();
                }
            }
            else
            {
                if (args.Error is WebException)
                {
                    MessageBox.Show(
                        @"There is a problem reaching update server. Please check your internet connection and try again later.",
                        @"Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(args.Error.Message,
                        args.Error.GetType().ToString(), MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }
    }
}