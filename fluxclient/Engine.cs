using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux.Client
{
    public class Engine
    {
        bool isLoggedIn = false;
        ushort FluxID = 0;
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
            AuthRequest authRequest = new AuthRequest();
            authRequest.Username = username;
            authRequest.Password = password;
            GASClient.GAS.SendPacket(authRequest.Write(), pID);
            GASClient.GAS.Recieve(pID);
            if (!GASClient.GAS.RecievedPacketsContains(pID))
                return false;
            IPacket ipacket = BasePacket.Read(GASClient.GAS.RecievedPacketsGet(pID));
            if (ipacket.GetPacketType() != PacketTypeEnum.LoginResponse)
                return false;
            AuthResponse authResponse = (AuthResponse)ipacket;
            if (authResponse.ResponseType == LoginResponseTypeEnum.LoginValidated)
            {
                this.isLoggedIn = true;
                this.FluxID = authResponse.FluxID;
                return true;
            }
            return false;
        }
    }
}
