using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Silkroad
{
    class CCharacter
    {
        #region Characters List Request
        public static void RequestCharacterlist()
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_CHARACTERLISTING, false, enumDestination.Server);
            packet.data.AddBYTE(0x02);
            Globals.ServerPC.SendPacket(packet);
        }
        #endregion
        #region Characters List
        public static void CharacterList(Packet packet)
        {
            try
            {
                Globals.MainWindow.char_name.Items.Clear();
                List<string> characters = new List<string>();
                if (packet.data.ReadBYTE() == 0x02)
                {
                    if (packet.data.ReadBYTE() == 0x01)
                    {
                        byte char_count = packet.data.ReadBYTE();
                        for (int i = 0; i < char_count; i++)
                        {
                            #region Main
                            packet.data.ReadDWORD(); //Model
                            characters.Add(packet.data.ReadSTRING(enumStringType.ASCII)); // Name
                            Globals.MainWindow.char_name.Items.Add(characters[i]);
                            packet.data.ReadBYTE(); //Volume/Height
                            packet.data.ReadBYTE(); //Level
                            packet.data.ReadQWORD(); //Exp
                            packet.data.ReadWORD(); //STR
                            packet.data.ReadWORD(); //INT
                            packet.data.ReadWORD(); //Stats points
                            packet.data.ReadDWORD(); //Hp
                            packet.data.ReadDWORD(); //Mp
                            #endregion
                            #region Deletion
                            byte char_delete = packet.data.ReadBYTE();
                            if (char_delete == 1)
                            {
                                packet.data.ReadDWORD();
                            }
                            packet.data.ReadBYTE(3); //Unknown
                            #endregion
                            #region Items
                            int itemscount = packet.data.ReadBYTE();
                            for (int a = 0; a < itemscount; a++)
                            {
                                packet.data.ReadDWORD(); //Item ID
                                packet.data.ReadBYTE(); //Plus Value
                            }
                            #endregion
                            #region Avatars
                            int avatarcount = packet.data.ReadBYTE(); //Avatar count
                            for (int a = 0; a < avatarcount; a++)
                            {
                                packet.data.ReadDWORD(); // Avatar ID
                                packet.data.ReadBYTE();
                            }
                            #endregion
                        }
                        if (char_count == 1)
                        {
                            Globals.MainWindow.char_name.SelectedIndex = 0;
                        }
                        if (!BotData.use_client)
                        {
                            Globals.MainWindow.char_name.Enabled = true;
                            Globals.MainWindow.select.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("CharList", ex.Message, packet);
                Globals.UpdateLogs("Error On CharList, Please upload files from debug folder !");
            }
        }
        #endregion
        #region Data
        public static Packet CharPacket;
        public static byte[] skip_charid;
        public struct CAVE_
        {
            public bool char_incave;
            public byte xsector;
            public float zcoord;
            public float xcoord;
        }
        public static CAVE_ cave = new CAVE_();
        public static List<string> explist = new List<string>();
        public static int Level;
        public static int maxLevel;
        public static ulong exp;
        public static ulong expmax;
        public static ulong Gold;
        public static uint SkillPoints;
        public static uint AvailableStatPoints;
        public static byte Zerk;
        public static uint CurrentHP;
        public static uint CurrentMP;
        public static byte itemscount;
        public static byte inventoryslot;
        public static byte questspending;
        public static ushort questscompleted;
        public static uint ID;
        public static uint AccountID;
        public static int X;
        public static int Y;
        public static string PlayerName;
        public static string guildName;
        public static ushort STR;
        public static ushort INT;
        public static uint MaxHP;
        public static uint MaxMP;
        public static uint some;
        public static uint model;
        public static byte volh;
        public static float speed;
        public static byte data_loaded = 0;
        public static System.Timers.Timer time = new System.Timers.Timer();
        #endregion
        #region Character Data
        public static void CharData(Packet packet)
        {
            try
            {
                #region Reset Data
                Char_Data.inventorycount.Clear();
                Char_Data.inventoryid.Clear();
                Char_Data.inventoryslot.Clear();
                Char_Data.inventorytype.Clear();
                Char_Data.inventorydurability.Clear();

                Char_Data.storageid.Clear();
                Char_Data.storagetype.Clear();
                Char_Data.storagecount.Clear();
                Char_Data.storageslot.Clear();
                Char_Data.storagedurability.Clear();

                Globals.MainWindow.inventory_list.Items.Clear();
                Globals.MainWindow.storage_list.Items.Clear();
                Char_Data.skillid.Clear();
                Char_Data.skillname.Clear();
                Char_Data.skilltype.Clear();
                Char_Data.skillnamewaiting.Clear();
                Char_Data.skillonid.Clear();
                Char_Data.skillontemp.Clear();
                Char_Data.skillontype.Clear();

                Globals.MainWindow.skills_list.Items.Clear();
                Char_Data.char_horseid = 0;
                Char_Data.storageopened = 0;

                for (int i = 0; i < Skills_Info.skillsstatuslist.Count; i++)
                {
                    Skills_Info.skillsstatuslist[i] = 0;
                }

                #endregion
                #region Main
                Character.some = packet.data.ReadDWORD();
                Character.model = packet.data.ReadDWORD(); //Model
                Character.volh = packet.data.ReadBYTE(); //Volume and Height
                Character.Level = packet.data.ReadBYTE();
                Character.maxLevel = packet.data.ReadBYTE();
                int maxlvl = (int)Character.Level - 1;
                Character.expmax = Convert.ToUInt64(Character.explist[maxlvl]);
                Character.exp = packet.data.ReadQWORD();
                packet.data.ReadWORD(); //SP bar
                packet.data.ReadWORD(); //Unknown
                Character.Gold = packet.data.ReadQWORD();
                Character.SkillPoints = packet.data.ReadDWORD();
                Character.AvailableStatPoints = packet.data.ReadWORD();
                Character.Zerk = packet.data.ReadBYTE();
                packet.data.ReadDWORD();
                Character.CurrentHP = packet.data.ReadDWORD();
                Character.CurrentMP = packet.data.ReadDWORD();
                packet.data.ReadBYTE(); //Unknown
                packet.data.ReadBYTE(); //Unknown
                packet.data.ReadQWORD(); //Unknown
                #endregion
                #region Items
                Character.inventoryslot = packet.data.ReadBYTE();
                Character.itemscount = packet.data.ReadBYTE();
                for (int y = 0; y < Character.itemscount; y++)
                {
                    byte slot = packet.data.ReadBYTE();
                    packet.data.ReadDWORD(); // 0 - Unknown
                    uint item_id = packet.data.ReadDWORD();
                    int index = Items_Info.itemsidlist.IndexOf(item_id);
                    string type = Items_Info.itemstypelist[index];
                    string name = Items_Info.itemsnamelist[index];
                    Char_Data.inventoryslot.Add(slot);
                    Char_Data.inventorytype.Add(type);
                    Char_Data.inventoryid.Add(item_id);
                    Globals.MainWindow.inventory_list.Items.Add(name);
                    if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU") || type.StartsWith("ITEM_MALL_AVATAR") || type.StartsWith("ITEM_ETC_E060529_GOLDDRAGONFLAG") || type.StartsWith("ITEM_EVENT_CH") || type.StartsWith("ITEM_EVENT_EU") || type.StartsWith("ITEM_EVENT_AVATAR_W_NASRUN") || type.StartsWith("ITEM_EVENT_AVATAR_M_NASRUN"))
                    {
                        byte item_plus = packet.data.ReadBYTE();
                        packet.data.ReadQWORD();
                        Char_Data.inventorydurability.Add(packet.data.ReadDWORD());
                        byte blueamm = packet.data.ReadBYTE();
                        for (int i = 0; i < blueamm; i++)
                        {
                            packet.data.ReadDWORD();
                            packet.data.ReadDWORD();
                        }
                        packet.data.ReadBYTE(); //Unknwon
                        packet.data.ReadBYTE(); //Unknwon
                        packet.data.ReadBYTE(); //Unknwon
                        byte flag1 = packet.data.ReadBYTE(); // Flag ?
                        if (flag1 == 1)
                        {
                            packet.data.ReadBYTE(); //Unknown
                            packet.data.ReadDWORD(); // Unknown ID ? ADV Elexir ID ?
                            packet.data.ReadDWORD(); // Unknwon Count
                        }
                        Char_Data.inventorycount.Add(1);
                    }
                    else 
                    {
                        if ((type.StartsWith("ITEM_COS") && type.Contains("SILK")) || (type.StartsWith("ITEM_EVENT_COS") && !type.Contains("_C_")))
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
                            Char_Data.inventorycount.Add(1);
                            Char_Data.inventorydurability.Add(0);
                        }
                        else
                        {
                            if (Data.Types.grabpet_spawn_types.IndexOf(type) != -1 || Data.Types.attack_spawn_types.IndexOf(type) != -1)
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
                                Char_Data.inventorycount.Add(1);
                                Char_Data.inventorydurability.Add(0);
                            }
                            else
                            {
                                if (type == "ITEM_ETC_TRANS_MONSTER")
                                {
                                    packet.data.ReadDWORD();
                                    Char_Data.inventorycount.Add(1);
                                    Char_Data.inventorydurability.Add(0);
                                }
                                else
                                {
                                    if (type.StartsWith("ITEM_MALL_MAGIC_CUBE"))
                                    {
                                        packet.data.ReadDWORD();
                                        Char_Data.inventorycount.Add(1);
                                        Char_Data.inventorydurability.Add(0);
                                    }
                                    else
                                    {
                                        ushort count = packet.data.ReadWORD();
                                        //if (Data.Types.stone_byte.IndexOf(type) != -1)
                                        //{
                                            packet.data.ReadBYTE();
                                        //}
                                        Char_Data.inventorycount.Add(count);
                                        Char_Data.inventorydurability.Add(0);
                                    }
                                }
                            }
                        }
                    }
                }
                ItemsCount.CountManager();
                #endregion
                #region Avatars
                packet.data.ReadBYTE(); // Avatars Max
                int avatarcount = packet.data.ReadBYTE();
                for (int i = 0; i < avatarcount; i++)
                {
                    packet.data.ReadBYTE(); //Slot
                    packet.data.ReadDWORD();
                    uint avatar_id = packet.data.ReadDWORD();
                    int index = Items_Info.itemsidlist.IndexOf(avatar_id);
                    string type = Items_Info.itemstypelist[index];
                    byte item_plus = packet.data.ReadBYTE();
                    packet.data.ReadQWORD();
                    packet.data.ReadDWORD();
                    byte blueamm = packet.data.ReadBYTE();
                    for (int a = 0; a < blueamm; a++)
                    {
                        packet.data.ReadDWORD();
                        packet.data.ReadDWORD();
                    }
                    packet.data.ReadDWORD();
                }
                #endregion
                packet.data.ReadBYTE(); //Avatars End

                int mastery = packet.data.ReadBYTE(); // Mastery Start
                while (mastery == 1)
                {
                    packet.data.ReadDWORD(); // Mastery ID
                    packet.data.ReadBYTE();  // Mastery LV
                    mastery = packet.data.ReadBYTE(); // New Mastery Start / List End
                }
                packet.data.ReadBYTE(); // Mastery END

                int skilllist = packet.data.ReadBYTE(); // Skill List Start
                while (skilllist == 1)
                {
                    uint skillid = packet.data.ReadDWORD(); // Skill ID
                    packet.data.ReadBYTE();
                    skilllist = packet.data.ReadBYTE(); // New Skill Start / List End

                    Char_Data.skillid.Add(skillid);
                    int index = Skills_Info.skillsidlist.IndexOf(skillid);
                    Char_Data.skillname.Add(Skills_Info.skillsnamelist[index]);
                    Char_Data.skilltype.Add(Skills_Info.skillstypelist[index]);
                    Globals.MainWindow.skills_list.Items.Add(Skills_Info.skillsnamelist[index]);
                }

                #region Skipping Quest Part
                byte[] tempe = new byte[4];
                while (true)
                {
                    tempe[0] = tempe[1];
                    tempe[1] = tempe[2];
                    tempe[2] = tempe[3];
                    tempe[3] = packet.data.ReadBYTE();
                    if((tempe[0] == skip_charid[0]) && (tempe[1] == skip_charid[1]) && (tempe[2] == skip_charid[2]) && (tempe[3] == skip_charid[3]))
                    {
                        Console.Beep();
                        packet.data.pointer -= 4;
                        break;
                    }
                }
                #endregion

                Character.ID = packet.data.ReadDWORD();
                byte xsec = packet.data.ReadBYTE();
                byte ysec = packet.data.ReadBYTE();
                float xcoord = packet.data.ReadSINGLE();
                float zcoord = packet.data.ReadSINGLE();
                float ycoord = packet.data.ReadSINGLE();
                if (ysec == 0x80)
                {
                    cave.char_incave = true;
                    cave.xsector = xsec;
                    cave.zcoord = zcoord;
                    cave.xcoord = xcoord;
                }
                else
                {
                    cave.char_incave = false;
                }
                Character.X = Action.CalculatePositionX(xsec, xcoord);
                Character.Y = Action.CalculatePositionY(ysec, ycoord);
                packet.data.ReadWORD(); // Position
                int move = packet.data.ReadBYTE(); // Move ?? Maybie Useless
                packet.data.ReadBYTE(); // Run
                packet.data.ReadBYTE();
                packet.data.ReadWORD();
                packet.data.ReadBYTE();
                packet.data.ReadBYTE(); //DeathFlag
                packet.data.ReadBYTE(); //Movement Flag
                packet.data.ReadBYTE(); //Berserker Flag
                packet.data.ReadDWORD(); //Walking Speed
                Character.speed = packet.data.ReadSINGLE(); //Running Speed
                packet.data.ReadDWORD(); //Berserker Speed
                packet.data.ReadBYTE();
                Character.PlayerName = packet.data.ReadSTRING(enumStringType.ASCII);
                packet.data.ReadSTRING(enumStringType.ASCII); // ALIAS

                packet.data.ReadBYTE(); // Job Level
                packet.data.ReadBYTE(); // Job Type
                packet.data.ReadDWORD(); // Trader Exp
                packet.data.ReadDWORD(); // Thief Exp
                packet.data.ReadDWORD(); // Hunter Exp
                packet.data.ReadBYTE(); // Trader LV
                packet.data.ReadBYTE(); // Thief LV
                packet.data.ReadBYTE(); // Hunter LV
                packet.data.ReadBYTE(); // PK Flag
                packet.data.ReadWORD(); // Unknown
                packet.data.ReadDWORD(); // Unknown
                packet.data.ReadWORD(); // Unknown
                AccountID = packet.data.ReadDWORD(); // Account ID
                Globals.MainWindow.Text = "zBot | " + Character.PlayerName + " | " + Globals.MainWindow.in_game_server_name.Text;
                Globals.MainWindow.tray.Text = "zBot | " + Character.PlayerName + " | " + Globals.MainWindow.in_game_server_name.Text;
                if (data_loaded == 0)
                {
                    Globals.MainWindow.config_button.Enabled = true;
                    data_loaded = 1;
                    BotData.Statistic.sp_begin = (int)Character.SkillPoints;
                    BotData.Statistic.gold_begin = (long)Character.Gold;
                    Configs.ReadConfigs();
                    Globals.MainWindow.start_button.Enabled = true;
                    if (Char_Data.f_wep_name != "" && Char_Data.f_wep_name != null)
                    {
                        int index = Char_Data.inventorytype.IndexOf(Char_Data.f_wep_name);
                        if (index == -1)
                        {
                            Char_Data.f_wep_name = "";
                        }
                    }
                    if (Char_Data.s_wep_name != "" && Char_Data.s_wep_name != null)
                    {
                        int index = Char_Data.inventorytype.IndexOf(Char_Data.s_wep_name);
                        if (index == -1)
                        {
                            Char_Data.s_wep_name = "";
                        }
                    }
                    System.Threading.Thread time_thread = new Thread(StartTimer);
                    time_thread.Start();
                }
                for (int i = 0; i < Globals.MainWindow.buffs_list2.Items.Count; i++)
                {
                    Char_Data.skillnamewaiting.Add(Globals.MainWindow.buffs_list2.Items[i].ToString());
                    Char_Data.skilltipwaiting.Add(2);
                    Buffas.buff_waiting = true;
                }
                for (int i = 0; i < Globals.MainWindow.buffs_list1.Items.Count; i++)
                {
                    Char_Data.skillnamewaiting.Add(Globals.MainWindow.buffs_list1.Items[i].ToString());
                    Char_Data.skilltipwaiting.Add(1);
                    Buffas.buff_waiting = true;
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("CharData", ex.Message + " Char ID: " + skip_charid[0].ToString("X2") + skip_charid[1].ToString("X2") + skip_charid[2].ToString("X2") + skip_charid[3].ToString("X2"), packet);
                Globals.UpdateLogs("Error On CharData, Please upload files from debug folder !");
            }
        }

        static void StartTimer()
        {
            time.Elapsed += new System.Timers.ElapsedEventHandler(time_Elapsed);
            time.Interval += 5000;
            time.Start();
            time.Enabled = true;
        }

        static void time_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BotData.Statistic.time_elapsed += 5;
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_gold_gained.Text = ((int)Character.Gold - BotData.Statistic.gold_begin).ToString();
            System.Threading.Thread.Sleep(10);
            int total_sp = (int)Character.SkillPoints - BotData.Statistic.sp_begin;
            System.Threading.Thread.Sleep(10);
            float sp_minute = (float)Math.Round((float)(total_sp / BotData.Statistic.time_elapsed * 60), 2);
            System.Threading.Thread.Sleep(10);
            float sp_hour = (float)Math.Round((float)(total_sp / BotData.Statistic.time_elapsed * 3600), 2);
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_sp_gained_total.Text = (Character.SkillPoints - BotData.Statistic.sp_begin).ToString();
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_sp_gained_hour.Text = sp_hour.ToString();
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_sp_gained_minute.Text = sp_minute.ToString();
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_mobs_killed_total.Text = BotData.Statistic.mob_killed.ToString();
            System.Threading.Thread.Sleep(10);
            float mob_killed_minute = (float)Math.Round((float)(BotData.Statistic.mob_killed / BotData.Statistic.time_elapsed * 60), 2);
            System.Threading.Thread.Sleep(10);
            float mob_killed_hour = (float)Math.Round((float)(BotData.Statistic.mob_killed / BotData.Statistic.time_elapsed * 3600), 2);
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_mobs_killed_hour.Text = mob_killed_hour.ToString();
            System.Threading.Thread.Sleep(10);
            Globals.MainWindow.lb_mobs_killed_minute.Text = mob_killed_minute.ToString();
        }
        #endregion
        #region StuffUpdate
        public static void StuffUpdate(Packet packet)
        {
            try
            {
                byte code = packet.data.ReadBYTE();
                switch (code)
                {
                    case 1:
                        #region Gold Update
                        Character.Gold = packet.data.ReadQWORD();
                        Globals.MainWindow.lb_gold.Text = Character.Gold.ToString();
                        break;
                        #endregion
                    case 2:
                        #region SP Update
                        Character.SkillPoints = packet.data.ReadDWORD();
                        break;
                        #endregion
                    case 4:
                        #region Zerk Update
                        Character.Zerk = packet.data.ReadBYTE();
                        Globals.MainWindow.zerk_bar.Value = (int)Character.Zerk;
                        if (BotData.bot && !BotData.loop && Character.Zerk == 5 && Globals.MainWindow.zerk_full.Checked == true)
                        {
                            Action.UseZerk();
                        }
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("StuffUpdate", ex.Message, packet);
            }
        }
        #endregion
        #region Character Info
        public static void CharacterInfo(Packet packet)
        {
            try
            {
                packet.data.ReadQWORD();
                packet.data.ReadQWORD();
                packet.data.ReadWORD();
                packet.data.ReadWORD();
                packet.data.ReadWORD();
                packet.data.ReadWORD();
                Character.MaxHP = packet.data.ReadDWORD();
                Character.MaxMP = packet.data.ReadDWORD();
                Character.STR = packet.data.ReadWORD();
                Character.INT = packet.data.ReadWORD();
                Globals.MainWindow.lb_gold.Text = Character.Gold.ToString();
                Globals.MainWindow.lb_int.Text = Character.INT.ToString();
                Globals.MainWindow.lb_level.Text = Character.Level.ToString();
                Globals.MainWindow.lb_charname.Text = Character.PlayerName;
                Globals.MainWindow.lb_sp.Text = Character.SkillPoints.ToString();
                Globals.MainWindow.lb_str.Text = Character.STR.ToString();
                Globals.MainWindow.lb_x.Text = Character.X.ToString();
                Globals.MainWindow.lb_y.Text = Character.Y.ToString();


                if (CurrentMP > MaxMP)
                {
                    CurrentMP = MaxMP;
                }
                if (CurrentHP > MaxHP)
                {
                    CurrentHP = MaxHP;
                }
                Globals.MainWindow.mp_bar.Maximum = (int)Character.MaxMP;
                Globals.MainWindow.mp_bar.Value = (int)Character.CurrentMP;
                Globals.MainWindow.hp_bar.Maximum = (int)Character.MaxHP;
                Globals.MainWindow.hp_bar.Value = (int)Character.CurrentHP;
                Globals.MainWindow.exp_bar.Maximum = (int)Character.expmax;
                Globals.MainWindow.exp_bar.Value = (int)Character.exp;
                Globals.MainWindow.zerk_bar.Value = (int)Character.Zerk;
                BotData.returning = 0;
                BotData.dead = false;
            }
            catch (Exception ex)
            {
                Globals.Debug("CharInfo", ex.Message, packet);
            }
        }
        #endregion
        #region SpeedUpdate
        public static void SpeedUpdate(Packet packet)
        {
            try
            {
                uint ID = packet.data.ReadDWORD(); // Char ID
                if (Character.ID == ID)
                {
                    packet.data.ReadSINGLE(); // Walk Speed
                    float speed = packet.data.ReadSINGLE(); // Run Speed
                    Character.speed = speed;
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("SpeedUpdate", ex.Message, packet);
            }
        }
        #endregion
        #region Guild Analyze
        public static void GuildAnalyze(Packet packet)
        {
            try
            {
                packet.data.ReadDWORD();
                Character.guildName = packet.data.ReadSTRING(enumStringType.ASCII);
                Globals.MainWindow.lb_guildname.Text = Character.guildName;
            }
            catch (Exception ex)
            {
                Globals.Debug("GuildAnalyze", ex.Message, packet);
            }
        }
        #endregion

        #region Level UP
        public static void LevelUp(Packet packet)
        {
            try
            {
                if (packet.data.ReadDWORD() == Character.ID)
                {
                    Character.Level += 1;
                    Globals.MainWindow.lb_level.Text = Character.Level.ToString();
                    int maxlvl = (int)Character.Level - 1;
                    Character.expmax = Convert.ToUInt64(Character.explist[maxlvl]);
                    Globals.MainWindow.exp_bar.Maximum = (int)Character.expmax;
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("LevelUp", ex.Message, packet);
            }
        }
        #endregion
        #region SP EXP Update
        public static void ExpSpUpdate(Packet packet)
        {
            try
            {
                packet.data.ReadDWORD();
                ulong exp = packet.data.ReadQWORD();
                packet.data.ReadQWORD(); //SP XP
                if (Character.exp + exp >= Character.expmax)
                {
                    int maxlvl = (int)Character.Level - 2;
                    ulong exp_max = Convert.ToUInt64(Character.explist[maxlvl]);
                    ulong new_exp = (Character.exp + exp) - exp_max;
                    Character.exp = new_exp;
                    Globals.MainWindow.exp_bar.Value = (int)Character.exp;
                }
                else
                {
                    Character.exp = Character.exp + exp;
                    Globals.MainWindow.exp_bar.Value = (int)Character.exp;
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("EXPSP Update", ex.Message, packet);
            }
            BotData.Statistic.mob_killed++;
        }
        #endregion
    }
}