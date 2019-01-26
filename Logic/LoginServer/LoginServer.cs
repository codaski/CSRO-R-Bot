using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Timers;

namespace Silkroad
{
    class LoginServer
    {
        public static uint id = 0;
        public static int port_sro = -1;
        public static byte patch = 0;

        #region Agent
        public static void Agent()
        {
            Globals.ServerPC.SendPacket(new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.HANDSHAKE_OK, false, enumDestination.Server));
            Packet packet = new Packet((ushort)LoginServerOpcodes.SERVER_OPCODES.AGENT_SERVER, true, enumDestination.Server);
            string text = "SR_Client";
            packet.data.AddSTRING(text, enumStringType.ASCII);
            packet.data.AddBYTE(0x00);
            Globals.ServerPC.SendPacket(packet);
        }
        #endregion
        #region Patch
        public static void ServerInfo(Packet packet)
        {
            string name = packet.data.ReadSTRING(enumStringType.ASCII);
            if (name == "GatewayServer")
            {
                string real_name = "SR_Client";
                packet = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.ACCEPT, false, enumDestination.Server);
                packet.data.AddBYTE(BotData.LoginServer.locale); //Locale
                packet.data.AddSTRING(real_name, enumStringType.ASCII);
                packet.data.AddDWORD(BotData.LoginServer.version); //Version
                Globals.ServerPC.SendPacket(packet);
            }
            if (name == "AgentServer")
            {
                #region Reset IP To Gateway Server
                for (int i = 0; i < BotData.Servers.Length; i++)
                {
                    if (BotData.Servers[i].name == Globals.MainWindow.server_name.Text)
                    {
                        BotData.LoginServer.ip = BotData.Servers[i].ip;
                        BotData.LoginServer.port = 15779;
                        BotData.LoginServer.locale = BotData.Servers[i].locale;
                        BotData.LoginServer.version = BotData.Servers[i].version;
                        break;
                    }
                }
                #endregion
                packet = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.GAME_LOGIN, true, enumDestination.Server);
                packet.data.AddDWORD(id);
                packet.data.AddSTRING(Globals.MainWindow.username.Text.ToLower(), enumStringType.ASCII);
                packet.data.AddSTRING(Globals.MainWindow.password.Text.ToLower(), enumStringType.ASCII);
                packet.data.AddWORD(BotData.LoginServer.locale);
                packet.data.AddWORD(0x0000);
                Random mac = new Random();
                packet.data.AddDWORD((uint)mac.Next());
                Globals.ServerPC.SendPacket(packet);
            }
        }
        public static bool showed = false;
        public static void AnalyzePatch(Packet packet)
        {
            patch++;
            if (patch >= 6)
            {
                patch = 0;
                BotData.ping = 1;
                if (!BotData.use_client)
                {
                    RequestServerList();
                }
                if (!showed)
                {
                    if (packet.data.len == 3)
                    {
                        Globals.UpdateLogs("You Using Newer Silkroad version than Server");
                    }
                    if (packet.data.len != 3 && packet.data.len != 2)
                    {
                        Globals.UpdateLogs("You Using Older Silkroad version than Server");
                    }
                }
                showed = true;
            }
        }
        #endregion

        public static void RequestServerList()
        {
            Packet packet = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.REQUEST_SERVER_LIST, false, enumDestination.Server);
            Globals.ServerPC.SendPacket(packet);
        }

        public static void LoginResponse(Packet packet)
        {
            BotData.use_al = true;
            if (!Globals.Parser.login_bug)
            {
                byte code = packet.data.ReadBYTE();
                if (code == 0x01)
                {
                    id = packet.data.ReadDWORD();
                    string ip = packet.data.ReadSTRING(enumStringType.ASCII);
                    ushort port = packet.data.ReadWORD();
                    BotData.LoginServer.ip = ip;
                    BotData.LoginServer.port = port;

                    BotData.ping = 0;
                    if (BotData.use_client)
                    {
                        Packet packet_game = new Packet((ushort)LoginServerOpcodes.SERVER_OPCODES.LOGIN_REPLY, true, enumDestination.Client);
                        packet_game.data.AddBYTE(0x01);
                        packet_game.data.AddDWORD(0x1234);
                        packet_game.data.AddSTRING("127.0.0.1", enumStringType.ASCII);
                        packet_game.data.AddWORD((ushort)port_sro);
                        Globals.Connection.ClientListen("127.0.0.1", port_sro);
                        Globals.ClientPC.SendPacket(packet_game);
                    }
                    else
                    {
                        Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
                    }
                }
                else
                {
                    byte subcode = packet.data.ReadBYTE();
                    switch (subcode)
                    {
                        case 1:
                            uint maxTry = packet.data.ReadDWORD();
                            uint curTry = packet.data.ReadDWORD();
                            Globals.UpdateLogs(string.Format("Wrong ID/PW. You have {0} attempts left", maxTry - curTry));
                            Globals.MainWindow.username.Enabled = true;
                            Globals.MainWindow.password.Enabled = true;
                            Globals.MainWindow.login.Enabled = true;
                            break;
                        case 2:
                            if (packet.data.ReadBYTE() == 1)
                            {
                                string reason = packet.data.ReadSTRING(enumStringType.ASCII);
                                string date = packet.data.ReadWORD() + "." + packet.data.ReadWORD() + "." + packet.data.ReadWORD() + " " + packet.data.ReadWORD() + ":" + packet.data.ReadWORD();
                                Globals.UpdateLogs("Your account has been banned ! Reason: " + reason + ". Till: " + date);
                            }
                            Globals.MainWindow.username.Enabled = true;
                            Globals.MainWindow.password.Enabled = true;
                            Globals.MainWindow.login.Enabled = true;
                            break;
                        case 3: // User Already Connected
                            Globals.UpdateLogs("User Already Connected");
                            System.Threading.Thread.Sleep(5000);
                            Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
                            break;
                        case 5: // Server Full
                            Globals.UpdateLogs("The Server Is Full");
                            Packet packet_n = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN, false, enumDestination.Server);
                            packet_n.data.AddBYTE(BotData.LoginServer.locale);
                            packet_n.data.AddSTRING(Globals.MainWindow.username.Text, enumStringType.ASCII);
                            packet_n.data.AddSTRING(Globals.MainWindow.password.Text, enumStringType.ASCII);
                            packet_n.data.AddWORD(BotData.Server_ID[BotData.Server_Name.IndexOf(Globals.MainWindow.in_game_server_name.Text)]);
                            System.Threading.Thread.Sleep(500);
                            Globals.ServerPC.SendPacket(packet_n);
                            break;
                        default:
                            Globals.UpdateLogs("Unknown login error code: " + subcode);
                            Globals.MainWindow.username.Enabled = true;
                            Globals.MainWindow.password.Enabled = true;
                            System.Threading.Thread.Sleep(5000);
                            Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
                            break;
                    }
                    Globals.ClientPC.SendPacket(packet);
                }
            }
            else
            {
                BotData.ping = 0;
                Packet packetas = new Packet((ushort)0xA323, false, enumDestination.Client);
                packetas.data.AddBYTE(0x01);
                Globals.ClientPC.SendPacket(packetas);
                Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
            }
        }

        public static void GameLoginResponse(Packet packet)
        {
            byte code = packet.data.ReadBYTE();
            if (code == 0x01)
            {
                BotData.use_al = false;
                if (!BotData.use_client)
                {
                    Character.RequestCharacterlist();
                }
            }
            else
            {
                BotData.ping = 0;
                byte subcode = packet.data.ReadBYTE();
                switch (subcode)
                {
                    case 4: // Server Full
                        Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
                        break;
                    default:
                        Globals.UpdateLogs("Unknown game login error code: " + subcode);
                        Globals.ClientPC.SendPacket(packet);
                        break;
                }
            }
        }

        public static void AnalyzeClientLogin(Packet packet)
        {
            packet.data.ReadBYTE();
            Globals.MainWindow.username.Text = packet.data.ReadSTRING(enumStringType.ASCII);
            Globals.MainWindow.password.Text = packet.data.ReadSTRING(enumStringType.ASCII);
            Globals.MainWindow.in_game_server_name.Text = BotData.Server_Name[BotData.Server_ID.IndexOf(packet.data.ReadWORD())];
            Globals.ServerPC.SendPacket(packet);
        }
        public static void AnalyzeClientGameLogin(Packet packet)
        {
            packet.data.ReadBYTE();
            Globals.MainWindow.username.Text = packet.data.ReadSTRING(enumStringType.ASCII);
            Globals.MainWindow.password.Text = packet.data.ReadSTRING(enumStringType.ASCII);
            Globals.MainWindow.in_game_server_name.Text = BotData.Server_Name[BotData.Server_ID.IndexOf(packet.data.ReadWORD())];
            Globals.ServerPC.SendPacket(packet);
        }
    }
}