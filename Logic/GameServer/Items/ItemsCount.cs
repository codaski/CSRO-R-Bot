using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silkroad
{
    class ItemsCount
    {
        public static void CountManager()
        {
            HPMPUNIVIGOR_Pots();
            Arrows_Bolts();
            HorseETC();
            Return();
            Speed();
            InventorySlots();
        }

        public static void HPMPUNIVIGOR_Pots()
        {
            uint hp = 0;
            uint mp = 0;
            uint uni = 0;
            uint vigor = 0;
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                string name = Char_Data.inventorytype[i];
                if (name.StartsWith("ITEM_ETC_HP_POTION"))
                {
                    hp = hp + Convert.ToUInt32(Char_Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_ETC_MP_POTION"))
                {
                    mp = mp + Convert.ToUInt32(Char_Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_ETC_CURE_ALL"))
                {
                    uni = uni + Convert.ToUInt32(Char_Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_ETC_ALL_POTION"))
                {
                    vigor = vigor + Convert.ToUInt32(Char_Data.inventorycount[i]);
                }
            }
            BotData.itemscount.hp_pots = hp;
            BotData.itemscount.mp_pots = mp;
            BotData.itemscount.uni_pills = uni;
            BotData.itemscount.vigor = vigor;
            if (Convert.ToInt32(Globals.MainWindow.low_hp_set.Text) >= hp && Globals.MainWindow.low_hp.Checked == true && !BotData.loop)
            {
                Globals.UpdateLogs("Returning To Town: Low HP Potions");
                Action.UseReturn();
            }
            if (Convert.ToInt32(Globals.MainWindow.low_mp_set.Text) >= hp && Globals.MainWindow.low_mp.Checked == true && !BotData.loop)
            {
                Globals.UpdateLogs("Returning To Town: Low MP Potions");
                Action.UseReturn();
            }
            if (Convert.ToInt32(Globals.MainWindow.low_uni_set.Text) >= hp && Globals.MainWindow.low_uni.Checked == true && !BotData.loop)
            {
                Globals.UpdateLogs("Returning To Town: Low Universal Pills");
                Action.UseReturn();
            }

        }

        public static void Arrows_Bolts()
        {
            int arrows = 0;
            int bolts = 0;
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                string name = Char_Data.inventorytype[i];
                if (name == "ITEM_ETC_AMMO_ARROW_01" || name == "ITEM_ETC_AMMO_ARROW_01_DEF" || name == "ITEM_MALL_QUIVER")
                {
                    arrows = arrows + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
                if (name == "ITEM_ETC_AMMO_BOLT_01" || name == "ITEM_ETC_AMMO_BOLT_01_DEF" || name == "ITEM_MALL_BOLT")
                {
                    bolts = bolts + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
            }
            BotData.itemscount.arrows = (uint)arrows;
            BotData.itemscount.bolts = (uint)bolts;
            if (Convert.ToInt32(Globals.MainWindow.low_arrows_set.Text) >= arrows && Globals.MainWindow.low_arrows.Checked == true && !BotData.loop)
            {
                Globals.UpdateLogs("Returning To Town: Low Arrows");
                Action.UseReturn();
            }
            if (Convert.ToInt32(Globals.MainWindow.low_bolts_set.Text) >= bolts && Globals.MainWindow.low_bolts.Checked == true && !BotData.loop)
            {
                Globals.UpdateLogs("Returning To Town: Low Bolts");
                Action.UseReturn();
            }
        }

        public static void HorseETC()
        {
            int pet_hp = 0;
            int horse = 0;
            int hgp = 0;
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                string name = Char_Data.inventorytype[i];
                if (name.StartsWith("ITEM_ETC_COS_HP_POTION"))
                {
                    pet_hp = pet_hp + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
                if (name.StartsWith("ITEM_COS_C_HORSE") || name == "ITEM_COS_C_DHORSE1")
                {
                    horse = horse + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
                if (name == "ITEM_COS_P_HGP_POTION_01")
                {
                    hgp = hgp + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
            }
            BotData.itemscount.pet_hp = (uint)pet_hp;
            BotData.itemscount.horse = (uint)horse;
            BotData.itemscount.hgp = (uint)hgp;
        }

        public static void Speed()
        {
            int speed = 0;
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                string name = Char_Data.inventorytype[i];
                if (name.StartsWith("ITEM_ETC_ARCHEMY_POTION_SPEED"))
                {
                    speed = speed + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
            }
            BotData.itemscount.speed_pots = (uint)speed;
        }

        public static void Return()
        {
            int return_s = 0;
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                string name = Char_Data.inventorytype[i];
                if (name == "ITEM_ETC_SCROLL_RETURN_NEWBIE_01" || name == "ITEM_ETC_SCROLL_RETURN_03" || name == "ITEM_ETC_SCROLL_RETURN_02" || name == "ITEM_ETC_SCROLL_RETURN_01")
                {
                    return_s = return_s + Convert.ToInt32(Char_Data.inventorycount[i]);
                }
            }
            BotData.itemscount.return_scrool = (uint)return_s;
        }

        public static void InventorySlots()
        {
            byte items_count = 0;
            for (int i = 0; i < Char_Data.inventoryslot.Count; i++)
            {
                if (Char_Data.inventoryslot[i] >= 13)
                {
                    items_count++;
                }
            }
            BotData.itemscount.items_count = items_count;
            if (items_count == Character.inventoryslot - 13 && Globals.MainWindow.inv_full.Checked == true && !BotData.loop)
            {
                Globals.UpdateLogs("Returning To Town: Inventory Full");
                Action.UseReturn();
            }
        }
    }
}