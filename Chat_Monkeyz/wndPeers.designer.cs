namespace Chat_Monkeyz
{
    partial class wndPeers
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wndPeers));
            this.menuPeers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ignoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.userControl11 = new Chat_Monkeyz.UserControl1();
            this.menuPeers.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPeers
            // 
            this.menuPeers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignoreToolStripMenuItem,
            this.sendFileToolStripMenuItem,
            this.mPToolStripMenuItem,
            this.infosToolStripMenuItem});
            this.menuPeers.Name = "menuPeers";
            this.menuPeers.Size = new System.Drawing.Size(116, 92);
            // 
            // ignoreToolStripMenuItem
            // 
            this.ignoreToolStripMenuItem.Name = "ignoreToolStripMenuItem";
            this.ignoreToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.ignoreToolStripMenuItem.Text = "Ignore";
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.sendFileToolStripMenuItem.Text = "Send file";
            // 
            // mPToolStripMenuItem
            // 
            this.mPToolStripMenuItem.Name = "mPToolStripMenuItem";
            this.mPToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.mPToolStripMenuItem.Text = "MP";
            // 
            // infosToolStripMenuItem
            // 
            this.infosToolStripMenuItem.Name = "infosToolStripMenuItem";
            this.infosToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.infosToolStripMenuItem.Text = "Infos";
            // 
            // elementHost1
            // 
            this.elementHost1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost1.Location = new System.Drawing.Point(0, -1);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(205, 277);
            this.elementHost1.TabIndex = 1;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.userControl11;
            // 
            // wndPeers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 274);
            this.Controls.Add(this.elementHost1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "wndPeers";
            this.RightToLeftLayout = true;
            this.Text = "Peers";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PeerList_KeyDown);
            this.menuPeers.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuPeers;
        private System.Windows.Forms.ToolStripMenuItem ignoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infosToolStripMenuItem;
        public System.Windows.Forms.Integration.ElementHost elementHost1;
        public UserControl1 userControl11;





    }
}

