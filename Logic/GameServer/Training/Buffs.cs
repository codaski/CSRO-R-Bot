using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class Buffas
    {
        public static Timer[] buff_timer = new Timer[Skills_Info.skillsidlist.Count];
        public static bool buff_waiting = false;
        public static bool changing_weapon = false;
        public static int min_mp_require = 200000;

        #region Cast Buff
        public static void CastBuff(uint id)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION, false, enumDestination.Server);
            packet.data.AddBYTE(0x01);
            packet.data.AddBYTE(0x04);
            string type = Skills_Info.skillstypelist[Skills_Info.skillsidlist.IndexOf(id)];
            if (!type.StartsWith("SKILL_CH_LIGHTNING_GYEONGGONG"))
            {
                packet.data.AddDWORD(id);
                packet.data.AddBYTE(0x01);
                packet.data.AddDWORD(Character.ID);
            }
            Globals.ServerPC.SendPacket(packet);
        }
        #endregion

        #region Buff Add
        public static void BuffAdd(Packet packet)
        {
            try
            {
                uint charid = packet.data.ReadDWORD();
                if (charid == Character.ID)
                {
                    uint id = packet.data.ReadDWORD();
                    int indexx = Skills_Info.skillsidlist.IndexOf(id);
                    if (indexx != -1)
                    {
                        uint temp = packet.data.ReadDWORD();
                        string type = Skills_Info.skillstypelist[indexx];
                        string name = Skills_Info.skillsnamelist[indexx];
                        Char_Data.skillonid.Add(id);
                        Char_Data.skillontemp.Add(temp);
                        Char_Data.skillontype.Add(type);
                        Char_Data.skillonname.Add(name);
                        Skills_Info.skillsstatuslist[indexx] = 1;
                        int index = Char_Data.skillnamewaiting.IndexOf(name);
                        Globals.MainWindow.listBox2.Items.Add(index);  
                        if (index != -1)
                        {
                            Char_Data.skillnamewaiting.RemoveAt(index);
                            Char_Data.skilltipwaiting.RemoveAt(index);
                            Globals.MainWindow.listBox2.Items.RemoveAt(index);                 
                        }
                        try
                        {
                            buff_timer[indexx].Stop();
                            buff_timer[indexx].Dispose();
                        }
                        catch { }
                        buff_timer[indexx] = new Timer();
                        buff_timer[indexx].Interval = Skills_Info.skillscasttimelist[indexx] + Skills_Info.skillcooldownlist[indexx] + 1;
                        buff_timer[indexx].Elapsed += new ElapsedEventHandler(Buffer_Elapsed);
                        buff_timer[indexx].Start();
                        buff_timer[indexx].Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("BuffCasted", ex.Message, packet);
            }
        }
        #endregion
        #region Buff Deletion
        public static void BuffDell(Packet packet)
        {
            try
            {
                byte packet_type = packet.data.ReadBYTE();
                if (packet_type == 0x01)
                {
                    uint temp = packet.data.ReadDWORD();
                    int index = Char_Data.skillontemp.IndexOf(temp);
                    if (index != -1)
                    {
                        string type = Char_Data.skillontype[index];
                        string name = Skills_Info.skillsnamelist[Skills_Info.skillstypelist.IndexOf(Char_Data.skillontype[index])];
                        if (Globals.MainWindow.buffs_list1.Items.IndexOf(name) != -1)
                        {
                            Char_Data.skilltipwaiting.Add(1);
                            Char_Data.skillnamewaiting.Add(name);
                            buff_waiting = true;
                        }
                        if (Globals.MainWindow.buffs_list2.Items.IndexOf(name) != -1)
                        {
                            Char_Data.skilltipwaiting.Add(2);
                            Char_Data.skillnamewaiting.Add(name);
                            buff_waiting = true;
                        }
                   /*     if (Globals.MainWindow.buffs_list3.Items.IndexOf(name) != -1)
                        {
                            Char_Data.skilltipwaiting.Add(3);
                            Char_Data.skillnamewaiting.Add(name);
                        }
                        if (Globals.MainWindow.buffs_list4.Items.IndexOf(name) != -1)
                        {
                            Char_Data.skilltipwaiting.Add(4);
                            Char_Data.skillnamewaiting.Add(name);
                        }*/
                        Char_Data.skillonid.RemoveAt(index);
                        Char_Data.skillontemp.RemoveAt(index);
                        Char_Data.skillontype.RemoveAt(index);
                        Char_Data.skillonname.RemoveAt(index);
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Debug("BuffDeleted", ex.Message, packet);
            }
        }
        #endregion
        #region Buffer Elapsed
        static void Buffer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int index = 0;
            for (int i = 0; i < buff_timer.Length; i++)
            {
                if ((Timer)sender == buff_timer[i])
                {
                    index = i;
                    break;
                }
            }
            buff_timer[index].Stop();
            buff_timer[index].Dispose();
            Skills_Info.skillsstatuslist[index] = 0;
            string name = Skills_Info.skillsnamelist[index];

            if (Globals.MainWindow.buffs_list1.Items.IndexOf(name) != -1 || Globals.MainWindow.buffs_list2.Items.IndexOf(name) != -1)
            {
                buff_waiting = true;
            }
        }
        #endregion

        #region Buffer
        public static void BuffChecker()
        {
            for (int i = 0; i < Char_Data.skillnamewaiting.Count; i++)
            {
                try
                {
                    if ((Char_Data.skilltipwaiting[i] == 2) || (Char_Data.skilltipwaiting[i] == 4 && MonsterControl.monster_type > 1)) // At First We need to check 2nd weapon buffs :)
                    {
                        //Checking If User Have 2nd Weapon !
                        if (Char_Data.s_wep_name != "" && Char_Data.s_wep_name != null)
                        {
                            int index = Char_Data.inventorytype.IndexOf(Char_Data.s_wep_name);
                            if (index == -1)
                            {
                                Char_Data.s_wep_name = "";
                                Globals.UpdateLogs("Could not find second weapon !");
                                break;
                            }
                        }
                        System.Threading.Thread.Sleep(2); //MicroSleep For Less CPU Usage

                        //Trying To Cast Buff
                        string skill_type = Char_Data.skilltype[Char_Data.skillname.IndexOf(Char_Data.skillnamewaiting[i])];
                        int skill_index = Skills_Info.skillstypelist.IndexOf(skill_type); // Getting Skill Index
                        if (Skills_Info.skillsstatuslist[skill_index] == 0 && Skills_Info.skillsmpreq[skill_index] <= Character.CurrentMP) // Checking if enough MP and it's possible to cast buff (cooldown).
                        {
                            System.Threading.Thread.Sleep(1);
                            int item_index = Char_Data.inventorytype.IndexOf(Char_Data.s_wep_name); // Getting Second Weapon Index
                            if (Char_Data.inventoryslot[item_index] != 6) // Checking If Second Weapon Is In Weapon Slot
                            {
                                System.Threading.Thread.Sleep(1);
                                //Weapon Isn't In Weapon Slot, Changing The Weapon !!!
                                changing_weapon = true;
                                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
                                packet.data.AddBYTE(0x00);
                                packet.data.AddBYTE((byte)Char_Data.inventoryslot[item_index]);
                                packet.data.AddBYTE(0x06);
                                packet.data.AddBYTE(0x00);
                                packet.data.AddWORD(0x0000);
                                Globals.ServerPC.SendPacket(packet);
                                break;
                            }
                            else
                            {
                                //Weapon Is In Weapon Slot, Casting The Buff !!!
                                System.Threading.Thread.Sleep(1);
                                CastBuff(Skills_Info.skillsidlist[skill_index]);
                                break;
                            }
                        }
                        else
                        {
                            if (Skills_Info.skillsmpreq[skill_index] > Character.CurrentMP)
                            {
                                min_mp_require = Skills_Info.skillsmpreq[skill_index];
                            }
                        }
                    }
                    if (i + 1 >= Char_Data.skillnamewaiting.Count)
                    {
                        //---------------------------------------------------------------------------------------------------------------------------------------------------------
                        for (int a = 0; a < Char_Data.skillnamewaiting.Count; a++)
                        {
                            if ((Char_Data.skilltipwaiting[i] == 1) || (Char_Data.skilltipwaiting[i] == 3 && MonsterControl.monster_type > 1)) // We need to check 1st weapon buffs :)
                            {
                                //Checking If User Have 1st Weapon !
                                if (Char_Data.f_wep_name != "" && Char_Data.f_wep_name != null)
                                {
                                    int index = Char_Data.inventorytype.IndexOf(Char_Data.f_wep_name);
                                    if (index == -1)
                                    {
                                        Char_Data.f_wep_name = "";
                                        Globals.UpdateLogs("Could not find first weapon !");
                                        break;
                                    }
                                }
                                System.Threading.Thread.Sleep(2); //MicroSleep For Less CPU Usage

                                //Trying To Cast Buff
                                string skill_type = Char_Data.skilltype[Char_Data.skillname.IndexOf(Char_Data.skillnamewaiting[a])];
                                int skill_index = Skills_Info.skillstypelist.IndexOf(skill_type); // Getting Skill Index
                                if (Skills_Info.skillsstatuslist[skill_index] == 0 && Skills_Info.skillsmpreq[skill_index] <= Character.CurrentMP) // Checking if enough MP and it's possible to cast buff (cooldown).
                                {
                                    System.Threading.Thread.Sleep(1);
                                    int item_index = Char_Data.inventorytype.IndexOf(Char_Data.f_wep_name); // Getting First Weapon Index
                                    if (Char_Data.inventoryslot[item_index] != 6) // Checking If First Weapon Is In Weapon Slot
                                    {
                                        System.Threading.Thread.Sleep(1);
                                        //Weapon Isn't In Weapon Slot, Changing The Weapon !!!
                                        changing_weapon = true;
                                        Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
                                        packet.data.AddBYTE(0x00);
                                        packet.data.AddBYTE((byte)Char_Data.inventoryslot[item_index]);
                                        packet.data.AddBYTE(0x06);
                                        packet.data.AddBYTE(0x00);
                                        packet.data.AddWORD(0x0000);
                                        Globals.ServerPC.SendPacket(packet);
                                        break;
                                    }
                                    else
                                    {
                                        //Weapon Is In Weapon Slot, Casting The Buff !!!
                                        System.Threading.Thread.Sleep(1);
                                        CastBuff(Skills_Info.skillsidlist[skill_index]);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (Skills_Info.skillsmpreq[skill_index] > Character.CurrentMP)
                                    {
                                        min_mp_require = Skills_Info.skillsmpreq[skill_index];
                                    }
                                }
                            }
                            if (i + 1 >= Char_Data.skillnamewaiting.Count)
                            {
                                //---------------------------------------------------------------------------------------------------------------------------------------------------------
                                buff_waiting = false;
                                System.Threading.Thread thread = new System.Threading.Thread(LogicControl.Manager);
                                thread.Start();
                                //---------------------------------------------------------------------------------------------------------------------------------------------------------
                            }
                            //---------------------------------------------------------------------------------------------------------------------------------------------------------
                        }
                    }
                }
                catch { }
            }
            if (Char_Data.skillnamewaiting.Count == 0)
            {
                buff_waiting = false;
                System.Threading.Thread thread = new System.Threading.Thread(LogicControl.Manager);
                thread.Start();
            }
        }

        #endregion

    }
}