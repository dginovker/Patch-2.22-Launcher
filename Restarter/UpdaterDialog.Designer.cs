﻿using System.Reflection;

namespace Restarter
{
    partial class UpdaterDialog
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
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            File.AppendAllText("log.txt", $"Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterDialog));
            SuspendLayout();
            // 
            // UpdaterDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(498, 60);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "UpdaterDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UpdaterDialog";
            Shown += UpdaterDialog_Shown;
            ResumeLayout(false);
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        #endregion
    }
}