using System;
using System.Collections.Generic;
using System.Text;

namespace Silkroad
{
    class BotData
    {
        public static uint bot_version = 328;
        #region Structs
        public struct LoginServer_
        {
            public string ip;
            public int port;
            public byte locale;
            public uint version;
            public byte reloging;
        }
        public struct Statistc_
        {
            public float time_elapsed;
            public int login_count;
            public int mob_killed;
            public int sp_begin;
            public long gold_begin;
            public int return_count;
        }
        public struct Servers_
        {
            public string ip;
            public string name;
            public byte locale;
            public uint version;
        }
        public static Servers_[] Servers = new Servers_[1];
        public static Statistc_ Statistic = new Statistc_();  
        public static LoginServer_ LoginServer = new LoginServer_();
        #endregion
        #region MainData
        public static bool use_client = false;
        public static string sro_path = null;
        public static string sro_server = null;
        public static byte ping = 0;
        public static bool use_al = false;
        public static int groupespawncount = 0;
        public static int groupespawninfo = 0;
        public static int groupespawned = 0;
        public static int chatbyte = -1;
        public static string walkscriptpath;
        #endregion
        #region Attack
        public static uint selectingid = 0;
        public static int selected = 0;
        public static uint selectedid = 0;
        public static string selectednpctype = null;
        #endregion
        #region BotLogic
        public static bool bot = false;
        public static byte loopend = 0;
        public static bool loop = false;
        public static string loopaction = null;
        #endregion
        #region WorldInfo
        public static uint teleportdata = 0;
        public static byte returning = 0;
        public static bool dead = false;
        #endregion
        #region Inventory
        public struct ItemsCount_
        {
            public uint hp_pots;
            public uint mp_pots;
            public uint arrows;
            public uint bolts;
            public uint uni_pills;
            public uint pet_hp;
            public uint horse;
            public uint hgp;
            public uint speed_pots;
            public uint return_scrool;
            public byte items_count;
            public uint vigor;
        }
        public static ItemsCount_ itemscount = new ItemsCount_();
        #endregion

        public static List<string> Server_Name = new List<string>();
        public static List<ushort> Server_ID = new List<ushort>();
    }
}
