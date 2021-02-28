namespace Chat_Monkeyz
{
    partial class Config
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
            this.b_ok = new System.Windows.Forms.Button();
            this.tb_pseudo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_udp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_tcp = new System.Windows.Forms.TextBox();
            this.b_cancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_buffer = new System.Windows.Forms.TextBox();
            this.l_info = new System.Windows.Forms.Label();
            this.l_info_pseudo = new System.Windows.Forms.Label();
            this.l_info_udp = new System.Windows.Forms.Label();
            this.l_info_tcp = new System.Windows.Forms.Label();
            this.l_info_buffer = new System.Windows.Forms.Label();
            this.l_incomingFolder = new System.Windows.Forms.Label();
            this.tb_incomingFolder = new System.Windows.Forms.TextBox();
            this.cb_encrypt = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_theme = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // b_ok
            // 
            this.b_ok.Location = new System.Drawing.Point(330, 338);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(67, 28);
            this.b_ok.TabIndex = 11;
            this.b_ok.Text = "Ok";
            this.b_ok.UseVisualStyleBackColor = true;
            this.b_ok.Click += new System.EventHandler(this.b_ok_Click);
            // 
            // tb_pseudo
            // 
            this.tb_pseudo.Location = new System.Drawing.Point(123, 23);
            this.tb_pseudo.Name = "tb_pseudo";
            this.tb_pseudo.Size = new System.Drawing.Size(115, 20);
            this.tb_pseudo.TabIndex = 2;
            this.tb_pseudo.TextChanged += new System.EventHandler(this.tb_pseudo_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Pseudo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Port UDP";
            // 
            // tb_udp
            // 
            this.tb_udp.Location = new System.Drawing.Point(123, 90);
            this.tb_udp.Name = "tb_udp";
            this.tb_udp.Size = new System.Drawing.Size(115, 20);
            this.tb_udp.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Port TCP";
            // 
            // tb_tcp
            // 
            this.tb_tcp.Location = new System.Drawing.Point(123, 64);
            this.tb_tcp.Name = "tb_tcp";
            this.tb_tcp.Size = new System.Drawing.Size(115, 20);
            this.tb_tcp.TabIndex = 4;
            // 
            // b_cancel
            // 
            this.b_cancel.Location = new System.Drawing.Point(408, 339);
            this.b_cancel.Name = "b_cancel";
            this.b_cancel.Size = new System.Drawing.Size(67, 28);
            this.b_cancel.TabIndex = 12;
            this.b_cancel.Text = "Annuler";
            this.b_cancel.UseVisualStyleBackColor = true;
            this.b_cancel.Click += new System.EventHandler(this.b_cancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Buffer";
            // 
            // tb_buffer
            // 
            this.tb_buffer.Location = new System.Drawing.Point(123, 137);
            this.tb_buffer.Name = "tb_buffer";
            this.tb_buffer.Size = new System.Drawing.Size(115, 20);
            this.tb_buffer.TabIndex = 8;
            this.tb_buffer.TextChanged += new System.EventHandler(this.tb_buffer_TextChanged);
            // 
            // l_info
            // 
            this.l_info.AutoSize = true;
            this.l_info.Location = new System.Drawing.Point(11, 346);
            this.l_info.Name = "l_info";
            this.l_info.Size = new System.Drawing.Size(0, 13);
            this.l_info.TabIndex = 10;
            // 
            // l_info_pseudo
            // 
            this.l_info_pseudo.AutoSize = true;
            this.l_info_pseudo.Location = new System.Drawing.Point(249, 26);
            this.l_info_pseudo.Name = "l_info_pseudo";
            this.l_info_pseudo.Size = new System.Drawing.Size(0, 13);
            this.l_info_pseudo.TabIndex = 11;
            // 
            // l_info_udp
            // 
            this.l_info_udp.AutoSize = true;
            this.l_info_udp.Location = new System.Drawing.Point(244, 93);
            this.l_info_udp.Name = "l_info_udp";
            this.l_info_udp.Size = new System.Drawing.Size(0, 13);
            this.l_info_udp.TabIndex = 12;
            // 
            // l_info_tcp
            // 
            this.l_info_tcp.AutoSize = true;
            this.l_info_tcp.Location = new System.Drawing.Point(244, 67);
            this.l_info_tcp.Name = "l_info_tcp";
            this.l_info_tcp.Size = new System.Drawing.Size(0, 13);
            this.l_info_tcp.TabIndex = 13;
            // 
            // l_info_buffer
            // 
            this.l_info_buffer.AutoSize = true;
            this.l_info_buffer.Location = new System.Drawing.Point(244, 140);
            this.l_info_buffer.Name = "l_info_buffer";
            this.l_info_buffer.Size = new System.Drawing.Size(0, 13);
            this.l_info_buffer.TabIndex = 14;
            // 
            // l_incomingFolder
            // 
            this.l_incomingFolder.AutoSize = true;
            this.l_incomingFolder.Location = new System.Drawing.Point(9, 177);
            this.l_incomingFolder.Name = "l_incomingFolder";
            this.l_incomingFolder.Size = new System.Drawing.Size(104, 13);
            this.l_incomingFolder.TabIndex = 9;
            this.l_incomingFolder.Text = "Dossier de réception";
            // 
            // tb_incomingFolder
            // 
            this.tb_incomingFolder.Location = new System.Drawing.Point(123, 177);
            this.tb_incomingFolder.Name = "tb_incomingFolder";
            this.tb_incomingFolder.ReadOnly = true;
            this.tb_incomingFolder.Size = new System.Drawing.Size(274, 20);
            this.tb_incomingFolder.TabIndex = 10;
            this.tb_incomingFolder.Click += new System.EventHandler(this.tb_incomingFolder_Click);
            // 
            // cb_encrypt
            // 
            this.cb_encrypt.AutoSize = true;
            this.cb_encrypt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_encrypt.Location = new System.Drawing.Point(12, 216);
            this.cb_encrypt.Name = "cb_encrypt";
            this.cb_encrypt.Size = new System.Drawing.Size(101, 17);
            this.cb_encrypt.TabIndex = 15;
            this.cb_encrypt.Text = "ENCRYPT !!1!§";
            this.cb_encrypt.UseVisualStyleBackColor = true;
            this.cb_encrypt.CheckedChanged += new System.EventHandler(this.cb_encrypt_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 260);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Theme";
            // 
            // cb_theme
            // 
            this.cb_theme.FormattingEnabled = true;
            this.cb_theme.Location = new System.Drawing.Point(123, 252);
            this.cb_theme.Name = "cb_theme";
            this.cb_theme.Size = new System.Drawing.Size(121, 21);
            this.cb_theme.TabIndex = 17;
            this.cb_theme.SelectedIndexChanged += new System.EventHandler(this.cb_theme_SelectedIndexChanged);
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 379);
            this.Controls.Add(this.cb_theme);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb_encrypt);
            this.Controls.Add(this.tb_incomingFolder);
            this.Controls.Add(this.l_incomingFolder);
            this.Controls.Add(this.l_info_buffer);
            this.Controls.Add(this.l_info_tcp);
            this.Controls.Add(this.l_info_udp);
            this.Controls.Add(this.l_info_pseudo);
            this.Controls.Add(this.l_info);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_buffer);
            this.Controls.Add(this.b_cancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_tcp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_udp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_pseudo);
            this.Controls.Add(this.b_ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Config";
            this.Text = "Config";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Config_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button b_ok;
        private System.Windows.Forms.Button b_cancel;
        private System.Windows.Forms.Label l_incomingFolder;
        private System.Windows.Forms.Label l_info;
        private System.Windows.Forms.Label l_info_pseudo;
        private System.Windows.Forms.Label l_info_udp;
        private System.Windows.Forms.Label l_info_tcp;
        private System.Windows.Forms.Label l_info_buffer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox tb_incomingFolder;
        public System.Windows.Forms.CheckBox cb_encrypt;
        public System.Windows.Forms.TextBox tb_udp;
        public System.Windows.Forms.TextBox tb_tcp;
        public System.Windows.Forms.TextBox tb_buffer;
        public System.Windows.Forms.ComboBox cb_theme;
        public System.Windows.Forms.TextBox tb_pseudo;
    }
}