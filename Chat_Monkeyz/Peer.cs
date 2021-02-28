using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Chat_Monkeyz
{
    public class Peer
    {

        public List<sFile> tabFile = new List<sFile>();
        public Boolean waitingForPong = false;
        public int nbPingTries = 0;

        String id;
        String pseudo;

        bool isAlive = false;
        bool isActive = true;
        bool isIgnored = false;
        bool isEncrypted = false;

        NetworkStream ns;
        ManualResetEvent doneEvent;
        EndPoint ep;
        IPAddress ip;
        int port;
        BinaryFormatter bf = new BinaryFormatter();

        RSA rsa;
        AES aes;

        
        public Peer(TcpClient tc, ManualResetEvent doneEvent)
        {
            this.doneEvent = doneEvent;
            ns = tc.GetStream();
            ep = tc.Client.RemoteEndPoint;
            isAlive = true;

            GetAddressFromEndPoint(ep.ToString(), out ip, out port);

            if (Program.encrypted)
            {
                rsa = new RSA(1024);
                aes = new AES();
            }


            SendData(new DataHandshake { id = Program.id, pseudo = Program.pseudo, encrypted = Program.encrypted });
        }


        public void InitKey()
        {
            rsa.AssignNewKey();
            SendData(new DataKey { rsaPublicKey = rsa.publickey, init = true });
        }


        public static void GetAddressFromEndPoint(String endPoint, out IPAddress ip, out int port)
        {
            String[] tab = endPoint.Split(':');

            if (!IPAddress.TryParse(tab[0], out ip)) ip = IPAddress.Parse("0.0.0.0");
            if (tab.Length == 1 || !Int32.TryParse(tab[1], out port)) port = 0;
        }

        
        public EndPoint EndPoint
        {
            get { return ep; }
        }

        public IPAddress IP
        {
            get { return ip; }
        }

        public int Port
        {
            get { return port; }
        }

        public String Pseudo
        {
            get { return pseudo; }
        }

        public String ID
        {
            get { return id; }
        }

        public Boolean IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public Boolean IsIgnored
        {
            get { return isIgnored; }
            set { isIgnored = value; }
        }

        public Boolean IsEncrypted
        {
            get { return isEncrypted; }
            set { isEncrypted = value; }
        }

        #region Envoi/Reception de Data/Fichier


        public void SendData(Data data)
        {
            if (!isAlive) return;
            else if (!isActive && data.type != DataType.Ping) return;


            Data data2 = data.Clone();

            if (Program.encrypted)
            {
                if (data2.type == DataType.Message)
                {
                    DataMessage d = (DataMessage)data2;
                    d.msg = aes.EncryptToString(d.msg, aes.sendKey, aes.sendIV);//msg = rsa.Encrypt(msg);
                    data2 = d;
                }
                else if (data2.type == DataType.Voice)
                {
                    DataVoice d = (DataVoice)data2;
                    d.buffer = aes.EncryptToBytes(d.buffer, aes.sendKey, aes.sendIV);
                    data2 = d;
                }
                else if (data2.type == DataType.File)
                {
                    DataFile d = (DataFile)data2;
                    d.data = aes.EncryptToBytes(d.data, aes.sendKey, aes.sendIV);
                    data2 = d;
                }
            }


            try
            {
                bf.Serialize(ns, data2);
            }
            catch (Exception e)
            {
                Program.Log("SendData() - " + e.Message + Environment.NewLine + data2.type.ToString() + " - " + data2.ToString());

                if (Program.safeMode)
                {
                    Program.myForm.WriteNotice("Peer disconnected : " + pseudo, Chat.MessageType.Channel | Chat.MessageType.Main);
                    Kill();
                }
                else
                {
                    if (isActive)
                    {
                        Program.myForm.WriteNotice(pseudo + " inactif", Chat_Monkeyz.Chat.MessageType.Main | Chat_Monkeyz.Chat.MessageType.Channel);
                        isActive = false;
                    }
                    TryReanimate();
                }
            }
        }

        public void TryReanimate()
        {
            if (nbPingTries < 2)
            {
                nbPingTries++;
                waitingForPong = true;

                ns.Flush();
                bf = new BinaryFormatter();
                Thread.Sleep(100);
                SendData(new DataPing { pong = false });
            }
            else
                Kill();
        }

        public void ReceiveData()
        {
            Data d = new Data();

            while (isAlive)
            {
                try
                {
                    d = (Data)bf.Deserialize(ns);
                    if (!isActive && d.type != DataType.Ping) continue;

                    HandleData(d);
                }
                catch (Exception e)
                {
                    Program.Log("ReceiveData() - " + e.Message);

                    if (Program.safeMode)
                    {
                        Program.myForm.WriteNotice("Peer disconnected : " + pseudo, Chat.MessageType.Channel | Chat.MessageType.Main);
                        Kill();
                    }
                    else
                    {
                        if (isActive)
                        {
                            Program.myForm.WriteNotice(pseudo + " inactif", Chat_Monkeyz.Chat.MessageType.Main | Chat_Monkeyz.Chat.MessageType.Channel);
                            isActive = false;
                        }
                        TryReanimate();
                    }
                }                
            }
        }



        public void ReceiveFile(sFile f, Byte[] data)
        {
            try
            {
                FileStream fs = new FileStream(Program.incomingFolder + f.name, FileMode.OpenOrCreate);
                fs.Position = f.position;
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
            catch (Exception e)
            {
                Program.Log("ReceiveFile - " + e.Message);
            }
        }



        public void SendFile(sFile f)
        {
            FileStream fs = new FileStream(f.path, FileMode.Open);
            Int64 buffersize = Program.buffersize, bytesOk = 0;

            while (bytesOk < f.size)
            {
                if (f.size - bytesOk < buffersize)
                    buffersize = f.size - bytesOk;

                Byte[] buffer = new Byte[buffersize];
                fs.Read(buffer, 0, (int)buffersize);
                f.position = bytesOk;
                bytesOk += buffersize;

                
                if (bytesOk >= f.size) f.etat = FileStatus.Finished;
                else f.etat = FileStatus.InProgress;

                SendData(new DataFile { fileinfo = f, data = buffer });
            }

            fs.Close();
        }

        #endregion



        private void HandleData(Data myData)
        {

            if (myData.type == DataType.Announce)
            {
                DataAnnounce d = (DataAnnounce)myData;


                IPAddress ip = d.ip;
                int port = d.port;

                Program.myForm.Connect(ip, port);

            }
            else if (myData.type == DataType.Handshake)
            {
                DataHandshake d = (DataHandshake)myData;

                id = d.id;
                pseudo = d.pseudo;
                isEncrypted = d.encrypted;

                if (d.encrypted && Program.encrypted) InitKey();

                Program.tabChannel[Program.currentChannel].tabPeer.Add(this);
                Program.myForm.WriteNotice("Nouveau peer : " + pseudo + ((d.encrypted && Program.encrypted) ? "" : "(liaison non chiffrée)"), Chat.MessageType.Main);

                Program.myForm.RefreshPeersList();
            }
            else if (myData.type == DataType.Key)
            {
                if (Program.encrypted)
                {
                    DataKey d = (DataKey)myData;

                    if (d.init)
                    {
                        rsa.publickey = d.rsaPublicKey;
                        aes.RegenerateKey();
                        aes.receiveKey = aes.Key;
                        aes.receiveIV = aes.IV;

                        SendData(new DataKey { aesKey = rsa.Encrypt(aes.receiveKey), IV = rsa.Encrypt(aes.receiveIV), init = false });
                    }
                    else
                    {
                        aes.sendKey = rsa.DecryptToBytes(d.aesKey);
                        aes.sendIV = rsa.DecryptToBytes(d.IV);
                    }
                }
            }
            else if (myData.type == DataType.Message)
            {
                if (!isIgnored)
                {
                    DataMessage d = (DataMessage)myData;
                    String msg = d.msg;
                    if (isEncrypted != d.encrypted) 
                    {
                        isEncrypted = d.encrypted;
                        Program.myForm.RefreshPeersList();
                    }

                    Program.Log("received : " + msg);

                    if (d.encrypted) msg = aes.DecryptToString(msg, aes.receiveKey, aes.receiveIV);
                    //msg = rsa.DecryptToString(msg);

                    //en cas de mp, remplacement de l'id local par celui du peer
                    if (d.channel == Program.id) d.channel = id;

                    Program.myForm.WriteText(pseudo, msg, d.channel, Chat.MessageType.Channel | ((msg.Contains(Program.pseudo) ? Chat.MessageType.Mention : Chat.MessageType.Message)));
                }
            }
            else if (myData.type == DataType.Voice)
            {
                if (!isIgnored)
                {
                    DataVoice d = (DataVoice)myData;

                    if (Program.myForm.IsPeerInCurrentChannel(id))
                    {
                        Byte[] buffer = d.buffer;
                        if (isEncrypted) buffer = aes.DecryptToBytes(buffer, aes.receiveKey, aes.receiveIV);
                        Program.myForm.snd.Play(buffer);

                    }
                    else
                    {
                        int idxChan = Program.myForm.GetChanIndexById(d.channel);
                        if (idxChan != -1)
                            Program.tabChannel[idxChan].PlayingSound();
                    }
                }
            }
            else if (myData.type == DataType.Disconnect)
            {
                Program.myForm.WriteNotice(pseudo + " vient de se déconnecter", Chat.MessageType.Channel | Chat.MessageType.Main);
                
                Kill();
            }
            else if (myData.type == DataType.ChangeNick)
            {
                DataChangeNick d = (DataChangeNick)myData;
                String newpseudo = d.newPseudo;

                int idxChan = Program.myForm.GetChanIndexById(id);

                if (idxChan > -1)
                {
                    Program.myForm.TabChatTitle(idxChan, newpseudo);
                }


                foreach (Channel c in Program.tabChannel)
                {
                    if (c.tabPeer.IndexOf(this) > -1)
                    {
                        Program.myForm.WriteNotice(pseudo + " s'appelle maintenant " + newpseudo, c.name, Chat.MessageType.Main | Chat.MessageType.Channel);
                    }
                }

                pseudo = newpseudo;
                Program.myForm.RefreshPeersList();
            }
            else if (myData.type == DataType.Channel)
            {
                DataChannel d = (DataChannel)myData;

                int idxChan = Program.myForm.GetChanIndexById(d.channel);

                if (idxChan != -1)
                {
                    if (Program.tabChannel[idxChan].isPrivate && !Program.tabChannel[idxChan].tabPeer.Contains(this)) return;

                    if (d.option.HasFlag(ChannelOption.Info))
                    {
                        if (d.idPeers != null)
                        {
                            for (int i = 0, n = d.idPeers.Length; i < n; i++)
                            {
                                int idxPeer = Program.myForm.GetPeerIndexById(d.idPeers[i]);
                                if (idxPeer > -1)
                                {
                                    if (!Program.tabChannel[idxChan].tabPeer.Contains(Program.tabPeer[idxPeer]))
                                        Program.tabChannel[idxChan].tabPeer.Add(Program.tabPeer[idxPeer]);
                                }
                            }
                        }
                        if (!Program.tabChannel[idxChan].tabPeer.Contains(this))
                            Program.tabChannel[idxChan].tabPeer.Add(this);
                    }
                    else if (d.option.HasFlag(ChannelOption.Join))
                    {
                        IEnumerable<String> idPeers = from p in Program.tabPeer select p.id;

                        Program.myForm.WriteNotice(pseudo + " a rejoint le channel", d.channel, Chat.MessageType.Channel);
                        SendData(new DataChannel { channel = d.channel, idPeers = idPeers.ToArray(), option = ChannelOption.Info });
                        Program.tabChannel[idxChan].tabPeer.Add(this);
                    }
                    else if (d.option.HasFlag(ChannelOption.Leave))
                    {
                        Program.myForm.WriteNotice(pseudo + " a quitté le channel", d.channel, Chat.MessageType.Channel);
                        Program.tabChannel[idxChan].tabPeer.Remove(this);
                    }

                }


            }
            else if (myData.type == DataType.Ping)
            {
                DataPing d = (DataPing)myData;

                if (!d.pong)
                {
                    SendData(new DataPing { pong = true });
                }
                else
                {
                    nbPingTries = 0;
                    waitingForPong = false;
                    if (!isActive) Program.myForm.WriteNotice(pseudo + " back to life", Chat.MessageType.Channel | Chat.MessageType.Main);
                    isActive = true;
                }
            }
            else if (myData.type == DataType.File)
            {
                DataFile d = (DataFile)myData;
                sFile f = d.fileinfo;

                int idxFile = tabFile.IndexOf(f);


                if (f.etat.HasFlag(FileStatus.Waiting))
                {
                    f.send = false;
                    tabFile.Add(f);
                    Program.myForm.RefreshFilesList();
                    Program.myForm.WriteNotice(pseudo + " veut vous envoyer " + f.name + " (" + f.StringSize() + ")", f.channel, Chat.MessageType.Channel);
                }
                else if (f.etat.HasFlag(FileStatus.InProgress) || f.etat.HasFlag(FileStatus.Finished))
                {
                    tabFile[idxFile].etat = f.etat;
                    Program.myForm.RefreshFilesList();

                    Byte[] data = d.data;
                    if (isEncrypted) data = aes.DecryptToBytes(data, aes.receiveKey, aes.receiveIV);

                    ReceiveFile(f, data);


                    if (f.etat.HasFlag(FileStatus.Finished))
                    {
                        if (f.hash == sFile.Hash(Program.incomingFolder + f.name))
                        {
                            Program.myForm.WriteNotice(f.name + " reçu", f.channel, Chat.MessageType.Channel);
                            f.transferSucceded = true;
                        }
                        else
                        {
                            Program.myForm.WriteNotice(f.name + " reçu avec des erreurs", f.channel, Chat.MessageType.Channel);
                            f.transferSucceded = false;
                        }

                        f.etat = FileStatus.Received;
                        Program.myForm.RefreshFilesList();
                        SendData(new DataFile { fileinfo = f });
                    }
                }
                else if (f.etat.HasFlag(FileStatus.Accepted))
                {
                    tabFile[idxFile].etat = FileStatus.InProgress;
                    Program.myForm.RefreshFilesList();
                    SendFile(f);
                }
                else if (f.etat.HasFlag(FileStatus.Rejected))
                {
                    tabFile[idxFile].etat = FileStatus.Rejected;
                    Program.myForm.RefreshFilesList();
                    Program.myForm.WriteNotice(pseudo + " a rejeté le fichier " + f.name, f.channel, Chat.MessageType.Channel);
                }
                else if (f.etat.HasFlag(FileStatus.Received))
                {
                    tabFile[idxFile].etat = f.etat;
                    Program.myForm.RefreshFilesList();
                    Program.myForm.WriteNotice(f.name + " envoyé à " + pseudo + " ("+ ((f.transferSucceded) ? "avec succès" : "avec erreurs")  +")", f.channel, Chat.MessageType.Channel);
                }

            }
        }



        public void Kill()
        {
            isAlive = isActive = false;
            ns.Close();

            foreach (Channel c in Program.tabChannel) c.tabPeer.Remove(this);
            Program.tabPeer.Remove(this);

            Program.myForm.RefreshPeersList();
            doneEvent.Set();
        }


    }
}