using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class LogicControl
    {
        public static void Manager()
        {
            try
            {
                if (BotData.bot && Movement.enablelogic && !BotData.dead)
                {
                    if (Globals.MainWindow.pick_with_pet.Checked && Char_Data.char_grabpetid != 0)
                    {
                        PickupControl.PickupManager();
                    }
                    if (!Globals.MainWindow.pick_with_pet.Checked && PickupControl.there_is_pickable)
                    {
                        PickupControl.PickupManager();
                    }
                    else
                    {
                        if (Buffas.buff_waiting)
                        {
                            Buffas.BuffChecker();
                        }
                        else
                        {
                            if (!MonsterControl.monster_selected)
                            {
                                MonsterControl.SelectMonster();
                            }
                            else
                            {
                                #region Cast
                               if (MonsterControl.monster_type == (byte)Globals.enumMobType.Normal || MonsterControl.monster_type == (byte)Globals.enumMobType.Champion)
                                {
                                    if (Globals.MainWindow.zerk_full.Checked && Character.Zerk == 5)
                                    {
                                        Action.UseZerk();
                                    }
                                    Skills.CheckSkills();
                                }
                                else
                                {
                                    if (MonsterControl.monster_type == (byte)Globals.enumMobType.PartyGiant)
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
                                 /*   if ((Globals.MainWindow.buffs_list3.Items.Count != 0 || Globals.MainWindow.buffs_list4.Items.Count != 0) && MonsterControl.monster_type > 1)
                                    {
                                        Buffas.buff_waiting = true;
                                    }
                                    Skills.CheckSkillsParty();*/
                                    Skills.CheckSkills();
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Globals.UpdateLogs("Error In Logic: " + ex.Message);
            }
        }
    }
}