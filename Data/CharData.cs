using System;
using System.Collections.Generic;
using System.Text;

namespace Silkroad
{
    class Char_Data
    {
        public static List<uint> skillid = new List<uint>();
        public static List<string> skillname = new List<string>();
        public static List<string> skilltype = new List<string>();

        public static List<uint> skillonid = new List<uint>();
        public static List<string> skillontype = new List<string>();
        public static List<uint> skillontemp = new List<uint>();
        public static List<string> skillonname = new List<string>();


        public static List<byte> inventoryslot = new List<byte>();
        public static List<uint> inventoryid = new List<uint>();
        public static List<string> inventorytype = new List<string>();
        public static List<ushort> inventorycount = new List<ushort>();
        public static List<uint> inventorydurability = new List<uint>();

        public static List<string> skillnamewaiting = new List<string>();
        public static List<byte> skilltipwaiting = new List<byte>();

        public static List<byte> storageslot = new List<byte>();
        public static List<uint> storageid = new List<uint>();
        public static List<string> storagetype = new List<string>();
        public static List<ushort> storagecount = new List<ushort>();
        public static List<uint> storagedurability = new List<uint>();
        public static ulong storagegold = 0;
        public static int storageopened = 0;

        public static string f_wep_name = null;
        public static string s_wep_name = null;

        public static uint char_horseid = 0;
        public static float char_horsespeed = 0;
        public static uint char_attackpetid = 0;
        public static uint char_grabpetid = 0;

        public struct Pet_
        {
            public struct Inventory_
            {
                public uint id;
                public string type;
                public byte slot;
                public ushort count;
                public uint durability;
            }

            public uint id;
            public ushort hgp;
            public uint maxhp;
            public uint curhp;
            public float speed;
            public Inventory_[] inventory;
        }

        public static Pet_[] pets = new Pet_[5];
    }
}
