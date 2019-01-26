using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Threading;

namespace Silkroad
{
    class Parser
    {
        public bool login_bug = false;
        public void Handler(Packet packet)
        {
            //Globals.Debug(packet.Opc.ToString("X2"), "idk", packet);
            switch (packet.Opc)
            {
                #region LoginServer
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.HANDSHAKE:
                    SilkroadSecurity.Analyze(packet);
                    break;
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.AGENT_SERVER:
                    LoginServer.ServerInfo(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.PATCH_INFO:
                    LoginServer.AnalyzePatch(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.SERVER_LIST:
                    Servers_Auto.Analyze(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.LOGIN_REPLY:
                    LoginServer.LoginResponse(packet);
                    break;
                case (ushort)LoginServerOpcodes.SERVER_OPCODES.GAME_LOGIN_REPLY:
                    LoginServer.GameLoginResponse(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                #region Character List
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHARACTERLISTING:
                    Character.CharacterList(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)0xB001:
                    Globals.MainWindow.select.Enabled = false;
                    Globals.MainWindow.char_name.Enabled = false;
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                #region Character Spawn
                case (ushort)0x3020:
                    Character.skip_charid = new byte[4];
                    Character.skip_charid[0] = packet.data.ReadBYTE();
                    Character.skip_charid[1] = packet.data.ReadBYTE();
                    Character.skip_charid[2] = packet.data.ReadBYTE();
                    Character.skip_charid[3] = packet.data.ReadBYTE();
                    Character.CharData(Character.CharPacket);
                    if (!BotData.use_client)
                    {
                        Globals.ServerPC.SendPacket(new Packet((ushort)0x3012, false, enumDestination.Server));
                    }
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHARDATA:
                    Character.CharPacket = packet;
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STUFFUPDATE:
                    Character.StuffUpdate(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHARACTERINFO:
                    Character.CharacterInfo(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_HPMPUPDATE:
                    HPMPPacket.HPMPUpdate(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
               case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SPEEDUPDATE:
                 Character.SpeedUpdate(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GUILDINFO:
                    Character.GuildAnalyze(packet);
                   Globals.ClientPC.SendPacket(packet);
                   break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_LVLUP:
                    Character.LevelUp(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
              //  case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_EXPSPUPDATE:
                   // Character.ExpSpUpdate(packet);
                //    Globals.ClientPC.SendPacket(packet);
                 //   break;
               // case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_CHAT:
                    //Chat.Manager(packet);
                 //   Globals.ClientPC.SendPacket(packet);
                   // break;
               // case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_HORSEACTION:
                 //   Pets.HorseAction(packet);
                  //  Globals.ClientPC.SendPacket(packet);
                   // break;

                #region Spawns
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPSPAWNB:
                    GroupeSpawn.GroupeSpawnB(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPESPAWN:
                    GroupeSpawn.Manager(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_GROUPSPAWNEND:
                    GroupeSpawn.GroupeSpawned();
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SINGLESPAWN:
                    Spawn.SingleSpawn(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SINGLEDESPAWN:
                    Spawn.SingleDeSpawn(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                #region Pets
             case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_PETINFO:
               Pets.PetInfo(packet);
                 Globals.ClientPC.SendPacket(packet);
                  break;
              case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_PETSTATS:
                  Pets.PetStats(packet);
                Globals.ClientPC.SendPacket(packet);
                 break;
                #endregion
                #region Items
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_ITEMFIXED:
                    InventoryControl.ItemFixed(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_DURABILITYCHANGE:
                    InventoryControl.Durability(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT:
                    InventoryControl.Inventory_Update1(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYUSE:
                    InventoryControl.Inventory_Update(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                #region Storage
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STORAGEITEMS:
                  StorageControl.ParseStorageItems(packet);
                 Globals.ClientPC.SendPacket(packet);
                 break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STORAGEGOLD:
                  StorageControl.StorageGold(packet);
                Globals.ClientPC.SendPacket(packet);
               break;
                #endregion
                #region Buffs
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_BUFFINFO:
                    Buffas.BuffAdd(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_BUFFDELL:
                    Buffas.BuffDell(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                #region Skills
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SKILLADD:
                    Skills.SkillAdd(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_SKILLUPDATE:
                    Skills.SkillUpdate(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
                #region Movement
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_STUCK:
                    Movement.Stuck(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_MOVE:
                    Movement.Move(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion

                #region Training
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_OBJECTDIE:
                    MonsterControl.MonsterAction(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                //case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_NPCDESELECT:
                  //  MonsterControl.NPCDeselect(packet);
                   // Globals.ClientPC.SendPacket(packet);
                   // break;
              //  case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_NPCSELECT:
               //     MonsterControl.NPCSelect(packet);
                 //   break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_OBJECTSELECT:
                    MonsterControl.Selected(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_OBJECTACTION:
                    MonsterControl.Refresh(packet);
                    Globals.ClientPC.SendPacket(packet);
                    break;
                #endregion
              //  case (ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_PARTYINVITATION:
                    //Party.PartyStuff(packet);
                //    Globals.ClientPC.SendPacket(packet);
                 //   break;
                default:
                    Globals.ClientPC.SendPacket(packet);
                    break;
            }
        }
    }
}

