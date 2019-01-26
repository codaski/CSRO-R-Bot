using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class Teleport
    {
        public static void Tele(uint id, byte type, uint data)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_TELEPORT, false, enumDestination.Server);
            packet.data.AddDWORD(id);
            packet.data.AddBYTE(type);
            packet.data.AddDWORD(data);
            Globals.ServerPC.SendPacket(packet);
            //LoopControl.WalkScript();
        }

        public static void ClientTeleport(Packet packet)
        {
            if (BotData.loopaction == "record")
            {
                uint id = packet.data.ReadDWORD();
                uint model = Mobs_Info.mobsidlist[Mobs_Info.mobstypelist.IndexOf(Spawns.npctype[Spawns.npcid.IndexOf(id)])];
                byte type = packet.data.ReadBYTE();
                uint data = packet.data.ReadDWORD();
                string text = "teleport," + model + "," + type + "," + data;
                Globals.MainWindow.script_record_box.Items.Add(text);
            }
        }
    }
}