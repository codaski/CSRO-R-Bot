using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Drawing;

namespace Silkroad
{

    class SilkroadSecurity
    {
        public static System.Threading.Thread ping_thread;
        public static System.Timers.Timer ping_timer;
        public static void Analyze(Packet packet)
        {
            BotData.ping = 0;
            if (packet.data.len == 37)
            {
                Globals.UpdateLogs("Connected To Silkroad Server !");
                Packet_0x5000.RetVal retValue;
               retValue = Packet_0x5000.Packet_0x5000_len37(packet);
                Globals.ServerPC.InitCounterByteCRCbyte((int)retValue.CounterSeed, (int)retValue.CrcSeed);
                Globals.ClientPC.SendPacket(new Packet(0x5000, false, enumDestination.Client, new byte[] { 0xE, 0x17, 0x7D, 0x4E, 0x92, 0x97, 0xF8, 0x8, 0xFE, 0x8A, 0x0, 0x0, 0x0, 0xF2, 0x0, 0x0, 0x0, 0x85, 0xD8, 0x1D, 0x98, 0x28, 0xC7, 0x1C, 0xCF, 0x1, 0x0, 0x0, 0x0, 0x1, 0x0, 0x0, 0x0, 0x4A, 0xCC, 0xEA, 0x22 }));
                Globals.ServerPC.SendPacket((Packet)retValue.packet);
               // BotData.ping = 1;
            }
            else
            {
                byte[] key;
                key = Packet_0x5000.Packet_0x5000_len9(packet);
                if (key == null)
                {
                    Globals.UpdateLogs("NULL KEY");
                    //BotData.ping = 1;
                   // Globals.Connection.Close();
                }
                else
                {
                    Globals.ServerPC.InitBlowfish(key);
                    Globals.ClientPC.SendPacket(new Packet(0x5000, false, enumDestination.Client, new byte[] { 0x10, 0x44, 0x4E, 0xA8, 0x2F, 0x86, 0xC5, 0x6D, 0x7A }));
                    Globals.ClientPC.InitBlowfish(new byte[] { 0xD, 0x3, 0x3D, 0x3, 0x3, 0xD, 0x3, 0x1D });
                    LoginServer.Agent();
                   // BotData.ping = 1;
                }
            }
        }

        public static void Initialize()
        {
            ping_thread = new System.Threading.Thread(Pinger);
            ping_thread.Start();
        }

        public static void Pinger()
        {
            ping_timer = new System.Timers.Timer();
            ping_timer.Interval = 4000;
            ping_timer.Elapsed += new System.Timers.ElapsedEventHandler(ping_timer_Elapsed);
            ping_timer.Start();
            ping_timer.Enabled = true;
        }

        static void ping_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (BotData.ping != 0)
            {
                Packet packet = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.PING, false, enumDestination.Server);
                Globals.ServerPC.SendPacket(packet);
            }
        }
    }
}
