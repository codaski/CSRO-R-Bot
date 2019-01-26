using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Silkroad
{
    class LoadTXT
    {
        public static void LoadData()
        {
            Globals.UpdateLogs("Loading Silkroad Data");
            try
            {
                //Load mobs begin
                TextReader tr = new StreamReader(Environment.CurrentDirectory + @"\data\parse_mobs.txt");
                string input;
                string[] txt;
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Mobs_Info.mobsidlist.Add(Globals.String_To_UInt32(txt[0]));
                        Mobs_Info.mobstypelist.Add(txt[1]);
                        Mobs_Info.mobsnamelist.Add(txt[2]);
                        Mobs_Info.mobslevellist.Add(Convert.ToByte(txt[3]));
                        Mobs_Info.mobshplist.Add(Convert.ToUInt32(txt[4]));
                    }
                }
                tr.Close();
                //Load mobs end
                //Load items begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\data\parse_items.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Items_Info.itemsidlist.Add(Globals.String_To_UInt32(txt[0]));
                        Items_Info.itemstypelist.Add(txt[1]);
                        Items_Info.itemsnamelist.Add(txt[2]);
                        Items_Info.itemslevellist.Add(Convert.ToByte(txt[3]));
                        Items_Info.items_maxlist.Add(Convert.ToUInt16(txt[4]));
                        Items_Info.itemsdurabilitylist.Add(Convert.ToUInt32(txt[5]));
                    }
                }
                tr.Close();
                //Load items end
                //Load skill begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\data\parse_skills.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Skills_Info.skillsidlist.Add(Globals.String_To_UInt32(txt[0]));
                        Skills_Info.skillstypelist.Add(txt[1]);
                        Skills_Info.skillsnamelist.Add(txt[2]);
                        Skills_Info.skillscasttimelist.Add(Convert.ToInt32(txt[3]));
                        Skills_Info.skillcooldownlist.Add(Convert.ToInt32(txt[4]));
                        Skills_Info.skillsmpreq.Add(Convert.ToInt32(txt[5]));
                        Skills_Info.skillsstatuslist.Add(0);
                    }
                }
                tr.Close();
                //Load skill end
                //Load exp begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\data\parse_exp.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        Character.explist.Add(txt[0]);
                    }
                }
                tr.Close();
                //Load exp end
                //Load Shop Begin
                tr = new StreamReader(Environment.CurrentDirectory + @"\data\parse_shop.txt");
                while ((input = tr.ReadLine()) != null)
                {
                    if (input != "" && !input.StartsWith("//"))
                    {
                        txt = input.Split(',');
                        string StoreName = txt[0];
                        for (int i = 0; i < Data.ShopTabData.Length; i++)
                        {
                            if (StoreName.StartsWith(Data.ShopTabData[i].StoreName))
                            {
                                for (int a = 0; a < Data.ShopTabData[i].Tab.Length; a++)
                                {
                                    if (Data.ShopTabData[i].Tab[a].TabName == StoreName)
                                    {
                                        Data.ShopTabData[i].Tab[a].ItemType[Convert.ToInt32(txt[2])] = txt[1];
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                tr.Close();
                //Load Shop End
            }
            catch (Exception a)
            {
                Globals.UpdateLogs(a.Message);
                Globals.UpdateLogs("Cannot Load Data Files !");
                Globals.Connection.ClientClose();
            }
        }

        public static void LoadTablets()
        {
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of Str");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of Int");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of master");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of strikes");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of discipline");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of penetration");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of dodging");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of stamina");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of magic");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of fogs");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of air");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of fire");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of immunity");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of revival");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of immortal");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of steady");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of luck");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of astral");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of Str");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of Int");
            Globals.MainWindow.tablet_dont.Items.Add("Magic stone of master");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of courage");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of warriors");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of philosophy");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of meditation");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of challenge");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of focus");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of flesh");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of life");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of mind");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of spirit");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of dodging");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of agility");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of training");
            Globals.MainWindow.tablet_dont.Items.Add("Attribute stone of prayer");
        }

        public static void CheckScripts()
        {
            byte error = 0;
            if (!File.Exists(Environment.CurrentDirectory + @"\scripts\ch_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\scripts\wc_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\scripts\kt_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\scripts\ca_town.txt"))
            {
                error++;
            }
            if (!File.Exists(Environment.CurrentDirectory + @"\scripts\eu_town.txt"))
            {
                error++;
            }
            if (error > 0)
            {
                Globals.UpdateLogs("Cannot Load Scripts !");
                Globals.Connection.ClientClose();
            }
        }

        public static void LoadServers()
        {
            try
            {
                TextReader tr = new StreamReader(Environment.CurrentDirectory + @"\configs\server.cfg");
                string temp = null;
                int count = 0;
                while ((temp = tr.ReadLine()) != null)
                {
                    if (!temp.StartsWith("//"))
                    {
                        count++;
                    }
                }
                tr.Close();
                BotData.Servers = new BotData.Servers_[count];
                tr = new StreamReader(Environment.CurrentDirectory + @"\configs\server.cfg");
                int index = 0;
                while ((temp = tr.ReadLine()) != null)
                {
                    if (!temp.StartsWith("//"))
                    {
                        BotData.Servers[index].name = temp.Split(';')[0];
                        Globals.MainWindow.server_name.Items.Add(temp.Split(';')[0]);
                        BotData.Servers[index].ip = temp.Split(';')[1];
                        BotData.Servers[index].locale = Convert.ToByte(temp.Split(';')[2]);
                        BotData.Servers[index].version = Convert.ToUInt32(temp.Split(';')[3]);
                        index++;
                    }
                }
                tr.Close();
            }
            catch
            {
               // Globals.UpdateLogs("Cannot Load Servers !");
                //Globals.Connection.ClientClose();
            }
        }
    }
}
