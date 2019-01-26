using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silkroad
{
    class Chat
    {
        public static void Manager(Packet packet)
        {
            try
            {
                string name = null;
                string text = null;
                byte type = packet.data.ReadBYTE();
                switch (type)
                {
                    case (byte)Globals.Chat_Type.NORMAL:
                        uint id = packet.data.ReadDWORD();
                        if (id == Character.ID)
                        {
                            name = Character.PlayerName;
                            text = packet.data.ReadSTRING(enumStringType.ASCII);
                        }
                        else
                        {
                            for (int i = 0; i < Spawns.characters.Length; i++)
                            {
                                if (id == Spawns.characters[i].id)
                                {
                                    name = Spawns.characters[i].charname;
                                    break;
                                }
                            }
                            text = packet.data.ReadSTRING(enumStringType.ASCII);
                        }
                        Globals.UpdateChat(text, 0x01, name);
                        break;
                    default:
                        if (type != 7)
                        {
                            name = packet.data.ReadSTRING(enumStringType.ASCII);
                        }
                            text = packet.data.ReadSTRING(enumStringType.ASCII);
                        Globals.UpdateChat(text, type, name);
                        break;
                }
            }
            catch(Exception ex)
            {
                Globals.Debug("Chat", ex.Message, packet);
            }
        }
    }
}