using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class ClientParser
    {
        public static byte first = 0;
        public void Handler(Packet packet)
        {
            switch (packet.Opc)
            {
                case (ushort)LoginServerOpcodes.CLIENT_OPCODES.HANDSHAKE:
                    break;
                case (ushort)LoginServerOpcodes.CLIENT_OPCODES.HANDSHAKE_OK:
                    break;
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.AGENT_SERVER:
                    break;
                case (ushort)LoginServerOpcodes.CLIENT_OPCODES.ACCEPT:
                    break;
                case (ushort)LoginServerOpcodes.CLIENT_OPCODES.PING:
                    break;
                case (ushort)LoginServerOpcodes.CLIENT_OPCODES.REQUEST_SERVER_LIST:
                    if (!Globals.Parser.login_bug)
                    {
                        Globals.ServerPC.SendPacket(packet);
                    }
                    break;
                case (ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN:
                    if (!Globals.Parser.login_bug)
                    {
                        LoginServer.AnalyzeClientLogin(packet);
                    }
                    break;
                case (ushort)0x6103:
                    break;
               // case (ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCSELECT:
                 //   if (!BotData.bot)
                   // {
                     //   Globals.ServerPC.SendPacket(packet);
                    //}
                    //break;
                case (ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_TELEPORT:
                    Teleport.ClientTeleport(packet);
                    Globals.ServerPC.SendPacket(packet);
                    break;
               /* case (ushort)0x34B6:
                    if (BotData.loopaction == "record")
                    {
                        Globals.MainWindow.script_record_box.Items.Add("waitchar");
                        Globals.MainWindow.script_record_box.Items.Add("wait,5000");
                    }
                    Globals.ServerPC.SendPacket(packet);
                    break;
                case (ushort)0x6323:
                    Globals.ServerPC.SendPacket(packet);
                    break;*/
                default:
                    Globals.ServerPC.SendPacket(packet);
                    break;
            }
        }
    }
}

