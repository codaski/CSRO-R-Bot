using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Silkroad
{
    class HPMPPacket
    {
        public static byte bad_status = 0;
        public static byte pet_status = 0;
        public static byte horse_status = 0;
        public static void HPMPUpdate(Packet hp_packet)
        {
            try
            {
                uint id = hp_packet.data.ReadDWORD();

                if (id == Character.ID)
                {
                    hp_packet.data.ReadBYTE();
                    hp_packet.data.ReadBYTE(); // 0x00
                    byte type2 = hp_packet.data.ReadBYTE();
                    switch (type2)
                    {
                        case 0x01:
                            Character.CurrentHP = hp_packet.data.ReadDWORD();
                            break;
                        case 0x02:
                            Character.CurrentMP = hp_packet.data.ReadDWORD();
                            break;
                        case 0x03:
                            Character.CurrentHP = hp_packet.data.ReadDWORD();
                            Character.CurrentMP = hp_packet.data.ReadDWORD();
                            break;
                        case 0x04:
                            if (hp_packet.data.ReadDWORD() == 0)
                            {
                                bad_status = 0;
                            }
                            else
                            {
                                bad_status = 1;
                            }
                            break;
                    }
                    if (Character.CurrentMP >= Buffas.min_mp_require)
                    {
                        Buffas.min_mp_require = 200000;
                        Buffas.buff_waiting = true;
                    }
                    if (Character.CurrentHP > 0)
                    {
                        BotData.dead = false;
                    }
                    else
                    {
                        BotData.dead = true;
                        if (Globals.MainWindow.alert_char_die.Checked)
                        {
                            Alert.StartAlert();
                        }
                        if (Globals.MainWindow.dead.Checked == true)
                        {
                            //Return To Town
                            BotData.returning = 1;
                            Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_ACCEPTDEAD, false, enumDestination.Server);
                            packetas.data.AddBYTE(0x01);
                            Globals.ServerPC.SendPacket(packetas);
                        }
                    }
                    if (Globals.MainWindow.universal_use.Checked == true && bad_status == 1)
                    {
                        Autopot.UseUni();
                    }
                    Globals.MainWindow.mp_bar1.Value = (int)Character.CurrentMP;
                    Globals.MainWindow.hp_bar1.Value = (int)Character.CurrentHP;
                    uint hp = Character.CurrentHP * 100 / Character.MaxHP;
                    uint mp = Character.CurrentMP * 100 / Character.MaxMP;

                    if (Globals.MainWindow.autopot_use.Checked == true)
                    {
                        if (hp < Convert.ToInt32(Globals.MainWindow.autopot_hp.Text))
                        {
                            Autopot.UseHP();
                        }
                        if (mp < Convert.ToInt32(Globals.MainWindow.autopot_mp.Text))
                        {
                            Autopot.UseMP();
                        }
                    }
                    if (Globals.MainWindow.vigor_use.Checked == true)
                    {
                        if (hp < Convert.ToInt32(Globals.MainWindow.vigor_hp.Text) || mp < Convert.ToInt32(Globals.MainWindow.vigor_mp.Text))
                        {
                            Autopot.UseVigor();
                        }
                    }
                }
                else if (id == Char_Data.char_attackpetid)
                {
                    hp_packet.data.ReadBYTE();
                    hp_packet.data.ReadBYTE();
                    byte type = hp_packet.data.ReadBYTE();
                    int pet_index = 0;
                    switch (type)
                    {
                        case 0x05:
                            for (int i = 0; i < Char_Data.pets.Length; i++)
                            {
                                if (Char_Data.pets[i].id == id)
                                {
                                    pet_index = i;
                                    break;
                                }
                            }
                            Char_Data.pets[pet_index].curhp = hp_packet.data.ReadDWORD();
                            if (Globals.MainWindow.attackpet_use.Checked == true)
                            {
                                if (Char_Data.pets[pet_index].curhp < Convert.ToUInt32(Globals.MainWindow.attackpet_hp.Text))
                                {
                                    Autopot.UsePetHP(Char_Data.pets[pet_index].id);
                                }
                            }
                            break;
                        case 0x04:
                            if (hp_packet.data.ReadDWORD() == 0)
                            {
                                pet_status = 0;
                            }
                            else
                            {
                                pet_status = 1;
                            }
                            break;
                    }
                    if (Globals.MainWindow.attackpet_bad.Checked == true && pet_status == 1)
                    {
                        Autopot.UsePetUni(Char_Data.pets[pet_index].id);
                    }
                }
                else if (id == Char_Data.char_horseid)
                {
                    int pet_index = 0;
                    hp_packet.data.ReadBYTE();
                    hp_packet.data.ReadBYTE();
                    byte type = hp_packet.data.ReadBYTE();
                    switch (type)
                    {
                        case 0x05:
                            for (int i = 0; i < Char_Data.pets.Length; i++)
                            {
                                if (Char_Data.pets[i].id == id)
                                {
                                    pet_index = i;
                                    break;
                                }
                            }
                            Char_Data.pets[pet_index].curhp = hp_packet.data.ReadDWORD();
                            if (Globals.MainWindow.horsepot_use.Checked == true)
                            {
                                if (Char_Data.pets[pet_index].curhp < Convert.ToUInt32(Globals.MainWindow.horsepot_hp.Text))
                                {
                                    Autopot.UsePetHP(Char_Data.pets[pet_index].id);
                                }
                            }
                            break;
                        case 0x04:
                            if (hp_packet.data.ReadDWORD() == 0)
                            {
                                horse_status = 0;
                            }
                            else
                            {
                                horse_status = 1;
                            }
                            break;
                    }
                    if (Globals.MainWindow.horsepot_bad_use.Checked == true && pet_status == 1)
                    {
                        Autopot.UsePetUni(Char_Data.pets[pet_index].id);
                    }
                }
                else
                {
                    hp_packet.data.ReadBYTE();
                    hp_packet.data.ReadBYTE();
                    byte type = hp_packet.data.ReadBYTE();
                    switch (type)
                    {
                        case 0x05:
                            uint hp = hp_packet.data.ReadDWORD();
                            Globals.MainWindow.lb_mob_hp.Text = hp.ToString();
                            if (hp == 0)
                            {
                                for (int i = 0; i < Spawns.mob_id.Count; i++)
                                {
                                    if (id == Spawns.mob_id[i])
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
                                        if (id == MonsterControl.monster_id)
                                        {
                                            MonsterControl.monster_id = 0;
                                            MonsterControl.monster_type = 0;
                                            MonsterControl.monster_selected = false;
                                            Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION, false, enumDestination.Server);
                                            packetas.data.AddBYTE(2);
                                            Globals.ServerPC.SendPacket(packetas);
                                            //Select New Monster, cause selected just died
                                            if (Globals.MainWindow.fast_train.Checked)
                                            {
                                                System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                                                n_t.Start();
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("HPMP", ex.Message, hp_packet);
            }
        }
    }
}