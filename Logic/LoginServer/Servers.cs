using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Drawing;
using System.Timers;

namespace Silkroad
{
    class Servers_Auto
    {
        public static void Analyze(Packet packet)
        {

            byte[] ar = packet.ToByteArray();
            string text = null;
            for (int i = 6; i < packet.data.len + 6; i++)
            {
                text += ar[i].ToString("X2");
            }
            // Globals.UpdateLogs(text);
            try
            {
                string name = null; ;
                if (packet.data.ReadBYTE() == 0x01)
                {
                    packet.data.ReadBYTE();
                    packet.data.ReadSTRING(enumStringType.ASCII);
                    packet.data.ReadBYTE();
                    byte server = packet.data.ReadBYTE();
                    BotData.Server_Name.Clear();
                    BotData.Server_ID.Clear();
                    while (server == 0x01)
                    {
                        ushort server_id = packet.data.ReadWORD(); //Server ID
                        name = packet.data.ReadSTRING(enumStringType.ASCII); //Server Name
                        packet.data.ReadWORD(); //Curr Users
                        packet.data.ReadWORD(); //Max Users
                        packet.data.ReadBYTE(); //State
                        server = packet.data.ReadBYTE();
                        BotData.Server_Name.Add(name);
                        BotData.Server_ID.Add(server_id);
                        if (Globals.MainWindow.in_game_server_name.Items.IndexOf(name) == -1)
                        {
                            Globals.MainWindow.in_game_server_name.Items.Add(name);
                        }
                    }
                }
                Globals.MainWindow.in_game_server_name.Enabled = true;
                if (Globals.MainWindow.in_game_server_name.Items.Count == 1)
                {
                    Globals.MainWindow.in_game_server_name.SelectedIndex = 0;
                }
                if (BotData.use_al == true)
                {
                    packet = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN, false, enumDestination.Server);
                    packet.data.AddBYTE(BotData.LoginServer.locale);
                    packet.data.AddSTRING(Globals.MainWindow.username.Text, enumStringType.ASCII);
                    packet.data.AddSTRING(Globals.MainWindow.password.Text, enumStringType.ASCII);
                    packet.data.AddWORD(BotData.Server_ID[BotData.Server_Name.IndexOf(Globals.MainWindow.in_game_server_name.Text)]);
                    System.Threading.Thread.Sleep(300);
                    Globals.ServerPC.SendPacket(packet);
                }
                else
                {
                    if (!BotData.use_client)
                    {
                        Globals.MainWindow.in_game_server_name.Enabled = true;
                        Globals.MainWindow.username.Enabled = true;
                        Globals.MainWindow.password.Enabled = true;
                        Globals.MainWindow.login.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Globals.Debug("ServerList", ex.Message, packet);
            }
        }
    }
}
