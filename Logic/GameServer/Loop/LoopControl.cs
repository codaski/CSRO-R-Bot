using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class LoopControl
    {
        public static TextReader read;
        public static Timer timer = new Timer();
        public static Timer repeat = new Timer();
        public static byte repeat_walk = 0;
        public struct Coordinates_
        {
            public int x;
            public int y;
        }
        public static Coordinates_ Coordinates = new Coordinates_();

        public static void WalkScript()
        {
            try
            {
                if (BotData.bot)
                {
                    BotData.loop = true;
                    string action = read.ReadLine();
                    if (action.StartsWith("waitchar"))
                    {
                        BotData.loopaction = "waitchar";
                        Globals.UpdateLogs("Waiting For Character To Spawn");
                    }
                    if (action.StartsWith("go"))
                    {
                        BotData.loopaction = "go";
                        string[] tmp = action.Split(',');
                        Globals.UpdateLogs("Walk To: " + tmp[1] + " " + tmp[2]);
                        Coordinates.x = Convert.ToInt32(tmp[1]);
                        Coordinates.y = Convert.ToInt32(tmp[2]);
                        Action.WalkTo(Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
                        try
                        {
                            repeat.Stop();
                            repeat.Dispose();
                        }
                        catch
                        {
                        }
                        repeat = new Timer();
                        repeat.Interval = 3000;
                        repeat.Start();
                        repeat.Enabled = true;
                        repeat.Elapsed += new ElapsedEventHandler(repeat_Elapsed);
                        repeat_walk = 0;
                    }
                    if (action.StartsWith("talk"))
                    {
                        string[] tmp = action.Split(',');
                        if (tmp[1] == "storage")
                        {
                            BotData.loopaction = "storage";
                            StorageControl.OpenStorage();
                        }
                        if (tmp[1] == "blacksmith")
                        {
                            BotData.loopaction = "blacksmith";
                            SellControl.OpenShop();
                        }
                        if (tmp[1] == "stable")
                        {
                            BotData.loopaction = "stable";
                            BuyControl.OpenShop();
                        }
                        if (tmp[1] == "accessory")
                        {
                            BotData.loopaction = "accessory";
                            BuyControl.OpenShop();
                        }
                        if (tmp[1] == "potion")
                        {
                            BotData.loopaction = "potion";
                            BuyControl.OpenShop();
                        }
                    }
                    if (action.StartsWith("wait"))
                    {
                        string[] tmp = action.Split(',');
                        Globals.UpdateLogs("Waiting " + tmp[1] + " ms.");
                        try
                        {
                            timer.Stop();
                            timer.Dispose();
                        }
                        catch
                        {
                        }
                        timer = new Timer();
                        timer.Interval = Convert.ToInt32(tmp[1]) + 1;
                        timer.Start();
                        timer.Enabled = true;
                        timer.Elapsed += new ElapsedEventHandler(OnTick);
                    }
                    if (action.StartsWith("teleport"))
                    {
                        string[] tmp = action.Split(',');
                        uint id = Spawns.npcid[Spawns.npctype.IndexOf(Mobs_Info.mobstypelist[Mobs_Info.mobsidlist.IndexOf(Convert.ToUInt32(tmp[1]))])];
                        Teleport.Tele(id, Convert.ToByte(tmp[2]), Convert.ToUInt32(tmp[3]));
                    }
                    if (action == "end")
                    {
                        BotData.loopaction = null;
                        BotData.loop = false;
                        read.Close();
                        if (BotData.loopend == 0)
                        {
                            Globals.UpdateLogs("TownLoop Ended");
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
                            Globals.UpdateLogs("Walkscript Ended");
                            Globals.MainWindow.trainx.Text = Character.X.ToString();
                            Globals.MainWindow.trainy.Text = Character.Y.ToString();
                            StartLooping.Start();
                        }
                    }
                }
            }
            catch { }

        }

        static void repeat_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (repeat_walk == 0)
            {
                Action.WalkTo(Coordinates.x, Coordinates.y);
                LoopControl.WalkScript();
            }
            else
            {
                repeat.Stop();
                repeat.Dispose();
            }
        }

        public static void OnTick(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Dispose();
            if (BotData.bot)
            {
                LoopControl.WalkScript();
            }
        }
    }
}