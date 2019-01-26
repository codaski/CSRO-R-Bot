using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class Pets
    {
        public static Timer hgp = new Timer();
        public static void PetInfo(Packet packet)
        {
            try
            {
                int pet_index = -1;
                for (int i = 0; i < Char_Data.pets.Length; i++)
                {
                    if (Char_Data.pets[i].id == 0)
                    {
                        pet_index = i;
                        break;
                    }
                }
                uint pet_id = packet.data.ReadDWORD();
                Char_Data.pets[pet_index].id = pet_id;
                //Speed
                for (int i = 0; i < Spawns.pets.Length; i++)
                {
                    if (Spawns.pets[i].id == pet_id)
                    {
                        Char_Data.pets[pet_index].speed = Spawns.pets[i].speed;
                        break;
                    }
                }
                //Speed
                uint pet_model = packet.data.ReadDWORD();
                string pet_type = Mobs_Info.mobstypelist[Mobs_Info.mobsidlist.IndexOf(pet_model)];
                if (pet_type.StartsWith("COS_C_HORSE") || pet_type.StartsWith("COS_C_DHORSE"))
                {
                    Char_Data.pets[pet_index].curhp = packet.data.ReadDWORD();
                    Char_Data.pets[pet_index].maxhp = packet.data.ReadDWORD();
                    Globals.UpdateLogs("Horse Summoned !");
                }
                if (pet_type.StartsWith("COS_P_WOLF") || pet_type.StartsWith("COS_P_WOLF_WHITE") || pet_type.StartsWith("COS_P_BEAR") || pet_type.StartsWith("COS_P_KANGAROO") || pet_type.StartsWith("COS_P_PENGUIN") || pet_type.StartsWith("COS_P_RAVEN") || pet_type.StartsWith("COS_P_FOX") || pet_type.StartsWith("COS_P_JINN"))
                {
                    Char_Data.pets[pet_index].curhp = packet.data.ReadDWORD();
                    packet.data.ReadDWORD(); // Unknown
                    packet.data.ReadQWORD(); // EXP
                    packet.data.ReadBYTE(); // Level
                    Char_Data.pets[pet_index].hgp = packet.data.ReadWORD(); // HGP
                    packet.data.ReadDWORD(); // Unknown
                    string name = packet.data.ReadSTRING(enumStringType.ASCII);
                    packet.data.ReadBYTE(); // Unknown
                    packet.data.ReadDWORD(); // Char ID
                    packet.data.ReadBYTE(); // Unknown
                    Char_Data.char_attackpetid = Char_Data.pets[pet_index].id;
                    Globals.UpdateLogs("Found Attack Pet: " + name);
                    try
                    {
                        hgp.Stop();
                        hgp.Dispose();
                    }
                    catch
                    {
                    }
                    hgp = new System.Timers.Timer();
                    hgp.Interval = 3000;
                    hgp.Elapsed += new ElapsedEventHandler(hgp_Elapsed);
                    hgp.Start();
                    hgp.Enabled = true;
                }
                if (Data.Types.grab_types.IndexOf(pet_type) != -1)
                {
                    Globals.MainWindow.pet_inv_list.Items.Clear();
                    Char_Data.char_grabpetid = Char_Data.pets[pet_index].id;
                    packet.data.ReadQWORD(); // Unknown
                    packet.data.ReadDWORD(); // Unknown
                    string pet_name = packet.data.ReadSTRING(enumStringType.ASCII); // Petname
                    if (pet_name == "")
                    {
                        Globals.UpdateLogs("Found Grab Pet: No Name");
                    }
                    else
                    {
                        Globals.UpdateLogs("Found Grab Pet: " + pet_name);
                    }
                    Char_Data.pets[pet_index].inventory = new Char_Data.Pet_.Inventory_[packet.data.ReadBYTE()];
                    byte items_count = packet.data.ReadBYTE();
                    for (int i = 0; i < items_count; i++)
                    {
                        byte slot = packet.data.ReadBYTE();
                        packet.data.ReadDWORD();
                        uint item_id = packet.data.ReadDWORD();
                        int index = Items_Info.itemsidlist.IndexOf(item_id);
                        string type = Items_Info.itemstypelist[index];
                        string name = Items_Info.itemsnamelist[index];
                        Char_Data.pets[pet_index].inventory[slot].id = item_id;
                        Char_Data.pets[pet_index].inventory[slot].slot = slot;
                        Char_Data.pets[pet_index].inventory[slot].type = type;

                        Globals.MainWindow.pet_inv_list.Items.Add(name);
                        if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU") || type.StartsWith("ITEM_MALL_AVATAR") || type.StartsWith("ITEM_ETC_E060529_GOLDDRAGONFLAG") || type.StartsWith("ITEM_EVENT_CH") || type.StartsWith("ITEM_EVENT_EU"))
                        {
                            byte item_plus = packet.data.ReadBYTE();
                            packet.data.ReadQWORD();
                            Char_Data.pets[pet_index].inventory[slot].durability = packet.data.ReadDWORD();
                            byte blueamm = packet.data.ReadBYTE();
                            for (int a = 0; a < blueamm; a++)
                            {
                                packet.data.ReadBYTE();
                                packet.data.ReadWORD();
                                packet.data.ReadDWORD();
                                packet.data.ReadBYTE();
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
                            Char_Data.pets[pet_index].inventory[slot].count = 1;
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
                            Char_Data.pets[pet_index].inventory[slot].count = 1;
                            Char_Data.pets[pet_index].inventory[slot].durability = 0;
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
                               // packet.data.ReadBYTE();
                            }
                            Char_Data.pets[pet_index].inventory[slot].count = 1;
                            Char_Data.pets[pet_index].inventory[slot].durability = 0;
                        }
                        else if (type == "ITEM_ETC_TRANS_MONSTER")
                        {
                            packet.data.ReadDWORD();
                            Char_Data.pets[pet_index].inventory[slot].count = 1;
                            Char_Data.pets[pet_index].inventory[slot].durability = 0;
                        }
                        else if (type.StartsWith("ITEM_MALL_MAGIC_CUBE"))
                        {
                            packet.data.ReadDWORD();
                            Char_Data.pets[pet_index].inventory[slot].count = 1;
                            Char_Data.pets[pet_index].inventory[slot].durability = 0;
                        }
                        else
                        {
                            ushort count = packet.data.ReadWORD();
                            if (type.Contains("ITEM_ETC_ARCHEMY_ATTRSTONE") || type.Contains("ITEM_ETC_ARCHEMY_MAGICSTONE"))
                            {
                                packet.data.ReadBYTE();
                            }
                            Char_Data.pets[pet_index].inventory[slot].count = count;
                            Char_Data.pets[pet_index].inventory[slot].durability = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("PetData", ex.Message, packet);
            }
        }

        public static void HorseAction(Packet packet)
        {
            if (packet.data.ReadBYTE() == 0x01)
            {
                uint char_id = packet.data.ReadDWORD();
                if (char_id == Character.ID)
                {
                    byte action = packet.data.ReadBYTE();
                    uint pet_id = packet.data.ReadDWORD();
                    for (int i = 0; i < Char_Data.pets.Length; i++)
                    {
                        if (Char_Data.pets[i].id == pet_id)
                        {
                            switch (action)
                            {
                                case 0x00:
                                    Char_Data.char_horseid = 0;
                                    Char_Data.char_horsespeed = 0;
                                    if (BotData.loopaction == "dismounthorse")
                                    {
                                        BotData.loopaction = "";
                                        StartLooping.Start();
                                    }
                                    break;
                                case 0x01:
                                    Char_Data.char_horseid = pet_id;
                                    Char_Data.char_horsespeed = Char_Data.pets[i].speed;
                                    if (BotData.loopaction == "mounthorse")
                                    {
                                        BotData.loopaction = "";
                                        BotData.loopend = 1;
                                        StartLooping.LoadTrainScript();
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static void hgp_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Char_Data.char_attackpetid != 0)
            {
                int pet_index = 0;
                for (pet_index = 0; pet_index < Char_Data.pets.Length; pet_index++)
                {
                    if (Char_Data.pets[pet_index].id == Char_Data.char_attackpetid)
                    {
                        break;
                    }
                }
                if (Char_Data.pets[pet_index].hgp > 0)
                {
                    Char_Data.pets[pet_index].hgp--;
                    if (Globals.MainWindow.attackpet_hgp_use.Checked == true)
                    {
                        uint hgp = (uint)Char_Data.pets[pet_index].hgp * 100 / 10000;
                        if (hgp < Convert.ToUInt32(Globals.MainWindow.attackpet_hgp.Text))
                        {
                            Autopot.UseHGP();
                        }
                    }
                }
                else
                {
                    hgp.Stop();
                    hgp.Dispose();
                }
            }
            else
            {
                hgp.Stop();
                hgp.Dispose();
            }
        }

        public static void PetStats(Packet packet)
        {
            uint pet_id = packet.data.ReadDWORD();
            int pet_index = 0;
            for (int i = 0; i < Char_Data.pets.Length; i++)
            {
                if (Char_Data.pets[i].id == pet_id)
                {
                    pet_index = i;
                    break;
                }
            }
            byte type = packet.data.ReadBYTE();
            switch (type)
            {
                case 0x01:
                    if (pet_id == Char_Data.char_attackpetid)
                    {
                        Char_Data.char_attackpetid = 0;
                    }
                    if (pet_id == Char_Data.char_grabpetid)
                    {
                        Char_Data.char_grabpetid = 0;
                    }
                    Char_Data.pets[pet_index] = new Char_Data.Pet_();
                    break;
            }
        }
    }
}