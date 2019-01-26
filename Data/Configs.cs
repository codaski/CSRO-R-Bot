using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Silkroad
{
    class Configs
    {
        public static void WriteConfigs()
        {
            if (Globals.MainWindow.lb_charname1.Text != "N/A")
            {
                if (!Directory.Exists(Application.StartupPath + @"\configs\" + BotData.sro_server + @"\"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\configs\" + BotData.sro_server + @"\");
                }
                if (Directory.Exists(Application.StartupPath + @"\configs\" + BotData.sro_server + @"\"))
                {
                    TextWriter config_writer = new StreamWriter(Application.StartupPath + @"\configs\" + BotData.sro_server + @"\" + Character.PlayerName + ".ini");
                    //TrainingSettings
                    config_writer.WriteLine("[Hunt]");
                    config_writer.WriteLine("huntx=" + Globals.MainWindow.trainx.Text);
                    config_writer.WriteLine("hunty=" + Globals.MainWindow.trainy.Text);
                    config_writer.WriteLine("huntr=" + Globals.MainWindow.trainr.Text);
                    config_writer.WriteLine("script=" + Globals.MainWindow.walkscript_path.Text); ;
                    config_writer.WriteLine("dont_att_gc=" + Globals.MainWindow.not_attack_general_champ.Checked);
                    config_writer.WriteLine("dont_att_g_p=" + Globals.MainWindow.not_attack_giant_pt.Checked);
                    config_writer.WriteLine("dont_att_gp=" + Globals.MainWindow.not_attack_giantpt.Checked);
                    config_writer.WriteLine("use_zerk_full=" + Globals.MainWindow.zerk_full.Checked);
                    config_writer.WriteLine("use_zerk_g_p=" + Globals.MainWindow.zerk_g_pt.Checked);
                    config_writer.WriteLine("use_zerk_gp=" + Globals.MainWindow.zerk_giantpt.Checked);
                    //TrainingSettings
                    //ReturnSettings
                    config_writer.WriteLine("[Return]");
                    config_writer.WriteLine("return_hp=" + Globals.MainWindow.low_hp.Checked);
                    config_writer.WriteLine("return_hp_count=" + Globals.MainWindow.low_hp_set.Text);
                    config_writer.WriteLine("return_mp=" + Globals.MainWindow.low_mp.Checked);
                    config_writer.WriteLine("return_mp_count=" + Globals.MainWindow.low_mp_set.Text);
                    config_writer.WriteLine("return_arrows=" + Globals.MainWindow.low_arrows.Checked);
                    config_writer.WriteLine("return_arrows_count=" + Globals.MainWindow.low_arrows_set.Text);
                    config_writer.WriteLine("return_bolts=" + Globals.MainWindow.low_bolts.Checked);
                    config_writer.WriteLine("return_bolts_count=" + Globals.MainWindow.low_bolts_set.Text);
                    config_writer.WriteLine("return_universal=" + Globals.MainWindow.low_uni.Checked);
                    config_writer.WriteLine("return_universal_count=" + Globals.MainWindow.low_uni_set.Text);
                    config_writer.WriteLine("return_inventory_full=" + Globals.MainWindow.inv_full.Checked);
                    config_writer.WriteLine("return_dead=" + Globals.MainWindow.dead.Checked);
                    config_writer.WriteLine("return_durability=" + Globals.MainWindow.low_wep.Checked);
                    //ReturnSettings
                    //BuySettings
                    config_writer.WriteLine("[Buy]");
                    config_writer.WriteLine("buy_hp=" + Globals.MainWindow.hp_buy.Text);
                    config_writer.WriteLine("buy_hp_count=" + Globals.MainWindow.hp_count.Text);
                    config_writer.WriteLine("buy_mp=" + Globals.MainWindow.mp_buy.Text);
                    config_writer.WriteLine("buy_mp_count=" + Globals.MainWindow.mp_count.Text);
                    config_writer.WriteLine("buy_uni=" + Globals.MainWindow.uni_buy.Text);
                    config_writer.WriteLine("buy_uni_count=" + Globals.MainWindow.uni_count.Text);
                    config_writer.WriteLine("buy_pt_hp=" + Globals.MainWindow.php_buy.Text);
                    config_writer.WriteLine("buy_pt_hp_count=" + Globals.MainWindow.php_count.Text);
                    config_writer.WriteLine("buy_speed=" + Globals.MainWindow.speed_buy.Text);
                    config_writer.WriteLine("buy_speed_count=" + Globals.MainWindow.speed_count.Text);
                    config_writer.WriteLine("buy_hgp_count=" + Globals.MainWindow.hgp_count.Text);
                    config_writer.WriteLine("buy_arrows_count=" + Globals.MainWindow.arrows_count.Text);
                    config_writer.WriteLine("buy_bolts_count=" + Globals.MainWindow.bolts_count.Text);
                    config_writer.WriteLine("buy_horse=" + Globals.MainWindow.horse_buy.Text);
                    config_writer.WriteLine("buy_return=" + Globals.MainWindow.scroll_buy.Text);
                    //BuySettings
                    //PickupSettings
                    config_writer.WriteLine("[Pick]");
                    config_writer.WriteLine("gold=" + Globals.MainWindow.gold_drop.Text);
                    config_writer.WriteLine("weapon=" + Globals.MainWindow.wep_drop.Text);
                    config_writer.WriteLine("armor=" + Globals.MainWindow.armor_drop.Text);
                    config_writer.WriteLine("accessory=" + Globals.MainWindow.acc_drop.Text);
                    config_writer.WriteLine("weapon_e=" + Globals.MainWindow.wepe_drop.Text);
                    config_writer.WriteLine("shield_e=" + Globals.MainWindow.shielde_drop.Text);
                    config_writer.WriteLine("protector_e=" + Globals.MainWindow.prote_drop.Text);
                    config_writer.WriteLine("accessory_e=" + Globals.MainWindow.acce_drop.Text);
                    config_writer.WriteLine("hp_pot=" + Globals.MainWindow.hp_drop.Text);
                    config_writer.WriteLine("mp_pot=" + Globals.MainWindow.mp_drop.Text);
                    config_writer.WriteLine("vigor_pot=" + Globals.MainWindow.vigorp_drop.Text);
                    config_writer.WriteLine("vigor_grain=" + Globals.MainWindow.vigorg_drop.Text);
                    config_writer.WriteLine("uni_pill=" + Globals.MainWindow.uni_drop.Text);
                    config_writer.WriteLine("return=" + Globals.MainWindow.return_drop.Text);
                    config_writer.WriteLine("arrow=" + Globals.MainWindow.arrow_drop.Text);
                    config_writer.WriteLine("bolt=" + Globals.MainWindow.bolt_drop.Text);
                    config_writer.WriteLine("material=" + Globals.MainWindow.materials_drop.Text);
                    config_writer.WriteLine("tablets=" + Globals.MainWindow.tablets_drop.Text);
                    config_writer.WriteLine("Token=" + Globals.MainWindow.comboBox2.Text);
                    config_writer.WriteLine("[Tablets]");
                    config_writer.WriteLine("tablet_count=" + Globals.MainWindow.tablet_pick.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.tablet_pick.Items.Count; i++)
                    {
                        config_writer.WriteLine("tablet=" + Globals.MainWindow.tablet_pick.Items[i]);
                    }
                    //PickupSettings
                    //AutoPotion
                    config_writer.WriteLine("[AutoPotion]");
                    config_writer.WriteLine("autopotion_use=" + Globals.MainWindow.autopot_use.Checked);
                    config_writer.WriteLine("autopotion_hp=" + Globals.MainWindow.autopot_hp.Text);
                    config_writer.WriteLine("autopotion_mp=" + Globals.MainWindow.autopot_mp.Text);
                    config_writer.WriteLine("autovigor_use=" + Globals.MainWindow.vigor_use.Checked);
                    config_writer.WriteLine("autovigor_hp=" + Globals.MainWindow.vigor_hp.Text);
                    config_writer.WriteLine("autovigor_mp=" + Globals.MainWindow.vigor_mp.Text);
                    config_writer.WriteLine("universal_use=" + Globals.MainWindow.universal_use.Checked);
                    //AutoPotion
                    //Weapon
                    config_writer.WriteLine("[Weapon]");
                    config_writer.WriteLine("weapon_first=" + Char_Data.f_wep_name);
                    config_writer.WriteLine("weapon_second=" + Char_Data.s_wep_name);
                    //Weapon
                    //Skills
                    config_writer.WriteLine("[Imbue]");
                    config_writer.WriteLine("imbue_name=" + Globals.MainWindow.imbue_name.Text);
                    config_writer.WriteLine("[SkillsGeneral]");
                    config_writer.WriteLine("skills_general_count=" + Globals.MainWindow.skills_general_list.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.skills_general_list.Items.Count; i++)
                    {
                        config_writer.WriteLine("skills_general=" + Globals.MainWindow.skills_general_list.Items[i]);
                    }
                    config_writer.WriteLine("[SkillsParty]");
                   // config_writer.WriteLine("skills_party_count=" + Globals.MainWindow.skills_party_list.Items.Count);
                    //for (int i = 0; i < Globals.MainWindow.skills_party_list.Items.Count; i++)
                    //{
                      //  config_writer.WriteLine("skills_party=" + Globals.MainWindow.skills_party_list.Items[i]);
                    //}
                    //Skills
                    //Buffs
                    config_writer.WriteLine("[FirstBuff]");
                    config_writer.WriteLine("first_buff_count=" + Globals.MainWindow.buffs_list1.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.buffs_list1.Items.Count; i++)
                    {
                        config_writer.WriteLine("first_buff=" + Globals.MainWindow.buffs_list1.Items[i]);
                    }
                    config_writer.WriteLine("[SecondBuff]");
                    config_writer.WriteLine("second_buff_count=" + Globals.MainWindow.buffs_list2.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.buffs_list2.Items.Count; i++)
                    {
                        config_writer.WriteLine("second_buff=" + Globals.MainWindow.buffs_list2.Items[i]);
                    }
                   /* config_writer.WriteLine("[FirstGIANTPTBuff]");
                    config_writer.WriteLine("first_GPT_buff_count=" + Globals.MainWindow.buffs_list3.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.buffs_list3.Items.Count; i++)
                    {
                        config_writer.WriteLine("first_GPT_buff=" + Globals.MainWindow.buffs_list3.Items[i]);
                    }
                    config_writer.WriteLine("[SecondGIANTPTBuff]");
                    config_writer.WriteLine("second_GPT_buff_count=" + Globals.MainWindow.buffs_list4.Items.Count);
                    for (int i = 0; i < Globals.MainWindow.buffs_list4.Items.Count; i++)
                    {
                        config_writer.WriteLine("second_GPT_buff=" + Globals.MainWindow.buffs_list4.Items[i]);
                    }*/
                    //Buffs
                    //Alerts
                    config_writer.WriteLine("[Alerts]");
                    config_writer.WriteLine("alert_die=" + Globals.MainWindow.alert_char_die.Checked);
                    config_writer.WriteLine("alert_dc=" + Globals.MainWindow.alert_dc.Checked);
                    config_writer.WriteLine("alert_unique=" + Globals.MainWindow.alert_unique.Checked);
                    config_writer.WriteLine("alert_pm=" + Globals.MainWindow.alert_pm.Checked);
                    config_writer.WriteLine("alert_pm=" + Globals.MainWindow.walk_random.Checked);
                    //Alerts

                    config_writer.Close();
                }
                else
                {
                    Directory.CreateDirectory(Application.StartupPath + @"\configs");
                    WriteConfigs();
                }
            }
        }


        public static void ReadConfigs()
        {
            try
            {
                if (File.Exists(Application.StartupPath + @"\configs\" + BotData.sro_server + @"\" + Character.PlayerName + ".ini"))
                {
                    TextReader config_reader = new StreamReader(Application.StartupPath + @"\configs\" + BotData.sro_server + @"\" + Character.PlayerName + ".ini");
                    string input;
                    while ((input = config_reader.ReadLine()) != null)
                    {
                        switch (input)
                        {
                            case "[Hunt]":
                                Globals.MainWindow.trainx.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.trainy.Text = config_reader.ReadLine().Split('=')[1];
                                int range = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                BotData.walkscriptpath = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.walkscript_path.Text = BotData.walkscriptpath;
                                Globals.MainWindow.trainr.Text = range.ToString();
                                Globals.MainWindow.trainr_control.Value = range;
                                Globals.MainWindow.not_attack_general_champ.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.not_attack_giant_pt.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.not_attack_giantpt.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.zerk_full.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.zerk_g_pt.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.zerk_giantpt.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[Pick]":
                                Globals.MainWindow.gold_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.wep_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.armor_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.acc_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.wepe_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.shielde_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.prote_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.acce_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.hp_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.mp_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.vigorp_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.vigorg_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.uni_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.return_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.arrow_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.bolt_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.materials_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.tablets_drop.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.comboBox2.Text = config_reader.ReadLine().Split('=')[1];
                                break;
                            case "[Return]":
                                Globals.MainWindow.low_hp.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.low_hp_set.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.low_mp.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.low_mp_set.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.low_arrows.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.low_arrows_set.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.low_bolts.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.low_bolts_set.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.low_uni.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.low_uni_set.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.inv_full.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.dead.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.low_wep.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[Buy]":
                                Globals.MainWindow.hp_buy.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.hp_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.mp_buy.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.mp_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.uni_buy.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.uni_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.php_buy.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.php_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.speed_buy.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.speed_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.hgp_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.arrows_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.bolts_count.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.horse_buy.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.scroll_buy.Text = config_reader.ReadLine().Split('=')[1];
                                break;
                            case "[Tablets]":
                                int count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < count; i++)
                                {
                                    int index = Globals.MainWindow.tablet_dont.Items.IndexOf(config_reader.ReadLine().Split('=')[1]);
                                    Globals.MainWindow.tablet_pick.Items.Add(Globals.MainWindow.tablet_dont.Items[index]);
                                    Globals.MainWindow.tablet_dont.Items.RemoveAt(index);
                                }
                                break;
                            case "[AutoPotion]":
                                Globals.MainWindow.autopot_use.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.autopot_hp.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.autopot_mp.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.vigor_use.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.vigor_hp.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.vigor_mp.Text = config_reader.ReadLine().Split('=')[1];
                                Globals.MainWindow.universal_use.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                break;
                            case "[Weapon]":
                                string f_wep = config_reader.ReadLine().Split('=')[1];
                                string s_wep = config_reader.ReadLine().Split('=')[1];
                                Char_Data.f_wep_name = f_wep;
                                if (s_wep != f_wep)
                                {
                                    Char_Data.s_wep_name = s_wep;
                                }
                                else
                                {
                                    Char_Data.s_wep_name = "";
                                }
                                break;
                            case "[Imbue]":
                                Globals.MainWindow.imbue_name.Text = config_reader.ReadLine().Split('=')[1];
                                break;
                            case "[SkillsGeneral]":
                                int general_list_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < general_list_count; i++)
                                {
                                    Globals.MainWindow.skills_general_list.Items.Add(config_reader.ReadLine().Split('=')[1]);
                                }
                                break;
                          /*  case "[SkillsParty]":
                                int partyg_list_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < partyg_list_count; i++)
                                {
                                    Globals.MainWindow.skills_party_list.Items.Add(config_reader.ReadLine().Split('=')[1]);
                                }
                                break;*/
                            case "[FirstBuff]":
                                int first_buff_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < first_buff_count; i++)
                                {
                                    Globals.MainWindow.buffs_list1.Items.Add(config_reader.ReadLine().Split('=')[1]);
                                }
                                break;
                            case "[SecondBuff]":
                                int second_buff_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < second_buff_count; i++)
                                {
                                    Globals.MainWindow.buffs_list2.Items.Add(config_reader.ReadLine().Split('=')[1]);
                                }
                                break;
                           /* case "[FirstGIANTPTBuff]":
                                int first_gpt_buff_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < first_gpt_buff_count; i++)
                                {
                                    Globals.MainWindow.buffs_list3.Items.Add(config_reader.ReadLine().Split('=')[1]);
                                }
                                break;
                            case "[SecondGIANTPTBuff]":
                                int second_gpt_buff_count = Convert.ToInt32(config_reader.ReadLine().Split('=')[1]);
                                for (int i = 0; i < second_gpt_buff_count; i++)
                                {
                                    Globals.MainWindow.buffs_list4.Items.Add(config_reader.ReadLine().Split('=')[1]);
                                }
                                break;*/
                            case "[Alerts]":
                                Globals.MainWindow.alert_char_die.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.alert_dc.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.alert_unique.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.alert_pm.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                Globals.MainWindow.walk_random.Checked = Convert.ToBoolean(config_reader.ReadLine().Split('=')[1]);
                                break;
                        }
                    }
                    config_reader.Close();
                }
            }
            catch (Exception a)
            {
                Globals.UpdateLogs("Cannot Load Character configs ! " + a.Message);
            }
        }
    }
}
