using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class RandomWalk
    {
        public static Timer RandomTimer = new Timer();
        public static bool walking_randomly = false;
        public static bool walking_center = false;
        public static Random random = new Random();

        public static void WalkManager()
        {
            if (Globals.MainWindow.walk_center.Checked)
            {
                Globals.UpdateLogs("Walk To Center");
                int trainx = Convert.ToInt32(Globals.MainWindow.trainx.Text);
                int trainy = Convert.ToInt32(Globals.MainWindow.trainy.Text);

                if (!walking_center)
                {
                    walking_center = true;
                    Action.WalkTo(trainx, trainy);
                    System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                    n_t.Start();
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                    n_t.Start();
                }
            }
            else
            {
                if (Globals.MainWindow.walk_random.Checked)
                {
                    //Globals.UpdateLogs("Walking Randomly");
                    int randomx = (Character.X + random.Next(-30, 30));
                    int randomy = (Character.Y + random.Next(-30, 30));
                    Action.WalkTo(randomx, randomy);
                    try
                    {
                        RandomTimer.Stop();
                        RandomTimer.Dispose();
                    }
                    catch { }
                    RandomTimer = new Timer();
                    int dist = Math.Abs((randomx - Character.X)) + Math.Abs((randomy - Character.Y));
                    int time = Convert.ToInt32(dist * 5000 / Convert.ToInt64(Character.speed)) + 1;
                    RandomTimer.Elapsed += new ElapsedEventHandler(OnTick);
                    RandomTimer.Interval = time;
                    RandomTimer.Start();
                    RandomTimer.Enabled = true;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
                    n_t.Start();
                }
            }
        }

        public static void OnTick(object sender, ElapsedEventArgs e)
        {
            System.Threading.Thread n_t = new System.Threading.Thread(LogicControl.Manager);
            n_t.Start();
            RandomTimer.Stop();
            RandomTimer.Dispose();
        }
    }
}