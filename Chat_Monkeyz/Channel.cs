using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Chat_Monkeyz
{
    public class Channel
    {

        public List<Peer> tabPeer = new List<Peer>();
        public String name;
        public String history = "";
        public int newMessages = 0;
        public DateTime lastMessage = DateTime.Now;
        public bool isPrivate;

        public Channel(String name, Boolean isPrivate = false)
        {
            this.name = name;
            this.isPrivate = isPrivate;
        }


        public bool IsPeerInChannel(String idPeer)
        {
            bool found = false;
            int idx = 0;

            for (int n = tabPeer.Count; idx < n && !found; idx++)
            {
                if (tabPeer[idx].IsAlive && tabPeer[idx].ID == idPeer)
                    found = true;
            }

            return found;
        }


        public void PlayingSound()
        {
            new Thread(new ThreadStart(() =>
            {
                Program.myForm.NotifyTab(name, Chat.MessageType.Voice);

                Thread.Sleep(2000);

                Program.myForm.NotifyTab(name, Chat.MessageType.Normal);
            }
                )).Start();
        }
    }

}
