using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Silkroad
{
    class StorageControl
    {
        public static void OpenStorage()
        {
            uint id = 0;
            for (int i = 0; i < Spawns.npcid.Count; i++)
            {
                if (Spawns.npctype[i].Contains("WAREHOUSE"))
                {
                    id = Spawns.npcid[i];
                    break;
                }
            }
            if (id != 0)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT, false, enumDestination.Server);
                packet.data.AddDWORD(id);
                BotData.selectingid = id;
                Globals.ServerPC.SendPacket(packet);
            }
            else
            {
                Globals.UpdateLogs("Storage Keeper Not In Range !");
            }
        }

        public static void GetStorageItems(uint id)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_GETSTORAGEITEMS, false, enumDestination.Server);
            packet.data.AddDWORD(id);
            packet.data.AddBYTE(0x00);
            Globals.ServerPC.SendPacket(packet);
        }

        public static void StorageGold(Packet packet)
        {
            Char_Data.storagegold = packet.data.ReadQWORD();
            Globals.UpdateLogs("Gold in storage: " + Char_Data.storagegold);
        }

        public static void ParseStorageItems(Packet packet)
        {
            try
            {
                if (packet.data.ReadBYTE() == 150)
                {
                    Char_Data.storageopened = 1;
                    int items_count = packet.data.ReadBYTE();
                    for (int i = 0; i < items_count; i++)
                    {
                        byte slot = packet.data.ReadBYTE();
                        packet.data.ReadDWORD();
                        uint id = packet.data.ReadDWORD();
                        string type = Items_Info.itemstypelist[Items_Info.itemsidlist.IndexOf(id)];
                        Char_Data.storageid.Add(id);
                        Char_Data.storageslot.Add(slot);
                        Char_Data.storagetype.Add(type);
                        if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU") || type.StartsWith("ITEM_MALL_AVATAR") || type.StartsWith("ITEM_EVENT_AVATAR") || type.StartsWith("ITEM_ETC_E060529_GOLDDRAGONFLAG") || type.StartsWith("ITEM_EVENT_CH") || type.StartsWith("ITEM_EVENT_EU") || type.StartsWith("ITEM_EVENT_AVATAR_W_NASRUN") || type.StartsWith("ITEM_EVENT_AVATAR_M_NASRUN"))
                        {
                            byte item_plus = packet.data.ReadBYTE();
                            packet.data.ReadQWORD();
                            Char_Data.storagedurability.Add(packet.data.ReadDWORD());
                            byte blueamm = packet.data.ReadBYTE();
                            for (int g = 0; g < blueamm; g++)
                            {
                                packet.data.ReadDWORD();
                                packet.data.ReadDWORD();
                            }
                            packet.data.ReadBYTE(); //Socket Stone 1
                            byte socketst = packet.data.ReadBYTE(); //Socket Stones Quanty
                            for (int e = 0; e < socketst; e++)
                            {
                                packet.data.ReadBYTE();  // Socket Slot
                                packet.data.ReadQWORD(); // Socket Code Stone
                            }
                            packet.data.ReadBYTE(); //ADV 2
                            byte flag1 = packet.data.ReadBYTE(); // Flag ?
                            if (flag1 == 1) // ADV Elixir
                            {
                                packet.data.ReadBYTE(); //Unknown
                                packet.data.ReadDWORD(); // ADV Elixir ID 
                                packet.data.ReadDWORD(); // Plus Count
                            }

                            packet.data.ReadBYTE(); //Unknown 3
                            byte Naosei = packet.data.ReadBYTE(); //Unknown
                            for (int f = 0; f < Naosei; f++)
                            {
                                packet.data.ReadBYTE();  //Unknown
                                packet.data.ReadQWORD(); //Unknown
                            }
                            packet.data.ReadBYTE(); //Unknown 4
                            packet.data.ReadBYTE(); //Unknown
                            Char_Data.storagecount.Add(1);
                        }
                        else if ((type.StartsWith("ITEM_COS") && type.Contains("SILK")) || (type.StartsWith("ITEM_EVENT_COS") && !type.Contains("_C_")))
                        {
                            byte flag = packet.data.ReadBYTE();
                            if (flag == 2 || flag == 3 || flag == 4)
                            {
                                packet.data.ReadDWORD(); //Model
                                packet.data.ReadSTRING(enumStringType.ASCII);
                                packet.data.ReadBYTE();
                                if (Data.Types.attack_spawn_types.IndexOf(type) == -1)
                                {
                                    packet.data.ReadDWORD();
                                }
                            }
                            Char_Data.storagecount.Add(1);
                            Char_Data.storagedurability.Add(0);
                        }
                        else if ((type.StartsWith("ITEM_PET2")))
                        {
                            byte flag = packet.data.ReadBYTE();
                            if (flag == 2 || flag == 3 || flag == 4)
                            {
                                packet.data.ReadDWORD(); //Model
                                //packet.data.ReadBYTE(); //Quantidade de letra
                                packet.data.ReadSTRING(enumStringType.ASCII);
                                packet.data.ReadBYTE(); //Level
                                packet.data.ReadBYTE(); //Unknown
                            }
                            Char_Data.storagecount.Add(1);
                            Char_Data.storagedurability.Add(0);
                        }
                        else if (Data.Types.grabpet_spawn_types.IndexOf(type) != -1 || Data.Types.attack_spawn_types.IndexOf(type) != -1)
                        {
                            byte flag = packet.data.ReadBYTE();
                            if (flag == 2 || flag == 3 || flag == 4)
                            {
                                packet.data.ReadDWORD(); //Model
                                packet.data.ReadSTRING(enumStringType.ASCII);
                                if (Data.Types.attack_spawn_types.IndexOf(type) == -1)
                                {
                                    packet.data.ReadDWORD();
                                }
                                packet.data.ReadBYTE();
                            }
                            Char_Data.storagecount.Add(1);
                            Char_Data.storagedurability.Add(0);
                        }
                        else if (type == "ITEM_ETC_TRANS_MONSTER")
                        {
                            packet.data.ReadDWORD();
                            Char_Data.storagecount.Add(1);
                            Char_Data.storagedurability.Add(0);
                        }
                        else if (type.StartsWith("ITEM_MALL_MAGIC_CUBE"))
                        {
                            packet.data.ReadDWORD();
                            Char_Data.storagecount.Add(1);
                            Char_Data.storagedurability.Add(0);
                        }
                        else if (type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_REINFORCE_RECIPE"))
                        {
                            packet.data.ReadBYTE(); // quantidade
                            packet.data.ReadBYTE(); // nao sei
                            Char_Data.storagecount.Add(1);
                            Char_Data.storagedurability.Add(0);
                        }
                              else if (type.Contains("ITEM_ETC_ARCHEMY_ATTRSTONE") || type.Contains("ITEM_ETC_ARCHEMY_LEVEL_ATTRSTONE") || type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE") || type.Contains("ITEM_ETC_ARCHEMY_LEVEL_MAGICSTONE"))
                    {
                        if (type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE_LUCK") || type.Contains("ITEM_ETC_ARCHEMY_LEVEL_MAGICSTONE_LUCK") || type.Contains("ITEM_ETC_ARCHEMY_LEVEL_MAGICSTONE_SOLID") || type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE_SOLID"))
                        {
                            ushort count1 = packet.data.ReadWORD(); // Stone luck apenas le quantidade
                            Char_Data.storagecount.Add(count1);
                                Char_Data.storagedurability.Add(0);
                        }
                        else
                        {
                            ushort count2 = packet.data.ReadWORD(); // outras stones
                            packet.data.ReadBYTE(); // nao sei
                           Char_Data.storagecount.Add(count2);
                                Char_Data.storagedurability.Add(0);
                        }
                    }
                    else
                    {
                        ushort count = packet.data.ReadWORD();
                       
                       Char_Data.storagecount.Add(count);
                                Char_Data.storagedurability.Add(0);
                    }
                    }
                    Globals.MainWindow.storage_list.Items.Clear();
                    for (int i = 0; i < Char_Data.storageid.Count; i++)
                    {
                        uint id = Char_Data.storageid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.storage_list.Items.Add(name);
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("Storage", ex.Message, packet);
            }
            OpenStorage1();
        }

        public static void OpenStorage1()
        {
            if (BotData.loop && BotData.bot)
            {
                Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCSELECT, false, enumDestination.Server);
                packetas.data.AddDWORD(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                packetas.data.AddDWORD(0x00000004);
                Globals.ServerPC.SendPacket(packetas);
            }
        }

        public static void StorageManager(uint id)
        {
            if (BotData.bot && BotData.loop)
            {
                for (byte i = 13; i < Character.inventoryslot; i++)
                {
                    int index = Char_Data.inventoryslot.IndexOf(i);
                    if (index != -1)
                    {
                        string type = Char_Data.inventorytype[index];
                        string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(type)];
                        if (type != null && type.Contains("_A_DEF") == false)
                        {
                            byte slot = 0;
                            for (slot = 0; slot < 150; slot++)
                            {
                                if (Char_Data.storageslot.IndexOf(slot) == -1)
                                {
                                    break;
                                }
                            }
                            if (type.Contains("RARE"))
                            {
                                if (type != Char_Data.f_wep_name && type != Char_Data.s_wep_name)
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                            {
                                if (Globals.MainWindow.wep_drop.Text == "Store" && type != Char_Data.f_wep_name && type != Char_Data.s_wep_name)
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_CH_M_HEAVY") || type.StartsWith("ITEM_CH_M_LIGHT") || type.StartsWith("ITEM_CH_M_CLOTHES") || type.StartsWith("ITEM_CH_W_HEAVY") || type.StartsWith("ITEM_CH_W_LIGHT") || type.StartsWith("ITEM_CH_W_CLOTHES") || type.StartsWith("ITEM_EU_M_HEAVY") || type.StartsWith("ITEM_EU_M_LIGHT") || type.StartsWith("ITEM_EU_M_CLOTHES") || type.StartsWith("ITEM_EU_W_HEAVY") || type.StartsWith("ITEM_EU_W_LIGHT") || type.StartsWith("ITEM_EU_W_CLOTHES"))
                            {
                                if (Globals.MainWindow.armor_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_EU_RING") || type.StartsWith("ITEM_EU_EARRING") || type.StartsWith("ITEM_EU_NECKLACE") || type.StartsWith("ITEM_CH_RING") || type.StartsWith("ITEM_CH_EARRING") || type.StartsWith("ITEM_CH_NECKLACE"))
                            {
                                if (Globals.MainWindow.acc_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_WEAPON"))
                            {
                                if (Globals.MainWindow.wepe_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_SHIELD"))
                            {
                                if (Globals.MainWindow.shielde_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ARMOR"))
                            {
                                if (Globals.MainWindow.prote_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ACCESSARY"))
                            {
                                if (Globals.MainWindow.acce_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ALL_SPOTION_01"))
                            {
                                if (Globals.MainWindow.vigorg_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }

                            if (type.StartsWith("ITEM_ETC_ALL_POTION"))
                            {
                                if (Globals.MainWindow.vigorp_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }

                            if (type.StartsWith("ITEM_ETC_SCROLL_RETURN_01") || type.StartsWith("ITEM_ETC_SCROLL_RETURN_02") || type.StartsWith("ITEM_ETC_SCROLL_RETURN_03"))
                            {
                                if (Globals.MainWindow.return_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_ATTRTABLET") || type.StartsWith("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                            {
                                if (Globals.MainWindow.tablets_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                            {
                                if (Globals.MainWindow.materials_drop.Text == "Store")
                                {
                                    Send(i, slot, id);
                                    break;
                                }
                            }
                        }
                    }
                    if (i + 1 >= Character.inventoryslot)
                    {
                        Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_NPCDESELECT, false, enumDestination.Server);
                        packetas.data.AddDWORD(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                        Globals.ServerPC.SendPacket(packetas);
                    }
                }
            }
        }

        public static void Send(byte slot_inv, byte slot_bnk, uint id)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
            packet.data.AddBYTE(0x02);
            packet.data.AddBYTE(slot_inv);
            packet.data.AddBYTE(slot_bnk);
            packet.data.AddDWORD(id);
            Globals.ServerPC.SendPacket(packet);
        }
    }
}