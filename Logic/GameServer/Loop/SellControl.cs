using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class SellControl
    {
        public static void OpenShop()
        {
            uint id = 0;
            for (int i = 0; i < Spawns.npcid.Count; i++)
            {
                if (Spawns.npctype[i].Contains("SMITH"))
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
                Globals.UpdateLogs("Weapon Shop Not In Range !");
            }
        }

        public static void SellManager(uint id)
        {
            if (BotData.loop && BotData.bot)
            {
                for (byte slot = 13; slot < Character.inventoryslot; slot++)
                {
                    int index = Char_Data.inventoryslot.IndexOf(slot);
                    if (index != -1)
                    {
                        string type = Char_Data.inventorytype[index];
                        string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(type)];
                        if (type != null && type.Contains("RARE") == false && type.Contains("ITEM_MALL") == false)
                        {
                            if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                            {
                                if (Globals.MainWindow.wep_drop.Text == "Sell" && type != Char_Data.f_wep_name && type != Char_Data.s_wep_name)
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_CH_M_HEAVY") || type.StartsWith("ITEM_CH_M_LIGHT") || type.StartsWith("ITEM_CH_M_CLOTHES") || type.StartsWith("ITEM_CH_W_HEAVY") || type.StartsWith("ITEM_CH_W_LIGHT") || type.StartsWith("ITEM_CH_W_CLOTHES") || type.StartsWith("ITEM_EU_M_HEAVY") || type.StartsWith("ITEM_EU_M_LIGHT") || type.StartsWith("ITEM_EU_M_CLOTHES") || type.StartsWith("ITEM_EU_W_HEAVY") || type.StartsWith("ITEM_EU_W_LIGHT") || type.StartsWith("ITEM_EU_W_CLOTHES"))
                            {
                                if (Globals.MainWindow.armor_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_EU_RING") || type.StartsWith("ITEM_EU_EARRING") || type.StartsWith("ITEM_EU_NECKLACE") || type.StartsWith("ITEM_CH_RING") || type.StartsWith("ITEM_CH_EARRING") || type.StartsWith("ITEM_CH_NECKLACE"))
                            {
                                if (Globals.MainWindow.acc_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_WEAPON"))
                            {
                                if (Globals.MainWindow.wepe_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_SHIELD"))
                            {
                                if (Globals.MainWindow.shielde_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ARMOR"))
                            {
                                if (Globals.MainWindow.prote_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_REINFORCE_RECIPE_ACCESSARY"))
                            {
                                if (Globals.MainWindow.acce_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_ATTRTABLET") || type.StartsWith("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                            {
                                if (Globals.MainWindow.tablets_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                            {
                                if (Globals.MainWindow.materials_drop.Text == "Sell")
                                {
                                    Send(slot, Char_Data.inventorycount[index], id);
                                    break;
                                }
                            }
                        }
                    }
                    if (slot + 1 >= Character.inventoryslot)
                    {
                        BotData.loopaction = "weapon";
                        BuyControl.BuyManager(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                        break;
                    }
                }
            }
        }

        public static void Send(byte slot, ushort count, uint id)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
            packet.data.AddBYTE(0x09); //Sell
            packet.data.AddBYTE(slot); //That says everything
            packet.data.AddWORD(count); //Hmmm ?
            packet.data.AddDWORD(id); //NPC ID
            Globals.ServerPC.SendPacket(packet);
        }
   
        
    }
}