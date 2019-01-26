using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography; 


namespace Silkroad
{
    /// <summary>
    /// Thanks to keinplain
    /// </summary>
    class PacketCreator
    {
        // Events
        public delegate void eNewDataToSendEventHandler(byte[] Packet);
        public event eNewDataToSendEventHandler eNewDataToSend;

        byte[] buffer = new byte[4096];
        int bufferSize;

        enumDestination destination;
        BlowfishECB Blowfish;
        SecurityBytesGenerator SecurityByteGen;

        public PacketCreator(enumDestination destination)
        {
            this.destination = destination;
        }

        public enumDestination Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public void Reset()
        {
            bufferSize = 0;
        }

        public void InitCounterByteCRCbyte(int CounterSeed, int CrcSeed)
        {
            SecurityByteGen = new SecurityBytesGenerator(CounterSeed, CrcSeed);
        }

        public void InitBlowfish(byte[] key)
        {
            Blowfish = new BlowfishECB(key, 0, 8);
        }

        public void CreatePacket(byte[] data)
        {
            // Add buffer to data
            if (bufferSize != 0)
            { 
                Array.Copy(data, 0, buffer, bufferSize, data.Length);
                Array.Resize<byte>(ref data, data.Length + bufferSize);//not sure if -1
                Array.Copy(buffer, data, data.Length);
                bufferSize = 0;
            }
            int pointer = 0;

            // Read data till end
            while (data.Length != pointer)
            {
                bool crypted = false;
                int lenoffset = 0;

                // If packet header is incomplete -> save data and exit
                if (data.Length < pointer + 6)
                {
                    bufferSize = data.Length - pointer;
                    Array.Copy(data, pointer, buffer, 0, bufferSize);
                    break;
                }
                // Read length
                ushort len = BitConverter.ToUInt16(data, pointer);
                pointer += 2;

                // Fin length if crypted
                if ((len >> 15) == 1) // Check if crypted
                {
                    crypted = true; // Mark it as crypted
                    len = (ushort)(len & 0x7FFF); // Remove HI Bit
                    // Check if we need lenoffset to fit blowfish requiment(divisible by 8)
                    lenoffset = (len + 4) % 8;
                    if (lenoffset != 0) lenoffset = 8 - lenoffset;
                    
                }

                // Check if packet data are incomplete else read it
                if (data.Length < pointer + len + 4 + lenoffset)
                {
                    // Incomplete packet -> save data and exit
                    bufferSize = data.Length - pointer + 2;
                    Array.Copy(data, pointer - 2, buffer, 0, bufferSize);
                    break;
                }
                else // Packet complete -> read it
                {
                    // Decrypt if required
                    if (crypted)
                    {
                        Blowfish.DecryptRev(data, pointer, data, pointer, len + 4 + lenoffset);
                    }

                    // Read the data
                    byte[] tempArray = new byte[len];
                    ushort opc = BitConverter.ToUInt16(data, pointer);

                    Array.ConstrainedCopy(data, pointer + 4, tempArray, 0, len);
                    pointer += len + 4 + lenoffset;

                    // Create packet
                    Packet packet = new Packet(opc, crypted, destination);
                    packet.data.AddBYTE(tempArray);

                    switch (Destination)
                    {
                        case enumDestination.Client:
                            Globals.UpdatePacketPC("[C->P]" + packet.Opc.ToString("X2") + " " + packet.data.len);
                            Globals.ClientParser.Handler(packet);
                            break;
                        case enumDestination.Server:
                            //Globals.LogData(packet, 1);
                            Globals.UpdatePacketPC("[S->P]" + packet.Opc.ToString("X2") + " " + packet.data.len);
                            Globals.Parser.Handler(packet);
                            break;
                    }
                }
            }
        }

        public void SendPacket(Packet packet)
        {
            Control(packet);
            switch (Destination)
            {
                case enumDestination.Server:
                 //   Globals.LogData(packet, 2);
                    Globals.UpdatePacketCP("[P->S]" + packet.Opc.ToString("X2") + " " + packet.data.len);
                    break;
                case enumDestination.Client:
                    Globals.UpdatePacketPC("[P->C]" + packet.Opc.ToString("X2") + " " + packet.data.len);
                    break;
            }
            // Get the packet as byte[]
            byte[] bytePacket = packet.ToByteArray();            

            // Add Security Bytes to the byte data
            if (SecurityByteGen != null)
                SecurityByteGen.AddSecurity(bytePacket);

            // Encrypt the bytes
            if (packet.Crypted)
            {
                int lenoffset = (packet.data.len + 4) % 8;
                if (lenoffset != 0)
                {
                    lenoffset = 8 - lenoffset;
                    Array.Resize<byte>(ref bytePacket, bytePacket.Length + lenoffset);
                }

                // Finally encrypt the byte[]
                Blowfish.EncryptRev(bytePacket, 2, bytePacket, 2, bytePacket.Length);
            }

            eNewDataToSend(bytePacket);
        }

        public static void Control(Packet packet)
        {
            switch (packet.Dest)
            {
                case enumDestination.Server:
                    if (packet.Opc == (ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN)
                    {
                        Globals.MainWindow.login.Enabled = false;
                        Globals.MainWindow.username.Enabled = false;
                        Globals.MainWindow.password.Enabled = false;
                        Globals.MainWindow.in_game_server_name.Enabled = false;
                    }
                    break;
                case enumDestination.Client:
                    break;
            }
        }
    }
}
