using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Configuration;
using System.Drawing;
using System.Text.RegularExpressions;
using NAudio.Wave;
using ExtensionMethods;
using System.Xml;


namespace Chat_Monkeyz
{

    public partial class Chat : Torbo.DockableForm
    {

        bool listeningTCP = false, listeningUDP = false, autoPing = false;
        Thread t_listenerTCP, t_listenerUDP, t_autoPing;
        List<ManualResetEvent> doneEvents = new List<ManualResetEvent>();
        IPAddress localIp = IPAddress.None;

        public Sound snd = new Sound();
        public bool voiceActivated = false;
        

        RichTextBox rb_history = new RichTextBox();
        

        Color cMessage = Color.LightGreen;
        Color cMention = Color.LightCoral;
        Color cVoice = Color.LightBlue;
        Color cNotice = Color.LightGray;
        Color cNormal = Color.WhiteSmoke;


        [Flags]
        public enum MessageType { Global = 1, Main = 2, Channel = 4, Current = 8, Notice = 16, Mention = 32, Message = 64, Voice = 128, Normal = 256 };

        public Chat()
        {
            InitializeComponent();
        }



        //Préparation de la fenêtre
        private void Chat_Load(object sender, EventArgs e)
        {
            //fenetres extérieures
            Program.wPeers = new wndPeers();
            Program.wFiles = new wndFiles();
            Program.wConfig = new Config();


            //création des évenements
            Program.wFiles.FormClosing += (_sender, _e) => { b_files.Checked = false; _e.Cancel = true; };
            Program.wPeers.FormClosing += (_sender, _e) => { b_peers.Checked = false; _e.Cancel = true; };
            Program.wConfig.FormClosing += (_sender, _e) => { Program.wConfig.activated = false; Program.wConfig.Hide(); _e.Cancel = true; };


            tb_message.DragDrop += new DragEventHandler(_DragDrop);
            tb_message.DragEnter += new DragEventHandler(_DragEnter);

            rb_affichage.DragDrop += new DragEventHandler(_DragDrop);
            rb_affichage.DragEnter += new DragEventHandler(_DragEnter);

            rb_affichage.TextChanged += rb_affichage_TextChanged;

            //récupération de la configuration
            
            localIp = getLocalIp();



            Program.tabChannel.Add(new Channel("Main"));
            UpdateTabChat("Main", "Main", true);

            //ready to start capture/play
            try
            {
                snd.Init();
            }
            catch (Exception ex)
            {
                Program.Log("sound init - " + ex.Message);
                WriteNotice("Micro indisponible", MessageType.Main);
            }

            WriteNotice("Vous êtes " + Program.pseudo + ". Pour changer de pseudo tapez /nick pseudo" + Environment.NewLine +
                "Pour plus d'aide tapez /help", MessageType.Main);


            //Ecoute des nouveaux clients
            t_listenerTCP = new Thread(new ThreadStart(ListenTCP));
            t_listenerTCP.Start();

            t_listenerUDP = new Thread(new ThreadStart(ListenUDP));
            t_listenerUDP.Start();

            //t_autoPing = new Thread(new ThreadStart(AutoPing));
            //t_autoPing.Start();


            //try auto connect
            ConnectBroadcast();

            //ReadCommands("usercommands.cm");
            LoadConfig();
            tb_message.Focus();
        }


        public void LoadConfig()
        {
            int tcpPort, udpPort;
            long buffersize;
            int.TryParse(ConfigurationManager.AppSettings.Get("tcpPort"), out tcpPort);
            int.TryParse(ConfigurationManager.AppSettings.Get("udpPort"), out udpPort);
            long.TryParse(ConfigurationManager.AppSettings.Get("buffer"), out buffersize);
            String pseudo = ConfigurationManager.AppSettings.Get("pseudo");
            String incomingFolder = ConfigurationManager.AppSettings.Get("incomingFolder");
            String encrypted = ConfigurationManager.AppSettings.Get("encrypted");
            String safeMode = ConfigurationManager.AppSettings.Get("safeMode");
            String theme = ConfigurationManager.AppSettings.Get("theme");

            if (tcpPort > 0 && tcpPort < 65336) Program.tcpPort = tcpPort;
            if (udpPort > 0 && udpPort < 65336) Program.udpPort = udpPort;
            if (buffersize > 0) Program.buffersize = buffersize;

            if (!String.IsNullOrWhiteSpace(pseudo)) Program.pseudo = pseudo;
            if (incomingFolder != String.Empty && Directory.Exists(incomingFolder)) Program.incomingFolder = incomingFolder;
            else Program.incomingFolder = Application.StartupPath;
            

            if (encrypted != null) Program.encrypted = (encrypted.ToLower() == "true");
            if (safeMode != null) Program.safeMode = (safeMode.ToLower() == "true");

            UpdateTheme(theme);
        }


