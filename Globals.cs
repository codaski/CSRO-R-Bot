using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Silkroad
{

    class Globals
    {
        public static string botName = "SRODev[CSRO-R]"; // nome do bot
        public static string botVersion = "v1.9"; // versao do bot
        public static string botTitle = botName + " " + botVersion; // junção
        public static string SilkroadVersion = "1.????";
        public static TextWriter tw;
        public static Form1 MainWindow;
        public static Form2 SecundaryPass;
        public static Form3 CharInsert;
        public static Connection Connection = new Connection();
        public static PacketCreator ServerPC = new PacketCreator(enumDestination.Server);
        public static PacketCreator ClientPC = new PacketCreator(enumDestination.Client);
       // public static PacketCreator HSSERV = new PacketCreator(enumDestination.HS);
        public static Parser Parser = new Parser();
        public static Images Images = new Images();
        public static ClientParser ClientParser = new ClientParser();
        public static void Init()
        {
            ServerPC.eNewDataToSend += new PacketCreator.eNewDataToSendEventHandler(Globals.Connection.Send);
            ClientPC.eNewDataToSend += new PacketCreator.eNewDataToSendEventHandler(Globals.Connection.ClientSend);
        }

        public static void UpdateLogs(string log)
        {
            string currentTime = "[" + DateTime.Now.ToLongTimeString() + "] ";
            Globals.MainWindow.logs.AppendText(currentTime + log + "\r\n");
        }
        public static void UpdatePacketPC(string log)
        {
            string currentTime = "[" + DateTime.Now.ToLongTimeString() + "] ";
            Globals.MainWindow.textBox5.AppendText(currentTime + log + "\r\n");
        }
        public static void UpdatePacketCP(string log)
        {
            string currentTime = "[" + DateTime.Now.ToLongTimeString() + "] ";
            Globals.MainWindow.textBox6.AppendText(currentTime + log + "\r\n");
        }
        public static void UpdateChat(string text, byte type, string name)
        {
            string currentTime = "[" + DateTime.Now.ToLongTimeString() + "] ";
            text = text + "\r\n";
            switch (type)
            {
                case (byte)Chat_Type.NORMAL:
                    Globals.MainWindow.chat_normal.AppendText(currentTime + name + ": " + text);
                    break;
                case (byte)Chat_Type.PRIVATE:
                    if (Globals.MainWindow.alert_pm.Checked)
                    {
                        Alert.StartAlert();
                    }
                    Globals.MainWindow.chat_private.AppendText(currentTime + name + ": " + text);
                    break;
                case (byte)Chat_Type.PARTY:
                    Globals.MainWindow.chat_party.AppendText(currentTime + name + ": " + text);
                    break;
                case (byte)Chat_Type.GUILD:
                    Globals.MainWindow.chat_guild.AppendText(currentTime + name + ": " + text);
                    break;
                case (byte)Chat_Type.UNION:
                    Globals.MainWindow.chat_union.AppendText(currentTime + name + ": " + text);
                    break;
                case (byte)Chat_Type.GLOBAL:
                    Globals.MainWindow.chat_global.AppendText(currentTime + name + ": " + text);
                    break;
                case (byte)Chat_Type.NOTICE:
                    Globals.MainWindow.chat_notice.AppendText(currentTime + "[NOTICE]" + ": " + text);
                    break;
            }
        }

        public static bool CheckPort(int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                socket.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void LogData(Packet packet, byte destination)
        {
            if (packet.Opc != (ushort)0x6102 && packet.Opc != (ushort)0x6103)
            {
                string text = null;
                if (destination == 1)
                {
                    text = "[S->C]";
                }
                else
                {
                    text = "[C->S]";
                }
                text = text + "[" + packet.Opc.ToString("X2") + "]";
                tw.Write(text);
                byte[] t = packet.ToByteArray();
                for (int i = 0; i < packet.data.len; i++)
                {
                    tw.Write(t[i + 6].ToString("X2"));
                }
                tw.Write("\r\n");
            }
        }

        public static void Debug(string name, string error, Packet packet)
        {
            try
            {
                string output_error = null;
                if (error == "" || error == null)
                {
                    output_error = "Error: <None>";
                }
                else
                {
                    output_error = "Error: " + error;
                }
                string date = DateTime.Now.TimeOfDay.ToString().Replace(':', '-');
                TextWriter tw = new StreamWriter(Environment.CurrentDirectory + @"/debug/[" + date + "] " + name + ".txt", true);
                tw.WriteLine("[S->C]" + packet.Opc.ToString("X2"));
                byte[] t = packet.ToByteArray();
                for (int i = 0; i < packet.data.len; i++)
                {
                    tw.Write(t[i + 6].ToString("X2"));
                }
                tw.Write("\r\n");
                tw.Write(output_error + "\r\n");
                tw.Close();
            }
            catch (Exception ex)
            {
                Globals.UpdateLogs(ex.Message);
            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            string a = arr[6].ToString() + arr[7].ToString() + arr[4].ToString() + arr[5].ToString() + "0000";
            return a;
        }
        public static UInt32 String_To_UInt32(string s)
        {
            char[] arr = s.ToCharArray();
            string a = "0000" + arr[2] + arr[3] + arr[0] + arr[1];
            UInt32 aa = UInt32.Parse(a, System.Globalization.NumberStyles.HexNumber);
            return aa;
        }

        public enum enumMobType : byte
        {
            Normal = 0x00,
            Champion = 0x01,
            Unique = 0x03,
            Giant = 0x04,
            Titan = 0x05,
            Elite1 = 0x06,
            Elite2 = 0x07,
            Party = 0x10,
            PartyChampion = 0x11,
            PartyGiant = 0x14
        }
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        public enum WindowAction : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10
        }

        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        public enum Chat_Type
        {
            NORMAL = 0x01,
            PRIVATE = 0x02,
            GAMEMASTER = 0x03,
            PARTY = 0x04,
            GUILD = 0x05,
            GLOBAL = 0x06,
            NOTICE = 0x07,
            UNION = 0x0B
        }

    }
}
