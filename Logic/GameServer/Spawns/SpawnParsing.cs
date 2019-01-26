using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silkroad
{
    class Parse
    {
        public static void ParseMob(Packet packet, int index)
        {
            try
            {
                uint id = packet.data.ReadDWORD(); // MOB ID
                byte xsec = packet.data.ReadBYTE();
                byte ysec = packet.data.ReadBYTE();
                float xcoord = packet.data.ReadSINGLE();
                packet.data.ReadSINGLE();
                float ycoord = packet.data.ReadSINGLE();
                packet.data.ReadWORD(); // Position
                byte move = packet.data.ReadBYTE(); // Moving
                packet.data.ReadBYTE(); // Running
                if (move == 1)
                {
                    xsec = packet.data.ReadBYTE();
                    ysec = packet.data.ReadBYTE();
                    if (ysec == 0x80)
                    {
                        xcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                    }
                    else
                    {
                        xcoord = packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD();
                    }
                }
                else
                {
                    packet.data.ReadBYTE(); // Unknown
                    packet.data.ReadWORD(); // Unknwon
                }
                int xas = Convert.ToInt32((xsec - 135) * 192 + (xcoord / 10));
                int yas = Convert.ToInt32((ysec - 92) * 192 + (ycoord / 10));
                int dist = Math.Abs((xas - Character.X)) + Math.Abs((yas - Character.Y));
                byte alive = packet.data.ReadBYTE(); // Alive
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Zerk Active
                packet.data.ReadSINGLE(); // Walk Speed
                packet.data.ReadSINGLE(); // Run Speed
                packet.data.ReadSINGLE(); // Zerk Speed
                byte buffs = packet.data.ReadBYTE(); // Buffs Count On Mobs
                for (byte i = 0; i < buffs; i++)
                {
                    packet.data.ReadDWORD(); // Skill ID ?
                    packet.data.ReadDWORD(); // Skill TempID / Skill Duration ?
                }
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Unknown
                byte type = packet.data.ReadBYTE();
                if (alive == 1)
                {
                    Spawns.mob_id.Add(id);
                    Spawns.mob_name.Add(Mobs_Info.mobsnamelist[index]);
                    Spawns.mob_dist.Add(dist);
                    Spawns.mob_x.Add(xas);
                    Spawns.mob_y.Add(yas);
                    Spawns.mob_status.Add(0);
                    Spawns.mob_type.Add(type);
                    Spawns.mob_priority.Add(0);

                    if (type == (byte)Globals.enumMobType.Unique && Globals.MainWindow.alert_unique.Checked)
                    {
                        Alert.StartAlert();
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnMOB", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }





        public static void ParsePets(Packet packet, int index)
        {
            try
            {
                int s_index = 0;
                for (int i = 0; i < Spawns.pets.Length; i++)
                {
                    if (Spawns.pets[i].type == null)
                    {
                        s_index = i;
                        break;
                    }
                }
                uint pet_id = packet.data.ReadDWORD(); // PET ID
                Spawns.pets[s_index].id = pet_id;
                Spawns.pets[s_index].type = Mobs_Info.mobstypelist[index];
                byte xsec = packet.data.ReadBYTE();
                byte ysec = packet.data.ReadBYTE();
                float xcoord = packet.data.ReadSINGLE();
                packet.data.ReadSINGLE();
                float ycoord = packet.data.ReadSINGLE();

                packet.data.ReadWORD(); // Position
                byte move = packet.data.ReadBYTE(); // Moving
                packet.data.ReadBYTE(); // Running

                if (move == 1)
                {
                    xsec = packet.data.ReadBYTE();
                    ysec = packet.data.ReadBYTE();
                    if (ysec == 0x80)
                    {
                        xcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                    }
                    else
                    {
                        xcoord = packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD();
                    }
                }
                else
                {
                    packet.data.ReadBYTE(); // Unknown
                    packet.data.ReadWORD(); // Unknwon
                }
                Spawns.pets[s_index].x = Convert.ToInt32((xsec - 135) * 192 + (xcoord / 10));
                Spawns.pets[s_index].y = Convert.ToInt32((ysec - 92) * 192 + (ycoord / 10));
                packet.data.ReadBYTE();
                packet.data.ReadBYTE();
                packet.data.ReadBYTE();
                packet.data.ReadBYTE();
                packet.data.ReadBYTE();
                packet.data.ReadSINGLE();
                Spawns.pets[s_index].speed = packet.data.ReadSINGLE();
                packet.data.ReadSINGLE();
                packet.data.ReadWORD();
                string type = Mobs_Info.mobstypelist[index];
                //if (Mobs_Info.mobstypelist[index].StartsWith("COS_C_PEGASUS") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_OSTRICH") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_SCARABAEUS") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_TIGER") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_WILDPIG") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_HORSE") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_CAMEL") || Mobs_Info.mobstypelist[index].StartsWith("COS_T_DHORSE") || Mobs_Info.mobstypelist[index].StartsWith("COS_C_DHORSE"))
                if (Mobs_Info.mobstypelist[index].StartsWith("COS_C") || Mobs_Info.mobstypelist[index].StartsWith("COS_T_DHORSE"))
                {
                }
                else
                {
                    if (type.StartsWith("COS_U_UNKNOWN"))
                    {
                        packet.data.ReadWORD();
                        packet.data.ReadBYTE();
                    }
                    else
                    {
                        packet.data.ReadSTRING(enumStringType.UNICODE);
                        if (type.StartsWith("COS_T_COW") || type == "COS_T_DONKEY" | type.StartsWith("COS_T_HORSE") || type.StartsWith("COS_T_CAMEL") || type.StartsWith("COS_T_DHORSE") || type.StartsWith("COS_T_BUFFALO") || type.StartsWith("COS_T_WHITEELEPHANT") || type.StartsWith("COS_T_RHINOCEROS"))
                        {
                            if (type.StartsWith("COS_T_BUFFALO") || type.StartsWith("COS_T_WHITEELEPHANT") || type.StartsWith("COS_T_RHINOCEROS"))
                            {
                                packet.data.ReadBYTE();
                            }
                            packet.data.ReadWORD();
                            packet.data.ReadDWORD();
                        }
                        else
                        {
                            packet.data.ReadSTRING(enumStringType.UNICODE);
                            if (type.StartsWith("COS_P_RAVEN"))
                            {
                                packet.data.ReadBYTE();
                            }
                            if (type.StartsWith("COS_P_WOLF"))
                            {
                                packet.data.ReadBYTE();
                            }
                            if (type.StartsWith("COS_P_BROWNIE"))
                            {
                                packet.data.ReadDWORD();
                                packet.data.ReadBYTE();
                            }
                            else
                            {
                                if (type.StartsWith("COS_P_JINN") || type.StartsWith("COS_P_KANGAROO") || type.StartsWith("COS_P_BEAR") || type.StartsWith("COS_P_FOX") || type.StartsWith("COS_P_PENGUIN"))
                                {
                                    packet.data.ReadBYTE();
                                }
                                packet.data.ReadBYTE();
                                packet.data.ReadDWORD();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnPET", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }
        public static void ParseNPC(Packet packet, int index)
        {
            try
            {
                uint id = packet.data.ReadDWORD();
                    Spawns.npcid.Add(id);
                    Spawns.npctype.Add(Mobs_Info.mobstypelist[index]);
                    byte xsec = packet.data.ReadBYTE();
                    byte ysec = packet.data.ReadBYTE();
                    float xcoord = packet.data.ReadSINGLE();
                    packet.data.ReadSINGLE();
                    float ycoord = packet.data.ReadSINGLE();

                    packet.data.ReadWORD(); // Position
                    byte move = packet.data.ReadBYTE(); // Moving
                    packet.data.ReadBYTE(); // Running

                    if (move == 1)
                    {
                        xsec = packet.data.ReadBYTE();
                        ysec = packet.data.ReadBYTE();
                        if (ysec == 0x80)
                        {
                            xcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                            packet.data.ReadWORD();
                            packet.data.ReadWORD();
                            ycoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        }
                        else
                        {
                            xcoord = packet.data.ReadWORD();
                            packet.data.ReadWORD();
                            ycoord = packet.data.ReadWORD();
                        }
                    }
                    else
                    {
                        packet.data.ReadBYTE(); // Unknown
                        packet.data.ReadWORD(); // Unknwon
                    }

                    packet.data.ReadBYTE(); // Alive
                    packet.data.ReadBYTE();
                    packet.data.ReadBYTE(); // Unknown
                    packet.data.ReadBYTE(); // Unknown
                    packet.data.ReadBYTE(); // Zerk Active
                    packet.data.ReadSINGLE(); // Walk Speed
                    packet.data.ReadSINGLE(); // Run Speed
                    packet.data.ReadSINGLE(); // Zerk Speed
                    packet.data.ReadBYTE();
                    ushort check = packet.data.ReadWORD();
                    if (check != 0)
                    {
                        byte count = packet.data.ReadBYTE();
                        for (byte i = 0; i < count; i++)
                        {
                            packet.data.ReadBYTE();
                        }
                    }
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnNPC", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }



        public static void ParsePortal(Packet packet, int index)
        {
            try
            {
                uint id = packet.data.ReadDWORD();
                Spawns.npcid.Add(id);
                Spawns.npctype.Add(Mobs_Info.mobstypelist[index]);
                byte xsec = packet.data.ReadBYTE();
                byte ysec = packet.data.ReadBYTE();
                float xcoord = packet.data.ReadSINGLE();
                packet.data.ReadSINGLE();
                float ycoord = packet.data.ReadSINGLE();
                packet.data.ReadWORD(); // Position
                packet.data.ReadDWORD();
                packet.data.ReadQWORD();
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnTELEPORT", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }
        public static void ParseChar(Packet packet, int index)
        {
            try
            {
                int s_index = 0;
                for (int i = 0; i < Spawns.characters.Length; i++)
                {
                    if (Spawns.characters[i].charname == null)
                    {
                        s_index = i;
                        break;
                    }
                }
                int trade = 0;
                int stall = 0;
                packet.data.ReadBYTE(); // Volume/Height
                packet.data.ReadBYTE(); // Rank
                packet.data.ReadBYTE(); // Icons
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Max Slots
                int items_count = packet.data.ReadBYTE();
                for (int a = 0; a < items_count; a++)
                {
                    uint itemid = packet.data.ReadDWORD();
                    int itemindex = Items_Info.itemsidlist.IndexOf(itemid);
                    if (Items_Info.itemstypelist[itemindex].StartsWith("ITEM_CH") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_EU") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_FORT") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_ROC_CH") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_ROC_EU"))
                    {
                        byte plus = packet.data.ReadBYTE(); // Item Plus
                    }
                    if (Items_Info.itemstypelist[itemindex].StartsWith("ITEM_EU_M_TRADE") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_EU_F_TRADE") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_CH_M_TRADE") || Items_Info.itemstypelist[itemindex].StartsWith("ITEM_CH_W_TRADE"))
                    {
                        trade = 1;
                    }
                }
                packet.data.ReadBYTE(); // Max Avatars Slot
                int avatar_count = packet.data.ReadBYTE();
                for (int a = 0; a < avatar_count; a++)
                {
                    uint avatarid = packet.data.ReadDWORD();
                   // int avatarindex = Items_Info.itemsidlist.IndexOf(avatarid);
                    byte plus = packet.data.ReadBYTE(); // Avatar Plus
                   // if (avatarindex == -1)
                    //{
                      //  Globals.UpdateLogs("Error on avatars !!! Avatar ID: " + avatarid);
                    //}
                }
                int mask = packet.data.ReadBYTE();
                if (mask == 1)
                {
                    uint id = packet.data.ReadDWORD();
                    string type = Mobs_Info.mobstypelist[Mobs_Info.mobsidlist.IndexOf(id)];
                    if (type.StartsWith("CHAR"))
                    {
                        packet.data.ReadBYTE();
                        byte count = packet.data.ReadBYTE();
                        for (int i = 0; i < count; i++)
                        {
                            packet.data.ReadDWORD();
                        }
                    }
                }
                uint charid = packet.data.ReadDWORD();
                Spawns.characters[s_index].id = charid;
                Globals.MainWindow.listCharsSpawned.Items.Add(charid);

                byte xsec = packet.data.ReadBYTE();
                byte ysec = packet.data.ReadBYTE();
                float xcoord = packet.data.ReadSINGLE();
                packet.data.ReadSINGLE();
                float ycoord = packet.data.ReadSINGLE();

                packet.data.ReadWORD(); // Position
                byte move = packet.data.ReadBYTE(); // Moving
                packet.data.ReadBYTE(); // Running

                if (move == 1)
                {
                    xsec = packet.data.ReadBYTE();
                    ysec = packet.data.ReadBYTE();
                    if (ysec == 0x80)
                    {
                        xcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                    }
                    else
                    {
                        xcoord = packet.data.ReadWORD();
                        packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD();
                    }
                }
                else
                {
                    packet.data.ReadBYTE(); // No Destination
                    packet.data.ReadWORD(); // Angle
                }

                Spawns.characters[s_index].alive = packet.data.ReadBYTE(); // Alive
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Unknown

                packet.data.ReadDWORD(); // Walking speed
                packet.data.ReadDWORD(); // Running speed
                packet.data.ReadDWORD(); // Berserk speed

                int active_skills = packet.data.ReadBYTE(); // Buffs count
                Spawns.characters[s_index].buffs = new Spawns.Characters_.Buffs_[100];
                for (int a = 0; a < active_skills; a++)
                {
                    uint skillid = packet.data.ReadDWORD();
                    int buffindex = Skills_Info.skillsidlist.IndexOf(skillid);
                    Spawns.characters[s_index].buffs[a].name = Skills_Info.skillsnamelist[Skills_Info.skillsidlist.IndexOf(skillid)];
                    string type = Skills_Info.skillstypelist[buffindex];
                    Spawns.characters[s_index].buffs[a].tempid = packet.data.ReadDWORD(); // Temp ID
                    if (type.StartsWith("SKILL_EU_CLERIC_RECOVERYA_GROUP") || type.StartsWith("SKILL_EU_BARD_BATTLAA_GUARD") || type.StartsWith("SKILL_EU_BARD_DANCEA") || type.StartsWith("SKILL_EU_BARD_SPEEDUPA_HITRATE"))
                    {
                        packet.data.ReadBYTE();
                    }
                }
                string name = packet.data.ReadSTRING(enumStringType.ASCII);
                Spawns.characters[s_index].charname = name;
               // Globals.MainWindow.listCharsSpawned.Items.Add(name);
                packet.data.ReadBYTE(); // Unknown
                packet.data.ReadBYTE(); // Job type
                packet.data.ReadBYTE(); // Job level
                int cnt = packet.data.ReadBYTE();
                packet.data.ReadBYTE();
                if (cnt == 1)
                {
                    packet.data.ReadDWORD();
                }
                packet.data.ReadBYTE(); // Unknown
                stall = packet.data.ReadBYTE(); // Stall flag
                packet.data.ReadBYTE(); // Unknown
                string guild = packet.data.ReadSTRING(enumStringType.ASCII); // Guild
                Spawns.characters[s_index].guildname = guild;
                if (trade == 1)
                {
                    packet.data.ReadWORD();
                }
                else
                {
                    packet.data.ReadDWORD(); // Guild ID
                    packet.data.ReadSTRING(enumStringType.ASCII); // Grant Name
                    packet.data.ReadDWORD();
                    packet.data.ReadDWORD();
                    packet.data.ReadDWORD();
                    packet.data.ReadWORD();
                    if (stall == 4)
                    {
                        packet.data.ReadSTRING(enumStringType.ASCII);
                        packet.data.ReadDWORD();
                        packet.data.ReadWORD();
                    }
                    else
                    {
                        packet.data.ReadWORD();
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnCHAROTHER", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }
        public static void ParseItems(Packet packet, int itemsindex)
        {
            try
            {
                string type = Items_Info.itemstypelist[itemsindex].ToString();
                if (type.StartsWith("ITEM_ETC_GOLD"))
                {
                    packet.data.ReadDWORD(); // Ammount
                    //Globals.UpdateLogs("GOLD IHULL");
                }
                if (type.StartsWith("ITEM_QSP"))
                {
                    packet.data.ReadSTRING(enumStringType.ASCII); // Name
                }
                if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU"))
                {
                    packet.data.ReadBYTE(); // Plus
                }
                uint id = packet.data.ReadDWORD(); // ID
                packet.data.ReadBYTE(); //XSEC
                packet.data.ReadBYTE(); //YSEC
                packet.data.ReadSINGLE(); //X
                packet.data.ReadSINGLE(); //Z
                packet.data.ReadSINGLE(); //Y
                packet.data.ReadWORD(); //POS
                if (packet.data.ReadBYTE() == 1) // Owner exist
                {
                    if (packet.data.ReadDWORD() == Character.AccountID) // Owner ID
                    {
                        Spawns.item_id.Add(id);
                        Spawns.item_status.Add(0);
                        Spawns.item_type.Add(type);
                        PickupControl.there_is_pickable = true;
                    }
                }
                packet.data.ReadBYTE(); //Item Blued
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnITEM", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }
        public static void ParseOther(Packet packet, int index)
        {
            try
            {
                if (Mobs_Info.mobstypelist[index] == "INS_QUEST_TELEPORT")
                {
                    packet.data.ReadDWORD(); // MOB ID
                    packet.data.ReadBYTE();
                    packet.data.ReadBYTE();
                    packet.data.ReadSINGLE();
                    packet.data.ReadSINGLE();
                    packet.data.ReadSINGLE();
                    packet.data.ReadWORD(); // Position
                    packet.data.ReadBYTE(); // Unknwon
                    packet.data.ReadBYTE(); // Unknwon
                    packet.data.ReadWORD(); // Unknwon
                    packet.data.ReadSTRING(enumStringType.ASCII);
                    packet.data.ReadDWORD();
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("SpawnOTHERIDK", ex.Message + " Count: " + BotData.groupespawncount, packet);
            }
        }
    }
}