        public void UpdateTheme(String theme)
        {
            if (theme != null && !theme.EndsWith(".tcm")) theme += ".tcm";
            if (File.Exists(theme)) Program.currentTheme = Theme.CreateThemeFromFile(theme);
            else Program.currentTheme = Program.defaultTheme.Clone();
            

            
            this.Font = Program.currentTheme.font;
            this.Size = Program.currentTheme.size;
            this.ForeColor = Program.currentTheme.fontcolor;
            this.BackColor = Program.currentTheme.backcolor;

            this.tb_message.BackColor = Program.currentTheme.backcolor;
            this.tb_message.ForeColor = Program.currentTheme.fontcolor;

            this.rb_affichage.BackColor = Program.currentTheme.backcolor;
            this.rb_affichage.ForeColor = Program.currentTheme.fontcolor;


            Program.wConfig.Font = Program.currentTheme.font;
            Program.wConfig.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.ForeColor = Program.currentTheme.fontcolor;

            Program.wConfig.tb_buffer.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.tb_buffer.ForeColor = Program.currentTheme.fontcolor;
            Program.wConfig.tb_incomingFolder.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.tb_incomingFolder.ForeColor = Program.currentTheme.fontcolor;
            Program.wConfig.tb_pseudo.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.tb_pseudo.ForeColor = Program.currentTheme.fontcolor;
            Program.wConfig.tb_tcp.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.tb_tcp.ForeColor = Program.currentTheme.fontcolor;
            Program.wConfig.tb_udp.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.tb_udp.ForeColor = Program.currentTheme.fontcolor;

            Program.wConfig.cb_theme.BackColor = Program.currentTheme.backcolor;
            Program.wConfig.cb_theme.ForeColor = Program.currentTheme.fontcolor;


            Program.wPeers.Font = Program.currentTheme.font;
            Program.wPeers.BackColor = Program.currentTheme.backcolor;
            Program.wPeers.ForeColor = Program.currentTheme.fontcolor;

            Program.wFiles.Font = Program.currentTheme.font;
            Program.wFiles.BackColor = Program.currentTheme.backcolor;
            Program.wFiles.ForeColor = Program.currentTheme.fontcolor;
            Program.wFiles.g_files.BackgroundColor = Program.currentTheme.backcolor;
            Program.wFiles.g_files.ForeColor = Program.currentTheme.fontcolor;
            Program.wFiles.g_files.ColumnHeadersDefaultCellStyle.BackColor = Program.currentTheme.backcolor;
            Program.wFiles.g_files.ColumnHeadersDefaultCellStyle.ForeColor = Program.currentTheme.fontcolor;


            DrawTabControl(Program.currentTheme.backcolor);
            Program.wPeers.userControl11.UpdateBackgroundColor(Program.currentTheme.backcolor);
        }

        private void DrawTabControl(Color backcolor)
        {
            Graphics g = tabChat.CreateGraphics();
            Rectangle r = tabChat.ClientRectangle;

            int tabsWidth = tabChat.ItemSize.Width * tabChat.TabPages.Count + 2;
            r.Width = r.Width - tabsWidth;
            r.X = tabsWidth;
            g.FillRectangle(new SolidBrush(backcolor), r);
        }



        private void rb_affichage_TextChanged(object sender, EventArgs e)
        {
            rb_affichage.SelectionStart = rb_affichage.TextLength;
            rb_affichage.ScrollToCaret();
        }



        #region Connexion


        /// <summary>
        /// Envoie un paquet UDP en broadcast demandant la connexion sur le port TCP en écoute.
        /// </summary>
        private void ConnectBroadcast()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, Program.udpPort);

