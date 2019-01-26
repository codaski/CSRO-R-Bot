using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.Timers;
using System.Media;

namespace Silkroad
{
    class Alert
    {
        public static SoundPlayer player = new SoundPlayer(@"data/alert.wav");
        public static SoundPlayer notif = new SoundPlayer(@"data/notification.wav");
        public static void StartAlert()
        {
            player.Play();
        }
        public static void StartNotification()
        {
            notif.Play();
        }


    }
}
