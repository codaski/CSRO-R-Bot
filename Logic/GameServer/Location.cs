using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Silkroad
{
    class Location
    {
        public static string FindTown()
        {
            string town = null;
            int ch_dist = Math.Abs((6432 - Character.X)) + Math.Abs((1096 - Character.Y));
            int wc_dist = Math.Abs((3553 - Character.X)) + Math.Abs((2072 - Character.Y));
            int kt_dist = Math.Abs((112 - Character.X)) + Math.Abs((16 - Character.Y));
            int ca_dist = Math.Abs((-5156 - Character.X)) + Math.Abs((2831 - Character.Y));
            int eu_dist = Math.Abs((-10659 - Character.X)) + Math.Abs((2603 - Character.Y));
            int train_dist = Math.Abs((Convert.ToInt32(Globals.MainWindow.trainx.Text) - Character.X)) + Math.Abs((Convert.ToInt32(Globals.MainWindow.trainy.Text) - Character.Y));

            if (train_dist <= Convert.ToInt32(Globals.MainWindow.trainr.Text))
            {
                town = "train";
            }
            if (ch_dist <= 20)
            {
                town = "ch";
            }
            if (wc_dist <= 20)
            {
                town = "wc";
            }
            if (kt_dist <= 20)
            {
                town = "kt";
            }
            if (ca_dist <= 20)
            {
                town = "ca";
            }
            if (eu_dist <= 20)
            {
                town = "eu";
            }

            return town;
        }
    }
}