            Byte[] b = BitConverter.GetBytes(Program.tcpPort);
            sock.SendTo(b, iep);
            sock.Close();
        }


        /// <summary>
        /// Ecoute les paquets UDP. A la réception d'un paquet le programme se connecte à l'émetteur au port indiqué
        /// </summary>
        private void ListenUDP()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint ep = (EndPoint)new IPEndPoint(IPAddress.Any, Program.udpPort);

            try
            {
                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                sock.Bind(ep);
                listeningUDP = true;
            }
            catch (Exception e)
            {
                Program.Log("Listen UDP - " + e.Message);
            }


            if (!listeningUDP)
                WriteNotice("Le serveur UDP n'a pas pu démarrer", MessageType.Global);



            while (listeningUDP)
            {

                Byte[] data = new Byte[4];
                int recv = sock.EndReceiveFrom(sock.BeginReceiveFrom(data, 0, data.Length, SocketFlags.None, ref ep, delegate { }, new Object()), ref ep);


                if (recv > 0)
                {
                    IPAddress ip;
                    int port;
                    Peer.GetAddressFromEndPoint(ep.ToString(), out ip, out port);
                    port = BitConverter.ToUInt16(data, 0);

                    Connect(ip, port);
                }
            }

            sock.Close();
        }


        /// <summary>
        /// Se connecte en TCP à l'ip/port indiqué. Si la connexion réussi, un objet Client est créé
        /// </summary>
        /// <param name="ip">ip du client</param>
        /// <param name="port">port TCP (facultatif)</param>
        /// <returns></returns>
        public bool Connect(IPAddress ip, int port)
        {
            bool connected = false;
            if (port <= 1 || port >= 65536) port = Program.tcpPort;

            if (GetPeerIndexByIp(ip) == -1 && !IsLocalAddress(ip))
            {
                TcpClient tc = new TcpClient();

                try
                {
                    tc.Connect(ip, port);
                    connected = true;
                }
                catch (Exception e)
                {
                    Program.Log("connect() - " + e.Message);
                    connected = false;
                }

                if (connected)
                {
                    ManualResetEvent mre = new ManualResetEvent(false);
                    Peer p = new Peer(tc, mre);

                    SendToAll(new DataAnnounce(p.EndPoint.ToString()));
                    Program.tabPeer.Add(p);

                    doneEvents.Add(mre);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate { p.ReceiveData(); }));
                }
            }

            return connected;
        }


        /// <summary>
        /// Attend une connexion TCP. Lors de la connexion, un objet Client est créé
        /// </summary>
        private void ListenTCP()
        {
            TcpListener tl;

            tl = new TcpListener(new IPEndPoint(IPAddress.Any, Program.tcpPort));

            try
            {
                tl.Start();
                listeningTCP = true;
            }
            catch (Exception e)
            {
                Program.Log("listenTCP() - " + e.Message);
                WriteNotice("Le serveur TCP n'a pas pu démarrer", MessageType.Global);
            }

            while (listeningTCP)
            {
                ManualResetEvent mre = new ManualResetEvent(false);
                TcpClient tc = tl.EndAcceptTcpClient(tl.BeginAcceptTcpClient(delegate { }, new Object()));

                IPAddress ip;
                int port;

                Peer.GetAddressFromEndPoint(tc.Client.RemoteEndPoint.ToString(), out ip, out port);

                //Connect(ip, 0);


                if (GetPeerIndexByIp(ip) == -1 && !IsLocalAddress(ip))
                {
                    Peer p = new Peer(tc, mre);
                    SendToAll(new DataAnnounce(p.EndPoint.ToString()));
                    Program.tabPeer.Add(p);

                    doneEvents.Add(mre);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate { p.ReceiveData(); }));
                }

            }

            tl.Stop();
            t_listenerTCP.Abort();
        }

        #endregion


        #region recherche


        public bool IsPeerInCurrentChannel(String idPeer)
        {
            bool found = false;
            int idx = 0;

            for (int n = Program.tabChannel[Program.currentChannel].tabPeer.Count; idx < n && !found; idx++)
            {
                if (Program.tabChannel[Program.currentChannel].tabPeer[idx].IsAlive && Program.tabChannel[Program.currentChannel].tabPeer[idx].ID == idPeer)
                    found = true;
            }

            return found;
        }

        public int GetChanIndexById(String idChan)
        {
            bool found = false;
            int idx = 0;
            idChan = idChan.ToLower();

            for (int n = Program.tabChannel.Count; idx < n && !found; idx++)
            {
                if (Program.tabChannel[idx].name.ToLower() == idChan)
                    found = true;
            }

            return (found) ? --idx : -1;
        }

        /// <summary>
        /// Obtient l'index du Client dans tabClient. -1 signifie que le client n'existe pas.
        /// </summary>
        /// <param name="clientName">Pseudo recherché</param>
        public int GetPeerIndexByPseudo(String pseudo)
        {
            bool found = false;
            int idx = 0;

            for (int n = Program.tabPeer.Count; idx < n && !found; idx++)
            {
                if (Program.tabPeer[idx].IsAlive && Program.tabPeer[idx].Pseudo == pseudo)
                    found = true;
            }

            return (found) ? --idx : -1;
        }



        /// <summary>
        /// Obtient l'index du Client dans tabClient. -1 signifie que le client n'existe pas.
        /// </summary>
        /// <param name="clientName">id recherché</param>
        public int GetPeerIndexById(String idPeer)
        {
            bool found = false;
            int idx = 0;

            for (int n = Program.tabPeer.Count; idx < n && !found; idx++)
            {
                if (Program.tabPeer[idx].IsAlive && Program.tabPeer[idx].ID == idPeer)
                    found = true;
            }

            return (found) ? --idx : -1;
        }


        /// <summary>
        /// Obtient l'index du Client dans tabClient. -1 signifie que le client n'existe pas.
        /// </summary>
        /// <param name="clientName">IP recherché</param>
        public int GetPeerIndexByIp(IPAddress ip)
        {
            bool found = false;
            int idx = 0;

            for (int n = Program.tabPeer.Count; idx < n && !found; idx++)
            {
                if (Program.tabPeer[idx].IsAlive && Program.tabPeer[idx].IP.Equals(ip))
                    found = true;
            }

            return (found) ? --idx : -1;
        }



        /// <summary>
        /// Vérifie si l'IP fait partie des IP locales
        /// </summary>
        /// <param name="ip">L'IP a vérifier</param>
        /// <returns></returns>
        private bool IsLocalAddress(IPAddress ip)
        {
            bool isLocalAddress = false;

            if (ip.Equals(IPAddress.Loopback) || ip.Equals(IPAddress.IPv6Loopback))
                isLocalAddress = true;
            else
            {
                IPAddress[] tab_ip = Dns.GetHostEntry(Environment.MachineName).AddressList;

                for (int idx = 0, n = tab_ip.Length; idx < n && !isLocalAddress; idx++)
                {
                    if (ip.Equals(tab_ip[idx]))
                        isLocalAddress = true;
                }
            }

            return isLocalAddress;
        }


        #endregion


        #region Fonctions thread safe



        delegate void UpdateTabChatCallback(String idTab, String title, bool add);


        public void UpdateTabChat(String idTab, String title, bool add)
        {
            if (tabChat.InvokeRequired)
            {
                UpdateTabChatCallback d = new UpdateTabChatCallback(UpdateTabChat);
                this.Invoke(d, new object[] { idTab, title, add });
            }
            else
            {
                if (add)
                {
                    tabChat.TabPages.Add(idTab, title);
                }
                else
                {
                    int idxTab = GetChanIndexById(idTab);

                    if (idxTab > -1)
                    {
                        tabChat.SelectedIndex = idxTab - 1;
                        tabChat.TabPages.RemoveAt(idxTab);
                    }

                }
            }
        }


        delegate int TabChatSelectedIndexCallback(int idxTab);

        /// <summary>
        /// Obtient ou définit l'index de l'onglet courant. (Thread Safe)
        /// </summary>
        /// <param name="idxTab">Si indiqué, bascule vers l'onglet</param>
        /// <returns></returns>
        public int TabChatSelectedIndex(int idxTab = -1)
        {
            if (tabChat.InvokeRequired)
            {
                TabChatSelectedIndexCallback d = new TabChatSelectedIndexCallback(TabChatSelectedIndex);
                return (int)this.Invoke(d, new object[] { idxTab });
            }
            else
            {
                if (idxTab > -1 && idxTab < tabChat.TabPages.Count)
                    tabChat.SelectedIndex = idxTab;

                return tabChat.SelectedIndex;
            }
        }


        delegate void TabChatTitleCallback(int idxTab, String title);


        public void TabChatTitle(int idxTab, String title)
        {
            
            if (tabChat.InvokeRequired)
            {
                TabChatTitleCallback d = new TabChatTitleCallback(TabChatTitle);
                this.Invoke(d, new object[] { idxTab, title });
            }
            else
            {
                tabChat.TabPages[idxTab].Text = title;
            }
        }


         delegate void RefreshFilesListCallback();


         public void RefreshFilesList()
         {

             if (Program.wFiles.g_files.InvokeRequired)
             {
                 RefreshFilesListCallback d = new RefreshFilesListCallback(RefreshFilesList);
                 this.Invoke(d, new object[] { });
             }
             else
             {
                 Program.wFiles.RefreshList();
             }
         }


         delegate void RefreshPeersListCallback();

         public void RefreshPeersList()
         {
             this.Invoke(new RefreshPeersListCallback(() =>
             {
                 Program.wPeers.RefreshList();
             }));

         }




        delegate void PaintTabCallback(String idChan, Color backcolor, Color forecolor);


        public void PaintTab(String idChan, Color backcolor, Color forecolor)
        {
            if (tabChat.InvokeRequired)
            {
                PaintTabCallback d = new PaintTabCallback(PaintTab);
                this.Invoke(d, new object[] { idChan, backcolor, forecolor });
            }
            else
            {
                int idxChan = GetChanIndexById(idChan);
                tabChat.TabPages[idxChan].BackColor = backcolor;
                tabChat.TabPages[idxChan].ForeColor = forecolor;
                tabChat.Refresh();
            }
        }



        #endregion


        #region Evenements de l'interface

        private void b_peers_CheckedChanged(object sender, EventArgs e)
        {
            if (!Program.wPeers.activated)
            {
                Program.wPeers.Show();
                Program.myForm.RefreshPeersList();
                ConnectForm(Program.wPeers);
                Program.wPeers.activated = true;
            }
            else
            {
                DisconnectForm(Program.wPeers);
                Program.wPeers.Hide();
                Program.wPeers.activated = false;
            }

        }

        private void b_files_CheckedChanged(object sender, EventArgs e)
        {
            if (!Program.wFiles.activated)
            {
                Program.wFiles.Show();
                Program.myForm.RefreshFilesList();
                ConnectForm(Program.wFiles);
                Program.wFiles.activated = true;
            }
            else
            {
                DisconnectForm(Program.wFiles);
                Program.wFiles.Hide();
                Program.wFiles.activated = false;
            }
        }



        private void Chat_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Tab)
            {
                if (tabChat.SelectedIndex == tabChat.TabCount - 1)
                    tabChat.SelectedIndex = 0;
                else
                    tabChat.SelectedIndex++;

                tb_message.Focus();
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Tab)
            {
                if (tabChat.SelectedIndex == 0)
                    tabChat.SelectedIndex = tabChat.TabCount - 1;
                else
                    tabChat.SelectedIndex--;

                tb_message.Focus();
                e.Handled = true;
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.W)
            {
                if (tabChat.SelectedIndex == 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("Fermer Chat_Monkeyz ?", "Deconnexion", MessageBoxButtons.YesNo))
                        Close();
                }
                else
                {
                    String idChan = tabChat.SelectedTab.Name;
                    int idxChan = GetChanIndexById(idChan);

                    if (idxChan > -1)
                    {
                        foreach (Peer p in Program.tabChannel[idxChan].tabPeer)
                        {
                            p.SendData(new DataChannel { channel = idChan, option = ChannelOption.Leave});
                        }
                    }

                    UpdateTabChat(idChan, "", false);
                    Program.tabChannel.RemoveAt(idxChan);
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Hide();
                systray.Visible = true;
            }
        }


        private void tb_message_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')//(char)13)
            {
                String cmd = tb_message.Text;
                e.Handled = true;
                tb_message.Clear();

                handleCmd(cmd);
            }
            else if (e.KeyChar == '\t')//(char)9)
            {
                Autocomplete();
                e.Handled = true;
            }
        }



        int previousTab = 0;

        private void tabChat_SelectedIndexChanged(object sender, EventArgs e)
        {
            int newTab = TabChatSelectedIndex();
            String currentChatHistory = rb_affichage.Rtf;

            int idxPreviousChan = GetChanIndexById(tabChat.TabPages[previousTab].Name);
            int idxNewChan = GetChanIndexById(tabChat.TabPages[newTab].Name);

            Program.tabChannel[idxPreviousChan].history = currentChatHistory;

            rb_affichage.Clear();
            rb_affichage.Rtf = Program.tabChannel[idxNewChan].history;


            tb_message.Focus();

            NotifyTab(tabChat.TabPages[newTab].Name, MessageType.Normal);
            Program.tabChannel[idxNewChan].newMessages = 0;

            previousTab = newTab;
            Program.currentChannel = idxNewChan;



            //update peer list
            Program.myForm.RefreshPeersList();
        }


        private void searchPeersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectBroadcast();
            WriteNotice("Searching for peers...", MessageType.Current);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String endpoint = Prompt.ShowDialog("Type the peer's IP (ex : 192.168.1.10[:1337] )", "Connect to peer");

            if (!String.IsNullOrWhiteSpace(endpoint))
            {
                IPAddress ip;
                int port;

                Peer.GetAddressFromEndPoint(endpoint, out ip, out port);
                Connect(ip, port);
            }
        }

        private void showTransfertsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.wFiles.Show();
        }


        private void _DragDrop(object sender, DragEventArgs e)
        {
            String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
            sFile file = new sFile(files[0]);
            file.channel = Program.tabChannel[Program.currentChannel].name;

            foreach (Peer p in Program.tabChannel[Program.currentChannel].tabPeer)
            {
                p.SendData(new DataFile { fileinfo = file });
            }
        }

        private void _DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }



        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.wConfig.activated)
            {
                Program.wConfig.Show();
                Program.wConfig.activated = false;
            }
        }

        private void sendFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ShowOpenDialog));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }


        private void b_voice_CheckedChanged(object sender, EventArgs e)
        {

            if (snd.ready)
            {
                voiceActivated = !voiceActivated;

                if (voiceActivated)
                {
                    snd.StartRecord();
                    snd.DataReceived += s_DataReceived;
                    WriteNotice("Voix activée", MessageType.Global);
                }
                else
                {
                    snd.StopRecord();
                    snd.DataReceived -= s_DataReceived;
                    WriteNotice("Voix désactivée", MessageType.Global);
                }
            }
        }


        #endregion


        public void WriteNotice(String text, MessageType ms) { WriteText("", text, "", ms | MessageType.Notice); }

        public void WriteNotice(String text, String idChannel, MessageType ms) { WriteText("", text, idChannel, ms | MessageType.Notice); }

        public void WriteText(String pseudo, String text, String idChannel, MessageType ms)
        {

            String txt = Environment.NewLine + "[" + GetTime() + "] ";
            if (!String.IsNullOrEmpty(pseudo)) txt += pseudo + " : ";
            txt += text;


            if (ms.HasFlag(MessageType.Global))
            {
                int idxChan = TabChatSelectedIndex();
                String chanName = tabChat.TabPages[idxChan].Name;

                chanName = Program.tabChannel[idxChan].name;


                foreach (Channel c in Program.tabChannel)
                {
                    if (c.name == chanName)
                    {
                        rb_affichage.addMessage(txt, ms);
                    }
                    else
                    {
                        rb_history.setRTF(c.history);


                        if (c.newMessages >= 10 && (DateTime.Now - c.lastMessage) >= TimeSpan.FromMinutes(5))
                        {
                            c.newMessages = 0;
                            rb_history.addLine();
                        }

                        c.newMessages++;
                        c.lastMessage = DateTime.Now;


                        rb_history.addMessage(txt, ms);
                        c.history = rb_history.getRTF();
                        NotifyTab(c.name, ms);
                    }
                }
            }
            else
            {

                if (ms.HasFlag(MessageType.Main))
                {
                    if (TabChatSelectedIndex() == 0 && !ms.HasFlag(MessageType.Current))
                    {
                        rb_affichage.addMessage(txt, ms);
                    }
                    else
                    {
                        rb_history.setRTF(Program.tabChannel[0].history);


                        if (Program.tabChannel[0].newMessages >= 10 && (DateTime.Now - Program.tabChannel[0].lastMessage) >= TimeSpan.FromMinutes(5))
                        {
                            Program.tabChannel[0].newMessages = 0;
                            rb_history.addLine();
                        }

                        rb_history.addMessage(txt, ms);

                        Program.tabChannel[0].history = rb_history.getRTF();
                        NotifyTab(Program.tabChannel[0].name, ms);

                    }
                }



                if (ms.HasFlag(MessageType.Channel))
                {

                    int idxChan = GetChanIndexById(idChannel);

                    if (ms.HasFlag(MessageType.Notice))
                    {
                        if (idxChan > -1)
                        {
                            if (TabChatSelectedIndex() == idxChan)
                            {
                                rb_affichage.addMessage(txt, ms);
                            }
                            else
                            {
                                rb_history.setRTF(Program.tabChannel[idxChan].history);

                                if (Program.tabChannel[idxChan].newMessages >= 10 && (DateTime.Now - Program.tabChannel[idxChan].lastMessage) >= TimeSpan.FromMinutes(5))
                                {
                                    Program.tabChannel[idxChan].newMessages = 0;
                                    rb_history.addLine();
                                }


                                rb_history.addMessage(txt, ms);
                                Program.tabChannel[idxChan].history = rb_history.getRTF();

                                NotifyTab(Program.tabChannel[idxChan].name, ms);
                            }
                        }
                    }
                    else
                    {
                        if (idxChan == -1 && !String.IsNullOrWhiteSpace(idChannel))
                        {
                            int idxPeer = GetPeerIndexById(idChannel);
                            bool isPrivate = (idxPeer != -1);

                            Channel c = new Channel(idChannel, isPrivate);
                            if (isPrivate) c.tabPeer.Add(Program.tabPeer[idxPeer]);

                            Program.tabChannel.Add(c);
                            UpdateTabChat(idChannel, pseudo, true);
                            idxChan = GetChanIndexById(idChannel);
                        }


                        if (idxChan > -1)
                        {
                            if (TabChatSelectedIndex() == idxChan)
                            {
                                rb_affichage.addMessage(txt, ms);
                            }
                            else
                            {
                                rb_history.setRTF(Program.tabChannel[idxChan].history);

                                if (Program.tabChannel[idxChan].newMessages >= 10 && (DateTime.Now - Program.tabChannel[idxChan].lastMessage) >= TimeSpan.FromMinutes(5))
                                {
                                    Program.tabChannel[idxChan].newMessages = 0;
                                    rb_history.addLine();
                                }

                                rb_history.addMessage(txt, ms);
                                Program.tabChannel[idxChan].history = rb_history.getRTF();

                                NotifyTab(Program.tabChannel[idxChan].name, ms);
                            }
                        }

                    }

                }


                if (ms.HasFlag(MessageType.Current))
                {
                    rb_affichage.addMessage(txt, ms);
                }
            }

        }


        private void handleCmd(String text)
        {
            String[] tabText = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (text == "/clear")
            {
                rb_affichage.Clear();
            }
            else if (text == "/clearall")
            {
                rb_affichage.Clear();

                foreach (Channel c in Program.tabChannel) c.history = "";
            }

                // /ip : fournit l'ip du peer recherché ou l'ip locale
            else if (text.StartsWith("/ip"))
            {
                if (tabText.Length > 1)
                {
                    int idxPeer = GetPeerIndexByPseudo(tabText[1]);
                    if (idxPeer > -1)
                        WriteNotice(Program.tabPeer[idxPeer].Pseudo + " : " + Program.tabPeer[idxPeer].EndPoint.ToString(), MessageType.Current);
                }
                else
                    WriteNotice("Votre IP : " + localIp.ToString(), MessageType.Current);
            }

                // /connect : connexion manuelle à l'ip spécifiée ou en broadcast
            else if (text.StartsWith("/connect"))
            {
                if (tabText.Length > 1)
                {
                    IPAddress ip;
                    int port;

                    Peer.GetAddressFromEndPoint(tabText[1], out ip, out port);
                    Connect(ip, port);

                }
                else
                    ConnectBroadcast();
            }

                // /ignore / /unignore ignore ou designore un peer
            else if (text.StartsWith("/ignore ") || text.StartsWith("/unignore "))
            {
                if (tabText.Length > 1)
                {
                    int idxPeer = GetPeerIndexByPseudo(tabText[1]);
                    bool ignore = (text.StartsWith("/ignore "));

                    if (idxPeer > -1)
                    {
                        WriteNotice(Program.tabPeer[idxPeer].Pseudo + ((ignore) ? " est ignoré" : " n'est plus ignoré"), MessageType.Current | MessageType.Main);
                        Program.tabPeer[idxPeer].IsIgnored = ignore;
                    }

                    Program.myForm.RefreshPeersList();
                }
                else
                    WriteNotice("Usage : /ignore Anon", MessageType.Current);
            }
            else if (text == "/about")
            {
                WriteNotice("Version de Chat_Monkeyz : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine +
                "Under license : Don't break my balls with licenses. Ce projet est open source, libre, gratuit, etc" + Environment.NewLine +
                "Développeur : Vincent Marie, aka VinceMonkeyz" + Environment.NewLine +
                "Remerciements : Heineken" + Environment.NewLine +
                "Questions, critiques, gâteaux, propositions malsaines : ghost80160@hotmail.com", MessageType.Current);
            }
            // /nick : change de pseudo
            else if (text.StartsWith("/nick ") || text.StartsWith("/pseudo "))
            {
                if (tabText.Length > 1)
                {
                    if (CheckPseudo(tabText[1]))
                    {
                        String newpseudo = tabText[1];

                        SendToAll(new DataChangeNick { newPseudo = newpseudo });
                        WriteNotice("Vous êtes maintenant " + newpseudo, MessageType.Global);
                        Program.pseudo = newpseudo;
                    }
                    else
                    {
                        WriteNotice("Pseudo invalide", MessageType.Current);
                    }
                }
                else
                    WriteNotice("Usage : /ignore Anon", MessageType.Current);

            }

                // /send envoie un fichier à un peer ou a tous
            else if (text.StartsWith("/send "))
            {
                String filename = tabText[1];

                if (File.Exists(filename))
                {
                    String path = Path.GetFullPath(filename);
                    sFile file = new sFile(path);
                    file.channel = Program.tabChannel[Program.currentChannel].name;


                    if (tabChat.SelectedIndex == 0)
                    {
                        SendToAll(new DataFile { fileinfo = file });
                    }
                    else
                    {

                        int idxPeer = GetPeerIndexById(tabChat.SelectedTab.Name);
                        if (idxPeer > -1)
                            Program.tabPeer[idxPeer].SendData(new DataFile { fileinfo = file });
                    }
                }
                else
                    WriteNotice("Fichier introuvable", MessageType.Current);

            }
            else if (text == "/exit")
            {
                Close();
            }
            else if (text == "/close")
            {
                int idxChan = TabChatSelectedIndex();
                if (idxChan != 0)
                {
                    String idChan = tabChat.TabPages[idxChan].Name;
                    Program.tabChannel[idxChan].history = "";

                    foreach (Peer p in Program.tabChannel[idxChan].tabPeer)
                    {
                        p.SendData(new DataChannel { channel = idChan, option = ChannelOption.Leave });
                    }

                    UpdateTabChat(idChan, "", false);
                    Program.tabChannel.RemoveAt(idxChan);
                }
            }
            else if (text == "/config")
            {
                if (!Program.wConfig.activated)
                {
                    Program.wConfig.Show();
                    Program.wConfig.activated = false;
                }
                else
                {
                    Program.wConfig.Focus();
                }
            }
            else if (text.StartsWith("/mp "))
            {
                if (tabText.Length > 1)
                {
                    int idxPeer = GetPeerIndexByPseudo(tabText[1]);

                    if (idxPeer > -1 && Program.tabPeer[idxPeer].IsAlive)
                    {
                        if (tabChat.TabPages.IndexOfKey(Program.tabPeer[idxPeer].ID) == -1)
                        {
                            Channel c = new Channel(Program.tabPeer[idxPeer].ID, true);
                            c.tabPeer.Add(Program.tabPeer[idxPeer]);
                            Program.tabChannel.Add(c);
                            UpdateTabChat(Program.tabPeer[idxPeer].ID, tabText[1], true);
                        }
                        tabChat.SelectedIndex = tabChat.TabPages.IndexOfKey(Program.tabPeer[idxPeer].ID);

                        if (tabText.Length > 2)
                        {
                            String msg = String.Join(" ", tabText, 2, tabText.Length - 2);
                            WriteText(Program.pseudo, msg, Program.tabPeer[idxPeer].ID, MessageType.Channel);
                            Program.tabPeer[idxPeer].SendData(new DataMessage { channel = Program.tabPeer[idxPeer].ID, msg = msg });
                        }
                    }
                    else
                        WriteNotice(tabText[1] + " n'est pas présent", MessageType.Current);
                }
                else
                    WriteNotice("Usage : /mp Anon hi", MessageType.Current);


            }
            else if (text.StartsWith("/join "))
            {

                if (tabText.Length > 1)
                {
                    int idxChan = GetChanIndexById(tabText[1]);

                    if (idxChan == -1)
                    {
                        Program.tabChannel.Add(new Channel(tabText[1]));
                        UpdateTabChat(tabText[1], tabText[1], true);

                        TabChatSelectedIndex(Program.tabChannel.Count - 1);
                        SendToAll(new DataChannel { channel = tabText[1], option = ChannelOption.Join });
                    }
                    else
                        TabChatSelectedIndex(idxChan);
                }
                else
                    WriteNotice("Usage : /join channel", MessageType.Current);

            }
            else if (text == "/peers")
            {
                String strPeers = "";
                foreach (Peer p in Program.tabPeer) strPeers += p.Pseudo + ", ";

                WriteNotice(strPeers, MessageType.Current);
            }
            else if (text == "/files")
            {
                String strFiles = "";
                foreach (Peer p in Program.tabPeer)
                {
                    strFiles += p.Pseudo + " : ";
                    foreach (sFile file in p.tabFile)
                    {
                        strFiles += file.ToString() + ",";
                    }
                    strFiles += Environment.NewLine;
                }
            }
            else if (text.StartsWith("/reset"))
            {
                if (text == "/reset" || tabText[1] == "peers")
                {
                    while (Program.tabPeer.Count > 0)
                        Program.tabPeer[Program.tabPeer.Count - 1].Kill();
                }

                if (text == "/reset" || tabText[1] == "files")
                {
                    foreach (Peer p in Program.tabPeer)
                    {
                        p.tabFile.Clear();
                    }
                }

            }
            else if (text.StartsWith("/encrypt "))
            {
                if (tabText.Length > 1)
                {
                    if (tabText[1] == "on")
                    {
                        Program.encrypted = true;
                        foreach (Peer p in Program.tabPeer)
                        {
                            p.InitKey();
                        }
                    }
                    else if (tabText[1] == "off")
                    {
                        Program.encrypted = false;
                    }
                }
            }
            else if (text.StartsWith("/exec"))
            {
                if (tabText.Length > 1)
                {
                    ReadCommands(tabText[1]);
                }
                else
                    WriteNotice("Usage : /exec <filename>", MessageType.Current);
            }
            else if (text.StartsWith("/help"))
            {
                if (tabText.Length > 1)
                {
                    String ret;
                    switch (tabText[1])
                    {
                        case "clear": ret = "Vide l'historique des messages."; break;
                        case "clearall": ret = "Vide l'historique de tout les onglets."; break;
                        case "ip": ret = "Affiche l'ip d'un peer. Usage: /ip <pseudo>"; break;
                        case "mp": ret = "Ouvre une session de chat privé. Usage: /mp <pseudo> [<message>]"; break;
                        case "connect": ret = "Tente la connexion à un réseau/peer. Usage: /connect [<ip>[:<port>]]"; break;
                        case "ignore": ret = "Ignore un peer. Ex: /ignore <pseudo>"; break;
                        case "unignore": ret = "Arrête d'ignorer un peer. Ex: /unignore <pseudo>"; break;
                        case "about": ret = "Encore en test"; break;
                        case "nick": ret = "Change votre pseudo. Ex: /nick <pseudo>"; break;
                        case "send": ret = "Envoi un fichier à un peer ou aux peers d'un canal. Ex: /send <filename>"; break;
                        case "close": ret = "Ferme l'onglet ou Chat_Monkeyz"; break;
                        case "exit": ret = "Ferme l'onglet ou Chat_Monkeyz"; break;
                        case "config": ret = "Ouvre les paramètres"; break;
                        case "join": ret = "Ouvre ou rejoint un canal. Usage: /join <channel>"; break;
                        case "help": ret = "Affiche l'aide. Ex: /help [<commande>]"; break;
                        case "me": ret = "Envoi un message a la 3eme personne. Ex: /me <message>"; break;
                        default: ret = "Commande inconnue."; break;
                    }
                    WriteNotice(ret, MessageType.Current);
                }
                else
                    WriteNotice("Commandes disponibles : clear, clearall, ip, mp, connect, ignore, unignore, about, " +
                                "nick, send, close, exit, config, join, help, me" + Environment.NewLine +
                                "Tapez /help <commande> pour plus de détail", MessageType.Current);

            }
            else
            {
                //Cas de message classique

                if (text.Length > 0)
                {

                    String chanName = Program.tabChannel[Program.currentChannel].name;


                    if (tabChat.SelectedIndex == 0)
                    {
                        SendToAll(new DataMessage { msg = text, channel = chanName });
                    }
                    else
                    {
                        //int idxPeer = GetPeerIndexById(chanName);

                        foreach (Peer p in Program.tabChannel[Program.currentChannel].tabPeer)
                        {
                            p.SendData(new DataMessage { msg = text, channel = chanName });
                        }
                    }

                    WriteText(Program.pseudo, text, Program.tabChannel[Program.currentChannel].name, MessageType.Current);

                }
            }


        }



        private void Autocomplete()
        {
            String msg = tb_message.Text;
            String strSearch, message = "";
            int idx = msg.LastIndexOf(' ');

            //get message (what the user is typing without the pseudo) and pseudo
            if (idx != -1)
            {
                strSearch = msg.Substring(idx + 1);
                message = msg.Substring(0, idx + 1);
            }
            else
                strSearch = msg;

            strSearch = strSearch.ToLower();

            
            //liste des fichiers dans le dossier courant
            if (message == "/send ")
            {
                String[] files = Directory.GetFiles(Application.StartupPath, strSearch + "*");

                for (int i = 0, n = files.Length; i < n; i++)
                {
                    files[i] = Path.GetFileName(files[i]);
                }

                if (files.Length == 1)
                {
                    tb_message.Text = message + files[0] + " ";
                    tb_message.Select(tb_message.TextLength, 0);
                }
                else if (files.Length > 1)
                {
                    WriteNotice(String.Join(", ", files), MessageType.Current);
                }
            }
            else
            {

                List<String> tabPseudo = new List<String>(Program.tabPeer.Count);

                foreach (Peer p in Program.tabPeer)
                {
                    if (p.IsAlive && p.Pseudo.ToLower().StartsWith(strSearch))
                        tabPseudo.Add(p.Pseudo);
                }


                if (tabPseudo.Count == 1)
                {
                    tb_message.Text = message + tabPseudo[0] + " ";
                    tb_message.Select(tb_message.TextLength, 0);
                }
                else if (tabPseudo.Count > 1)
                {
                    WriteNotice(String.Join(", ", tabPseudo.ToArray()), MessageType.Current);
                }
            }
        }



        /// <summary>
        /// Retourne l'heure sous la forme hh:mm
        /// </summary>
        public String GetTime()
        {
            return DateTime.Now.ToShortTimeString();
        }



        private String BytesToString(Byte[] b)
        {
            String str = "";
            for (int i = 0, n = b.Length; i < n; i++) str += b[i] + "-";
            str = str.Remove(str.Length - 2);

            return str;
        }

        private Byte[] StringToBytes(String str)
        {
            String[] tabstr = str.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            Byte[] b = new Byte[tabstr.Length];
            for (int i = 0, n = tabstr.Length; i < n; i++) b[i] = Byte.Parse(tabstr[i]);

            return b;
        }


        public void SendToAll(Data data)
        {
            foreach (Peer p in Program.tabPeer)
            {
                if (p.IsAlive) p.SendData(data);
            }
        }



        private void ShowOpenDialog()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    String path = ofd.FileName;
                    sFile f = new sFile(path);
                    f.channel = Program.tabChannel[Program.currentChannel].name;

                    DataFile df = new DataFile { fileinfo = f };

                    foreach (Peer p in Program.tabChannel[Program.currentChannel].tabPeer)
                    {
                        p.SendData(df);
                        p.tabFile.Add(f);
                    }
                }
            }
        }


        public IPAddress getLocalIp()
        {
            IPAddress ip = IPAddress.None;

            foreach (IPAddress tmpIP in Dns.GetHostEntry(Environment.MachineName).AddressList)
            {
                if (ip.Equals(IPAddress.None) && tmpIP.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
                    ip = tmpIP;
            }

            return ip;
        }


        public void NotifyTab(String idChan, MessageType ms)
        {
            if (ms.HasFlag(MessageType.Mention)) PaintTab(idChan, cMention, Color.Black);
            else if (ms.HasFlag(MessageType.Message)) PaintTab(idChan, cMessage, Color.Black);
            else if (ms.HasFlag(MessageType.Notice)) PaintTab(idChan, cNotice, Color.Black);
            else if (ms.HasFlag(MessageType.Voice)) PaintTab(idChan, cVoice, Color.Black);
            else if (ms.HasFlag(MessageType.Normal)) PaintTab(idChan, cNormal, Color.Black);
        }



        private void InfosPeer(String pseudo)
        {
            Peer p = Program.tabPeer[GetPeerIndexByPseudo(pseudo)];

            MessageBox.Show("IP : " + p.IP + ":" + p.Port + Environment.NewLine +
                "Etat : " + ((p.IsIgnored) ? "ignoré" : "non ignoré") + Environment.NewLine +
                ((p.IsAlive) ? "actif" : "inactif"), "Infos " + pseudo);

        }

        public static bool CheckPseudo(String pseudo)
        {
            Regex rgx = new Regex("^[a-zA-Z0-9-_\\(\\)\\[\\]]{1,20}$");
            return rgx.IsMatch(pseudo);
        }



        void s_DataReceived(byte[] buffer)
        {
            String chanName = Program.tabChannel[Program.currentChannel].name;

            foreach (Peer p in Program.tabChannel[Program.currentChannel].tabPeer)
            {
                p.SendData(new DataVoice() { buffer = buffer, channel = chanName });
            }
        }



        public void AutoPing()
        {
            autoPing = false;

            while (autoPing)
            {
                foreach (Peer p in Program.tabPeer)
                {
                    if (p.waitingForPong)
                    {
                        if (p.nbPingTries >= 3)
                        {
                            p.IsAlive = false;
                            WriteNotice(p.Pseudo + " inactif (ping)", MessageType.Main | MessageType.Channel);
                        }
                        p.nbPingTries++;
                    }
                    p.nbPingTries = 0;
                    p.waitingForPong = true;
                    p.SendData(new DataPing { pong = false });
                }
                Thread.Sleep(5000);
            }

        }




        private void tabChat_DrawItem(object sender, DrawItemEventArgs e)
        {
            DrawTabControl(Program.currentTheme.backcolor);

            Rectangle paddedBounds = e.Bounds;
            TabPage page = tabChat.TabPages[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(Program.currentTheme.backcolor), paddedBounds);

            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, this.Font, paddedBounds, Program.currentTheme.fontcolor);
        }


        private void Chat_FormClosing(object sender, FormClosingEventArgs e)
        {

            SendToAll(new DataDisconnect { });
            listeningTCP = listeningUDP = autoPing = false;

            if (t_listenerUDP != null) t_listenerUDP.Abort();
            if (t_listenerTCP != null) t_listenerTCP.Abort();
            if (t_autoPing != null) t_autoPing.Abort();

            while (Program.tabPeer.Count > 0) Program.tabPeer[Program.tabPeer.Count - 1].Kill();

            Thread t = new Thread(new ThreadStart(() => {
                if (doneEvents.Count > 0) WaitHandle.WaitAll(doneEvents.ToArray());
            }));
            t.SetApartmentState(ApartmentState.MTA);
            t.Start();
            t.Join();
        }



        private void ReadCommands(String filename)
        {
            if (File.Exists(filename))
            {
                String line;
                using (StreamReader sr = new StreamReader(filename))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!String.IsNullOrWhiteSpace(line))
                            handleCmd(line);
                    }
                }
            }
        }

        private void systray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            systray.Visible = false;
        }


        Boolean mouseDown = false;
        Point originPoint;

        private void Chat_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            originPoint = e.Location;
        }

        private void Chat_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Chat_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                int x = this.Location.X - (originPoint.X - e.X);
                int y = this.Location.Y - (originPoint.Y - e.Y);

                this.Location = new Point(x, y);

            }
        }


    }
}