using System.Reflection;

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
            // 
            // UpdaterDialog
            // 
            File.AppendAllText("log.txt", $"Done Running {MethodBase.GetCurrentMethod()?.Name}" + Environment.NewLine);
        }

        #endregion
    }
}