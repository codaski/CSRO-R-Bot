using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class StartLooping
    {
        public static void Start()
        {
            if (BotData.bot)
            {
                string type = Location.FindTown();
                switch (type)
                {
                    case null:
                        BotData.loopend = 0;
                        BotData.loop = false;
                        Action.UseReturn();
                        break;
                    case "train":
                        if (Char_Data.char_horseid == 0)
                        {
                            BotData.loopend = 0;
                            BotData.loop = false;
                            BotData.bot = true;
                            PickupControl.there_is_pickable = true;
                            Buffas.buff_waiting = true;
                            Globals.UpdateLogs("Bot Started");
                            Globals.MainWindow.start_button.Text = "Stop Bot";
                            System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                            n_t.Start();
                        }
                        else
                        {
                            BotData.loopaction = "dismounthorse";
                            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_KILLHORSE, false, enumDestination.Server);
                            packet.data.AddDWORD(Char_Data.char_horseid);
                            Globals.ServerPC.SendPacket(packet);
                        }
                        break;
                   /* case "ch":
                        if (Globals.MainWindow.loop_off.Checked)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            if (Char_Data.char_horseid == 0)
                            {
                                BotData.loopaction = "mounthorse";
                                Action.MountHorse();
                            }
                            else
                            {
                                BotData.loopend = 1;
                                StartLooping.LoadTrainScript();
                            }
                        }
                        else
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"scripts/ch_town.txt");
                            BotData.loopend = 0;
                            BotData.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "wc":
                        if (Globals.MainWindow.loop_off.Checked)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            if (Char_Data.char_horseid == 0)
                            {
                                BotData.loopaction = "mounthorse";
                                Action.MountHorse();
                            }
                            else
                            {
                                BotData.loopend = 1;
                                StartLooping.LoadTrainScript();
                            }
                        }
                        else
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"scripts/wc_town.txt");
                            BotData.loopend = 0;
                            BotData.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "kt":
                        if (Globals.MainWindow.loop_off.Checked)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            if (Char_Data.char_horseid == 0)
                            {
                                BotData.loopaction = "mounthorse";
                                Action.MountHorse();
                            }
                            else
                            {
                                BotData.loopend = 1;
                                StartLooping.LoadTrainScript();
                            }
                        }
                        else
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"scripts/kt_town.txt");
                            BotData.loopend = 0;
                            BotData.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "ca":
                        if (Globals.MainWindow.loop_off.Checked)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            if (Char_Data.char_horseid == 0)
                            {
                                BotData.loopaction = "mounthorse";
                                Action.MountHorse();
                            }
                            else
                            {
                                BotData.loopend = 1;
                                StartLooping.LoadTrainScript();
                            }
                        }
                        else
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"scripts/ca_town.txt");
                            BotData.loopend = 0;
                            BotData.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;
                    case "eu":
                        if (Globals.MainWindow.loop_off.Checked)
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            if (Char_Data.char_horseid == 0)
                            {
                                BotData.loopaction = "mounthorse";
                                Action.MountHorse();
                            }
                            else
                            {
                                BotData.loopend = 1;
                                StartLooping.LoadTrainScript();
                            }
                        }
                        else
                        {
                            try
                            {
                                LoopControl.read.Close();
                            }
                            catch { }
                            LoopControl.read = new StreamReader(@"scripts/eu_town.txt");
                            BotData.loopend = 0;
                            BotData.loop = false;
                            LoopControl.WalkScript();
                        }
                        break;*/

                }
            }
        }

        public static void LoadTrainScript()
        {
            if (BotData.bot)
            {
                if (BotData.walkscriptpath != null | BotData.walkscriptpath != "")
                {
                    if (File.Exists(BotData.walkscriptpath))
                    {
                        try
                        {
                            LoopControl.read.Close();
                        }
                        catch { }
                        LoopControl.read = new StreamReader(BotData.walkscriptpath);
                        LoopControl.WalkScript();
                    }
                    else
                    {
                        BotData.bot = false;
                        BotData.loop = false;
                        Globals.MainWindow.start_button.Text = "Start Bot";
                        Globals.UpdateLogs("Cannot Find WalkScript !");
                    }
                }
                else
                {
                    BotData.bot = false;
                    BotData.loop = false;
                    Globals.MainWindow.start_button.Text = "Start Bot";
                    Globals.UpdateLogs("Cannot Find WalkScript !");
                }
            }
        }

        public static void CheckStart()
        {
            if (Char_Data.f_wep_name != null)
            {
                BotData.loopend = 0;
                BotData.loop = false;
                BotData.bot = true;
                PickupControl.there_is_pickable = true;
                Buffas.buff_waiting = true;
                Globals.UpdateLogs("Bot Started");
                Globals.MainWindow.start_button.Text = "Stop Bot";
                System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                n_t.Start();
            }
            else
            {
                Globals.UpdateLogs("Please Select First (And) Second Weapon !");
            }
        }
    }
}