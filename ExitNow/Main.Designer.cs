namespace ExitNow
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.labelInfo = new System.Windows.Forms.Label();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.listViewHistory = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.FormMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autostartItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartAsAdminItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.FormMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelInfo
            // 
            this.labelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfo.Location = new System.Drawing.Point(12, 277);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(130, 13);
            this.labelInfo.TabIndex = 2;
            this.labelInfo.Text = "Alt + F3 to kill a process";
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.BalloonTipTitle = "ExitNow";
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "ExitNow";
            this.trayIcon.Visible = true;
            this.trayIcon.Click += new System.EventHandler(this.OnTrayIconClick);
            // 
            // listViewHistory
            // 
            this.listViewHistory.Location = new System.Drawing.Point(0, 16);
            this.listViewHistory.Name = "listViewHistory";
            this.listViewHistory.Size = new System.Drawing.Size(540, 231);
            this.listViewHistory.TabIndex = 3;
            this.listViewHistory.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "History:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listViewHistory);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 227);
            this.panel1.TabIndex = 6;
            // 
            // FormMenu
            // 
            this.FormMenu.BackColor = System.Drawing.Color.White;
            this.FormMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.extrasToolStripMenuItem});
            this.FormMenu.Location = new System.Drawing.Point(0, 0);
            this.FormMenu.Name = "FormMenu";
            this.FormMenu.Size = new System.Drawing.Size(564, 24);
            this.FormMenu.TabIndex = 7;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoItem,
            this.hideItem,
            this.exitItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // infoItem
            // 
            this.infoItem.Name = "infoItem";
            this.infoItem.Size = new System.Drawing.Size(99, 22);
            this.infoItem.Text = "Info";
            this.infoItem.Click += new System.EventHandler(this.OnInfoItemClick);
            // 
            // hideItem
            // 
            this.hideItem.Name = "hideItem";
            this.hideItem.Size = new System.Drawing.Size(99, 22);
            this.hideItem.Text = "Hide";
            this.hideItem.Click += new System.EventHandler(this.OnHideItemClick);
            // 
            // exitItem
            // 
            this.exitItem.Name = "exitItem";
            this.exitItem.Size = new System.Drawing.Size(99, 22);
            this.exitItem.Text = "Exit";
            this.exitItem.Click += new System.EventHandler(this.OnExitItemClick);
            // 
            // extrasToolStripMenuItem
            // 
            this.extrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autostartItem,
            this.restartAsAdminItem});
            this.extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
            this.extrasToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.extrasToolStripMenuItem.Text = "Tools";
            // 
            // autostartItem
            // 
            this.autostartItem.Name = "autostartItem";
            this.autostartItem.Size = new System.Drawing.Size(200, 22);
            this.autostartItem.Text = "Start with Windows";
            this.autostartItem.Click += new System.EventHandler(this.OnAutostartItemClick);
            // 
            // restartAsAdminItem
            // 
            this.restartAsAdminItem.Name = "restartAsAdminItem";
            this.restartAsAdminItem.Size = new System.Drawing.Size(200, 22);
            this.restartAsAdminItem.Text = "Restart as Administrator";
            this.restartAsAdminItem.Click += new System.EventHandler(this.OnRestartAsAdminItemClick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(564, 299);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.FormMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.FormMenu;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExitNow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnExit);
            this.Load += new System.EventHandler(this.OnLoad);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.FormMenu.ResumeLayout(false);
            this.FormMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewHistory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip FormMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autostartItem;
        private System.Windows.Forms.ToolStripMenuItem exitItem;
        private System.Windows.Forms.ToolStripMenuItem restartAsAdminItem;
        private System.Windows.Forms.ToolStripMenuItem infoItem;
        private System.Windows.Forms.ToolStripMenuItem hideItem;
    }
}

