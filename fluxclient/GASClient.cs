using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Flux.Client
{
    internal class GASClient
    {
        public static GASClient GAS;

        private Dictionary<uint, byte[]> RecievedPackets = new Dictionary<uint,byte[]>();
        private object RecievedPacketsLock = "";

        public delegate void PacketRecievedHandler(uint RequestID);
        public event PacketRecievedHandler PacketRecieved;

        public void RecievedPacketsAdd(uint id, byte[] value)
        {
            lock (RecievedPacketsLock)
            {
                try
                {
                    RecievedPackets.Add(id, value);
                }
                catch { }
            }
        }

        public bool RecievedPacketsContains(uint id)
        {
            lock (RecievedPacketsLock)
            {
                return RecievedPackets.ContainsKey(id);
            }
        }

        public byte[] RecievedPacketsGet(uint id)
        {
            lock (RecievedPacketsLock)
            {
                if (RecievedPackets.ContainsKey(id))
                    return RecievedPackets[id];
                else
                    return null;
            }
        }

        public static void Connect(string hostname, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(hostname, port);
            new GASClient(s);
        }

        Socket client;

        public GASClient(Socket s)
        {
            GAS = this;
            client = s;
            new Thread(RecieveLoop).Start();
        }

        public void SendPacket(byte[] buffer, uint RequestID)
        {
            try
            {
                MemoryStream memStr = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memStr);
                writer.Write(RequestID);
                writer.Write(buffer);
                Log.Debug("Message length is " + memStr.Length.ToString());
                client.Send(memStr.ToArray());
            }
            catch
            {

            }
        }

        private void RecieveLoop()
        {
            while (client.Connected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    client.Receive(buffer);
                    MemoryStream memstr = new MemoryStream(buffer);
                    BinaryReader reader = new BinaryReader(memstr);
                    uint RequestID = reader.ReadUInt32();
                    byte[] packet = reader.ReadBytes((int)(memstr.Length - 4));
                    RecievedPacketsAdd(RequestID, packet);
                    if (PacketRecieved != null)
                        PacketRecieved(RequestID);
                }
                catch
                {

                }
            }
        }

        public byte[] Recieve(uint RequestID)
        {
            int timeout = 0;
            while (!RecievedPacketsContains(RequestID))
            {
                timeout++;
                System.Threading.Thread.Sleep(1);
                if (timeout > 3000)
                    return null;
            }
            return RecievedPacketsGet(RequestID);
        }
    }
}
