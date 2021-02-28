namespace Chat_Monkeyz
{
    partial class Chat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.tb_message = new System.Windows.Forms.TextBox();
            this.tabChat = new System.Windows.Forms.TabControl();
            this.actionsStrip = new System.Windows.Forms.MenuStrip();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchPeersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.b_files = new System.Windows.Forms.CheckBox();
            this.b_peers = new System.Windows.Forms.CheckBox();
            this.b_voice = new System.Windows.Forms.CheckBox();
            this.rb_affichage = new System.Windows.Forms.RichTextBox();
            this.systray = new System.Windows.Forms.NotifyIcon(this.components);
            this.actionsStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_message
            // 
            this.tb_message.AcceptsTab = true;
            this.tb_message.AllowDrop = true;
            this.tb_message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_message.Location = new System.Drawing.Point(-1, 264);
            this.tb_message.Multiline = true;
            this.tb_message.Name = "tb_message";
            this.tb_message.Size = new System.Drawing.Size(678, 50);
            this.tb_message.TabIndex = 1;
            this.tb_message.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_message_KeyPress);
            // 
            // tabChat
            // 
            this.tabChat.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabChat.Location = new System.Drawing.Point(-1, 5);
            this.tabChat.Name = "tabChat";
            this.tabChat.SelectedIndex = 0;
            this.tabChat.Size = new System.Drawing.Size(642, 23);
            this.tabChat.TabIndex = 3;
            this.tabChat.TabStop = false;
            this.tabChat.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabChat_DrawItem);
            this.tabChat.SelectedIndexChanged += new System.EventHandler(this.tabChat_SelectedIndexChanged);
            // 
            // actionsStrip
            // 
            this.actionsStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionsStrip.BackColor = System.Drawing.Color.Transparent;
            this.actionsStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.actionsStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionsToolStripMenuItem});
            this.actionsStrip.Location = new System.Drawing.Point(613, 3);
            this.actionsStrip.Name = "actionsStrip";
            this.actionsStrip.Size = new System.Drawing.Size(61, 24);
            this.actionsStrip.TabIndex = 12;
            this.actionsStrip.Text = "Param";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.actionsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchPeersToolStripMenuItem,
            this.connectToolStripMenuItem,
            this.sendFileToolStripMenuItem,
            this.configToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.actionsToolStripMenuItem.Text = "Param";
            // 
            // searchPeersToolStripMenuItem
            // 
            this.searchPeersToolStripMenuItem.Name = "searchPeersToolStripMenuItem";
            this.searchPeersToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.searchPeersToolStripMenuItem.Text = "Search peers";
            this.searchPeersToolStripMenuItem.Click += new System.EventHandler(this.searchPeersToolStripMenuItem_Click);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.connectToolStripMenuItem.Text = "Connect...";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // sendFileToolStripMenuItem
            // 
            this.sendFileToolStripMenuItem.Name = "sendFileToolStripMenuItem";
            this.sendFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.sendFileToolStripMenuItem.Text = "Send file";
            this.sendFileToolStripMenuItem.Click += new System.EventHandler(this.sendFileToolStripMenuItem1_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // b_files
            // 
            this.b_files.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_files.Appearance = System.Windows.Forms.Appearance.Button;
            this.b_files.AutoSize = true;
            this.b_files.Location = new System.Drawing.Point(62, 240);
            this.b_files.Name = "b_files";
            this.b_files.Size = new System.Drawing.Size(38, 23);
            this.b_files.TabIndex = 13;
            this.b_files.Text = "Files";
            this.b_files.UseVisualStyleBackColor = true;
            this.b_files.CheckedChanged += new System.EventHandler(this.b_files_CheckedChanged);
            // 
            // b_peers
            // 
            this.b_peers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_peers.Appearance = System.Windows.Forms.Appearance.Button;
            this.b_peers.AutoSize = true;
            this.b_peers.Location = new System.Drawing.Point(12, 240);
            this.b_peers.Name = "b_peers";
            this.b_peers.Size = new System.Drawing.Size(44, 23);
            this.b_peers.TabIndex = 14;
            this.b_peers.Text = "Peers";
            this.b_peers.UseVisualStyleBackColor = true;
            this.b_peers.CheckedChanged += new System.EventHandler(this.b_peers_CheckedChanged);
            // 
            // b_voice
            // 
            this.b_voice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_voice.Appearance = System.Windows.Forms.Appearance.Button;
            this.b_voice.AutoSize = true;
            this.b_voice.Location = new System.Drawing.Point(623, 240);
            this.b_voice.Name = "b_voice";
            this.b_voice.Size = new System.Drawing.Size(44, 23);
            this.b_voice.TabIndex = 15;
            this.b_voice.Text = "Voice";
            this.b_voice.UseVisualStyleBackColor = true;
            this.b_voice.CheckedChanged += new System.EventHandler(this.b_voice_CheckedChanged);
            // 
            // rb_affichage
            // 
            this.rb_affichage.AllowDrop = true;
            this.rb_affichage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rb_affichage.Location = new System.Drawing.Point(-1, 23);
            this.rb_affichage.Name = "rb_affichage";
            this.rb_affichage.ReadOnly = true;
            this.rb_affichage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rb_affichage.Size = new System.Drawing.Size(678, 218);
            this.rb_affichage.TabIndex = 16;
            this.rb_affichage.Text = "";
            // 
            // systray
            // 
            this.systray.Icon = ((System.Drawing.Icon)(resources.GetObject("systray.Icon")));
            this.systray.Text = "Chat_Monkeyz";
            this.systray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.systray_MouseDoubleClick);
            // 
            // Chat
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 313);
            this.Controls.Add(this.rb_affichage);
            this.Controls.Add(this.b_voice);
            this.Controls.Add(this.b_peers);
            this.Controls.Add(this.b_files);
            this.Controls.Add(this.actionsStrip);
            this.Controls.Add(this.tabChat);
            this.Controls.Add(this.tb_message);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Chat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Chat_Monkeyz";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Chat_FormClosing);
            this.Load += new System.EventHandler(this.Chat_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Chat_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Chat_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chat_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Chat_MouseUp);
            this.actionsStrip.ResumeLayout(false);
            this.actionsStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TabControl tabChat;
        private System.Windows.Forms.MenuStrip actionsStrip;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchPeersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendFileToolStripMenuItem;
        public System.Windows.Forms.CheckBox b_files;
        public System.Windows.Forms.CheckBox b_peers;
        private System.Windows.Forms.CheckBox b_voice;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rb_affichage;
        public System.Windows.Forms.TextBox tb_message;
        private System.Windows.Forms.NotifyIcon systray;




    }
}

