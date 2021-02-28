using System;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace Chat_Monkeyz
{

    public enum DataType { Announce, Handshake, Key, Message, Disconnect, ChangeNick, File, Voice, Channel, Ping };

    [Serializable]
    public class Data
    {
        public DataType type;
        public String from;
        public String to;

        public Data Clone()
        {
            return (Data)MemberwiseClone();
        }

        public override String ToString()
        {
            return type.ToString();
        }
    }


    [Serializable]
    public class DataAnnounce : Data
    {
        public IPAddress ip;
        public Int32 port;


        public DataAnnounce() { base.type = DataType.Announce; }

        public override String ToString()
        {
            return ip.ToString() + ":" + port;
        }

        public DataAnnounce(String endPoint)
        {
            Peer.GetAddressFromEndPoint(endPoint, out ip, out port);
        }

    }

    [Serializable]
    public class DataHandshake : Data
    {
        public String id;
        public String pseudo;
        public Boolean encrypted;

        public DataHandshake() { base.type = DataType.Handshake; }

        public override String ToString()
        {
            return id + " - " + pseudo + " - " + encrypted;
        }
    }

    [Serializable]
    public class DataKey : Data
    {
        public String rsaPublicKey;
        public String aesKey;
        public String IV;
        public Boolean init;

        public DataKey() { base.type = DataType.Key; }

        public override String ToString()
        {
            return rsaPublicKey + " - " + aesKey + " - " + IV + " - " + init;
        }
    }


    [Serializable]
    public class DataMessage : Data
    {
        public String msg;
        public String channel;
        public Boolean encrypted = Program.encrypted;

        public DataMessage() { base.type = DataType.Message; }

        public override String ToString()
        {
            return channel + " - " + msg;
        }
    }

    [Serializable]
    public class DataDisconnect : Data
    {
        public DataDisconnect() { base.type = DataType.Disconnect; }
    }

    [Serializable]
    public class DataChangeNick : Data
    {
        public String newPseudo;

        public DataChangeNick() { base.type = DataType.ChangeNick; }

        public override String ToString()
        {
            return newPseudo;
        }
    }


    [Serializable]
    public class DataFile : Data
    {
        public sFile fileinfo;
        public Byte[] data;

        public DataFile() { base.type = DataType.File; }


        public override String ToString()
        {
            return fileinfo.ToString();
        }
    }

    [Serializable]
    public class DataVoice : Data
    {
        public Byte[] buffer;
        public String channel;

        public DataVoice() { base.type = DataType.Voice; }


        public override String ToString()
        {
            return channel;
        }
    }


    [Flags]
    public enum ChannelOption { Join = 1, Leave = 2, Info = 4};


    [Serializable]
    public class DataChannel : Data
    {
        public ChannelOption option;
        public String channel;
        public String[] idPeers;

        public DataChannel() { base.type = DataType.Channel; }

    }


    [Serializable]
    public class DataPing : Data
    {
        public bool pong;
        public DataPing() { base.type = DataType.Ping; }
    }


    

    [Flags]
    public enum FileStatus { Accepted = 1, Rejected = 2, InProgress = 4, Finished = 8, Waiting = 16, Received = 32};


    [Serializable]
    public class sFile
    {
        public String hash;
        public String path;
        public String name;
        public Int64 size;
        public Int64 position;
        public FileStatus etat;
        public Boolean send;
        public String channel;
        public Boolean transferSucceded;

        String[] tabSize = { "o", "ko", "mo", "go", "to" };


        public sFile(String path)
        {
            this.path = path;
            name = Path.GetFileName(path);
            size = new FileInfo(path).Length;
            etat = FileStatus.Waiting;
            hash = Hash(path);
            position = 0;

            send = true;
        }

        public String StringSize()
        {
            Double s = size;
            int x = 0;

            while (s > 1024 && x < tabSize.Length - 1)
            {
                s /= 1024;
                x++;
            }

            return Math.Round(s, 2) + tabSize[x];
        }

        public static String Hash(String filename)
        {
            String hash;

            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(filename))
                {
                    hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }

            return hash;
        }


        public override string ToString()
        {
            return name + " ("+ StringSize() +")";
        }

        public override bool Equals(object obj)
        {
            return ((sFile)obj).hash == hash;
        }

        public override int GetHashCode()
        {
            return hash.GetHashCode();
        }

    }
}