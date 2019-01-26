using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class MonsterControl
    {
        public static uint general_id = 0;

        public static bool monster_selected = false;
        public static uint monster_id = 0;
        public static byte monster_type = 0;
        public static void SelectMonster()
        {
            try
            {
                if (monster_selected)
                {
                    //Monster Is Still Selected, Check If it's alive and do the right actions.
                    if (CheckLowerMonster())
                     {
                    if (monster_type == (byte)Globals.enumMobType.Normal || monster_type == (byte)Globals.enumMobType.Champion)
                    {
                        if (Globals.MainWindow.zerk_full.Checked && Character.Zerk == 5)
                        {
                            Action.UseZerk();
                        }
                        Skills.CheckSkills();
                    }
                    else
                    {
                        if (monster_type == (byte)Globals.enumMobType.PartyGiant)
                        {
                            if ((Globals.MainWindow.zerk_giantpt.Checked || Globals.MainWindow.zerk_full.Checked) && Character.Zerk == 5)
                            {
                                Action.UseZerk();
                            }
                        }
                        else
                        {
                            if ((Globals.MainWindow.zerk_g_pt.Checked || Globals.MainWindow.zerk_full.Checked) && Character.Zerk == 5)
                            {
                                Action.UseZerk();
                            }
                        }
                     /*   if ((Globals.MainWindow.buffs_list3.Items.Count != 0 || Globals.MainWindow.buffs_list4.Items.Count != 0) && monster_type > 1)
                        {
                            Buffas.buff_waiting = true;
                        }
                        Skills.CheckSkillsParty();*/
                    }
                     }
                }
                else
                {
                    //Monster isn't selected ! Select New Monster !
                    int distance = 1000; // Set the huge distance.
                    uint id = 0;
                    int index = -1;

                    for (int i = 0; i < Spawns.mob_id.Count; i++)
                    {
                        //Looping Through Monsters List.
                        System.Threading.Thread.Sleep(2); // MicroSleep To Reduce CPU usage !
                        if (Spawns.mob_priority[i] == 1)
                        {
                            distance = Spawns.mob_dist[i];
                            id = Spawns.mob_id[i];
                            index = i;
                            break;
                        }
                        else
                        {
                            if (CheckStatus(i) && CheckDistance(i, distance) && CheckAvoid(i) && CheckRange(i))
                            {
                                distance = Spawns.mob_dist[i];
                                id = Spawns.mob_id[i];
                                index = i;
                            }
                        }
                    }

                    if (id != 0)
                    {
                        monster_selected = true;
                        monster_id = id;
                        monster_type = Spawns.mob_type[index];
                        Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT, false, enumDestination.Server);
                        packetas.data.AddDWORD(monster_id);
                        Globals.ServerPC.SendPacket(packetas);


                        if (RandomWalk.walking_center)
                        {
                            RandomWalk.walking_center = false;
                        }
                        if (RandomWalk.walking_randomly)
                        {
                            RandomWalk.walking_randomly = false;
                        }
                    }
                    else
                    {
                        //Globals.UpdateLogs("No Available Monsters Found ! Add random walk or smth.");
                        RandomWalk.WalkManager();
                    }
                }
            }
            catch
            {
                LogicControl.Manager();
            }
        }

        public static bool CheckLowerMonster()
        {
            byte type = MonsterControl.monster_type;
            uint attack_id = 0;

            for (int i = 0; i < Spawns.mob_id.Count; i++)
            {
                if (Spawns.mob_priority[i] == 1)
                {
                    if (Spawns.mob_type[i] < type)
                    {
                        attack_id = Spawns.mob_id[i];
                        type = Spawns.mob_type[i];
                        Globals.UpdateLogs("Found Lower Type: " + type);
                    }
                }
            }
            if (attack_id != 0)
            {
                Globals.UpdateLogs("Bot attacks type: " + MonsterControl.monster_type + " Deselecting, and select new type: " + Spawns.mob_type[Spawns.mob_id.IndexOf(attack_id)]);
                MonsterControl.monster_id = 0;
                MonsterControl.monster_selected = false;
                MonsterControl.monster_type = 0;
                Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION, false, enumDestination.Server);
                packetas.data.AddBYTE(2);
                Globals.ServerPC.SendPacket(packetas);
                packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTSELECT, false, enumDestination.Server);
                packetas.data.AddDWORD(attack_id);
                Globals.ServerPC.SendPacket(packetas);
                return false;
            }
            else
            {
                return true;
            }
        }




        public static bool CheckStatus(int index)
        {
            try
            {
                if (Spawns.mob_status[index] == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch { return false; }
        }
        public static bool CheckDistance(int index, int distance)
        {
            try
            {
            if ((Spawns.mob_dist[index] <= distance))
            {
                return true;
            }
            else
            {
                return false;
            }
            }
            catch { return false; }
        }
        public static bool CheckAvoid(int index)
        {
            try
            {
                switch (Spawns.mob_type[index])
                {
                    case (byte)Globals.enumMobType.Normal:
                        if (!Globals.MainWindow.not_attack_general_champ.Checked)
                        {
                            return true;
                        }
                        break;
                    case (byte)Globals.enumMobType.Champion:
                        if (!Globals.MainWindow.not_attack_general_champ.Checked)
                        {
                            return true;
                        }
                        break;
                    case (byte)Globals.enumMobType.PartyGiant:
                        if (!Globals.MainWindow.not_attack_giantpt.Checked)
                        {
                            return true;
                        }
                        break;
                    default:
                        if (!Globals.MainWindow.not_attack_giant_pt.Checked)
                        {
                            return true;
                        }
                        break;
                }
                return false;
            }
            catch { return false; }
        }
        public static bool CheckRange(int index)
        {
            try
            {
                int dist_train = Math.Abs((Spawns.mob_x[index] - Convert.ToInt32(Globals.MainWindow.trainx.Text))) + Math.Abs((Spawns.mob_y[index] - Convert.ToInt32(Globals.MainWindow.trainy.Text)));
                if (dist_train <= Globals.MainWindow.trainr_control.Value)
                {
                    return true;
                }
                return false;
            }
            catch { return false; }
        }








        public static void MonsterAction(Packet packet)
        {
            uint id = packet.data.ReadDWORD();
            string type = packet.data.ReadBYTE().ToString("X2") + packet.data.ReadBYTE().ToString("X2");
            if (type == "0002")
            {
                if (id != Character.ID)
                {
                    for (int i = 0; i < Spawns.mob_id.Count; i++)
                    {
                        if (id == Spawns.mob_id[i])
                        {
                            if (id != monster_id)
                            {
                                Spawns.mob_dist.RemoveAt(i);
                                Spawns.mob_id.RemoveAt(i);
                                Spawns.mob_name.RemoveAt(i);
                                Spawns.mob_priority.RemoveAt(i);
                                Spawns.mob_status.RemoveAt(i);
                                Spawns.mob_type.RemoveAt(i);
                                Spawns.mob_x.RemoveAt(i);
                                Spawns.mob_y.RemoveAt(i);
                                Stuck.DeleteMob(id);
                            }
                            break;
                        }
                    }
                }
            }
        }

        public static void Selected(Packet packet)
        {
            if (packet.data.ReadBYTE() == 1)
            {
                BotData.selectedid = packet.data.ReadDWORD();
                try
                {
                    BotData.selectednpctype = Spawns.npctype[Spawns.npcid.IndexOf(BotData.selectedid)];
                }
                catch{ }
                #region Loop
                if (BotData.loop && BotData.bot)
                {
                    if (BotData.loopaction == "storage")
                    {
                        if (Char_Data.storageopened == 0)
                        {
                            StorageControl.GetStorageItems(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                        }
                        else
                        {
                            StorageControl.OpenStorage1();
                        }
                    }
                    if (BotData.loopaction == "blacksmith")
                    {
                        SellControl.SellManager(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                    }
                    if (BotData.loopaction == "stable" || BotData.loopaction == "accessory" || BotData.loopaction == "potion")
                    {
                        BuyControl.BuyManager(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                    }
                }
                #endregion
                else
                {
                    general_id = BotData.selectedid;
                    if (BotData.bot && monster_selected)
                    {
                        if (packet.data.ReadBYTE() == 0x01)
                        {
                            if (BotData.bot)
                            {
                                if (MonsterControl.monster_selected)
                                {
                                    Movement.stuck_count = 0;
                                }
                            }
                            uint hp = packet.data.ReadDWORD();
                            Globals.MainWindow.lb_mob_hp.Text = hp.ToString();
                            if (hp > 0)
                            {
                                if (BotData.selectedid == monster_id)
                                {
                                    if (Globals.MainWindow.attackpet_attack.Checked == true)
                                    {
                                        Action.AttackWithPet();
                                    }
                                    if (Globals.MainWindow.fast_train.Checked)
                                    {
                                        Action.WalkTo(Spawns.mob_x[Spawns.mob_id.IndexOf(monster_id)], Spawns.mob_y[Spawns.mob_id.IndexOf(monster_id)]);
                                    }
                                   /* if ((Globals.MainWindow.buffs_list3.Items.Count != 0 || Globals.MainWindow.buffs_list4.Items.Count != 0) && monster_type > 1)
                                    {
                                        Buffas.buff_waiting = true;
                                    }*/
                                    System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                                    n_t.Start();
                                }
                            }
                            else
                            {
                                monster_selected = false;
                                monster_id = 0;
                                monster_type = 0;
                                System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                                n_t.Start();
                            }
                        }
                    }
                }
            }
            else
            {
                MonsterControl.monster_id = 0;
                MonsterControl.monster_selected = false;
                MonsterControl.monster_type = 0;
                Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION, false, enumDestination.Server);
                packetas.data.AddBYTE(2);
                Globals.ServerPC.SendPacket(packetas);
            }
        }

        public static void NPCDeselect(Packet packet)
        {
            if (packet.data.ReadBYTE() == 1)
            {
                if (BotData.loopaction == "storage" || BotData.loopaction == "blacksmith" || BotData.loopaction == "stable" || BotData.loopaction == "accessory" || BotData.loopaction == "potion")
                {
                    BotData.selectedid = 0;
                    BotData.selected = 0;
                    LoopControl.WalkScript();
                }
            }
        }

        public static void NPCSelect(Packet packet)
        {
            Packet vienas = packet;
            if (BotData.bot && BotData.loop)
            {
                string type = BotData.selectednpctype;
                uint id = Spawns.npcid[Spawns.npctype.IndexOf(type)];
                if (type.Contains("WAREHOUSE"))
                {
                    StorageControl.StorageManager(id);
                    Globals.ClientPC.SendPacket(vienas);
                }
                if (type.Contains("SMITH"))
                {
                    SellControl.SellManager(id);
                }
                if (type.Contains("POTION") || type.Contains("ACCESSORY") || type.Contains("HORSE"))
                {
                    BuyControl.BuyManager(id);
                }
            }
            else
            {
                Globals.ClientPC.SendPacket(vienas);
            }
        }

        public static void Refresh(Packet packet)
        {
            byte flag1 = packet.data.ReadBYTE();
            byte flag2 = packet.data.ReadBYTE();
            if (flag1 == 2 && (flag2 == 0 || flag2 == 1))
            {
                System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                n_t.Start();
            }
        }

    }
}