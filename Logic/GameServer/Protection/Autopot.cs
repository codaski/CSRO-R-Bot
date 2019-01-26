using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silkroad
{
    class Autopot
    {
        public static void UseHP()
        {
            if (!BotData.dead)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_HP_POTION") || type.StartsWith("ITEM_ETC_HP_SPOTION"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.HP);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }

        public static void UseHGP()
        {
            if (Char_Data.char_attackpetid != 0)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.StartsWith("ITEM_COS_P_HGP_POTION"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.HGP);
                        packet.data.AddDWORD(Char_Data.char_attackpetid);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }

        public static void UsePetHP(uint id)
        {
            if (Char_Data.char_horseid != 0 || Char_Data.char_attackpetid != 0)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_COS_HP_POTION"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.RecoveryKit);
                        packet.data.AddDWORD(id);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }
        public static void UsePetUni(uint id)
        {
            if (Char_Data.char_horseid != 0 || Char_Data.char_attackpetid != 0)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.Contains("ITEM_COS_P_CURE_ALL"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.Abnormal);
                        packet.data.AddDWORD(id);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }

        public static void UseUni()
        {
            if (!BotData.dead)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.Contains("ITEM_ETC_CURE_ALL"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.UNIVERSAL);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }

        public static void UseVigor()
        {
            if (!BotData.dead)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_ALL_POTION") ||type.StartsWith("ITEM_ETC_ALL_SPOTION"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.VIGOR);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }

        public static void UseMP()
        {
            if (!BotData.dead)
            {
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i];
                    if (type.StartsWith("ITEM_ETC_MP_POTION") || type.StartsWith("ITEM_ETC_MP_SPOTION"))
                    {
                        uint slot = Char_Data.inventoryslot[i];
                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.MP);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                }
            }
        }
    }
}