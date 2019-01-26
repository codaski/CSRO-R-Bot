using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class BuyControl
    {
        public static void OpenShop()
        {
            uint id = 0;
            for (int i = 0; i < Spawns.npcid.Count; i++)
            {
                if (Spawns.npctype[i] != null)
                {
                    string type = Spawns.npctype[i];
                    if (BotData.loopaction == "stable")
                    {
                        if (type.Contains("HORSE"))
                        {
                            id = Spawns.npcid[i];
                            break;
                        }
                    }
                    if (BotData.loopaction == "accessory")
                    {
                        if (type.Contains("ACCESSORY"))
                        {
                            id = Spawns.npcid[i];
                            break;
                        }
                    }
                    if (BotData.loopaction == "potion")
                    {
                        if (type.Contains("POTION"))
                        {
                            id = Spawns.npcid[i];
                            break;
                        }
                    }
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
                Globals.UpdateLogs("Shop Not In Range !");
            }
        }

        public static void BuyManager(uint id)
        {
            if (BotData.loop && BotData.bot)
            {
                string npc_type = Spawns.npctype[Spawns.npcid.IndexOf(id)];
                while (true)
                {
                    if (BotData.loopaction == "weapon")
                    {
                        if (BotData.itemscount.arrows < Convert.ToUInt32(Globals.MainWindow.arrows_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.arrows_count.Text) - BotData.itemscount.arrows > 250)
                            {
                                if (npc_type.StartsWith("NPC_CH_SMITH") || npc_type.StartsWith("NPC_WC_SMITH"))
                                {
                                    Buy(id, 2, 0, 250);
                                    break;
                                }
                                else
                                {
                                    if (npc_type.StartsWith("NPC_KT_SMITH"))
                                    {
                                        Buy(id, 5, 0, 250);
                                        break;
                                    }
                                    else
                                    {
                                        BotData.bot = false;
                                        BotData.loop = false;
                                        Globals.MainWindow.start_button.Text = "Start Bot";
                                        Globals.UpdateLogs("Cannot Buy Arrows !");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.arrows_count.Text) - BotData.itemscount.arrows;
                                if (npc_type.StartsWith("NPC_CH_SMITH") || npc_type.StartsWith("NPC_WC_SMITH"))
                                {
                                    Buy(id, 2, 0, count);
                                    break;
                                }
                                else
                                {
                                    if (npc_type.StartsWith("NPC_KT_SMITH"))
                                    {
                                        Buy(id, 5, 0, count);
                                        break;
                                    }
                                    else
                                    {
                                        BotData.bot = false;
                                        BotData.loop = false;
                                        Globals.MainWindow.start_button.Text = "Start Bot";
                                        Globals.UpdateLogs("Cannot Buy Arrows !");
                                        break;
                                    }
                                }
                            }
                        }

                        if (BotData.itemscount.bolts < Convert.ToUInt32(Globals.MainWindow.bolts_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.bolts_count.Text) - BotData.itemscount.bolts > 250)
                            {
                                if (npc_type.StartsWith("NPC_CA_SMITH") || npc_type.StartsWith("NPC_EU_SMITH") || npc_type.StartsWith("NPC_KT_SMITH"))
                                {
                                    Buy(id, 2, 0, 250);
                                    break;
                                }
                                else
                                {
                                    BotData.bot = false;
                                    BotData.loop = false;
                                    Globals.MainWindow.start_button.Text = "Start Bot";
                                    Globals.UpdateLogs("Cannot Buy Bolts !");
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.bolts_count.Text) - BotData.itemscount.bolts;
                                if (npc_type.StartsWith("NPC_CA_SMITH") || npc_type.StartsWith("NPC_EU_SMITH") || npc_type.StartsWith("NPC_KT_SMITH"))
                                {
                                    Buy(id, 2, 0, count);
                                    break;
                                }
                                else
                                {
                                    BotData.bot = false;
                                    BotData.loop = false;
                                    Globals.MainWindow.start_button.Text = "Start Bot";
                                    Globals.UpdateLogs("Cannot Buy Bolts !");
                                    break;
                                }
                            }
                        }
                        Close();
                        break;
                    }
                    if (BotData.loopaction == "stable")
                    {
                        if (BotData.itemscount.horse < 1)
                        {
                            switch (Globals.MainWindow.horse_buy.Text)
                            {
                                case "<None>":
                                    BotData.itemscount.horse = 1;
                                    break;
                                case "Red Horse":
                                    Buy(id, 0, 0, 1);
                                    break;
                                case "Shadow Horse":
                                    Buy(id, 0, 1, 1);
                                    break;
                                case "Dragon Horse":
                                    Buy(id, 0, 2, 1);
                                    break;
                                case "Ironclad Horse":
                                    Buy(id, 0, 3, 1);
                                    break;
                            }
                            if (Globals.MainWindow.horse_buy.Text != "<None>")
                            {
                                break;
                            }
                        }
                        if (BotData.itemscount.hgp < Convert.ToUInt32(Globals.MainWindow.hgp_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.hgp_count.Text) - BotData.itemscount.hgp > 50)
                            {
                                Buy(id, 3, 3, 50);
                                break;
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.hgp_count.Text) - BotData.itemscount.hgp;
                                Buy(id, 3, 3, count);
                                break;
                            }

                        }
                        if (BotData.itemscount.pet_hp < Convert.ToUInt32(Globals.MainWindow.php_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.php_count.Text) - BotData.itemscount.pet_hp > 50)
                            {
                                switch (Globals.MainWindow.php_buy.Text)
                                {
                                    case "Recovery kit (small)":
                                        Buy(id, 3, 0, 50);
                                        break;
                                    case "Recovery kit (large)":
                                        Buy(id, 3, 1, 50);
                                        break;
                                    case "Recovery kit (x-large)":
                                        Buy(id, 3, 6, 50);
                                        break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.php_count.Text) - BotData.itemscount.pet_hp;
                                switch (Globals.MainWindow.php_buy.Text)
                                {
                                    case "Recovery kit (small)":
                                        Buy(id, 3, 0, count);
                                        break;
                                    case "Recovery kit (large)":
                                        Buy(id, 3, 1, count);
                                        break;
                                    case "Recovery kit (x-large)":
                                        Buy(id, 3, 6, count);
                                        break;
                                }
                            }
                            break;
                        }
                        Close();
                        break;
                    }
                    if (BotData.loopaction == "accessory")
                    {
                        if (BotData.itemscount.return_scrool < 1)
                        {
                            if (npc_type.StartsWith("NPC_EU_ACCESSORY"))
                            {
                                switch (Globals.MainWindow.scroll_buy.Text)
                                {
                                    case "Return Scroll":
                                        Buy(id, 1, 11, 1);
                                        break;
                                    case "Special Return Scroll":
                                        Buy(id, 1, 12, 1);
                                        break;
                                }
                                break;
                            }
                            else
                            {
                                switch (Globals.MainWindow.scroll_buy.Text)
                                {
                                    case "Return Scroll":
                                        Buy(id, 1, 0, 1);
                                        break;
                                    case "Special Return Scroll":
                                        Buy(id, 1, 1, 1);
                                        break;
                                }
                                break;
                            }
                        }
                        if (BotData.itemscount.speed_pots < Convert.ToUInt32(Globals.MainWindow.speed_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.speed_count.Text) - BotData.itemscount.speed_pots > 10)
                            {
                                if (npc_type.StartsWith("NPC_CH_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, 10);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_EU_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 0, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 1, 10);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_ACCESSORY") || npc_type.StartsWith("NPC_CA_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, 10);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 5, 10);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 6, 10);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.speed_count.Text) - BotData.itemscount.speed_pots;
                                if (npc_type.StartsWith("NPC_CH_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_EU_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 0, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 1, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_ACCESSORY") || npc_type.StartsWith("NPC_CA_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 8, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 9, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_ACCESSORY"))
                                {
                                    switch (Globals.MainWindow.speed_buy.Text)
                                    {
                                        case "Drug of wind":
                                            Buy(id, 2, 5, count);
                                            break;
                                        case "Drug of typoon":
                                            Buy(id, 2, 6, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        Close();
                        break;
                    }
                    if (BotData.loopaction == "potion")
                    {
                        if (BotData.itemscount.hp_pots < Convert.ToUInt32(Globals.MainWindow.hp_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.hp_count.Text) - BotData.itemscount.hp_pots > 50)
                            {
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.hp_buy.Text)
                                    {
                                        case "HP Recovery Herb":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION") || npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.hp_buy.Text)
                                    {
                                        case "HP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy HP Pots ! Cannot Find [HP Recovery Herb] In This Shop !");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.hp_buy.Text)
                                    {
                                        case "HP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy HP Pots ! Cannot Find [HP Recovery Herb] In This Shop !");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy HP Pots ! Cannot Find [HP Recovery Potion (Small)] In This Shop !");
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 0, 50);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 1, 50);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 2, 50);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.hp_count.Text) - BotData.itemscount.hp_pots;
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.hp_buy.Text)
                                    {
                                        case "HP Recovery Herb":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 2, count);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 3, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 4, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION") || npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.hp_buy.Text)
                                    {
                                        case "HP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy HP Pots ! Cannot Find [HP Recovery Herb] In This Shop !");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 2, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 3, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.hp_buy.Text)
                                    {
                                        case "HP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy HP Pots ! Cannot Find [HP Recovery Herb] In This Shop !");
                                            break;
                                        case "HP Recovery Potion (Small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy HP Pots ! Cannot Find [HP Recovery Potion (Small)] In This Shop !");
                                            break;
                                        case "HP Recovery Potion (Medium)":
                                            Buy(id, 0, 0, count);
                                            break;
                                        case "HP Recovery Potion (Large)":
                                            Buy(id, 0, 1, count);
                                            break;
                                        case "HP Recovery Potion (X-Large)":
                                            Buy(id, 0, 2, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (BotData.itemscount.mp_pots < Convert.ToUInt32(Globals.MainWindow.mp_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.mp_count.Text) - BotData.itemscount.mp_pots > 50)
                            {
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.mp_buy.Text)
                                    {
                                        case "MP Recovery Herb":
                                            Buy(id, 0, 5, 50);
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION") || npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.mp_buy.Text)
                                    {
                                        case "MP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy MP Pots ! Cannot Find [MP Recovery Herb] In This Shop !");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 5, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.mp_buy.Text)
                                    {
                                        case "MP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy MP Pots ! Cannot Find [MP Recovery Herb] In This Shop !");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy MP Pots ! Cannot Find [MP Recovery Potion (Small)] In This Shop !");
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 3, 50);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 4, 50);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 5, 50);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.mp_count.Text) - BotData.itemscount.mp_pots;
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.mp_buy.Text)
                                    {
                                        case "MP Recovery Herb":
                                            Buy(id, 0, 5, count);
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 6, count);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 7, count);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 8, count);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 9, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION") || npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.mp_buy.Text)
                                    {
                                        case "MP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy MP Pots ! Cannot Find [MP Recovery Herb] In This Shop !");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            Buy(id, 0, 4, count);
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 5, count);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 6, count);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 7, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.mp_buy.Text)
                                    {
                                        case "MP Recovery Herb":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy MP Pots ! Cannot Find [MP Recovery Herb] In This Shop !");
                                            break;
                                        case "MP Recovery Potion (Small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy MP Pots ! Cannot Find [MP Recovery Potion (Small)] In This Shop !");
                                            break;
                                        case "MP Recovery Potion (Medium)":
                                            Buy(id, 0, 3, count);
                                            break;
                                        case "MP Recovery Potion (Large)":
                                            Buy(id, 0, 4, count);
                                            break;
                                        case "MP Recovery Potion (X-Large)":
                                            Buy(id, 0, 5, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }

                        if (BotData.itemscount.uni_pills < Convert.ToUInt32(Globals.MainWindow.uni_count.Text))
                        {
                            if (Convert.ToUInt32(Globals.MainWindow.uni_count.Text) - BotData.itemscount.uni_pills > 50)
                            {
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.uni_buy.Text)
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 10, 50);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 11, 50);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 12, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 13, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION") || npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.uni_buy.Text)
                                    {
                                        case "Universal Pill (small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy Universal Pills ! Cannot Find [Universal Pill (small)] In This Shop !");
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 8, 50);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 9, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 10, 50);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.uni_buy.Text)
                                    {
                                        case "Universal Pill (small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy Universal Pills ! Cannot Find [Universal Pill (small)] In This Shop !");
                                            break;
                                        case "Universal Pill (medium)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy Universal Pills ! Cannot Find [Universal Pill (medium)] In This Shop !");
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 6, 50);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 7, 50);
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                uint count = Convert.ToUInt32(Globals.MainWindow.uni_count.Text) - BotData.itemscount.uni_pills;
                                if (npc_type.StartsWith("NPC_CH_POTION") || npc_type.StartsWith("NPC_EU_POTION"))
                                {
                                    switch (Globals.MainWindow.uni_buy.Text)
                                    {
                                        case "Universal Pill (small)":
                                            Buy(id, 0, 10, count);
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 11, count);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 12, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 13, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_WC_POTION") || npc_type.StartsWith("NPC_CA_POTION"))
                                {
                                    switch (Globals.MainWindow.uni_buy.Text)
                                    {
                                        case "Universal Pill (small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy Universal Pills ! Cannot Find [Universal Pill (small)] In This Shop !");
                                            break;
                                        case "Universal Pill (medium)":
                                            Buy(id, 0, 8, count);
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 9, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 10, count);
                                            break;
                                    }
                                    break;
                                }
                                if (npc_type.StartsWith("NPC_KT_POTION"))
                                {
                                    switch (Globals.MainWindow.uni_buy.Text)
                                    {
                                        case "Universal Pill (small)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy Universal Pills ! Cannot Find [Universal Pill (small)] In This Shop !");
                                            break;
                                        case "Universal Pill (medium)":
                                            BotData.bot = false;
                                            BotData.loop = false;
                                            Globals.MainWindow.start_button.Text = "Start Bot";
                                            Globals.UpdateLogs("Cannot Buy Universal Pills ! Cannot Find [Universal Pill (medium)] In This Shop !");
                                            break;
                                        case "Universal Pill (large)":
                                            Buy(id, 0, 6, count);
                                            break;
                                        case "Special Universal Pill (small)":
                                            Buy(id, 0, 7, count);
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        Close();
                        break;
                    }
                }
            }
        }

        public static void Buy(uint id, byte tab, byte slot, uint count)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
            packet.data.AddBYTE(0x08);
            packet.data.AddBYTE(tab);
            packet.data.AddBYTE(slot);
            packet.data.AddWORD((ushort)count);
            packet.data.AddDWORD(id);
            Globals.ServerPC.SendPacket(packet);
        }

        public static void Close()
        {
            if (BotData.loopaction == "weapon")
            {
                Packet packetas = new Packet((ushort)0x703E, false, enumDestination.Server);
                packetas.data.AddDWORD(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                packetas.data.AddBYTE(0x02);
                Globals.ServerPC.SendPacket(packetas);
            }
            else
            {
                BotData.selectedid = 0;
                BotData.selected = 0;
                System.Threading.Thread.Sleep(2000);
                InventoryControl.MergeItems();
            }
        }
    }
}