using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silkroad
{
    class PickupControl
    {
        public static bool there_is_pickable = false;
        public static bool picking = false;
        public static uint picking_id = 0;
        private static void PickItem(uint id)
        {
            if (Char_Data.char_grabpetid != 0 && Globals.MainWindow.pick_with_pet.Checked)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_PETACTION, false, enumDestination.Server);
                packet.data.AddDWORD(Char_Data.char_grabpetid);
                packet.data.AddBYTE(0x08); //Pickup
                packet.data.AddDWORD(id);
                Globals.ServerPC.SendPacket(packet);
            }
            else
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION, false, enumDestination.Server);
                packet.data.AddBYTE(0x01);
                packet.data.AddBYTE(0x02);
                packet.data.AddBYTE(0x01);
                packet.data.AddDWORD(id);
                Globals.ServerPC.SendPacket(packet);
            }
        }

        public static void PickupManager()
        {
            picking = true;
            for (int i = 0; i < Spawns.item_id.Count; i++)
            {
                try
                {
                    System.Threading.Thread.Sleep(10);
                    if (Spawns.item_status[i] == 0 && Spawns.item_type[i] != null)
                    {
                        System.Threading.Thread.Sleep(1);
                        string type = Spawns.item_type[i];
                        if (type.StartsWith("ITEM_ETC_GOLD"))
                        {
                            if (Globals.MainWindow.gold_drop.Text != "Ignore")
                            {
                                PickItem(Spawns.item_id[i]);
                                //Globals.UpdateLogs("PEGUEEEEEEEEE");
                                break;
                            }
                        }
                        if (BotData.itemscount.items_count < Character.inventoryslot - 13)
                        {
                            if (type.Contains("RARE") || type.Contains("ITEM_MALL") || type.Contains("ITEM_QSP") || type.Contains("ITEM_EVENT_CH") || type.Contains("ITEM_EVENT_EU") || type.Contains("ITEM_QNO"))
                            {
                                PickItem(Spawns.item_id[i]);
                                break;
                            }
                            if (type.StartsWith("ITEM_CH_SWORD") || type.StartsWith("ITEM_CH_BLADE") || type.StartsWith("ITEM_CH_SPEAR") || type.StartsWith("ITEM_CH_TBLADE") || type.StartsWith("ITEM_CH_BOW") || type.StartsWith("ITEM_CH_SHIELD") || type.StartsWith("ITEM_EU_DAGGER") || type.StartsWith("ITEM_EU_SWORD") || type.StartsWith("ITEM_EU_TSWORD") || type.StartsWith("ITEM_EU_AXE") || type.StartsWith("ITEM_EU_CROSSBOW") || type.StartsWith("ITEM_EU_DARKSTAFF") || type.StartsWith("ITEM_EU_TSTAFF") || type.StartsWith("ITEM_EU_HARP") || type.StartsWith("ITEM_EU_STAFF") || type.StartsWith("ITEM_EU_SHIELD"))
                            {
                                if (Globals.MainWindow.wep_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_CH_M_HEAVY") || type.StartsWith("ITEM_CH_M_LIGHT") || type.StartsWith("ITEM_CH_M_CLOTHES") || type.StartsWith("ITEM_CH_W_HEAVY") || type.StartsWith("ITEM_CH_W_LIGHT") || type.StartsWith("ITEM_CH_W_CLOTHES") || type.StartsWith("ITEM_EU_M_HEAVY") || type.StartsWith("ITEM_EU_M_LIGHT") || type.StartsWith("ITEM_EU_M_CLOTHES") || type.StartsWith("ITEM_EU_W_HEAVY") || type.StartsWith("ITEM_EU_W_LIGHT") || type.StartsWith("ITEM_EU_W_CLOTHES"))
                            {
                                if (Globals.MainWindow.armor_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_EU_RING") || type.StartsWith("ITEM_EU_EARRING") || type.StartsWith("ITEM_EU_NECKLACE") || type.StartsWith("ITEM_CH_RING") || type.StartsWith("ITEM_CH_EARRING") || type.StartsWith("ITEM_CH_NECKLACE"))
                            {
                                if (Globals.MainWindow.acc_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_REINFORCE_RECIPE_WEAPON"))
                            {
                                if (Globals.MainWindow.wepe_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_REINFORCE_RECIPE_SHIELD"))
                            {
                                if (Globals.MainWindow.shielde_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_LEVEL_TOKEN"))
                            {
                                if (Globals.MainWindow.comboBox2.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_REINFORCE_RECIPE_ARMOR"))
                            {
                                if (Globals.MainWindow.prote_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_REINFORCE_RECIPE_ACCESSARY"))
                            {
                                if (Globals.MainWindow.acce_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_AMMO_ARROW"))
                            {
                                if (Globals.MainWindow.arrow_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_AMMO_BOLT"))
                            {
                                if (Globals.MainWindow.bolt_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_SCROLL_RETURN"))
                            {
                                if (Globals.MainWindow.return_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_CURE_ALL"))
                            {
                                if (Globals.MainWindow.uni_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ALL_SPOTION"))
                            {
                                if (Globals.MainWindow.vigorg_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ALL_POTION"))
                            {
                                if (Globals.MainWindow.vigorp_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_MP_POTION"))
                            {
                                if (Globals.MainWindow.mp_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_HP_POTION"))
                            {
                                if (Globals.MainWindow.hp_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_MAGICSTONE") || type.StartsWith("ITEM_ETC_ARCHEMY_LEVEL_ATTRSTONE") || type.StartsWith("ITEM_ETC_ARCHEMY_ATTRSTONE") || type.StartsWith("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                            {
                                if (Globals.MainWindow.tablets_drop.Text != "Ignore")
                                {
                                    string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(type)].Split('(')[0];
                                    if (Globals.MainWindow.tablet_pick.Items.IndexOf(name) != -1)
                                    {
                                        PickItem(Spawns.item_id[i]);
                                        break;
                                    }
                                }
                            }
                            if (type.StartsWith("ITEM_ETC_ARCHEMY_MATERIAL"))
                            {
                                if (Globals.MainWindow.materials_drop.Text != "Ignore")
                                {
                                    PickItem(Spawns.item_id[i]);
                                    break;
                                }
                            }
                        }
                    }
                    if (i + 1 >= Spawns.item_id.Count)
                    {
                        System.Threading.Thread.Sleep(1);
                        picking = false;
                        there_is_pickable = false;
                        if (!Globals.MainWindow.pick_with_pet.Checked)
                        {
                            LogicControl.Manager();
                        }
                    }
                }
                catch
                {
                    if (i + 1 >= Spawns.item_id.Count)
                    {
                        System.Threading.Thread.Sleep(1);
                        picking = false;
                        there_is_pickable = false;
                        if (!Globals.MainWindow.pick_with_pet.Checked)
                        {
                            LogicControl.Manager();
                        }
                    }
                }
            }
            if (Spawns.item_id.Count == 0)
            {
                System.Threading.Thread.Sleep(1);
                picking = false;
                there_is_pickable = false;
                if (!Globals.MainWindow.pick_with_pet.Checked)
                {
                    LogicControl.Manager();
                }
            }
        }
    }
}