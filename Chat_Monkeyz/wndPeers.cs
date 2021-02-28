using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;


namespace Chat_Monkeyz
{

    public partial class wndPeers : Torbo.DockableForm
    {
        public bool activated = false;


        public wndPeers()
        {
            InitializeComponent();
        }


        private void PeerList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        public void RefreshList()
        {
            List<Peer> tabClient = Program.tabChannel[Program.currentChannel].tabPeer;

            
            //lb_peers.Items.Clear();
            userControl11.listPeers.Items.Clear();

            foreach (Peer p in tabClient)
            {
                AddPeer(p);
            }
        }




        public void AddPeer(Peer p)
        {
            //lb_peers.Items.Add(new PeerItem { Id = p.ID, Pseudo = p.Pseudo, IsIgnored = p.IsIgnored, IsEncrypted = p.IsEncrypted });
            userControl11.listPeers.Items.Add(new PeerItem { Id = p.ID, Pseudo = p.Pseudo, IsIgnored = p.IsIgnored, IsEncrypted = p.IsEncrypted });

        }




    }



    class PeerItem
    {
        public String Id { get; set; }
        public String Pseudo { get; set; }
        public Boolean IsIgnored { get; set; }
        public Boolean IsEncrypted { get; set; }

        public override string ToString()
        {
            return Pseudo + " " + ((IsEncrypted) ? "e" : "!e") + " " + ((IsIgnored) ? "i" : "!i");
        }
    }
}