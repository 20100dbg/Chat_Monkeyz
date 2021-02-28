namespace Chat_Monkeyz
{
    partial class wndFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wndFiles));
            this.b_accept = new System.Windows.Forms.Button();
            this.b_reject = new System.Windows.Forms.Button();
            this.b_clear = new System.Windows.Forms.Button();
            this.g_files = new System.Windows.Forms.DataGridView();
            this.Filename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Taille = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Send_Receive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Peer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.g_files)).BeginInit();
            this.SuspendLayout();
            // 
            // b_accept
            // 
            this.b_accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_accept.Location = new System.Drawing.Point(12, 226);
            this.b_accept.Name = "b_accept";
            this.b_accept.Size = new System.Drawing.Size(75, 23);
            this.b_accept.TabIndex = 1;
            this.b_accept.Text = "Accept";
            this.b_accept.UseVisualStyleBackColor = true;
            this.b_accept.Click += new System.EventHandler(this.b_accept_Click);
            // 
            // b_reject
            // 
            this.b_reject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_reject.Location = new System.Drawing.Point(93, 227);
            this.b_reject.Name = "b_reject";
            this.b_reject.Size = new System.Drawing.Size(75, 23);
            this.b_reject.TabIndex = 2;
            this.b_reject.Text = "Reject";
            this.b_reject.UseVisualStyleBackColor = true;
            this.b_reject.Click += new System.EventHandler(this.b_reject_Click);
            // 
            // b_clear
            // 
            this.b_clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_clear.Location = new System.Drawing.Point(401, 227);
            this.b_clear.Name = "b_clear";
            this.b_clear.Size = new System.Drawing.Size(75, 23);
            this.b_clear.TabIndex = 3;
            this.b_clear.Text = "Clear";
            this.b_clear.UseVisualStyleBackColor = true;
            this.b_clear.Click += new System.EventHandler(this.b_clear_Click);
            // 
            // g_files
            // 
            this.g_files.AllowUserToAddRows = false;
            this.g_files.AllowUserToDeleteRows = false;
            this.g_files.AllowUserToOrderColumns = true;
            this.g_files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.g_files.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.g_files.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.g_files.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Filename,
            this.Taille,
            this.State,
            this.Send_Receive,
            this.Peer});
            this.g_files.EnableHeadersVisualStyles = false;
            this.g_files.Location = new System.Drawing.Point(12, 12);
            this.g_files.MultiSelect = false;
            this.g_files.Name = "g_files";
            this.g_files.ReadOnly = true;
            this.g_files.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.g_files.Size = new System.Drawing.Size(464, 209);
            this.g_files.TabIndex = 4;
            this.g_files.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.g_files_CellClick);
            // 
            // Filename
            // 
            this.Filename.HeaderText = "Filename";
            this.Filename.Name = "Filename";
            this.Filename.ReadOnly = true;
            // 
            // Taille
            // 
            this.Taille.HeaderText = "Size";
            this.Taille.Name = "Taille";
            this.Taille.ReadOnly = true;
            // 
            // State
            // 
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // Send_Receive
            // 
            this.Send_Receive.HeaderText = "Send_Receive";
            this.Send_Receive.Name = "Send_Receive";
            this.Send_Receive.ReadOnly = true;
            // 
            // Peer
            // 
            this.Peer.HeaderText = "Peer";
            this.Peer.Name = "Peer";
            this.Peer.ReadOnly = true;
            // 
            // wndFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 255);
            this.Controls.Add(this.g_files);
            this.Controls.Add(this.b_clear);
            this.Controls.Add(this.b_reject);
            this.Controls.Add(this.b_accept);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "wndFiles";
            this.Text = "Fichiers";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FileTransfer_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.g_files)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button b_accept;
        private System.Windows.Forms.Button b_reject;
        private System.Windows.Forms.Button b_clear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Filename;
        private System.Windows.Forms.DataGridViewTextBoxColumn Taille;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Send_Receive;
        private System.Windows.Forms.DataGridViewTextBoxColumn Peer;
        public System.Windows.Forms.DataGridView g_files;





    }
}

