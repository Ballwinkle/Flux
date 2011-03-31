using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace Flux.Server
{
    public class GAS
    {
        public static List<GAS> Users = new List<GAS>();

        public static void StartServer(int port)
        {
            Log.Info("Starting GAS...");
            new Thread(InternalIO).Start(port);
        }

        private static void InternalIO(object o)
        {
            int port = (int)o;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(IPAddress.Any, port));
            s.Listen(10);
            while (true)
            {
                try
                {
                    new GAS(s.Accept());
                }
                catch
                {

                }
            }
        }

        public uint FluxID;

        public DateTime LastTouched;

        Socket client;

        public GAS(Socket s)
        {
            Log.Info("User connected from " + ((IPEndPoint)s.RemoteEndPoint).Address.ToString());
            Users.Add(this);
            client = s;
            LastTouched = DateTime.Now;
            new Thread(RecieveLoop).Start();
        }

        public void Disconnect()
        {
            client.Disconnect(false);
        }

        private void SendPacket(byte[] buffer, uint RequestID)
        {
            try
            {
                MemoryStream memStr = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memStr);
                writer.Write(RequestID);
                writer.Write(buffer);
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
                    Log.Debug("Starting recieve...");
                    client.Receive(buffer);
                    LastTouched = DateTime.Now;
                    MemoryStream memstr = new MemoryStream(buffer);
                    BinaryReader reader = new BinaryReader(memstr);
                    uint RequestID = reader.ReadUInt32();
                    byte[] packet = reader.ReadBytes((int)(memstr.Length - 4));
                    byte[] response = HandlePacket(packet);
                    SendPacket(response, RequestID);
                }
                catch (Exception e)
                {
                    if(client.Connected)
                    Log.Debug("Error recieving: " + e.Message);
                }
            }
        }

        private byte[] HandlePacket(byte[] packet)
        {
            try
            {
                IPacket ipacket = BasePacket.Read(packet);
                switch (ipacket.GetPacketType())
                {
                    case PacketTypeEnum.LoginRequest:
                        Log.Debug("Handling LoginRequest...");
                        AuthResponse authResponse = new AuthResponse();
                        AuthRequest authRequest = (AuthRequest)ipacket;
                        UserInfo userInfo = DB.Current.AuthorizeUser(authRequest.Username, authRequest.Password);
                        authResponse.ResponseType = userInfo.LoginResponse;
                        if (userInfo.LoginResponse == LoginResponseTypeEnum.LoginValidated)
                        {
                            this.FluxID = userInfo.FluxID;
                            authResponse.FluxID = userInfo.FluxID;
                        }
                        return authResponse.Write();

                    case PacketTypeEnum.LoginResponse:
                        Log.Debug("Recieved LoginResponse - are you sure we want to login somewhere?");
                        break;

                    default:
                        Log.Debug("Unhandled PacketType: " + ipacket.GetPacketType().ToString() + ". Ignoring.");
                        return null;
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Debug("Can't handle packet: " + e.Message);
                return null; 
            }
        }
    }
}
