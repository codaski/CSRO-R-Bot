using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Silkroad
{
    class Connection
    {

        public Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Socket ClientList;
        public Socket winSock;
        byte[] clientbuffer = new byte[4096];
        byte[] buffer = new byte[4096];

        public void ClientListen(string ip, int port)
        {
            ClientClose();
            IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
            winSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                winSock.Bind(localEP);
                winSock.Listen(1);
                winSock.BeginAccept(new AsyncCallback(AcceptClient), winSock);
                Globals.UpdateLogs("Waiting Client");
            }
            catch (SocketException a)
            {
            }
        }

        public void AcceptClient(IAsyncResult ar)
        {
            try
            {
                Globals.UpdateLogs("Client Connection Received" + BotData.LoginServer.ip + BotData.LoginServer.port);
                //Globals.MainWindow.start_client.Dispatcher.Invoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate { Globals.MainWindow.start_client.IsEnabled = false; return null; }), null);
                Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
                Socket winSock2 = winSock.EndAccept(ar);
                ClientList = winSock2;
                winSock2.BeginReceive(clientbuffer, 0, clientbuffer.Length, SocketFlags.None, new AsyncCallback(OnClientReceive), winSock2);
                BotData.use_client = true;
            }
            catch
            {
                // Globals.Connection.ClientListen("127.0.0.1", LoginServer.port_sro);
            }
        }

        public void OnClientReceive(IAsyncResult ar)
        {
            try
            {
                Socket tmp = (Socket)ar.AsyncState;
                int rcvdBytes = tmp.EndReceive(ar);
                if (rcvdBytes > 0)
                {
                    Array.Resize<byte>(ref clientbuffer, rcvdBytes);
                    Globals.ClientPC.CreatePacket(clientbuffer);
                    tmp.BeginReceive(clientbuffer, 0, clientbuffer.Length, SocketFlags.None, new AsyncCallback(OnClientReceive), tmp);
                }
            }
            catch
            {
                // Globals.Connection.ClientListen("127.0.0.1", LoginServer.port_sro);
            }
        }

        public void ClientSend(byte[] data)
        {
            try
            {
                ClientList.Send(data, data.Length, SocketFlags.None);
            }
            catch
            {
                // Globals.Connection.ClientListen("127.0.0.1", LoginServer.port_sro);
            }
        }

        public void Close()
        {
            if (client.Connected)
            {
                buffer = new byte[4096];
            }
        }

        public void ClientClose()
        {
            try
            {

                winSock.Close();
                winSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientbuffer = new byte[4096];
            }
            catch
            {
            }
        }

        public void Connect(string ip, int port)
        {
            try
            {
                Globals.UpdateLogs("Connecting To Silkroad Server");
                //Relogin.ingame = 0;
                //BotData.ping = 0;
                Close();
                IPEndPoint ipp = new IPEndPoint(IPAddress.Parse(ip), port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ipp);
                if (client.Connected)
                {
                    client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), client);
                }
            }
            catch (SocketException a)
            {
                BotData.ping = 0;
                for (int i = 0; i < BotData.Servers.Length; i++)
                {
                    if (BotData.Servers[i].name == Globals.MainWindow.server_name.Text)
                    {
                        BotData.LoginServer.ip = BotData.Servers[i].ip;
                        BotData.LoginServer.port = 15779;
                        BotData.LoginServer.locale = BotData.Servers[i].locale;
                        BotData.LoginServer.version = BotData.Servers[i].version;
                        break;
                    }
                }
                Globals.UpdateLogs("Cannot connect to server");
                System.Threading.Thread.Sleep(5000);
                Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
            }
        }
        public void Send(byte[] data)
        {
            try
            {
                    client.Send(data, 0,data.Length, SocketFlags.None);
            }
            catch (SocketException a)
            {
                BotData.ping = 0;
                for (int i = 0; i < BotData.Servers.Length; i++)
                {
                    if (BotData.Servers[i].name == Globals.MainWindow.server_name.Text)
                    {
                        BotData.LoginServer.ip = BotData.Servers[i].ip;
                        BotData.LoginServer.port = 15779;
                        BotData.LoginServer.locale = BotData.Servers[i].locale;
                        BotData.LoginServer.version = BotData.Servers[i].version;
                        break;
                    }
                }
                Globals.UpdateLogs("Disconnected From Server | Cannot Send Packet | " + a.Message + " | " + a.StackTrace);
                System.Threading.Thread.Sleep(5000);
                Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
            }
        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                Socket tmp1 = (Socket)ar.AsyncState;
                int rcvdBytes = tmp1.EndReceive(ar);
                if (rcvdBytes > 0)
                {
                    Array.Resize<byte>(ref buffer, rcvdBytes);
                    Globals.ServerPC.CreatePacket(buffer);
                    tmp1.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), tmp1);
                }
            }
            catch (SocketException a)
            {
                BotData.ping = 0;
                for (int i = 0; i < BotData.Servers.Length; i++)
                {
                    if (BotData.Servers[i].name == Globals.MainWindow.server_name.Text)
                    {
                        BotData.LoginServer.ip = BotData.Servers[i].ip;
                        BotData.LoginServer.port = 15779;
                        BotData.LoginServer.locale = BotData.Servers[i].locale;
                        BotData.LoginServer.version = BotData.Servers[i].version;
                        break;
                    }
                }
                Globals.UpdateLogs("Disconnected From Server | Cannot Receive packet | " + a.Message + " | " + a.StackTrace);
                System.Threading.Thread.Sleep(5000);
                Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
            }
        }
    }
}