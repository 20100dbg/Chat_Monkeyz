using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;


namespace Chat_Monkeyz
{
    static class Program
    {
        public static Chat myForm;
        public static String id = Guid.NewGuid().ToString();


        //default config
        public static Int32 tcpPort = 1337;
        public static Int32 udpPort = 1338;
        public static String pseudo = "Anon";
        public static Int64 buffersize = 1000000;
        public static String incomingFolder = Application.StartupPath;
        public static Boolean encrypted = false;
        public static Boolean safeMode = false;
        public static Theme defaultTheme = new Theme { name = "default", backcolor = Color.Black, fontcolor = Color.White, font = new Font("Verdana", 12), size = new Size(680, 340) };
        public static Theme currentTheme;


        public static int currentChannel = 0;
        public static wndPeers wPeers;
        public static wndFiles wFiles;
        public static Config wConfig;

        public static List<Peer> tabPeer = new List<Peer>();
        public static List<Channel> tabChannel = new List<Channel>();

        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            myForm = new Chat();
            Application.Run(myForm);
        }



        public static void Log(String txt)
        {
            String logfile = "Chat_Monkeyz.log";
            String oldtxt = String.Empty;
            
            lock (typeof(Program))
            {
                if (File.Exists(logfile))
                {
                    using (StreamReader sr = new StreamReader(logfile))
                    {
                        oldtxt = sr.ReadToEnd();
                    }
                }

                using (StreamWriter sw = new StreamWriter(logfile))
                {
                    sw.WriteLine(oldtxt + DateTime.Now.ToString() + " - " + txt);
                }
            }
        }

    }
}
