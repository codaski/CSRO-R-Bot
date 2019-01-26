using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class Movement
    {
        public static Timer timer = new Timer();
        public static byte moved_count = 0;
        public static byte stuck = 0;
        public static void Move(Packet packet)
        {
            uint id = packet.data.ReadDWORD();
            if (id == Character.ID)
            {
                if (packet.data.ReadBYTE() == 1)
                {
                    LoopControl.repeat_walk = 1;
                    try
                    {
                        LoopControl.repeat.Stop();
                        LoopControl.repeat.Dispose();
                    }
                    catch { }
                    byte xsec = packet.data.ReadBYTE();
                    byte ysec = packet.data.ReadBYTE();
                    float xcoord = 0;
                    float zcoord = 0;
                    float ycoord = 0;
                    if (ysec == 0x80)
                    {
                        xcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        zcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                    }
                    else
                    {
                        xcoord = packet.data.ReadWORD();
                        zcoord = packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD();
                    }
                    int real_xcoord = 0;
                    int real_ycoord = 0;
                    if (xcoord > 32768)
                    {
                        real_xcoord = (int)(65536 - xcoord);
                    }
                    else
                    {
                        real_xcoord = (int)xcoord;
                    }
                    if (ycoord > 32768)
                    {
                        real_ycoord = (int)(65536 - ycoord);
                    }
                    else
                    {
                        real_ycoord = (int)ycoord;
                    }
                    int x = Action.CalculatePositionX(xsec, real_xcoord);
                    int y = Action.CalculatePositionY(ysec, real_ycoord);
                    if (BotData.loop && BotData.loopaction == "go")
                    {
                        int dist = Math.Abs((x - Character.X)) + Math.Abs((y - Character.Y));
                        timer.Stop();
                        timer.Dispose();
                        timer = new Timer();
                        int time = Convert.ToInt32(dist * 10000 / Convert.ToInt64(Character.speed)) + 500;
                        timer.Interval = time;
                        timer.Start();
                        timer.Enabled = true;
                        timer.Elapsed += new ElapsedEventHandler(OnTick);
                    }
                    if (BotData.bot && BotData.loopaction == "randwalk")
                    {
                        int dist = Math.Abs((x - Character.X)) + Math.Abs((y - Character.Y));
                        timer.Stop();
                        timer.Dispose();
                        timer = new Timer();
                        int time = Convert.ToInt32(dist * 10000 / Convert.ToInt64(Character.speed)) + 1;
                        timer.Interval = time;
                        timer.Start();
                        timer.Enabled = true;
                        timer.Elapsed += new ElapsedEventHandler(OnTick);
                    }
                    if (BotData.loopaction == "record")
                    {
                        string text = "go," + x + "," + y;
                        Globals.MainWindow.script_record_box.Items.Add(text);
                    }

                    Character.X = x;
                    Character.Y = y;
                    Globals.MainWindow.lb_x1.Text = Character.X.ToString();
                    Globals.MainWindow.lb_y1.Text = Character.Y.ToString();
                    //Recalculate Mob Distance
                    for (int i = 0; i < Spawns.mob_id.Count; i++)
                    {
                        Spawns.mob_dist[i] = Math.Abs((Spawns.mob_x[i] - Character.X)) + Math.Abs((Spawns.mob_y[i] - Character.Y));
                    }
                    //Recalculate Mob Distance
                }
            }
            else if (id == Char_Data.char_horseid)
            {
                if (packet.data.ReadBYTE() == 1)
                {
                    LoopControl.repeat_walk = 1;
                    try
                    {
                        LoopControl.repeat.Stop();
                        LoopControl.repeat.Dispose();
                    }
                    catch { }
                    byte xsec = packet.data.ReadBYTE();
                    byte ysec = packet.data.ReadBYTE();
                    float xcoord = 0;
                    float zcoord = 0;
                    float ycoord = 0;
                    if (ysec == 0x80)
                    {
                        xcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        zcoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD() - packet.data.ReadWORD();
                    }
                    else
                    {
                        xcoord = packet.data.ReadWORD();
                        zcoord = packet.data.ReadWORD();
                        ycoord = packet.data.ReadWORD();
                    }
                    int real_xcoord = 0;
                    int real_ycoord = 0;
                    if (xcoord > 33000)
                    {
                        real_xcoord = (int)(65352 - xcoord);
                    }
                    else
                    {
                        real_xcoord = (int)xcoord;
                    }
                    if (ycoord > 33000)
                    {
                        real_ycoord = (int)(65352 - ycoord);
                    }
                    else
                    {
                        real_ycoord = (int)ycoord;
                    }
                    int x = Action.CalculatePositionX(xsec, real_xcoord);
                    int y = Action.CalculatePositionY(ysec, real_ycoord);
                    if (BotData.loop && BotData.loopaction == "go")
                    {
                        int dist = Math.Abs((x - Character.X)) + Math.Abs((y - Character.Y));
                        timer.Stop();
                        timer.Dispose();
                        timer = new Timer();
                        int time = Convert.ToInt32(dist * 10000 / Convert.ToInt64(Char_Data.char_horsespeed)) + 100;
                        timer.Interval = time;
                        timer.Start();
                        timer.Enabled = true;
                        timer.Elapsed += new ElapsedEventHandler(OnTick);
                    }
                    if (BotData.bot && BotData.loopaction == "randwalk")
                    {
                        int dist = Math.Abs((x - Character.X)) + Math.Abs((y - Character.Y));
                        timer.Stop();
                        timer.Dispose();
                        timer = new Timer();
                        int time = Convert.ToInt32(dist * 10000 / Convert.ToInt64(Character.speed)) + 1;
                        timer.Interval = time;
                        timer.Start();
                        timer.Enabled = true;
                        timer.Elapsed += new ElapsedEventHandler(OnTick);
                    }
                    if (BotData.loopaction == "record")
                    {
                        string text = "go," + x + "," + y;
                        Globals.MainWindow.script_record_box.Items.Add(text);
                    }            

                    Character.X = x;
                    Character.Y = y;
                    Globals.MainWindow.lb_x1.Text = Character.X.ToString();
                    Globals.MainWindow.lb_y1.Text = Character.Y.ToString();
                    //Recalculate Mob Distance
                    for (int i = 0; i < Spawns.mob_id.Count; i++)
                    {
                        Spawns.mob_dist[i] = Math.Abs((Spawns.mob_x[i] - Character.X)) + Math.Abs((Spawns.mob_y[i] - Character.Y));
                    }
                    //Recalculate Mob Distance
                }
            }
            else
            {
                if (packet.data.ReadBYTE() == 0x01)
                {
                    for (int i = 0; i < Spawns.mob_id.Count; i++)
                    {
                        if (Spawns.mob_id[i] == id)
                        {
                            byte xsec = packet.data.ReadBYTE();
                            byte ysec = packet.data.ReadBYTE();
                            float xcoord = 0;
                            float ycoord = 0;
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
                            int x = Action.CalculatePositionX(xsec, xcoord);
                            int y = Action.CalculatePositionY(ysec, ycoord);
                            int dist = Math.Abs((x - Character.X)) + Math.Abs((y - Character.Y));
                            Spawns.mob_x[i] = x;
                            Spawns.mob_y[i] = y;
                            Spawns.mob_dist[i] = dist;
                            break;
                        }
                    }
                }
            }
        }
        public static void OnTick(object sender, ElapsedEventArgs e)
        {
            if (BotData.bot && BotData.loopaction != "randwalk")
            {
                BotData.loopaction = "";
                LoopControl.WalkScript();
            }
            if (BotData.bot && BotData.loopaction == "randwalk")
            {
                BotData.loopaction = "";
            }
            timer.Stop();
            timer.Dispose();
        }

        public static byte stuck_count = 0;
        public static bool enablelogic = true;
        public static void Stuck(Packet packet)
        {

            if (packet.data.ReadDWORD() == Character.ID)
            {
                stuck_count++;
                byte xsec = packet.data.ReadBYTE();
                byte ysec = packet.data.ReadBYTE();
                float xcoord = packet.data.ReadSINGLE();
                packet.data.ReadSINGLE();
                float ycoord = packet.data.ReadSINGLE();
                Character.X = Action.CalculatePositionX(xsec, xcoord);
                Character.Y = Action.CalculatePositionY(ysec, ycoord);
                if (stuck_count > 2)
                {
                    stuck_count = 0;
                    if (BotData.bot)
                    {
                        if (MonsterControl.monster_selected)
                        {
                            Silkroad.Stuck.AddMob(MonsterControl.monster_id, 10);
                            MonsterControl.monster_id = 0;
                            MonsterControl.monster_selected = false;
                            MonsterControl.monster_type = 0;
                            Packet packetas = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_OBJECTACTION, false, enumDestination.Server);
                            packetas.data.AddBYTE(2);
                            Globals.ServerPC.SendPacket(packetas);

                            enablelogic = false;
                            System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                            n_t.Start();
                            enablelogic = true;
                        }
                    }
                }
            }
        }
    }
}