using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Client
{
    public class Engine
    {
        bool isLoggedIn = false;
        public void StartFluxEngine() 
        {
            GASClient.Connect("gas.fluxhandled.net", 29301);
            GASClient.GAS.PacketRecieved += new GASClient.PacketRecievedHandler(GAS_PacketRecieved);
            LocalTCP.Start();
        }

        void GAS_PacketRecieved(uint RequestID)
        {
            byte[] rawPacket = GASClient.GAS.RecievedPacketsGet(RequestID);
            IPacket packet = BasePacket.Read(rawPacket);
            switch (packet.GetPacketType())
            {
                case PacketTypeEnum.LoginResponse:

                    break;
                default:
                    break;
            }
        }

        public bool Login(string username, string password)
        {
            if (isLoggedIn)
                return false;
            uint pID = IntGen.GetNewGUID();
            AuthPacket authRequest = new AuthPacket();
            authRequest.Username = username;
            authRequest.Password = password;
            GASClient.GAS.SendPacket(authRequest.Write(), pID);
            GASClient.GAS.Recieve(pID);
            if (!GASClient.GAS.RecievedPacketsContains(pID))
                return false;
        }
    }
}
