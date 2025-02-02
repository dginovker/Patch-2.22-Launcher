﻿namespace PatchLauncher
{
    partial class InstallPathDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallPathDialog));
            TxtInstallPath = new System.Windows.Forms.TextBox();
            LblChooseDir = new System.Windows.Forms.Label();
            BtnChoose = new System.Windows.Forms.Button();
            BtnAccept = new System.Windows.Forms.Button();
            CmbSelectGameLanguage = new System.Windows.Forms.ComboBox();
            LblSelectGameLanguage = new System.Windows.Forms.Label();
            PibLanguageSupport = new System.Windows.Forms.PictureBox();
            PibPathBorder = new System.Windows.Forms.PictureBox();
            LblDesktopShortCut = new System.Windows.Forms.Label();
            ChkDesktopShortcut = new System.Windows.Forms.Button();
            LblStartMenuShortCut = new System.Windows.Forms.Label();
            ChkStartMenuShortcut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)PibLanguageSupport).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PibPathBorder).BeginInit();
            SuspendLayout();
            // 
            // TxtInstallPath
            // 
            TxtInstallPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(TxtInstallPath, "TxtInstallPath");
            TxtInstallPath.Name = "TxtInstallPath";
            // 
            // LblChooseDir
            // 
            resources.ApplyResources(LblChooseDir, "LblChooseDir");
            LblChooseDir.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            LblChooseDir.Name = "LblChooseDir";
            // 
            // BtnChoose
            // 
            BtnChoose.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(BtnChoose, "BtnChoose");
            BtnChoose.FlatAppearance.BorderSize = 0;
            BtnChoose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            BtnChoose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            BtnChoose.ForeColor = System.Drawing.Color.Transparent;
            BtnChoose.Name = "BtnChoose";
            BtnChoose.TabStop = false;
            BtnChoose.UseMnemonic = false;
            BtnChoose.UseVisualStyleBackColor = false;
            BtnChoose.Click += BtnChoose_Click;
            BtnChoose.MouseDown += BtnChoose_MouseDown;
            BtnChoose.MouseEnter += BtnChoose_MouseEnter;
            BtnChoose.MouseLeave += BtnChoose_MouseLeave;
            // 
            // BtnAccept
            // 
            BtnAccept.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(BtnAccept, "BtnAccept");
            BtnAccept.FlatAppearance.BorderSize = 0;
            BtnAccept.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            BtnAccept.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            BtnAccept.ForeColor = System.Drawing.Color.Transparent;
            BtnAccept.Name = "BtnAccept";
            BtnAccept.TabStop = false;
            BtnAccept.UseMnemonic = false;
            BtnAccept.UseVisualStyleBackColor = false;
            BtnAccept.Click += BtnAccept_Click;
            BtnAccept.MouseDown += BtnAccept_MouseDown;
            BtnAccept.MouseEnter += BtnAccept_MouseEnter;
            BtnAccept.MouseLeave += BtnAccept_MouseLeave;
            // 
            // CmbSelectGameLanguage
            // 
            CmbSelectGameLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(CmbSelectGameLanguage, "CmbSelectGameLanguage");
            CmbSelectGameLanguage.FormattingEnabled = true;
            CmbSelectGameLanguage.Name = "CmbSelectGameLanguage";
            CmbSelectGameLanguage.SelectedIndexChanged += CmbSelectGameLanguage_SelectedIndexChanged;
            // 
            // LblSelectGameLanguage
            // 
            resources.ApplyResources(LblSelectGameLanguage, "LblSelectGameLanguage");
            LblSelectGameLanguage.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            LblSelectGameLanguage.Name = "LblSelectGameLanguage";
            // 
            // PibLanguageSupport
            // 
            PibLanguageSupport.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(PibLanguageSupport, "PibLanguageSupport");
            PibLanguageSupport.Name = "PibLanguageSupport";
            PibLanguageSupport.TabStop = false;
            // 
            // PibPathBorder
            // 
            PibPathBorder.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(PibPathBorder, "PibPathBorder");
            PibPathBorder.Name = "PibPathBorder";
            PibPathBorder.TabStop = false;
            // 
            // LblDesktopShortCut
            // 
            resources.ApplyResources(LblDesktopShortCut, "LblDesktopShortCut");
            LblDesktopShortCut.BackColor = System.Drawing.SystemColors.ActiveCaption;
            LblDesktopShortCut.Name = "LblDesktopShortCut";
            // 
            // ChkDesktopShortcut
            // 
            ChkDesktopShortcut.BackColor = System.Drawing.Color.DarkGray;
            resources.ApplyResources(ChkDesktopShortcut, "ChkDesktopShortcut");
            ChkDesktopShortcut.FlatAppearance.BorderSize = 0;
            ChkDesktopShortcut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            ChkDesktopShortcut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            ChkDesktopShortcut.ForeColor = System.Drawing.Color.Transparent;
            ChkDesktopShortcut.Name = "ChkDesktopShortcut";
            ChkDesktopShortcut.TabStop = false;
            ChkDesktopShortcut.UseMnemonic = false;
            ChkDesktopShortcut.UseVisualStyleBackColor = false;
            ChkDesktopShortcut.Click += ChkDesktopShortcut_Click;
            ChkDesktopShortcut.MouseDown += ChkDesktopShortcut_MouseDown;
            ChkDesktopShortcut.MouseEnter += ChkDesktopShortcut_MouseEnter;
            ChkDesktopShortcut.MouseLeave += ChkDesktopShortcut_MouseLeave;
            // 
            // LblStartMenuShortCut
            // 
            resources.ApplyResources(LblStartMenuShortCut, "LblStartMenuShortCut");
            LblStartMenuShortCut.BackColor = System.Drawing.Color.Red;
            LblStartMenuShortCut.Name = "LblStartMenuShortCut";
            // 
            // ChkStartMenuShortcut
            // 
            ChkStartMenuShortcut.BackColor = System.Drawing.Color.IndianRed;
            resources.ApplyResources(ChkStartMenuShortcut, "ChkStartMenuShortcut");
            ChkStartMenuShortcut.FlatAppearance.BorderSize = 0;
            ChkStartMenuShortcut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            ChkStartMenuShortcut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            ChkStartMenuShortcut.ForeColor = System.Drawing.Color.Transparent;
            ChkStartMenuShortcut.Name = "ChkStartMenuShortcut";
            ChkStartMenuShortcut.TabStop = false;
            ChkStartMenuShortcut.UseMnemonic = false;
            ChkStartMenuShortcut.UseVisualStyleBackColor = false;
            ChkStartMenuShortcut.Click += ChkStartMenuShortcut_Click;
            ChkStartMenuShortcut.MouseDown += ChkStartMenuShortcut_MouseDown;
            ChkStartMenuShortcut.MouseEnter += ChkStartMenuShortcut_MouseEnter;
            ChkStartMenuShortcut.MouseLeave += ChkStartMenuShortcut_MouseLeave;
            // 
            // InstallPathDialog
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = System.Drawing.SystemColors.ControlDarkDark;
            resources.ApplyResources(this, "$this");
            Controls.Add(LblStartMenuShortCut);
            Controls.Add(ChkStartMenuShortcut);
            Controls.Add(LblDesktopShortCut);
            Controls.Add(ChkDesktopShortcut);
            Controls.Add(PibLanguageSupport);
            Controls.Add(LblSelectGameLanguage);
            Controls.Add(CmbSelectGameLanguage);
            Controls.Add(BtnAccept);
            Controls.Add(BtnChoose);
            Controls.Add(LblChooseDir);
            Controls.Add(TxtInstallPath);
            Controls.Add(PibPathBorder);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "InstallPathDialog";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)PibLanguageSupport).EndInit();
            ((System.ComponentModel.ISupportInitialize)PibPathBorder).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox TxtInstallPath;
        private System.Windows.Forms.Label LblChooseDir;
        private System.Windows.Forms.Button BtnChoose;
        private System.Windows.Forms.Button BtnAccept;
        private System.Windows.Forms.ComboBox CmbSelectGameLanguage;
        private System.Windows.Forms.Label LblSelectGameLanguage;
        private System.Windows.Forms.PictureBox PibLanguageSupport;
        private System.Windows.Forms.PictureBox PibPathBorder;
        private System.Windows.Forms.Label LblDesktopShortCut;
        private System.Windows.Forms.Button ChkDesktopShortcut;
        private System.Windows.Forms.Label LblStartMenuShortCut;
        private System.Windows.Forms.Button ChkStartMenuShortcut;
    }
}