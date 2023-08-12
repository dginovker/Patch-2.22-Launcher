using Helper;
using AutoUpdaterDotNET;
using System.Net;

namespace Restarter
{
    public partial class UpdaterDialog : Form
    {
        public UpdaterDialog()
        {
            Console.WriteLine("Grabbing UpdaterDialog..");
            InitializeComponent();
            BackgroundImage = Helper.Properties.Resources.UpdaterSplash;
        }

        private void UpdaterDialog_Shown(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Calling UpdaterDialog_Shown...");
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
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            Console.WriteLine("AutoUpdaterOnCheckForUpdateEvent...");
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
        }
    }
}