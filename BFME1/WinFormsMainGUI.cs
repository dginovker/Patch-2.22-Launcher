using AutoUpdaterDotNET;
using Downloader;
using Helper;
using Microsoft.Web.WebView2.Core;
using PatchLauncher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace PatchLauncher
{
    public partial class WinFormsMainGUI : Form
    {
        int iconNumber = Settings.Default.BackgroundMusicIcon;
        readonly SoundPlayerHelper _soundPlayerHelper = new();

        readonly string gameISOPath = Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, ConstStrings.C_MAINGAMEFILE_ZIP);

        LanguageSettings _languageSettings = InstallLanguageList._DictionarylanguageSettings[Settings.Default.InstalledLanguageISOCode];

        public WinFormsMainGUI()
        {
            #region logic

            InitializeComponent();
            InitializeWebView2Settings();

            SysTray.ContextMenuStrip = NotifyContextMenu;

            try
            {
                if (!Directory.Exists(ConstStrings.GameAppdataFolderPath()))
                    Directory.CreateDirectory(ConstStrings.GameAppdataFolderPath());

                if (!File.Exists(Path.Combine(ConstStrings.GameAppdataFolderPath(), ConstStrings.C_OPTIONSINI_FILENAME)))
                    File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_OPTIONSINI_FILENAME), Path.Combine(ConstStrings.GameAppdataFolderPath(), ConstStrings.C_OPTIONSINI_FILENAME));

                XMLFileHelper.GetXMLFileData(true);
                XMLFileHelper.GetXMLFileData(false);

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }

            BtnInstall.Text = Strings.BtnInstall_TextLaunch;

            #endregion

            #region Styles
            //Main Form style behaviour

            PibLoadingRing.Show();
            PibLoadingBorder.Show();
            PiBArrow.Hide();
            LblPatchNotes.Show();
            PnlPlaceholder.Hide();
            Wv2Patchnotes.Hide();

            PiBVersion222_33.Show();
            PiBVersion222_34.Show();

            // label-Styles
            LblDownloadSpeed.Font = FontHelper.GetFont(0, 16); ;
            LblDownloadSpeed.ForeColor = Color.FromArgb(192, 145, 69);
            LblDownloadSpeed.BackColor = Color.Transparent;

            LblFileName.Font = FontHelper.GetFont(0, 16); ;
            LblFileName.ForeColor = Color.FromArgb(192, 145, 69);
            LblFileName.BackColor = Color.Transparent;

            LblPatchNotes.Font = FontHelper.GetFont(0, 16);
            LblPatchNotes.ForeColor = Color.FromArgb(192, 145, 69);
            LblPatchNotes.BackColor = Color.Transparent;
            LblPatchNotes.BorderStyle = BorderStyle.None;

            LblInstalledPatches.Font = FontHelper.GetFont(1, 24);
            LblInstalledPatches.ForeColor = Color.FromArgb(192, 145, 69);
            LblInstalledPatches.BackColor = Color.Transparent;
            LblInstalledPatches.BorderStyle = BorderStyle.None;
            LblInstalledPatches.OutlineWidth = 6;

            LblModExplanation.Font = FontHelper.GetFont(0, 20);
            LblModExplanation.ForeColor = Color.FromArgb(192, 145, 69);
            LblModExplanation.BackColor = Color.Transparent;
            LblModExplanation.BorderStyle = BorderStyle.None;
            LblModExplanation.OutlineWidth = 6;

            PBarActualFile.ForeColor = Color.FromArgb(192, 145, 69);
            PBarActualFile.BackColor = Color.FromArgb(255, 100, 0);

            BtnInstall.FlatAppearance.BorderSize = 0;
            BtnInstall.FlatStyle = FlatStyle.Flat;
            BtnInstall.BackColor = Color.Transparent;
            BtnInstall.BackgroundImage = ConstStrings.C_BUTTONIMAGE_NEUTR;
            BtnInstall.Font = FontHelper.GetFont(0, 16); ;
            BtnInstall.ForeColor = Color.FromArgb(192, 145, 69);

            #endregion

            #region Tooltips
            //Tooltips
            ToolTip.SetToolTip(PiBThemeSwitcher, Strings.ToolTip_MusicSwitcher);
            #endregion

            #region HUD Elements
            PibHeader.Image = Helper.Properties.Resources.header;
            PiBYoutube.Image = Helper.Properties.Resources.youtube;
            PiBDiscord.Image = Helper.Properties.Resources.discord;
            PiBModDB.Image = Helper.Properties.Resources.moddb;
            PiBTwitch.Image = Helper.Properties.Resources.twitch;

            if (Settings.Default.IsPatchModsShown)
                PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft;

            PibHeader.Image = Helper.Properties.Resources.header;
            PibLoadingBorder.Image = Helper.Properties.Resources.loadingBorder;
            PibLoadingRing.Image = Helper.Properties.Resources.loadingRing;

            PiBVersion103.Image = Helper.Properties.Resources.BtnPatchSelection_103;

            if (Settings.Default.PlayBackgroundMusic)
                PibMute.Image = Helper.Properties.Resources.Unmute;
            else
                PibMute.Image = Helper.Properties.Resources.Mute;


            if (Settings.Default.IsPatch106Installed)
                PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106_Selected;
            else
                PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;


            if (Settings.Default.IsPatch33Installed)
                PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Selected;
            else
                PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;


            if (Settings.Default.IsPatch34Installed)
                PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Selected;
            else
                PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;

            if (Settings.Default.IsPatch34Installed)
                Settings.Default.PatchVersionInstalled = 34;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (Settings.Default.BackgroundMusicIcon == 0)
            {
                PiBThemeSwitcher.Image = Helper.Properties.Resources.icoDefault;
                BackgroundImage = Helper.Properties.Resources.bgDefault;
            }
            else if (Settings.Default.BackgroundMusicIcon == 1)
            {
                PiBThemeSwitcher.Image = Helper.Properties.Resources.icoGondor;
                BackgroundImage = Helper.Properties.Resources.bgGondor;
            }
            else if (Settings.Default.BackgroundMusicIcon == 2)
            {
                PiBThemeSwitcher.Image = Helper.Properties.Resources.icoRohan;
                BackgroundImage = Helper.Properties.Resources.bgRohan;
            }
            else if (Settings.Default.BackgroundMusicIcon == 3)
            {
                PiBThemeSwitcher.Image = Helper.Properties.Resources.icoIsengard;
                BackgroundImage = Helper.Properties.Resources.bgIsengard;
            }
            else if (Settings.Default.BackgroundMusicIcon == 4)
            {
                PiBThemeSwitcher.Image = Helper.Properties.Resources.icoMordor;
                BackgroundImage = Helper.Properties.Resources.bgMordor;
            }
            #endregion

            TmrPatchNotes.Tick += new EventHandler(TmrPatchNotes_Tick);
            TmrPatchNotes.Interval = 2000;
            TmrPatchNotes.Start();
        }

        #region Launcher Auto-Updater

        private static async void InitializeWebView2Settings()
        {
            try
            {
                File.WriteAllText(Path.Combine(Application.StartupPath, ConstStrings.C_LOGFOLDER_NAME, "webView2_Version.log"), CoreWebView2Environment.GetAvailableBrowserVersionString());
            }
            catch (WebView2RuntimeNotFoundException)
            {
                await RunWebViewSilentSetupAsync(Path.Combine(Application.StartupPath, ConstStrings.C_TOOLFOLDER_NAME, "MicrosoftEdgeWebview2Setup.exe"));
            }
        }

        public static async Task RunWebViewSilentSetupAsync(string fileName)
        {
            Process _p = Process.Start(fileName, new[] { "/silent", "/install" });
            await _p.WaitForExitAsync().ConfigureAwait(false);
        }

        public static void CheckForUpdates()
        {
            try
            {
                AutoUpdater.Start("https://ravo92.github.io/LauncherUpdater.xml");
                AutoUpdater.InstalledVersion = Assembly.GetEntryAssembly()!.GetName().Version;
                AutoUpdater.ShowSkipButton = false;
                AutoUpdater.ShowRemindLaterButton = true;
                AutoUpdater.LetUserSelectRemindLater = false;
                AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
                AutoUpdater.RemindLaterAt = 10;
                AutoUpdater.UpdateFormSize = new Size(1296, 759);
                AutoUpdater.HttpUserAgent = "BFME Launcher Update";
                AutoUpdater.AppTitle = Application.ProductName;
                AutoUpdater.RunUpdateAsAdmin = true;
                AutoUpdater.DownloadPath = Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME);
                AutoUpdater.ClearAppDirectory = false;
                AutoUpdater.ReportErrors = false;

                string jsonPath = Path.Combine(Environment.CurrentDirectory, "AutoUpdaterSettings.json");
                AutoUpdater.PersistenceProvider = new JsonFilePersistenceProvider(jsonPath);
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        #endregion

        #region Button Behaviours

        public void GameisClosed(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                    SysTray.Visible = false;
                    PiBVersion103.Enabled = true;
                    PiBVersion106.Enabled = true;
                    PiBVersion222_33.Enabled = true;
                    PiBVersion222_34.Enabled = true;
                }));
            }

            if (Settings.Default.PlayBackgroundMusic)
            {
                _soundPlayerHelper.PlayTheme(Settings.Default.BackgroundMusicFile);
            }
        }

        private async void BtnInstall_Click(object sender, EventArgs e)
        {
            if (Settings.Default.IsGameInstalled == false)
            {
                InstallPathDialog _install = new();

                DialogResult dr = _install.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    await InstallRoutine(false);
                }
            }
            else
            {
                LaunchGameToolStripMenuItem.PerformClick();
            }
        }

        private void BtnInstall_MouseLeave(object sender, EventArgs e)
        {
            BtnInstall.BackgroundImage = ConstStrings.C_BUTTONIMAGE_NEUTR;
            BtnInstall.ForeColor = Color.FromArgb(192, 145, 69);
        }

        private void BtnInstall_MouseEnter(object sender, EventArgs e)
        {
            BtnInstall.BackgroundImage = ConstStrings.C_BUTTONIMAGE_HOVER;
            BtnInstall.ForeColor = Color.FromArgb(100, 53, 5);
            Task.Run(() => SoundPlayerHelper.PlaySoundHover());
        }

        private void BtnInstall_MouseDown(object sender, MouseEventArgs e)
        {
            BtnInstall.BackgroundImage = ConstStrings.C_BUTTONIMAGE_CLICK;
            BtnInstall.ForeColor = Color.FromArgb(192, 145, 69);
            Task.Run(() => SoundPlayerHelper.PlaySoundClick());
        }

        private void PiBYoutube_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.youtube.com/BeyondStandards") { UseShellExecute = true });
        }

        private void PiBDiscord_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://discord.gg/Q5Yyy3XCuu") { UseShellExecute = true });
        }

        private void PiBModDB_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.moddb.com/mods/battle-for-middle-earth-patch-222") { UseShellExecute = true });
        }

        private void PiBTwitch_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.twitch.tv/beyondstandards") { UseShellExecute = true });
        }

        private void PibMute_Click(object sender, EventArgs e)
        {
            if (Settings.Default.PlayBackgroundMusic)
            {
                PibMute.Image = Helper.Properties.Resources.Mute;
                Settings.Default.PlayBackgroundMusic = false;
                _soundPlayerHelper.StopTheme();
                Settings.Default.Save();
            }
            else
            {
                PibMute.Image = Helper.Properties.Resources.Unmute;
                Settings.Default.PlayBackgroundMusic = true;
                Settings.Default.Save();
                _soundPlayerHelper.PlayTheme(Settings.Default.BackgroundMusicFile);
            }
        }

        private void PiBThemeSwitcher_Click(object sender, EventArgs e)
        {
            iconNumber++;
            if (iconNumber >= 5)
                iconNumber = 0;

            switch (iconNumber)
            {
                case 0:
                    {
                        Settings.Default.BackgroundMusicFile = ConstStrings.C_THEMESOUND_DEFAULT;
                        Settings.Default.BackgroundMusicIcon = 0;
                        Settings.Default.Save();
                        PiBThemeSwitcher.Image = Helper.Properties.Resources.icoDefault;
                        BackgroundImage = Helper.Properties.Resources.bgDefault;

                        if (Settings.Default.PlayBackgroundMusic == true)
                        {
                            _soundPlayerHelper.PlayTheme(ConstStrings.C_THEMESOUND_DEFAULT);
                        }

                        break;
                    }
                case 1:
                    {
                        Settings.Default.BackgroundMusicFile = ConstStrings.C_THEMESOUND_GONDOR;
                        Settings.Default.BackgroundMusicIcon = 1;
                        Settings.Default.Save();
                        PiBThemeSwitcher.Image = Helper.Properties.Resources.icoGondor;
                        BackgroundImage = Helper.Properties.Resources.bgGondor;

                        if (Settings.Default.PlayBackgroundMusic == true)
                        {
                            _soundPlayerHelper.PlayTheme(ConstStrings.C_THEMESOUND_GONDOR);
                        }
                        break;
                    }
                case 2:
                    {
                        Settings.Default.BackgroundMusicFile = ConstStrings.C_THEMESOUND_ROHAN;
                        Settings.Default.BackgroundMusicIcon = 2;
                        Settings.Default.Save();
                        PiBThemeSwitcher.Image = Helper.Properties.Resources.icoRohan;
                        BackgroundImage = Helper.Properties.Resources.bgRohan;

                        if (Settings.Default.PlayBackgroundMusic == true)
                        {
                            _soundPlayerHelper.PlayTheme(ConstStrings.C_THEMESOUND_ROHAN);
                        }
                        break;
                    }
                case 3:
                    {
                        Settings.Default.BackgroundMusicFile = ConstStrings.C_THEMESOUND_ISENGARD;
                        Settings.Default.BackgroundMusicIcon = 3;
                        Settings.Default.Save();
                        PiBThemeSwitcher.Image = Helper.Properties.Resources.icoIsengard;
                        BackgroundImage = Helper.Properties.Resources.bgIsengard;

                        if (Settings.Default.PlayBackgroundMusic == true)
                        {
                            _soundPlayerHelper.PlayTheme(ConstStrings.C_THEMESOUND_ISENGARD);
                        }
                        break;
                    }
                case 4:
                    {
                        Settings.Default.BackgroundMusicFile = ConstStrings.C_THEMESOUND_MORDOR;
                        Settings.Default.BackgroundMusicIcon = 4;
                        Settings.Default.Save();
                        PiBThemeSwitcher.Image = Helper.Properties.Resources.icoMordor;
                        BackgroundImage = Helper.Properties.Resources.bgMordor;

                        if (Settings.Default.PlayBackgroundMusic == true)
                        {
                            _soundPlayerHelper.PlayTheme(ConstStrings.C_THEMESOUND_MORDOR);
                        }
                        break;
                    }
            }
        }

        private void PiBVersion103_Click(object sender, EventArgs e)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                PiBVersion103.Enabled = false;
                PiBVersion106.Enabled = false;

                PiBVersion222_33.Enabled = false;
                PiBVersion222_34.Enabled = false;

                if (PatchModDetectionHelper.DetectPatch106())
                {
                    PatchModDetectionHelper.DeletePatch106();
                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;
                }

                PatchModDetectionHelper.DeletePatch222Files();

                Settings.Default.PatchVersionInstalled = 103;

                Settings.Default.IsPatch106Installed = false;
                Settings.Default.IsPatch33Installed = false;
                Settings.Default.IsPatch34Installed = false;


                if (Settings.Default.IsPatch106Downloaded)
                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;
                else
                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106_Download;


                if (Settings.Default.IsPatch33Downloaded)
                    PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;
                else
                    PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Download;


                if (Settings.Default.IsPatch34Downloaded)
                    PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;
                else
                    PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Download;

                PiBVersion103.Enabled = true;
                PiBVersion106.Enabled = true;

                PiBVersion222_33.Enabled = true;
                PiBVersion222_34.Enabled = true;

                Settings.Default.Save();

                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        private async void PiBVersion106_Click(object sender, EventArgs e)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                PiBVersion103.Enabled = false;
                PiBVersion106.Enabled = false;

                PiBVersion222_33.Enabled = false;
                PiBVersion222_34.Enabled = false;

                if (!Settings.Default.IsPatch106Downloaded)
                {
                    PBarActualFile.Show();
                    LblDownloadSpeed.Show();
                    LblFileName.Show();

                    await UpdateRoutine(ConstStrings.C_PATCHZIP06_NAME, "https://dl.dropboxusercontent.com/s/0j4u35hetr3if5j/Patch_1.06.7z");

                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106_Selected;

                    Settings.Default.PatchVersionInstalled = 106;
                    Settings.Default.IsPatch106Downloaded = true;
                    Settings.Default.IsPatch106Installed = true;

                    Settings.Default.IsPatch33Installed = false;
                    Settings.Default.IsPatch34Installed = false;


                    if (Settings.Default.IsPatch33Downloaded)
                        PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;
                    else
                        PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Download;


                    if (Settings.Default.IsPatch34Downloaded)
                        PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;
                    else
                        PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Download;
                }
                else
                {
                    if (Settings.Default.IsPatch106Installed)
                    {
                        Settings.Default.PatchVersionInstalled = 103;
                        Settings.Default.IsPatch106Installed = false;
                        PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;
                        PatchModDetectionHelper.DeletePatch222Files();
                        PatchModDetectionHelper.DeletePatch106();
                    }
                    else
                    {
                        PBarActualFile.Show();
                        LblDownloadSpeed.Show();
                        LblFileName.Show();

                        await UpdateRoutine(ConstStrings.C_PATCHZIP06_NAME, "https://dl.dropboxusercontent.com/s/0j4u35hetr3if5j/Patch_1.06.7z");

                        PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106_Selected;

                        Settings.Default.PatchVersionInstalled = 106;
                        Settings.Default.IsPatch106Downloaded = true;
                        Settings.Default.IsPatch106Installed = true;

                        Settings.Default.IsPatch33Installed = false;
                        Settings.Default.IsPatch34Installed = false;


                        if (Settings.Default.IsPatch33Downloaded)
                            PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;
                        else
                            PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Download;


                        if (Settings.Default.IsPatch34Downloaded)
                            PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;
                        else
                            PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Download;
                    }
                }

                PiBVersion103.Enabled = true;
                PiBVersion106.Enabled = true;

                PiBVersion222_33.Enabled = true;
                PiBVersion222_34.Enabled = true;

                Settings.Default.Save();
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        private async void PiBVersion222_33_Click(object sender, EventArgs e)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                PiBVersion103.Enabled = false;
                PiBVersion106.Enabled = false;

                PiBVersion222_33.Enabled = false;
                PiBVersion222_34.Enabled = false;

                if (!Settings.Default.IsPatch33Downloaded)
                {
                    PBarActualFile.Show();
                    LblDownloadSpeed.Show();
                    LblFileName.Show();

                    await UpdateRoutine(ConstStrings.C_PATCHZIP33_NAME, "https://www.dropbox.com/scl/fi/0oc0ebbqk2ps1cb3qws46/Patch_2.22v33.7z?dl=1&rlkey=0zcwxhphd8smtvjobsgh0rbs9");

                    PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Selected;
                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;

                    if (Settings.Default.InstalledLanguageISOCode == "de")
                    {
                        File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                    }

                    Settings.Default.PatchVersionInstalled = 33;
                    Settings.Default.IsPatch33Downloaded = true;
                    Settings.Default.IsPatch33Installed = true;

                    Settings.Default.IsPatch106Installed = false;
                    Settings.Default.IsPatch34Installed = false;


                    if (Settings.Default.IsPatch34Downloaded)
                        PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;
                    else
                        PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Download;
                }
                else
                {
                    if (Settings.Default.IsPatch33Installed)
                    {
                        Settings.Default.PatchVersionInstalled = 103;
                        Settings.Default.IsPatch33Installed = false;
                        PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;
                        PatchModDetectionHelper.DeletePatch222Files();
                        PatchModDetectionHelper.DeletePatch106();
                    }
                    else
                    {
                        PBarActualFile.Show();
                        LblDownloadSpeed.Show();
                        LblFileName.Show();

                        await UpdateRoutine(ConstStrings.C_PATCHZIP33_NAME, "https://www.dropbox.com/scl/fi/0oc0ebbqk2ps1cb3qws46/Patch_2.22v33.7z?dl=1&rlkey=0zcwxhphd8smtvjobsgh0rbs9");

                        if (Settings.Default.InstalledLanguageISOCode == "de")
                        {
                            File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                        }

                        PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Selected;
                        PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;

                        Settings.Default.PatchVersionInstalled = 33;
                        Settings.Default.IsPatch33Downloaded = true;
                        Settings.Default.IsPatch33Installed = true;

                        Settings.Default.IsPatch106Installed = false;
                        Settings.Default.IsPatch34Installed = false;


                        if (Settings.Default.IsPatch34Downloaded)
                            PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;
                        else
                            PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Download;
                    }
                }

                PiBVersion103.Enabled = true;
                PiBVersion106.Enabled = true;

                PiBVersion222_33.Enabled = true;
                PiBVersion222_34.Enabled = true;

                Settings.Default.Save();
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        private async void PiBVersion222_34_Click(object sender, EventArgs e)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                PiBVersion103.Enabled = false;
                PiBVersion106.Enabled = false;

                PiBVersion222_33.Enabled = false;
                PiBVersion222_34.Enabled = false;

                if (!Settings.Default.IsPatch34Downloaded)
                {
                    PBarActualFile.Show();
                    LblDownloadSpeed.Show();
                    LblFileName.Show();

                    await UpdateRoutine(ConstStrings.C_PATCHZIP34_NAME, "https://www.dropbox.com/scl/fi/uyuh8jl936z7t9l6vf4jb/Patch_2.22v34.7z?dl=1&rlkey=94y4ifn40pb78djvihmewoib3");

                    PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Selected;
                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;

                    if (Settings.Default.InstalledLanguageISOCode == "de")
                    {
                        File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                    }

                    Settings.Default.PatchVersionInstalled = 34;
                    Settings.Default.IsPatch34Downloaded = true;
                    Settings.Default.IsPatch34Installed = true;

                    Settings.Default.IsPatch106Installed = false;
                    Settings.Default.IsPatch33Installed = false;


                    if (Settings.Default.IsPatch33Downloaded)
                        PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;
                    else
                        PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Download;
                }
                else
                {
                    if (Settings.Default.IsPatch34Installed)
                    {
                        Settings.Default.PatchVersionInstalled = 103;
                        Settings.Default.IsPatch34Installed = false;
                        PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34;
                        PatchModDetectionHelper.DeletePatch222Files();
                        PatchModDetectionHelper.DeletePatch106();
                    }
                    else
                    {
                        PBarActualFile.Show();
                        LblDownloadSpeed.Show();
                        LblFileName.Show();

                        await UpdateRoutine(ConstStrings.C_PATCHZIP34_NAME, "https://www.dropbox.com/scl/fi/uyuh8jl936z7t9l6vf4jb/Patch_2.22v34.7z?dl=1&rlkey=94y4ifn40pb78djvihmewoib3");

                        if (Settings.Default.InstalledLanguageISOCode == "de")
                        {
                            File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                        }

                        PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Selected;
                        PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106;

                        Settings.Default.PatchVersionInstalled = 34;
                        Settings.Default.IsPatch34Downloaded = true;
                        Settings.Default.IsPatch34Installed = true;

                        Settings.Default.IsPatch106Installed = false;
                        Settings.Default.IsPatch33Installed = false;


                        if (Settings.Default.IsPatch33Downloaded)
                            PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33;
                        else
                            PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Download;
                    }
                }

                PiBVersion103.Enabled = true;
                PiBVersion106.Enabled = true;

                PiBVersion222_33.Enabled = true;
                PiBVersion222_34.Enabled = true;

                Settings.Default.Save();
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        private void PiBArrow_Click(object sender, EventArgs e)
        {
            TmrAnimation.Enabled = true;
        }

        #endregion

        #region ToolTip System
        public void Tooltip_Draw(object sender, DrawToolTipEventArgs e)
        {
            Font tooltipFont = FontHelper.GetFont(0, 16); ;
            e.DrawBackground();
            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, tooltipFont, Brushes.SandyBrown, new PointF(2, 2));
        }

        public void TooltipPopup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = TextRenderer.MeasureText(ToolTip.GetToolTip(e.AssociatedControl), FontHelper.GetFont(0, 16));
        }
        #endregion

        #region Update

        public async Task UpdateRoutine(string ZIPFileName, string DownloadUrl)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                LaunchGameToolStripMenuItem.Enabled = false;
                OptionsToolStripMenuItem.Enabled = false;
                RepairGameToolStripMenuItem.Enabled = false;

                PiBArrow.Enabled = false;

                if (Settings.Default.IsPatchModsShown)
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft_Disabled;
                }
                else
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowRight_Disabled;
                }

                PBarActualFile.Show();

                LblDownloadSpeed.Show();
                LblFileName.Show();

                BtnInstall.Enabled = false;

                LblFileName.Text = Strings.Info_PreparingUpdate;

                PatchModDetectionHelper.DeletePatch222Files();
                PatchModDetectionHelper.DeletePatch106();

                Task download = DownloadUpdate(ZIPFileName, DownloadUrl);
                await download;

                Task extract = ExtractUpdate(ZIPFileName);
                await extract;

                PBarActualFile.Hide();

                LblDownloadSpeed.Hide();
                LblFileName.Hide();

                BtnInstall.Enabled = true;

                PiBArrow.Enabled = true;

                if (Settings.Default.IsPatchModsShown)
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft;
                    LblModExplanation.Show();
                }
                else
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowRight;
                    LblModExplanation.Hide();
                }

                Settings.Default.PatchVersionInstalled = XMLFileHelper.GetXMLFileVersion(false);
                Settings.Default.Save();

                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

                LaunchGameToolStripMenuItem.Enabled = true;
                OptionsToolStripMenuItem.Enabled = true;
                RepairGameToolStripMenuItem.Enabled = true;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        public async Task UpdateRoutineBeta()
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                int XmlVersion = XMLFileHelper.GetXMLFileVersion(true);

                PiBArrow.Enabled = false;

                if (Settings.Default.IsPatchModsShown)
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft_Disabled;
                }
                else
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowRight_Disabled;
                }

                PBarActualFile.Show();

                LblDownloadSpeed.Show();
                LblFileName.Show();

                BtnInstall.Enabled = false;

                LblFileName.Text = Strings.Info_PreparingUpdateBeta;

                if (XmlVersion > Settings.Default.BetaChannelVersion && Settings.Default.BetaChannelVersion > 0 && Directory.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + Settings.Default.BetaChannelVersion.ToString())))
                    Directory.Delete(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + Settings.Default.BetaChannelVersion.ToString()), true);

                Task download = DownloadUpdate(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_MAIN_ASSET_FILE), "https://dl.dropboxusercontent.com/s/zmze7c5asdlq44u/asset.dat");
                await download;

                Task download2 = DownloadUpdate(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_TEXTURES_PATCH_FILE), "https://dl.dropboxusercontent.com/s/ok5a5507fpwmge3/_patch222textures.big");
                await download2;

                Task download3 = DownloadUpdate(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_MAIN_PATCH_FILE), "https://dl.dropboxusercontent.com/s/gi431h0gnqgjfh3/_patch222.big");
                await download3;

                Task download4 = DownloadUpdate(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_BASES_PATCH_FILE), "https://dl.dropboxusercontent.com/s/rlshiuuiaalsu1t/_patch222bases.big");
                await download4;

                Task download5 = DownloadUpdate(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_MAPS_PATCH_FILE), "https://dl.dropboxusercontent.com/s/b4ubdfi8uuk181m/_patch222maps.big");
                await download5;

                Task download6 = DownloadUpdate(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_LIBRARIES_PATCH_FILE), "https://dl.dropboxusercontent.com/s/9027uabrxkx7z85/_patch222libraries.big");
                await download6;

                File.Copy(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_MAIN_ASSET_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_MAIN_ASSET_FILE), true);
                File.Copy(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_TEXTURES_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_TEXTURES_PATCH_FILE), true);
                File.Copy(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_MAIN_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_MAIN_PATCH_FILE), true);
                File.Copy(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_BASES_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_BASES_PATCH_FILE), true);
                File.Copy(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_LIBRARIES_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_LIBRARIES_PATCH_FILE), true);
                File.Copy(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ConstStrings.C_BETAFOLDER_NAME + XmlVersion.ToString(), ConstStrings.C_MAPS_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_MAPS_PATCH_FILE), true);

                PBarActualFile.Hide();

                LblDownloadSpeed.Hide();
                LblFileName.Hide();

                BtnInstall.Enabled = true;

                Settings.Default.BetaChannelVersion = XMLFileHelper.GetXMLFileVersion(true);
                Settings.Default.PatchVersionInstalled = XmlVersion + 1;
                Settings.Default.IsPatch34Installed = false;
                Settings.Default.IsPatch106Installed = false;
                Settings.Default.Save();

                if (Settings.Default.IsPatchModsShown)
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft;
                    LblModExplanation.Show();
                }
                else
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowRight;
                    LblModExplanation.Hide();
                }

                PiBArrow.Enabled = true;
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLine(ConstStrings.LogTime + ex.ToString());
            }
        }

        public async Task DownloadUpdate(string ZIPFileName, string DownloadUrl)
        {
            try
            {
                SetPBarFiles(0);
                SetPBarFilesMax(100);

                var downloadOpt = new DownloadConfiguration()
                {
                    ChunkCount = 1,
                    ParallelDownload = false,
                    ReserveStorageSpaceBeforeStartingDownload = true,
                    BufferBlockSize = 8000,
                    MaximumBytesPerSecond = 131072000,
                    ClearPackageOnCompletionWithFailure = true
                };

                var downloader = new DownloadService(downloadOpt);

                downloader.DownloadStarted += OnDownloadStarted;
                downloader.DownloadProgressChanged += OnDownloadProgressChanged;
                downloader.DownloadFileCompleted += OnDownloadFileCompleted;

                if (!File.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ZIPFileName)))
                {
                    await downloader.DownloadFileTaskAsync(DownloadUrl, Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME, ZIPFileName));
                }
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        public async Task ExtractUpdate(string ZIPFileName)
        {
            try
            {
                Invoke((MethodInvoker)(() => LblFileName.Text = Strings.Info_CopyFilesAndApplyPatch));

                var progressHandler = new Progress<ProgressHelper>(progress =>
                {
                    SetPBarFiles(progress.Count);
                    SetPBarFilesMax(progress.Max);
                    SetTextFileName(string.Concat(progress.Count, "/", progress.Max));
                    SetTextDlSpeed(progress.Filename!);
                });

                ZIPFileHelper _ZIPFileHelper = new();
                await _ZIPFileHelper.ExtractArchive(Path.Combine(ConstStrings.C_PATCHFOLDER_NAME, ZIPFileName), Settings.Default.GameInstallPath, progressHandler)!;

                Invoke((MethodInvoker)(() => PBarActualFile.Hide()));
                Invoke((MethodInvoker)(() => LblDownloadSpeed.Hide()));
                Invoke((MethodInvoker)(() => LblFileName.Hide()));
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        #endregion

        #region GameInstall

        public async Task InstallRoutine(bool onlyLanguagePack)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                if (!Directory.Exists(Settings.Default.GameInstallPath))
                {
                    Directory.CreateDirectory(Settings.Default.GameInstallPath);
                }

                if (Directory.Exists(Settings.Default.GameInstallPath) && Directory.GetFileSystemEntries(Settings.Default.GameInstallPath).Length != 0 && !onlyLanguagePack)
                {
                    DialogResult _dialogResult = MessageBox.Show(Strings.Msg_InstallFolderNotEmpty_Text, Strings.Msg_InstallFolderNotEmpty_Title, MessageBoxButtons.OKCancel);
                    if (_dialogResult == DialogResult.OK)
                    {
                        Directory.Delete(Settings.Default.GameInstallPath, true);
                    }
                    else if (_dialogResult == DialogResult.Cancel)
                    {
                        IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;
                        return;
                    }
                }

                _languageSettings = InstallLanguageList._DictionarylanguageSettings[Settings.Default.InstalledLanguageISOCode];

                BtnInstall.Text = Strings.BtnInstall_TextLaunch;
                BtnInstall.Enabled = false;

                PBarActualFile.Show();

                LblDownloadSpeed.Show();
                LblFileName.Show();

                PiBArrow.Enabled = false;
                PiBArrow.Image = Helper.Properties.Resources.btnArrowRight_Disabled;


                RegistryService.WriteRegKeysInstallation(Settings.Default.GameInstallPath, _languageSettings.RegistrySelectedLocale, _languageSettings.RegistrySelectedLanguageName, _languageSettings.RegistrySelectedLanguage);

                await DownloadGame(_languageSettings.RegistrySelectedLocale);
                await ExtractGame(onlyLanguagePack);

                if (!onlyLanguagePack)
                {
                    if (Settings.Default.CreateDesktopShortcut && !StartMenuHelper.DoesTheShortCutExist(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_LAUNCHER_SHORTCUT_NAME))
                    {
                        GameDesktopShortcutToolStripMenuItem.PerformClick();
                    }

                    if (Settings.Default.CreateStartMenuShortcut && !StartMenuHelper.DoesTheShortCutExist(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs\\Electronic Arts" + ConstStrings.C_GAMETITLE_NAME_EN), ConstStrings.C_GAMETITLE_NAME_EN))
                    {
                        GameStartmenuShortcutsToolStripMenuItem.PerformClick();
                    }
                }

                if (Settings.Default.UseBetaChannel == true)
                {
                    await UpdateRoutineBeta();
                }
                else
                {
                    await UpdateRoutine(ConstStrings.C_PATCHZIP34_NAME, "https://www.dropbox.com/scl/fi/uyuh8jl936z7t9l6vf4jb/Patch_2.22v34.7z?dl=1&rlkey=94y4ifn40pb78djvihmewoib3");

                    Settings.Default.IsGameInstalled = true;
                    Settings.Default.IsPatch34Downloaded = true;
                    Settings.Default.IsPatch34Installed = true;
                    Settings.Default.Save();

                    if (Settings.Default.InstalledLanguageISOCode == "de")
                    {
                        File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                    }

                    PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Selected;
                }


                LblDownloadSpeed.Hide();
                LblFileName.Hide();
                BtnInstall.Enabled = true;
                PBarActualFile.Hide();

                PiBArrow.Enabled = true;

                if (Settings.Default.IsPatchModsShown)
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft;
                }
                else
                {
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowRight;
                }

                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

                LaunchGameToolStripMenuItem.Enabled = true;
                OptionsToolStripMenuItem.Enabled = true;
                RepairGameToolStripMenuItem.Enabled = true;

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        public async Task DownloadGame(string isoLanguage)
        {
            try
            {
                string langPackPath = Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, _languageSettings.LanguagPackName);

                var downloadOpt = new DownloadConfiguration()
                {
                    ChunkCount = 1,
                    ParallelDownload = false,
                    ReserveStorageSpaceBeforeStartingDownload = true,
                    BufferBlockSize = 8000,
                    MaximumBytesPerSecond = 131072000,
                    ClearPackageOnCompletionWithFailure = true
                };

                var downloader = new DownloadService(downloadOpt);

                downloader.DownloadStarted += OnDownloadStarted;
                downloader.DownloadProgressChanged += OnDownloadProgressChanged;
                downloader.DownloadFileCompleted += OnDownloadFileCompleted;

                if (!File.Exists(gameISOPath))
                {
                    await downloader.DownloadFileTaskAsync(XMLFileHelper.GetXMLGameMainPackageURL(), gameISOPath);
                }
                else if (MD5Tools.CalculateMD5(gameISOPath) != ConstStrings.C_MAINGAMEFILE_ZIP_MD5_HASH)
                {
                    File.Delete(gameISOPath);
                    await downloader.DownloadFileTaskAsync(XMLFileHelper.GetXMLGameMainPackageURL(), gameISOPath);
                }

                if (!File.Exists(langPackPath))
                {
                    await downloader.DownloadFileTaskAsync(XMLFileHelper.GetXMLGameLanguagePackURL(isoLanguage), langPackPath);
                }
                else if (MD5Tools.CalculateMD5(langPackPath) != XMLFileHelper.GetXMLGameLanguageMD5Hash(isoLanguage))
                {
                    File.Delete(langPackPath);
                    await downloader.DownloadFileTaskAsync(XMLFileHelper.GetXMLGameLanguagePackURL(isoLanguage), langPackPath);
                }
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        public async Task ExtractGame(bool onlyLanguagePack)
        {
            try
            {
                SetPBarFilesMax(100);

                var progressHandler = new Progress<ProgressHelper>(progress =>
                {
                    SetPBarFiles(progress.Count);
                    SetPBarFilesMax(progress.Max);
                    SetPBarPercentages(progress.Filename!);
                    SetTextDlSpeed(string.Concat(progress.Count, "/", progress.Max));
                });

                var archiveFileNames = new List<string>();

                if (!onlyLanguagePack)
                {
                    archiveFileNames.Add(ConstStrings.C_MAINGAMEFILE_ZIP);
                }

                archiveFileNames.Add(_languageSettings.LanguagPackName);

                for (int i = 0; i < archiveFileNames.Count; i++)
                {
                    SetTextFileName($"Extracting {i + 1}/{archiveFileNames.Count}: {archiveFileNames[i]}");
                    ZIPFileHelper _ZIPFileHelper = new();
                    await _ZIPFileHelper.ExtractArchive(Path.Combine(ConstStrings.C_DOWNLOADFOLDER_NAME, archiveFileNames[i]), Settings.Default.GameInstallPath, progressHandler)!;
                }
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        private void OnDownloadStarted(object? sender, DownloadStartedEventArgs e)
        {
            SetPBarFiles(0);
            SetTextFileName("Downloading: " + Path.GetFileName(e.FileName));
        }

        private void OnDownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
        {
            SetPBarFiles((int)e.ProgressPercentage);
            SetTextDlSpeed("@ " + Math.Round(e.AverageBytesPerSecondSpeed / 1024000).ToString() + " MB/s");
            SetPBarPercentages(Math.Round(e.ProgressPercentage).ToString() + " %");
        }

        private void OnDownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            SetTextFileName(Strings.Info_PleaseWait);

            if (e.Error != null)
            {
                if (PBarActualFile is null)
                {
                    SetTextFileName(e.Error.Message);
                    using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                    file.WriteLineAsync(e.Error.Message);
                }
            }
            else
            {
                SetTextFileName(Strings.Info_PleaseWait);
                SetPBarFiles(100);
            }
        }
        #endregion

        #region Delegates
        delegate void SetTextDLSpeedCallback(string text);
        private void SetTextDlSpeed(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (LblDownloadSpeed.InvokeRequired)
            {
                SetTextDLSpeedCallback d = new(SetTextDlSpeed);
                Invoke(d, new object[] { text });
            }
            else
            {
                LblDownloadSpeed.Text = text;
            }
        }

        delegate void SetTextFileNameCallback(string text);
        private void SetTextFileName(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (LblFileName.InvokeRequired)
            {
                SetTextFileNameCallback d = new(SetTextFileName);
                Invoke(d, new object[] { text });
            }
            else
            {
                LblFileName.Text = text;
            }
        }

        delegate void SetPBarFilesCallback(int value);
        public void SetPBarFiles(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (PBarActualFile.InvokeRequired)
            {
                SetPBarFilesCallback d = new(SetPBarFiles);
                Invoke(d, new object[] { value });
            }
            else
            {
                PBarActualFile.Value = value;
            }
        }

        delegate void SetPBarFilesMaxCallback(int value);
        private void SetPBarFilesMax(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (PBarActualFile.InvokeRequired)
            {
                SetPBarFilesMaxCallback d = new(SetPBarFilesMax);
                Invoke(d, new object[] { value });
            }
            else
            {
                PBarActualFile.Maximum = value;
            }
        }

        delegate void SetPBarPercentagesCallback(string value);
        private void SetPBarPercentages(string value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (PBarActualFile.InvokeRequired)
            {
                SetPBarPercentagesCallback d = new(SetPBarPercentages);
                Invoke(d, new object[] { value });
            }
            else
            {
                PBarActualFile.CustomText = value;
            }
        }
        #endregion

        private void TmrPatchNotes_Tick(object? sender, EventArgs e)
        {
            TmrPatchNotes.Stop();
            PibLoadingRing.Hide();
            PibLoadingBorder.Hide();
            LblPatchNotes.Hide();

            PiBArrow.BackColor = Color.FromArgb(24, 24, 24);
            PiBArrow.Show();

            Wv2Patchnotes.Show();
            PnlPlaceholder.Show();

            if (Settings.Default.ShowPatchesFirst)
            {
                PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft;
                PiBArrow.BackColor = Color.Transparent;
                PnlPlaceholder.BackgroundImage = Helper.Properties.Resources.borderRectangleModPanel;
                PnlPlaceholder.BackColor = Color.Transparent;
                LblInstalledPatches.BackColor = Color.Transparent;
                LblModExplanation.BackColor = Color.Transparent;
                LblModExplanation.Show();
                LblInstalledPatches.Show();

                PiBArrow.Left = 1212;
                Wv2Patchnotes.Left = 1300;

                Settings.Default.IsPatchModsShown = true;
                Settings.Default.Save();
            }
            else
            {
                PnlPlaceholder.BackColor = Color.FromArgb(24, 24, 24);
                PnlPlaceholder.BackgroundImage = null;
                PiBArrow.Image = Helper.Properties.Resources.btnArrowRight;
                PiBArrow.BackColor = Color.FromArgb(24, 24, 24);
                LblModExplanation.Hide();
                LblInstalledPatches.Hide();

                PiBArrow.Left = 14;
                Wv2Patchnotes.Left = 12;

                Settings.Default.IsPatchModsShown = false;
                Settings.Default.Save();
            }
        }

        private void TmrAnimation_Tick(object sender, EventArgs e)
        {
            int _startLeft = 12;  // start position of the panel
            int _endLeft = 1300;      // end position of the panel
            int _endLeftArrow = 1212;
            int _stepSize = 5;     // pixels to move
            int _endRight = 12;      // end position of the panel

            // incrementally move

            if (Wv2Patchnotes.Left == _startLeft)
            {
                while (Wv2Patchnotes.Left != _endLeft)
                {
                    Wv2Patchnotes.Left += _stepSize;

                    if (PiBArrow.Left < _endLeftArrow)
                    {
                        PiBArrow.Left += _stepSize;
                    }
                    // make sure we didn't over shoot
                    if (Wv2Patchnotes.Left > _endLeft) Wv2Patchnotes.Left = _endLeft;

                    // have we arrived?
                    if (Wv2Patchnotes.Left == _endLeft)
                    {
                        TmrAnimation.Enabled = false;
                    }
                }

                PiBArrow.Left = _endLeftArrow;

                PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft;
                PiBArrow.BackColor = Color.Transparent;
                PnlPlaceholder.BackgroundImage = Helper.Properties.Resources.borderRectangleModPanel;
                PnlPlaceholder.BackColor = Color.Transparent;
                LblInstalledPatches.BackColor = Color.Transparent;
                LblModExplanation.BackColor = Color.Transparent;
                LblModExplanation.Show();
                LblInstalledPatches.Show();

                Wv2Patchnotes.Hide();

                Settings.Default.IsPatchModsShown = true;
                Settings.Default.Save();
            }
            else
            {
                PnlPlaceholder.BackColor = Color.FromArgb(24, 24, 24);
                PnlPlaceholder.BackgroundImage = null;
                PiBArrow.Image = Helper.Properties.Resources.btnArrowRight;
                PiBArrow.BackColor = Color.FromArgb(24, 24, 24);
                LblModExplanation.Hide();
                LblInstalledPatches.Hide();
                Wv2Patchnotes.Show();

                Settings.Default.IsPatchModsShown = false;
                Settings.Default.Save();

                while (Wv2Patchnotes.Left != _endRight)
                {
                    Wv2Patchnotes.Left -= _stepSize;

                    if (PiBArrow.Left > _endRight)
                    {
                        PiBArrow.Left -= _stepSize;
                    }
                    // make sure we didn't over shoot
                    if (Wv2Patchnotes.Left < _endRight) Wv2Patchnotes.Left = _endRight;

                    // have we arrived?
                    if (Wv2Patchnotes.Left == _endRight)
                    {
                        TmrAnimation.Enabled = false;
                    }
                }

                PiBArrow.Left = _endRight;
            }
        }

        private async void BFME1_Shown(object sender, EventArgs e)
        {
            try
            {
                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

                PiBArrow.Enabled = false;
                LaunchGameToolStripMenuItem.Enabled = false;
                OptionsToolStripMenuItem.Enabled = false;
                RepairGameToolStripMenuItem.Enabled = false;

                if (Settings.Default.IsPatchModsShown)
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowLeft_Disabled;
                else
                    PiBArrow.Image = Helper.Properties.Resources.btnArrowRight_Disabled;

                // Check Music Settings
                if (Settings.Default.PlayBackgroundMusic)
                {
                    _soundPlayerHelper.PlayTheme(Settings.Default.BackgroundMusicFile);
                }

                // Get Latest 2.22 Patch Version to the Launcher Settings. Save comes at the end of this function
                Settings.Default.LatestPatchVersion = XMLFileHelper.GetXMLFileVersion(false);

                // Check if Game is installed, if not show install button
                if ((Settings.Default.GameInstallPath == "" && !Directory.Exists(RegistryService.ReadRegKey("path"))) || RegistryService.ReadRegKey("path") == "ValueNotFound" || !File.Exists(Path.Combine(RegistryService.ReadRegKey("path"), ConstStrings.C_MAIN_GAME_FILE)))
                {
                    Settings.Default.IsGameInstalled = false;
                    BtnInstall.Text = Strings.BtnInstall_TextInstall;
                }
                // Check if new Update is available via XML file and Update to latest 2.22 Patch version OR Check if MD5 Hash matches the installed patch 2.22 version, if not -> Update; If Older patch is selected manually, dont Update!
                else if (XMLFileHelper.GetXMLFileVersion(false) > Settings.Default.PatchVersionInstalled && !Settings.Default.SelectedOlderPatch && !Settings.Default.UseBetaChannel)
                {
                    Settings.Default.IsPatch106Installed = false;
                    Settings.Default.IsPatch33Installed = false;
                    Settings.Default.IsPatch34Installed = false;

                    Settings.Default.IsGameInstalled = true;
                    Settings.Default.GameInstallPath = RegistryService.ReadRegKey("path");
                    Settings.Default.InstalledLanguageISOCode = ConstStrings.GameLanguage();

                    await UpdateRoutine(ConstStrings.C_PATCHZIP34_NAME, "https://www.dropbox.com/scl/fi/uyuh8jl936z7t9l6vf4jb/Patch_2.22v34.7z?dl=1&rlkey=94y4ifn40pb78djvihmewoib3");

                    if (Settings.Default.InstalledLanguageISOCode == "de")
                    {
                        File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                    }

                    Settings.Default.IsPatch34Downloaded = true;
                    Settings.Default.IsPatch34Installed = true;

                    PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Selected;
                }
                else
                {
                    Settings.Default.IsGameInstalled = true;
                    Settings.Default.GameInstallPath = RegistryService.ReadRegKey("path");
                    Settings.Default.InstalledLanguageISOCode = ConstStrings.GameLanguage();
                    PiBArrow.Enabled = true;
                }

                if (Settings.Default.UseBetaChannel)
                {
                    PiBVersion103.Hide();
                    PiBVersion106.Hide();
                    PiBVersion222_33.Hide();
                    PiBVersion222_34.Hide();

                    LblModExplanation.Text = Strings.Info_BetaActivated;

                    if (XMLFileHelper.GetXMLFileVersion(true) > Settings.Default.BetaChannelVersion)
                    {
                        await UpdateRoutineBeta();
                    }
                }


                if (!Settings.Default.IsPatch106Downloaded)
                    PiBVersion106.Image = Helper.Properties.Resources.BtnPatchSelection_106_Download;

                if (!Settings.Default.IsPatch33Downloaded)
                    PiBVersion222_33.Image = Helper.Properties.Resources.BtnPatchSelection_222V33_Download;

                if (!Settings.Default.IsPatch34Downloaded)
                    PiBVersion222_34.Image = Helper.Properties.Resources.BtnPatchSelection_222V34_Download;




                if (StartMenuHelper.DoesTheShortCutExist(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_GAMETITLE_NAME_EN))
                    GameDesktopShortcutToolStripMenuItem.Checked = true;
                else
                    GameDesktopShortcutToolStripMenuItem.Checked = false;

                if (StartMenuHelper.DoesTheShortCutExist(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN), ConstStrings.C_GAMETITLE_NAME_EN))
                    GameStartmenuShortcutsToolStripMenuItem.Checked = true;
                else
                    GameStartmenuShortcutsToolStripMenuItem.Checked = false;




                if (StartMenuHelper.DoesTheShortCutExist(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_LAUNCHER_SHORTCUT_NAME))
                    LauncherDesktopShortcutToolStripMenuItem.Checked = true;
                else
                    LauncherDesktopShortcutToolStripMenuItem.Checked = false;

                if (StartMenuHelper.DoesTheShortCutExist(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Patch 2.22 Launcher"), ConstStrings.C_LAUNCHER_SHORTCUT_NAME))
                    LauncherStartmenuShortcutToolStripMenuItem.Checked = true;
                else
                    LauncherStartmenuShortcutToolStripMenuItem.Checked = false;


                PiBArrow.Enabled = true;
                LaunchGameToolStripMenuItem.Enabled = true;
                OptionsToolStripMenuItem.Enabled = true;
                RepairGameToolStripMenuItem.Enabled = true;

                IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;

                Settings.Default.Save();

                if (!IsCurrentlyWorkingState.IsLauncherCurrentlyWorking)
                {
                    CheckForUpdates();
                }

            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ConstStrings.LogTime + ex.ToString());
            }
        }

        private void BFME1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsCurrentlyWorkingState.IsLauncherCurrentlyWorking)
            {
                MessageBox.Show(Strings.Warning_CantStopNow, Strings.Warning_CantStopNowTitle);
                e.Cancel = true;
            }

            if (Settings.Default.PatchVersionInstalled == 103 || Settings.Default.IsPatch106Installed || Settings.Default.IsPatch33Installed)
            {
                Settings.Default.SelectedOlderPatch = true;
            }
            else
            {
                Settings.Default.SelectedOlderPatch = false;
            }

            Settings.Default.Save();
        }

        private void BFME1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                SysTray.Visible = true;
                SysTray.ShowBalloonTip(2000);
                _soundPlayerHelper.StopTheme();
            }
        }

        private void SysTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            SysTray.Visible = false;

            if (Settings.Default.PlayBackgroundMusic)
            {
                _soundPlayerHelper.PlayTheme(Settings.Default.BackgroundMusicFile);
            }
        }

        private void LaunchGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PiBVersion103.Enabled = false;
            PiBVersion106.Enabled = false;
            PiBVersion222_33.Enabled = false;
            PiBVersion222_34.Enabled = false;

            Process _process = new();
            _process.StartInfo.FileName = Path.Combine(Settings.Default.GameInstallPath, ConstStrings.C_MAIN_GAME_FILE);

            // Start game windowed
            if (Settings.Default.StartGameWindowed)
            {
                _process.StartInfo.Arguments = "-win";
            }

            _process.StartInfo.WorkingDirectory = Settings.Default.GameInstallPath;
            _process.Start();

            WindowState = FormWindowState.Minimized;
            Hide();

            SysTray.Visible = true;
            SysTray.ShowBalloonTip(2000);
            _soundPlayerHelper.StopTheme();

            _process.WaitForExitAsync();
            _process.Exited += GameisClosed;
        }

        private void CloseTheLauncherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Wv2Patchnotes.Dispose();
            Application.Exit();
        }

        private void OpenSaveDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", ConstStrings.GameAppdataFolderPath() + "\\Save");
        }

        private void OpenMapDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", ConstStrings.GameAppdataFolderPath() + "\\Maps");
        }

        private void OpenReplayDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", ConstStrings.GameAppdataFolderPath() + "\\Replays");
        }

        private void OpenGameDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", ConstStrings.GameInstallPath());
        }

        private void OpenLauncherDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Application.StartupPath);
        }

        private void OpenLauncherLogfileDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(Application.StartupPath, ConstStrings.C_LOGFOLDER_NAME));
        }

        private void CreditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreditsForm _about = new();

            DialogResult dr = _about.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _about.Close();
            }
        }

        private void MessagesFromTheTeamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessagesForm _message = new();

            DialogResult dr = _message.ShowDialog();
            if (dr == DialogResult.OK)
            {
                _message.Close();
            }
        }

        private async void RepairGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = true;

            try
            {
                LaunchGameToolStripMenuItem.Enabled = false;
                OptionsToolStripMenuItem.Enabled = false;
                RepairGameToolStripMenuItem.Enabled = false;

                RepairLogConsole repairLogConsole = new();

                bool FlagIsCorrupt = false;

                BtnInstall.Enabled = false;
                var locationInForm = repairLogConsole.Location;
                var locationOnScreen = PointToScreen(locationInForm);

                repairLogConsole.StartPosition = FormStartPosition.Manual;
                repairLogConsole.Location = new Point(locationOnScreen.X + 1275, locationOnScreen.Y - 31);
                repairLogConsole.Show();

                repairLogConsole.TxtConsole.Text = "Checking game integrity...";
                repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                repairLogConsole.TxtConsole.AppendText(Environment.NewLine);

                foreach (var fileName in RepairFileList._DictionaryRepairFileList)
                {
                    string isMD5Value = MD5Tools.CalculateMD5(Path.Combine(ConstStrings.GameInstallPath(), fileName.Key));
                    string shouldMD5Value = fileName.Value;

                    if (isMD5Value == shouldMD5Value)
                    {
                        repairLogConsole.TxtConsole.AppendText(string.Format("File {0} has the correct value: {1}", fileName.Key, shouldMD5Value));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    }
                    else
                    {
                        repairLogConsole.TxtConsole.AppendText(string.Format("File {0} ist corrupted and will be reaquired... Value: {1}", fileName.Key, isMD5Value));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        FlagIsCorrupt = true;
                        break;
                    }
                }

                repairLogConsole.TxtConsole.AppendText(Environment.NewLine);

                if (FlagIsCorrupt)
                {
                    Settings.Default.IsPatch34Downloaded = false;
                    Settings.Default.IsPatch34Installed = false;
                    Settings.Default.Save();

                    if (Directory.Exists(ConstStrings.GameInstallPath()))
                    {
                        Directory.Delete(ConstStrings.GameInstallPath(), true);

                        if (!Directory.Exists(ConstStrings.GameInstallPath()))
                        {
                            repairLogConsole.TxtConsole.AppendText(string.Format("Deleted the game folder \"{0}\" sucessfully", ConstStrings.GameInstallPath()));
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        }
                        else
                        {
                            repairLogConsole.TxtConsole.AppendText(string.Format("Was not able to delete the folder \"{0}\". Please check \"{1}\" for details.", ConstStrings.GameInstallPath(), ConstStrings.C_ERRORLOGGING_FILE));
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        }
                    }

                    // if (Directory.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_PATCHFOLDER_NAME)))
                    //     Directory.Delete(ConstStrings.C_PATCHFOLDER_NAME, true);
                    //
                    // if (Directory.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_BETAFOLDER_NAME)))
                    //     Directory.Delete(ConstStrings.C_BETAFOLDER_NAME, true);

                    if (File.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, ConstStrings.C_MAINGAMEFILE_ZIP)))
                    {
                        repairLogConsole.TxtConsole.AppendText(string.Format("Checking file \"{0}\"...", ConstStrings.C_MAINGAMEFILE_ZIP));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        if (MD5Tools.CalculateMD5(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, ConstStrings.C_MAINGAMEFILE_ZIP)) != ConstStrings.C_MAINGAMEFILE_ZIP_MD5_HASH)
                        {
                            File.Delete(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, ConstStrings.C_MAINGAMEFILE_ZIP));
                            repairLogConsole.TxtConsole.AppendText(string.Format("The file \"{0}\" was corrupted and will be reaquired...", ConstStrings.C_MAINGAMEFILE_ZIP));
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        }
                        repairLogConsole.TxtConsole.AppendText(string.Format("File \"{0}\" is okay. No action needed.", ConstStrings.C_MAINGAMEFILE_ZIP));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    }

                    if (File.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, _languageSettings.LanguagPackName)))
                    {
                        repairLogConsole.TxtConsole.AppendText(string.Format("Checking file \"{0}\"...", _languageSettings.LanguagPackName));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        if (MD5Tools.CalculateMD5(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, _languageSettings.LanguagPackName)) != XMLFileHelper.GetXMLGameLanguageMD5Hash(_languageSettings.RegistrySelectedLocale))
                        {
                            File.Delete(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, ConstStrings.C_MAINGAMEFILE_ZIP));
                            repairLogConsole.TxtConsole.AppendText(string.Format("The file \"{0}\" was corrupted and will be reaquired...", _languageSettings.LanguagPackName));
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        }
                        repairLogConsole.TxtConsole.AppendText(string.Format("File \"{0}\" is okay. No action needed.", _languageSettings.LanguagPackName));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    }

                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    repairLogConsole.TxtConsole.AppendText("Check for EAX-Support...");
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);

                    if (Settings.Default.EAXSupport)
                    {
                        repairLogConsole.TxtConsole.AppendText("EAX-Support is active... Installing neccessary files...");
                        List<string> _EAXFiles = new() { "dsoal-aldrv.dll", "dsound.dll", "dsound.ini", };

                        foreach (var file in _EAXFiles)
                        {
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                            repairLogConsole.TxtConsole.AppendText("Installed file: " + file);
                            File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, file), Path.Combine(ConstStrings.GameInstallPath(), file), true);
                        }
                    }
                    else
                    {
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        repairLogConsole.TxtConsole.AppendText("EAX support disabled via launcher... no action needed.");
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    }

                    repairLogConsole.TxtConsole.AppendText("We are now renewing every file...");
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    await InstallRoutine(false);
                }
                else
                {
                    repairLogConsole.TxtConsole.AppendText(string.Format("Detected game language: \"{0}\"", _languageSettings.RegistrySelectedLanguage));
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    repairLogConsole.TxtConsole.AppendText(string.Format("Reinstalling installed language: \"{0}\" ...", _languageSettings.RegistrySelectedLanguage));
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);

                    if (File.Exists(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, _languageSettings.LanguagPackName)))
                    {
                        repairLogConsole.TxtConsole.AppendText(string.Format("Checking file \"{0}\"...", _languageSettings.LanguagPackName));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        if (MD5Tools.CalculateMD5(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, _languageSettings.LanguagPackName)) != XMLFileHelper.GetXMLGameLanguageMD5Hash(_languageSettings.RegistrySelectedLocale))
                        {
                            File.Delete(Path.Combine(Application.StartupPath, ConstStrings.C_DOWNLOADFOLDER_NAME, ConstStrings.C_MAINGAMEFILE_ZIP));
                            repairLogConsole.TxtConsole.AppendText(string.Format("The file \"{0}\" was corrupted and will be reaquired...", _languageSettings.LanguagPackName));
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        }
                        repairLogConsole.TxtConsole.AppendText(string.Format("File \"{0}\" is okay. No action needed.", _languageSettings.LanguagPackName));
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    }

                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    repairLogConsole.TxtConsole.AppendText("Check for EAX-Support...");
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);

                    if (Settings.Default.EAXSupport)
                    {
                        repairLogConsole.TxtConsole.AppendText("EAX-Support is active... Installing neccessary files...");
                        List<string> _EAXFiles = new() { "dsoal-aldrv.dll", "dsound.dll", "dsound.ini", };

                        foreach (var file in _EAXFiles)
                        {
                            repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                            repairLogConsole.TxtConsole.AppendText("Installed file: " + file);
                            File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, file), Path.Combine(ConstStrings.GameInstallPath(), file), true);
                        }
                    }
                    else
                    {
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                        repairLogConsole.TxtConsole.AppendText("EAX support disabled via launcher... no action needed.");
                        repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                    }

                    await InstallRoutine(true);
                }

                if (Settings.Default.InstalledLanguageISOCode == "de")
                {
                    File.Copy(Path.Combine(ConstStrings.C_TOOLFOLDER_NAME, ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_GERMANLANGUAGE_PATCH_FILE), true);
                    repairLogConsole.TxtConsole.AppendText(string.Format("Copied German translation for 2.22 \"{0}\" into \"{1}\"", ConstStrings.C_GERMANLANGUAGE_PATCH_FILE, ConstStrings.GameInstallPath()));
                    repairLogConsole.TxtConsole.AppendText(Environment.NewLine);
                }

                await File.WriteAllTextAsync(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, "Repair_" + DateTime.Now.ToString("yyyy'_'MM'_'ddTHH'_'mm'_'ss") + ".log"), repairLogConsole.TxtConsole.Text);

                Thread.Sleep(1500);

                MessageBox.Show(Strings.Msg_RepairDone_Text, Strings.Msg_RepairDone_Title, MessageBoxButtons.OK);

                repairLogConsole.Hide();
                repairLogConsole.Dispose();

                BtnInstall.Enabled = true;

                LaunchGameToolStripMenuItem.Enabled = true;
                OptionsToolStripMenuItem.Enabled = true;
                RepairGameToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                await file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }

            IsCurrentlyWorkingState.IsLauncherCurrentlyWorking = false;
        }

        private void LauncherSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LauncherOptionsForm _Launcheroptions = new();
            _Launcheroptions.ShowDialog();

            if (StartMenuHelper.DoesTheShortCutExist(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_GAMETITLE_NAME_EN))
                GameDesktopShortcutToolStripMenuItem.Checked = true;
            else
                GameDesktopShortcutToolStripMenuItem.Checked = false;

            if (StartMenuHelper.DoesTheShortCutExist(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN), ConstStrings.C_GAMETITLE_NAME_EN))
                GameStartmenuShortcutsToolStripMenuItem.Checked = true;
            else
                GameStartmenuShortcutsToolStripMenuItem.Checked = false;
        }

        private void GameSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameOptionsForm _Gameoptions = new();
            _Gameoptions.ShowDialog();
        }

        private void GameDesktopShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (StartMenuHelper.DoesTheShortCutExist(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_GAMETITLE_NAME_EN))
                {
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_GAMETITLE_NAME_EN + ".lnk"));
                    GameDesktopShortcutToolStripMenuItem.Checked = false;
                }
                else
                {
                    StartMenuHelper.CreateShortcutToDesktop(Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_MAIN_GAME_FILE), ConstStrings.C_GAMETITLE_NAME_EN, Settings.Default.StartGameWindowed == true ? "-win" : "");
                    GameDesktopShortcutToolStripMenuItem.Checked = true;
                }
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        private void GameStartmenuShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (StartMenuHelper.DoesTheShortCutExist(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN), ConstStrings.C_GAMETITLE_NAME_EN))
                {
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN, ConstStrings.C_GAMETITLE_NAME_EN + ".lnk"));
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN, "Worldbuilder" + ".lnk"));

                    if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN)))
                        Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN));

                    GameStartmenuShortcutsToolStripMenuItem.Checked = false;
                }
                else
                {
                    if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN)))
                        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN));

                    StartMenuHelper.CreateShortcutToStartMenu(Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_MAIN_GAME_FILE), ConstStrings.C_GAMETITLE_NAME_EN, Path.Combine("Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN), Settings.Default.StartGameWindowed == true ? "-win" : "");
                    StartMenuHelper.CreateShortcutToStartMenu(Path.Combine(ConstStrings.GameInstallPath(), ConstStrings.C_WORLDBUILDER_FILE), "Worldbuilder", Path.Combine("Programs", "Electronic Arts", ConstStrings.C_GAMETITLE_NAME_EN));
                    GameStartmenuShortcutsToolStripMenuItem.Checked = true;
                }
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        private void LauncherDesktopShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (StartMenuHelper.DoesTheShortCutExist(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_LAUNCHER_SHORTCUT_NAME))
                {
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), ConstStrings.C_LAUNCHER_SHORTCUT_NAME + ".lnk"));
                    LauncherDesktopShortcutToolStripMenuItem.Checked = false;
                }
                else
                {
                    StartMenuHelper.CreateShortcutToDesktop(Path.Combine(Application.StartupPath, "Restarter.exe"), ConstStrings.C_LAUNCHER_SHORTCUT_NAME, "--startLauncher");
                    LauncherDesktopShortcutToolStripMenuItem.Checked = true;
                }
            }
            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }

        private void LauncherStartmenuShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (StartMenuHelper.DoesTheShortCutExist(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), Path.Combine("Programs", "Patch 2.22 Launcher")), ConstStrings.C_LAUNCHER_SHORTCUT_NAME))
                {
                    File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), Path.Combine("Programs", "Patch 2.22 Launcher"), ConstStrings.C_LAUNCHER_SHORTCUT_NAME + ".lnk"));
                    LauncherStartmenuShortcutToolStripMenuItem.Checked = false;
                }
                else
                {
                    if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), Path.Combine("Programs", "Patch 2.22 Launcher"))))
                        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), Path.Combine("Programs", "Patch 2.22 Launcher")));

                    StartMenuHelper.CreateShortcutToStartMenu(Path.Combine(Application.StartupPath, "Restarter.exe"), ConstStrings.C_LAUNCHER_SHORTCUT_NAME, Path.Combine("Programs", "Patch 2.22 Launcher"), "--startLauncher");

                    LauncherStartmenuShortcutToolStripMenuItem.Checked = true;
                }
            }

            catch (Exception ex)
            {
                using StreamWriter file = new(Path.Combine(ConstStrings.C_LOGFOLDER_NAME, ConstStrings.C_ERRORLOGGING_FILE), append: true);
                file.WriteLineAsync(ConstStrings.LogTime + ex.ToString());
            }
        }
    }
}