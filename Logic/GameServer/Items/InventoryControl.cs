using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Silkroad
{
    class InventoryControl
    {
        public static void Durability(Packet packet)
        {
            byte slot = packet.data.ReadBYTE();
            uint new_durability = packet.data.ReadDWORD();
            int index = Char_Data.inventoryslot.IndexOf(slot);
            if (index != -1)
            {
                Char_Data.inventorydurability[index] = new_durability;
                if (Globals.MainWindow.low_wep.Checked == true)
                {
                    if (new_durability <= 1 && slot == Char_Data.inventoryslot[Char_Data.inventorytype.IndexOf(Char_Data.f_wep_name)])
                    {
                        Globals.UpdateLogs("Returning To Town:! Low Weapon Durability");
                        Action.UseReturn();
                    }
                }
            }
        }

        public static void Inventory_Update(Packet packet)
        {
            int type = packet.data.ReadBYTE();
            if (type == 1)
            {
                byte slot = packet.data.ReadBYTE();
                ushort count = packet.data.ReadWORD();
                int index = Char_Data.inventoryslot.IndexOf(slot);
                if (count > 0)
                {
                    Char_Data.inventorycount[index] = count;
                }
                else
                {
                    Char_Data.inventoryid.RemoveAt(index);
                    Char_Data.inventorytype.RemoveAt(index);
                    Char_Data.inventoryslot.RemoveAt(index);
                    Char_Data.inventorycount.RemoveAt(index);
                    Char_Data.inventorydurability.RemoveAt(index);
                    Globals.MainWindow.inventory_list.Items.RemoveAt(index);
                }
            }
            ItemsCount.CountManager();
        }

        public static void Inventory_Update1(Packet packet)
        {
            Packet vienas = packet;
            int check = packet.data.ReadBYTE();
            if (check == 1)
            {
                int typ = packet.data.ReadBYTE();
                if (typ == 0) // Inventory <> Inventory
                {
                    byte inv_1 = packet.data.ReadBYTE();
                    byte inv_2 = packet.data.ReadBYTE();
                    ushort count = packet.data.ReadWORD();
                    int index_1 = Char_Data.inventoryslot.IndexOf(inv_1);
                    int index_2 = Char_Data.inventoryslot.IndexOf(inv_2);
                    if (index_2 == -1)
                    {
                        // No item, Moving !
                        Char_Data.inventoryslot[index_1] = inv_2;
                    }
                    else
                    {
                        //The item exist !
                        if (Char_Data.inventorytype[index_1] == Char_Data.inventorytype[index_2])
                        {
                            // Items Are Same, Merge It !
                            if (Char_Data.inventorycount[index_2] == Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Char_Data.inventorytype[index_2])])
                            {
                                // Items Are Maxed, Move It !
                                Char_Data.inventoryslot[index_1] = inv_2;
                                Char_Data.inventoryslot[index_2] = inv_1;
                            }
                            else
                            {
                                // Items Are Same, Merge It !
                                if (Char_Data.inventorycount[index_1] == count)
                                {
                                    // Merged Everything, Delete The First Item !
                                    Char_Data.inventorycount[index_2] += count;

                                    Char_Data.inventoryid.RemoveAt(index_1);
                                    Char_Data.inventorytype.RemoveAt(index_1);
                                    Char_Data.inventorycount.RemoveAt(index_1);
                                    Char_Data.inventoryslot.RemoveAt(index_1);
                                    Char_Data.inventorydurability.RemoveAt(index_1);
                                }
                                else
                                {
                                    // Merged Not Everything, Recalculate Quantity !
                                    Char_Data.inventorycount[index_2] += count;
                                    Char_Data.inventorycount[index_1] -= count;
                                }
                            }
                        }
                        else
                        {
                            // Items Are Different, Move It !
                            Char_Data.inventoryslot[index_1] = inv_2;
                            Char_Data.inventoryslot[index_2] = inv_1;
                        }
                    }
                    Globals.MainWindow.inventory_list.Items.Clear();
                    for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                    {
                        uint id = Char_Data.inventoryid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.inventory_list.Items.Add(name);
                    }
                    if (Buffas.changing_weapon)
                    {
                        System.Threading.Thread thread = new System.Threading.Thread(LogicControl.Manager);
                        thread.Start();
                    }
                    if (BotData.loopaction == "merge")
                    {
                        MergeItems();
                    }
                }
                if (typ == 1) // Storage -> Storage
                {
                    byte str_1 = packet.data.ReadBYTE();
                    byte str_2 = packet.data.ReadBYTE();
                    ushort count = packet.data.ReadWORD();
                    int index_1 = Char_Data.storageslot.IndexOf(str_1);
                    int index_2 = Char_Data.storageslot.IndexOf(str_2);
                    if (index_2 == -1)
                    {
                        // No item, Moving !
                        Char_Data.storageslot[index_1] = str_2;
                    }
                    else
                    {
                        //The item exist !
                        if (Char_Data.storagetype[index_1] == Char_Data.storagetype[index_2])
                        {
                            // Items Are Same, Merge It !
                            if (Char_Data.storagecount[index_2] == Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Char_Data.storagetype[index_2])])
                            {
                                // Items Are Maxed, Move It !
                                Char_Data.storageslot[index_1] = str_2;
                                Char_Data.storageslot[index_2] = str_1;
                            }
                            else
                            {
                                // Items Are Same, Merge It !
                                if (Char_Data.storagecount[index_1] == count)
                                {
                                    // Merged Everything, Delete The First Item !
                                    Char_Data.storagecount[index_2] += count;
                                    string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Char_Data.storagetype[index_1])];
                                    Globals.MainWindow.storage_list.Items.Remove(name);

                                    Char_Data.storageid.RemoveAt(index_1);
                                    Char_Data.storagetype.RemoveAt(index_1);
                                    Char_Data.storagecount.RemoveAt(index_1);
                                    Char_Data.storageslot.RemoveAt(index_1);
                                    Char_Data.storagedurability.RemoveAt(index_1);
                                }
                                else
                                {
                                    // Merged Not Everything, Recalculate Quantity !
                                    Char_Data.storagecount[index_2] += count;
                                    Char_Data.storagecount[index_1] -= count;
                                }
                            }
                        }
                        else
                        {
                            // Items Are Different, Move It !
                            Char_Data.storageslot[index_1] = str_2;
                            Char_Data.storageslot[index_2] = str_1;
                        }
                    }
                }
                if (typ == 2) // From INV to BANK
                {
                    byte slot_inv = packet.data.ReadBYTE();
                    byte slot_bnk = packet.data.ReadBYTE();
                    int index = Char_Data.inventoryslot.IndexOf(slot_inv);
                    Char_Data.storageid.Add(Char_Data.inventoryid[index]);
                    Char_Data.storagetype.Add(Char_Data.inventorytype[index]);
                    Char_Data.storageslot.Add(slot_bnk);
                    Char_Data.storagecount.Add(Char_Data.inventorycount[index]);
                    Char_Data.storagedurability.Add(Char_Data.inventorydurability[index]);

                    Char_Data.inventoryid.RemoveAt(index);
                    Char_Data.inventorytype.RemoveAt(index);
                    Char_Data.inventoryslot.RemoveAt(index);
                    Char_Data.inventorycount.RemoveAt(index);
                    Char_Data.inventorydurability.RemoveAt(index);

                    Globals.MainWindow.inventory_list.Items.Clear();
                    for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                    {
                        uint id = Char_Data.inventoryid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.inventory_list.Items.Add(name);
                    }
                    Globals.MainWindow.storage_list.Items.Clear();
                    for (int i = 0; i < Char_Data.storageid.Count; i++)
                    {
                        uint id = Char_Data.storageid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.storage_list.Items.Add(name);
                    }
                    ItemsCount.CountManager();
                    StorageControl.StorageManager(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                }
                if (typ == 3) // From BANK to INV
                {
                    byte slot_bnk = packet.data.ReadBYTE();
                    byte slot_inv = packet.data.ReadBYTE();
                    int index = Char_Data.storageslot.IndexOf(slot_bnk);

                    Char_Data.inventoryid.Add(Char_Data.storageid[index]);
                    Char_Data.inventorytype.Add(Char_Data.storagetype[index]);
                    Char_Data.inventoryslot.Add(slot_inv);
                    Char_Data.inventorycount.Add(Char_Data.storagecount[index]);
                    Char_Data.inventorydurability.Add(Char_Data.storagedurability[index]);

                    Char_Data.storageid.RemoveAt(index);
                    Char_Data.storagetype.RemoveAt(index);
                    Char_Data.storageslot.RemoveAt(index);
                    Char_Data.storagecount.RemoveAt(index);
                    Char_Data.storagedurability.RemoveAt(index);

                    Globals.MainWindow.inventory_list.Items.Clear();
                    for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                    {
                        uint id = Char_Data.inventoryid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.inventory_list.Items.Add(name);
                    }
                    Globals.MainWindow.storage_list.Items.Clear();
                    for (int i = 0; i < Char_Data.storageid.Count; i++)
                    {
                        uint id = Char_Data.storageid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.storage_list.Items.Add(name);
                    }
                    ItemsCount.CountManager();
                }
                if (typ == 6) // PICKED ITEM
                {
                    byte slot = packet.data.ReadBYTE();
                    if (slot == 254)
                    {
                        packet.data.ReadDWORD();
                    }
                    else
                    {
                        packet.data.ReadDWORD();
                        uint item_id = packet.data.ReadDWORD();
                        int index = Items_Info.itemsidlist.IndexOf(item_id);
                        string type = Items_Info.itemstypelist[index];
                        if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU"))
                        {
                            byte item_plus = packet.data.ReadBYTE();
                            packet.data.ReadQWORD();
                            uint durability = packet.data.ReadDWORD();
                            byte blueamm = packet.data.ReadBYTE();
                            for (int i = 0; i < blueamm; i++)
                            {
                                packet.data.ReadBYTE();
                                packet.data.ReadWORD();
                                packet.data.ReadDWORD();
                                packet.data.ReadBYTE();
                            }
                            Char_Data.inventoryid.Add(item_id);
                            Char_Data.inventorytype.Add(type);
                            Char_Data.inventoryslot.Add(slot);
                            Char_Data.inventorycount.Add(1);
                            Char_Data.inventorydurability.Add(durability);
                            Globals.MainWindow.inventory_list.Items.Add(Items_Info.itemsnamelist[index]);

                        }
                        else
                        {
                            ushort count = packet.data.ReadWORD();
                            int indexas = Char_Data.inventoryslot.IndexOf(slot);
                            if (indexas != -1)
                            {
                                Char_Data.inventorycount[indexas] = count;
                            }
                            else
                            {
                                Char_Data.inventoryid.Add(item_id);
                                Char_Data.inventorytype.Add(type);
                                Char_Data.inventoryslot.Add(slot);
                                Char_Data.inventorydurability.Add(0);
                                Char_Data.inventorycount.Add(count);
                                Globals.MainWindow.inventory_list.Items.Add(Items_Info.itemsnamelist[index]);
                            }
                            ItemsCount.CountManager();
                        }
                    }
                }
                if (typ == 7)
                {
                    byte slot = packet.data.ReadBYTE();
                    int index = Char_Data.inventoryslot.IndexOf(slot);
                    Char_Data.inventoryid.RemoveAt(index);
                    Char_Data.inventorytype.RemoveAt(index);
                    Char_Data.inventoryslot.RemoveAt(index);
                    Char_Data.inventorycount.RemoveAt(index);
                    Char_Data.inventorydurability.RemoveAt(index);
                    Globals.MainWindow.inventory_list.Items.RemoveAt(index);
                    ItemsCount.CountManager();
                }
                if (typ == 8)
                {
                    byte tab = packet.data.ReadBYTE();
                    byte slot = packet.data.ReadBYTE();
                    byte count = packet.data.ReadBYTE();
                    #region Finding Item Info
                    uint item_id = 0;
                    for (int i = 0; i < Data.ShopTabData.Length; i++)
                    {
                        if (Data.ShopTabData[i].StoreName.Replace("STORE_", "NPC_") == BotData.selectednpctype)
                        {
                            item_id = Items_Info.itemsidlist[Items_Info.itemstypelist.IndexOf(Data.ShopTabData[i].Tab[tab].ItemType[slot])];
                            break;
                        }
                    }
                    string item_type = Items_Info.itemstypelist[Items_Info.itemsidlist.IndexOf(item_id)];
                    #endregion
                    if (count == 1)
                    {
                        byte inv_slot = packet.data.ReadBYTE();
                        ushort inv_count = packet.data.ReadWORD();
                        Packet du = new Packet((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT, false, enumDestination.Client);
                        du.data.AddBYTE(0x01);
                        du.data.AddBYTE(0x06);
                        du.data.AddBYTE(inv_slot);
                        du.data.AddDWORD(0x00000000);
                        du.data.AddDWORD(item_id);
                        if (item_type.StartsWith("ITEM_CH") == false && item_type.StartsWith("ITEM_EU") == false)
                        {
                            du.data.AddWORD(inv_count);
                        }
                        else
                        {
                            du.data.AddBYTE(0x00);
                            du.data.AddQWORD(0x0000000000000000);
                            du.data.AddDWORD(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                            du.data.AddBYTE(0x00);
                            du.data.AddWORD(1);
                            du.data.AddWORD(2);
                        }
                        Globals.ClientPC.SendPacket(du);
                        Char_Data.inventoryid.Add(item_id);
                        Char_Data.inventorytype.Add(item_type);
                        Char_Data.inventoryslot.Add(inv_slot);
                        Char_Data.inventorycount.Add(inv_count);
                        Char_Data.inventorydurability.Add(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            byte inv_slot = packet.data.ReadBYTE();
                            Packet du = new Packet((ushort)WorldServerOpcodes.SERVER_OPCODES.SERVER_INVENTORYMOVEMENT, false, enumDestination.Client);
                            du.data.AddBYTE(0x01);
                            du.data.AddBYTE(0x06);
                            du.data.AddBYTE(inv_slot);
                            du.data.AddDWORD(0x00000000);
                            du.data.AddDWORD(item_id);
                            if (item_type.StartsWith("ITEM_CH") == false && item_type.StartsWith("ITEM_EU") == false)
                            {
                                du.data.AddWORD(1);
                            }
                            else
                            {
                                du.data.AddBYTE(0x00);
                                du.data.AddQWORD(0x0000000000000000);
                                du.data.AddDWORD(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                                du.data.AddBYTE(0x00);
                                du.data.AddWORD(1);
                                du.data.AddWORD(2);
                            }
                            Globals.ClientPC.SendPacket(du);
                            Char_Data.inventoryid.Add(item_id);
                            Char_Data.inventorytype.Add(item_type);
                            Char_Data.inventoryslot.Add(inv_slot);
                            Char_Data.inventorycount.Add(1);
                            Char_Data.inventorydurability.Add(Items_Info.itemsdurabilitylist[Items_Info.itemsidlist.IndexOf(item_id)]);
                        }
                    }
                    Globals.MainWindow.inventory_list.Items.Clear();
                    for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                    {
                        uint id = Char_Data.inventoryid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.inventory_list.Items.Add(name);
                    }
                    ItemsCount.CountManager();
                    BuyControl.BuyManager(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                }
                if (typ == 9) // Inventory -> Shop
                {
                    byte inv_slot = packet.data.ReadBYTE();
                    ushort count = packet.data.ReadWORD();

                    int index = Char_Data.inventoryslot.IndexOf(inv_slot);
                    ushort real_count = Char_Data.inventorycount[index];

                    if (count == real_count)
                    {
                        //Sold Everything - Delete Item
                        Char_Data.inventoryid.RemoveAt(index);
                        Char_Data.inventorytype.RemoveAt(index);
                        Char_Data.inventoryslot.RemoveAt(index);
                        Char_Data.inventorycount.RemoveAt(index);
                        Char_Data.inventorydurability.RemoveAt(index);
                    }
                    else
                    {
                        //Reduce count of item
                        ushort new_count = (ushort)(real_count - count);
                        Char_Data.inventorycount[index] = new_count;
                    }
                    Globals.MainWindow.inventory_list.Items.Clear();
                    for (int i = 0; i < Char_Data.inventoryid.Count; i++)
                    {
                        uint id = Char_Data.inventoryid[i];
                        string name = Items_Info.itemsnamelist[Items_Info.itemsidlist.IndexOf(id)];
                        Globals.MainWindow.inventory_list.Items.Add(name);
                    }
                    ItemsCount.CountManager();
                    SellControl.SellManager(Spawns.npcid[Spawns.npctype.IndexOf(BotData.selectednpctype)]);
                }
                if (typ == 16) // From Pet Inventory To Pet Inventory
                {
                    uint petid = packet.data.ReadDWORD();
                    byte pet_1 = packet.data.ReadBYTE();
                    byte pet_2 = packet.data.ReadBYTE();
                    ushort count = packet.data.ReadWORD();

                    for (int i = 0; i < Char_Data.pets.Length; i++)
                    {
                        if (Char_Data.pets[i].id == petid)
                        {
                            if (Char_Data.pets[i].inventory[pet_1].type == Char_Data.pets[i].inventory[pet_2].type)
                            {
                                // Items Are Same, Merge It !
                                if (Char_Data.pets[i].inventory[pet_2].count == Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Char_Data.pets[i].inventory[pet_2].type)])
                                {
                                    // Items Are Maxed, Move It !
                                    Char_Data.Pet_.Inventory_ inv_temp = Char_Data.pets[i].inventory[pet_1];
                                    Char_Data.pets[i].inventory[pet_1] = Char_Data.pets[i].inventory[pet_2];
                                    Char_Data.pets[i].inventory[pet_2] = inv_temp;
                                }
                                else
                                {
                                    if (Char_Data.pets[i].inventory[pet_1].count == count)
                                    {
                                        // Merged Everything, Delete The First Item !
                                        Char_Data.pets[i].inventory[pet_2].count += count;
                                        string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Char_Data.pets[i].inventory[pet_1].type)];
                                        Globals.MainWindow.pet_inv_list.Items.Remove(name);
                                        Char_Data.pets[i].inventory[pet_1] = new Char_Data.Pet_.Inventory_();
                                    }
                                    else
                                    {
                                        // Merged Not Everything, Recalculate Quantity !
                                        Char_Data.pets[i].inventory[pet_2].count += count;
                                        Char_Data.pets[i].inventory[pet_1].count -= count;
                                    }
                                }
                            }
                            else
                            {
                                // Items Are Different, Move It !
                                Char_Data.Pet_.Inventory_ inv_temp = Char_Data.pets[i].inventory[pet_1];
                                Char_Data.pets[i].inventory[pet_1] = Char_Data.pets[i].inventory[pet_2];
                                Char_Data.pets[i].inventory[pet_2] = inv_temp;
                            }
                            break;
                        }
                    }
                }
                if (typ == 17) // PET PICKED ITEM
                {
                    uint pet_id21 = packet.data.ReadDWORD();
                    for (int i = 0; i < Char_Data.pets.Length; i++)
                    {
                        if (pet_id21 == Char_Data.pets[i].id)
                        {
                            byte slot = packet.data.ReadBYTE();
                            packet.data.ReadDWORD();
                            uint item_id = packet.data.ReadDWORD();
                            int index = Items_Info.itemsidlist.IndexOf(item_id);
                            string type = Items_Info.itemstypelist[index];
                            if (type.StartsWith("ITEM_CH") || type.StartsWith("ITEM_EU"))
                            {
                                byte item_plus = packet.data.ReadBYTE();
                                packet.data.ReadQWORD();
                                uint durability = packet.data.ReadDWORD();
                                byte blueamm = packet.data.ReadBYTE();
                                for (int g = 0; g < blueamm; g++)
                                {
                                    packet.data.ReadBYTE();
                                    packet.data.ReadWORD();
                                    packet.data.ReadDWORD();
                                    packet.data.ReadBYTE();
                                }
                                packet.data.ReadBYTE(); //Socket Stone 1
                                byte socketst = packet.data.ReadBYTE(); //Socket Stones Quanty
                                for (int e = 0; e < socketst; e++)
                                {
                                    packet.data.ReadBYTE();  // Socket Slot
                                    packet.data.ReadQWORD(); // Socket Code Stone
                                }
                                packet.data.ReadBYTE(); //ADV 2
                                byte flag1 = packet.data.ReadBYTE(); // Flag ?
                                if (flag1 == 1) // ADV Elixir
                                {
                                    packet.data.ReadBYTE(); //Unknown
                                    packet.data.ReadDWORD(); // ADV Elixir ID 
                                    packet.data.ReadDWORD(); // Plus Count
                                }

                                packet.data.ReadBYTE(); //Unknown 3
                                byte Naosei = packet.data.ReadBYTE(); //Unknown
                                for (int f = 0; f < Naosei; f++)
                                {
                                    packet.data.ReadBYTE();  //Unknown
                                    packet.data.ReadQWORD(); //Unknown
                                }
                                packet.data.ReadBYTE(); //Unknown 4
                                packet.data.ReadBYTE(); //Unknown
                                Char_Data.pets[i].inventory[slot].id = item_id;
                                Char_Data.pets[i].inventory[slot].type = type;
                                Char_Data.pets[i].inventory[slot].slot = slot;
                                Char_Data.pets[i].inventory[slot].durability = durability;
                                Char_Data.pets[i].inventory[slot].count = 1;
                                Globals.MainWindow.pet_inv_list.Items.Add(Items_Info.itemsnamelist[index]);
                            }
                            else
                            {
                                ushort count = packet.data.ReadWORD();
                                Char_Data.pets[i].inventory[slot].id = item_id;
                                Char_Data.pets[i].inventory[slot].type = type;
                                Char_Data.pets[i].inventory[slot].slot = slot;
                                Char_Data.pets[i].inventory[slot].durability = 0;
                                Char_Data.pets[i].inventory[slot].count = count;
                                Globals.MainWindow.pet_inv_list.Items.Add(Items_Info.itemsnamelist[index]);
                            }
                        }
                    }
                }
                if (typ == 26) // From Pet Inventory To Inventory
                {
                    uint pet_id = packet.data.ReadDWORD();
                    for (int i = 0; i < Char_Data.pets.Length; i++)
                    {
                        if (pet_id == Char_Data.pets[i].id)
                        {
                            byte pet_slot = packet.data.ReadBYTE();
                            byte inv_slot = packet.data.ReadBYTE();
                            Char_Data.inventoryid.Add(Char_Data.pets[i].inventory[pet_slot].id);
                            Char_Data.inventorytype.Add(Char_Data.pets[i].inventory[pet_slot].type);
                            Char_Data.inventoryslot.Add(inv_slot);
                            Char_Data.inventorydurability.Add(Char_Data.pets[i].inventory[pet_slot].durability);
                            Char_Data.inventorycount.Add(Char_Data.pets[i].inventory[pet_slot].count);
                            string name = Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Char_Data.pets[i].inventory[pet_slot].type)];
                            Globals.MainWindow.inventory_list.Items.Add(name);
                            Globals.MainWindow.pet_inv_list.Items.Remove(name);
                            Char_Data.pets[i].inventory[pet_slot] = new Char_Data.Pet_.Inventory_();
                            break;
                        }
                    }
                }
               
                if (typ == 27) // From Inventory To Pet Inventory
                {
                    uint pet_id = packet.data.ReadDWORD();
                    for (int i = 0; i < Char_Data.pets.Length; i++)
                    {
                        if (pet_id == Char_Data.pets[i].id)
                        {
                            byte inv_slot = packet.data.ReadBYTE();
                            byte pet_slot = packet.data.ReadBYTE();
                            int inv_index = Char_Data.inventoryslot.IndexOf(inv_slot);
                            Char_Data.pets[i].inventory[pet_slot].id = Char_Data.inventoryid[inv_index];
                            Char_Data.pets[i].inventory[pet_slot].type = Char_Data.inventorytype[inv_index];
                            Char_Data.pets[i].inventory[pet_slot].slot = pet_slot;
                            Char_Data.pets[i].inventory[pet_slot].durability = Char_Data.inventorydurability[inv_index];
                            Char_Data.pets[i].inventory[pet_slot].count = Char_Data.inventorycount[inv_index];
                            Globals.MainWindow.pet_inv_list.Items.Add(Items_Info.itemsnamelist[Items_Info.itemstypelist.IndexOf(Char_Data.inventorytype[inv_index])]);
                            Char_Data.inventoryid.RemoveAt(inv_index);
                            Char_Data.inventorytype.RemoveAt(inv_index);
                            Char_Data.inventorycount.RemoveAt(inv_index);
                            Char_Data.inventorydurability.RemoveAt(inv_index);
                            Char_Data.inventoryslot.RemoveAt(inv_index);
                            Globals.MainWindow.inventory_list.Items.RemoveAt(inv_index);
                            break;
                        }
                    }
                }
                if (typ != 8)
                {
                    Globals.ClientPC.SendPacket(vienas);
                }
            }
            if (check == 2)
            {
                byte check1 = packet.data.ReadBYTE();
                switch (check1)
                {
                    case 0x03:
                        //Unknown
                        break;
                    case 0x02:
                        Globals.ClientPC.SendPacket(packet);
                        break;
                }


            }
        }

        public static void ItemFixed(Packet packet)
        {
            if (packet.data.ReadBYTE() == 1)
            {
                if (BotData.loop && BotData.bot)
                {
                    BotData.selectedid = 0;
                    BotData.selected = 0;
                    System.Threading.Thread.Sleep(1000);
                    InventoryControl.MergeItems();
                }
            }
        }


        public static List<string> mergetypewaiting = new List<string>();
        public static List<byte> mergeslotwaiting = new List<byte>();
        public static void MergeItems()
        {
            mergetypewaiting.Clear();
            mergeslotwaiting.Clear();
            BotData.loopaction = "merge";
            for (int i = 0; i < Char_Data.inventoryid.Count; i++)
            {
                System.Threading.Thread.Sleep(5);
                if (!Char_Data.inventorytype[i].StartsWith("ITEM_CH") && !Char_Data.inventorytype[i].StartsWith("ITEM_EU") && Char_Data.inventoryslot[i] >= 13)
                {
                    if(Char_Data.inventorycount[i] < Items_Info.items_maxlist[Items_Info.itemstypelist.IndexOf(Char_Data.inventorytype[i])])
                    {
                        if (mergetypewaiting.IndexOf(Char_Data.inventorytype[i]) != -1)
                        {
                            //There are another not merged same type item
                            //Merge IT
                            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
                            packet.data.AddBYTE(0x00);
                            packet.data.AddBYTE(Char_Data.inventoryslot[i]);
                            packet.data.AddBYTE(mergeslotwaiting[mergetypewaiting.IndexOf(Char_Data.inventorytype[i])]);
                            packet.data.AddBYTE((byte)Char_Data.inventoryslot[i]); // Count
                            packet.data.AddWORD(0x0000);
                            Globals.ServerPC.SendPacket(packet);
                            break;

                        }
                        else
                        {
                            mergetypewaiting.Add(Char_Data.inventorytype[i]);
                            mergeslotwaiting.Add(Char_Data.inventoryslot[i]);
                        }
                    }
                }
                if (i + 1 >= Char_Data.inventoryid.Count)
                {
                    LoopControl.WalkScript();
                }
            }
        }

    }
}