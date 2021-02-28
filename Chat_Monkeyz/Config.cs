using System;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.IO;
using Chat_Monkeyz.Properties;

namespace Chat_Monkeyz
{
    public partial class Config : Form
    {
        public bool activated = false;


        public Config()
        {
            InitializeComponent();

            cb_theme.Items.Add("default");
            l_info.Text = "";

            String[] themes = Directory.GetFiles(Application.StartupPath, "*.tcm");

            foreach (String file in themes)
            {
                cb_theme.Items.Add(Path.GetFileNameWithoutExtension(file));
            }


            tb_pseudo.Text = ConfigurationManager.AppSettings["pseudo"];
            tb_tcp.Text = ConfigurationManager.AppSettings["tcpPort"];
            tb_udp.Text = ConfigurationManager.AppSettings["udpPort"];
            tb_buffer.Text = ConfigurationManager.AppSettings["buffer"];
            tb_incomingFolder.Text = ConfigurationManager.AppSettings["incomingFolder"];
            cb_encrypt.Checked = (ConfigurationManager.AppSettings["encrypted"] == "true");


            tb_tcp.TextChanged += tb_tcp_TextChanged;
            tb_udp.TextChanged += tb_udp_TextChanged;
        }

        private void b_ok_Click(object sender, EventArgs e)
        {
            
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["pseudo"].Value = tb_pseudo.Text;
            config.AppSettings.Settings["tcpPort"].Value = tb_tcp.Text;
            config.AppSettings.Settings["udpPort"].Value = tb_udp.Text;
            config.AppSettings.Settings["buffer"].Value = tb_buffer.Text;
            config.AppSettings.Settings["incomingFolder"].Value = tb_incomingFolder.Text;
            config.AppSettings.Settings["encrypted"].Value = ((cb_encrypt.Checked) ? "true" : "false");
            config.AppSettings.Settings["theme"].Value = cb_theme.Items[cb_theme.SelectedIndex].ToString();

            config.Save();
            ConfigurationManager.RefreshSection("appSettings");

            if (Program.pseudo != tb_pseudo.Text)
            {
                Program.pseudo = tb_pseudo.Text;
                Program.myForm.WriteNotice("Vous êtes maintenant " + Program.pseudo, Chat.MessageType.Global | Chat.MessageType.Notice);
            }

            long.TryParse(tb_buffer.Text, out Program.buffersize);

            Close();
        }

        private void b_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }




        private void tb_pseudo_TextChanged(object sender, EventArgs e)
        {
            if (!Chat.CheckPseudo(tb_pseudo.Text))
            {
                l_info_pseudo.Text = "Pseudo invalide";
                b_ok.Enabled = false;
            }
            else
            {
                l_info_pseudo.Text = "";
                b_ok.Enabled = true;
            }
        }


        private void tb_buffer_TextChanged(object sender, EventArgs e)
        {
            int buffer = 0;

            if (!int.TryParse(tb_buffer.Text, out buffer))
            {
                l_info_buffer.Text = "Nombre invalide";
                b_ok.Enabled = false;
            }
            else
            {
                l_info_buffer.Text = "";
                b_ok.Enabled = true;
            }
        }


        private void tb_udp_TextChanged(object sender, EventArgs e)
        {
            int udpPort = 0;

            if (!int.TryParse(tb_udp.Text, out udpPort))
            {
                l_info_udp.Text = "Nombre invalide";
                b_ok.Enabled = false;
            }
            else
            {
                if (udpPort < 1024 || udpPort > 65535)
                {
                    b_ok.Enabled = false;
                    l_info_udp.Text = "Port invalide";
                }
                else
                {
                    b_ok.Enabled = true;
                    l_info_udp.Text = "";
                    l_info.Text = "Le nouveau port sera utilisé après redémarrage";
                }
            }
        }

        private void tb_tcp_TextChanged(object sender, EventArgs e)
        {
            int tcpPort = 0;

            if (!int.TryParse(tb_tcp.Text, out tcpPort))
            {
                l_info_tcp.Text = "Nombre invalide";
                b_ok.Enabled = false;
            }
            else
            {
                if (tcpPort < 1024 || tcpPort > 65535)
                {
                    b_ok.Enabled = false;
                    l_info_tcp.Text = "Port invalide";
                }
                else
                {
                    b_ok.Enabled = true;
                    l_info_tcp.Text = "";
                    l_info.Text = "Le nouveau port sera utilisé après redémarrage";
                }
            }
        }

        private void Config_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }





        private void tb_incomingFolder_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ShowFolderDialog));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            //t.Join();
        }



        private void ShowFolderDialog()
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Program.incomingFolder;

                if (DialogResult.OK == fbd.ShowDialog())
                {
                    //tb_incomingFolder.Text = fbd.SelectedPath;
                    UpdateIncomingFolder(fbd.SelectedPath);
                }
            }
        }

    
        delegate void UpdateIncomingFolderCallback(String path);

        public void UpdateIncomingFolder(String path)
        {
            if (tb_incomingFolder.InvokeRequired)
            {
                UpdateIncomingFolderCallback d = new UpdateIncomingFolderCallback(UpdateIncomingFolder);
                this.Invoke(d, new object[] { path });
            }
            else
            {
                tb_incomingFolder.Text = path;
            }
        }

        private void cb_encrypt_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_encrypt.Checked)
            {
                Program.encrypted = true;

                foreach (Peer p in Program.tabPeer)
                {
                    p.InitKey();
                }
            }
            else
            {
                Program.encrypted = false;
            }
        }

        private void cb_theme_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idxTheme = cb_theme.SelectedIndex;
            String theme = cb_theme.Items[idxTheme].ToString();
            Program.myForm.UpdateTheme(theme);
        }


    }
}
