using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silkroad
{
    class Action
    {

        public static void Chat(string text)
        {
            Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_CHAT, false, enumDestination.Server);
            packetas.data.AddBYTE(0x01);
            int a = BotData.chatbyte + 1;
            packetas.data.AddBYTE((byte)a);
            packetas.data.AddSTRING(text, enumStringType.ASCII);
            Globals.ServerPC.SendPacket(packetas);
        }

        public static void MountHorse()
        {
            byte found_horse = 0;
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                string type = Char_Data.inventorytype[i].ToString();
                if (type.StartsWith("ITEM_COS_C_HORSE") || type.StartsWith("ITEM_COS_C_DHORSE"))
                {
                    found_horse = 1;
                    Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                    uint slot = Char_Data.inventoryslot[i];
                    packet.data.AddBYTE((byte)slot);
                    packet.data.AddWORD(0x0C30);
                    Globals.ServerPC.SendPacket(packet);
                    break;
                }
            }
            if (found_horse == 0)
            {
                BotData.loopend = 1;
                StartLooping.LoadTrainScript();
            }
        }

        public static void UseReturn()
        {
            if (BotData.returning == 0)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i].ToString();
                    if (type == "ITEM_ETC_SCROLL_RETURN_NEWBIE_01" || type == "ITEM_ETC_SCROLL_RETURN_03" || type == "ITEM_ETC_SCROLL_RETURN_02" || type == "ITEM_ETC_SCROLL_RETURN_01")
                    {
                        BotData.returning = 1;
                        BotData.Statistic.return_count++;
                        //Globals.MainWindow.lb_return_count.Content = BotData.Statistic.return_count.ToString();
                        uint slot = Char_Data.inventoryslot[i];
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddWORD((ushort)Action.UsageID.RETURN);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                    else
                    {
                        if (i + 1 == Char_Data.inventorytype.Count)
                        {
                            Globals.UpdateLogs("Return Scroll Not Found");
                        }
                    }
                }
            }
        }
        public static void UseDrugSpeed()
        {
            string name11 = "SKILL_ETC_ARCHEMY_POTION_SPEED_11_01";
            int index1 = Char_Data.skillnamewaiting.IndexOf(name11);
            if (index1 == -1)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYUSE, true, enumDestination.Server);
                for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                {
                    string type = Char_Data.inventorytype[i].ToString();
                    if (type == "ITEM_EVENT_RENT_ARCHEMY_POTION_SPEED_11" || type == "ITEM_EVENT_ARCHEMY_POTION_SPEED_11" || type == "ITEM_ETC_ARCHEMY_LEVEL_POTION_SPEED_11" || type == "ITEM_ETC_ARCHEMY_POTION_SPEED_11")
                    {
                        //BotData.returning = 1;
                        //BotData.Statistic.return_count++;
                        //Globals.MainWindow.lb_return_count.Content = BotData.Statistic.return_count.ToString();
                        uint slot = Char_Data.inventoryslot[i];
                        packet.data.AddBYTE((byte)slot);
                        packet.data.AddWORD(0x0C30);
                        packet.data.AddBYTE(0x0D);
                        packet.data.AddBYTE(0x01);
                        Globals.ServerPC.SendPacket(packet);
                        break;
                    }
                    else
                    {
                        if (i + 1 == Char_Data.inventorytype.Count)
                        {
                            Globals.UpdateLogs("Drug Not found");
                        }
                    }
                }
            }
            else
            {
                Globals.UpdateLogs("Drug in Use");
            }
        }

        public static void UseZerk()
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_ZERK, false, enumDestination.Server);
            packet.data.AddBYTE(0x01);
            Globals.ServerPC.SendPacket(packet);
        }

        public static void WalkTo(int X, int Y)
        {
            uint xPos = 0;
            uint yPos = 0;

            if (X > 0 && Y > 0)
            {
                xPos = (uint)((X % 192) * 10);
                yPos = (uint)((Y % 192) * 10);
            }
            else
            {
                if(X < 0 && Y > 0)
                {
                    xPos = (uint)((192 + (X % 192)) * 10);
                    yPos = (uint)((Y % 192) * 10);
                }
                else
                {
                    if (X > 0 && Y < 0)
                    {
                        xPos = (uint)((X % 192) * 10);
                        yPos = (uint)((192 + (Y % 192)) * 10);
                    }
                }
            }

            byte xSector = (byte)((X - (int)(xPos / 10)) / 192 + 135);
            byte ySector = (byte)((Y - (int)(yPos / 10)) / 192 + 92);


            if (Char_Data.char_horseid == 0)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_MOVEMENT, false, enumDestination.Server);
                packet.data.AddBYTE(0x01);
                if (Character.cave.char_incave == true)
                {
                    xPos = (uint)((X - (int)((xSector - 135) * 192)) * 10);
                    yPos = (uint)((Y - (int)((162 - 92) * 192)) * 10);
                    packet.data.AddBYTE(Character.cave.xsector);
                    packet.data.AddBYTE(0x80);
                    packet.data.AddDWORD(xPos);
                    packet.data.AddDWORD(0x00000000);
                    Packet.Data data = new Packet.Data();
                    data.AddDWORD(yPos - 250);
                    packet.data.AddBYTE(data.ReadBYTE());
                    packet.data.AddBYTE(data.ReadBYTE());
                    packet.data.AddBYTE(0xFF);
                    packet.data.AddBYTE(0xFF);
                }
                else
                {
                    ushort xposition = (ushort)((X - (int)((xSector - 135) * 192)) * 10);
                    ushort yposition = (ushort)((Y - (int)((ySector - 92) * 192)) * 10);
                    packet.data.AddBYTE(xSector);
                    packet.data.AddBYTE(ySector);
                    packet.data.AddWORD(xposition);
                    packet.data.AddWORD(0x0000);
                    packet.data.AddWORD(yposition);
                }
                Globals.ServerPC.SendPacket(packet);
            }
            else
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_PETACTION, false, enumDestination.Server);
                packet.data.AddDWORD(Char_Data.char_horseid);
                packet.data.AddBYTE(0x01);
                packet.data.AddBYTE(0x01);
                if (Character.cave.char_incave == true)
                {
                    xPos = (uint)((X - (int)((xSector - 135) * 192)) * 10);
                    yPos = (uint)((Y - (int)((162 - 92) * 192)) * 10);

                    packet.data.AddBYTE(Character.cave.xsector);
                    packet.data.AddBYTE(0x80);
                    packet.data.AddDWORD(xPos);
                    packet.data.AddDWORD(0x00000000);
                    Packet.Data data = new Packet.Data();
                    data.AddDWORD(yPos - 250);
                    packet.data.AddBYTE(data.ReadBYTE());
                    packet.data.AddBYTE(data.ReadBYTE());
                    packet.data.AddBYTE(0xFF);
                    packet.data.AddBYTE(0xFF);
                }
                else
                {
                    ushort xposition = (ushort)((X - (int)((xSector - 135) * 192)) * 10);
                    ushort yposition = (ushort)((Y - (int)((ySector - 92) * 192)) * 10);
                    packet.data.AddBYTE(xSector);
                    packet.data.AddBYTE(ySector);
                    packet.data.AddWORD(xposition);
                    packet.data.AddWORD(0x0000);
                    packet.data.AddWORD(yposition);
                }
                Globals.ServerPC.SendPacket(packet);
            }
        }

        public static void AttackWithPet()
        {
            if (Char_Data.char_attackpetid != 0)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_PETACTION, false, enumDestination.Server);
                packet.data.AddDWORD(Char_Data.char_attackpetid);
                packet.data.AddBYTE(0x02);
                packet.data.AddDWORD(MonsterControl.monster_id);
                Globals.ServerPC.SendPacket(packet);
            }
        }

        public static int CalculatePositionX(ushort xSector, float X)
        {
            return (int)((xSector - 135) * 192 + X / 10);
        }
        public static int CalculatePositionY(ushort ySector, float Y)
        {
            return (int)((ySector - 92) * 192 + Y / 10);
        }
        public enum UsageID : ushort
        {
            HP = 0x0101, // OK
            MP = 0x0201, // ok
            VIGOR = 0x0301, // Ok
            UNIVERSAL = 0x0902, // OK
            RETURN = 0x0103, // OK
            HGP = 0x0901,
            RecoveryKit = 0x0401,
            Abnormal = 0x0702,
            DRUG =  0113
        }
    }
}