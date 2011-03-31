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
            Log.Initialize("client.log", LogLevel.All, true);
            GASClient.Connect("localhost", 29301);
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

        public LoginCompletedEventArgs Login(string username, string password)
        {
            if (isLoggedIn)
                return new LoginCompletedEventArgs("Already logged in");
            uint pID = IntGen.GetNewGUID();
            AuthRequest authRequest = new AuthRequest();
            authRequest.Username = username;
            authRequest.Password = password;
            GASClient.GAS.SendPacket(authRequest.Write(), pID);
            GASClient.GAS.Recieve(pID);
            if (!GASClient.GAS.RecievedPacketsContains(pID))
                return new LoginCompletedEventArgs("Server timed out");
            IPacket ipacket = BasePacket.Read(GASClient.GAS.RecievedPacketsGet(pID));
            if (ipacket.GetPacketType() != PacketTypeEnum.LoginResponse)
                return new LoginCompletedEventArgs("Incorrect server response");
            AuthResponse authResponse = (AuthResponse)ipacket;
            Log.Debug(authResponse.ResponseType.ToString());
            if (authResponse.ResponseType == LoginResponseTypeEnum.LoginValidated)
            {
                this.isLoggedIn = true;
                this.FluxID = authResponse.FluxID;
                return new LoginCompletedEventArgs(true);
            }
            if (authResponse.ResponseType == LoginResponseTypeEnum.LoginInvalid)
                return new LoginCompletedEventArgs("Your login doesn't seem to be valid.");
            if (authResponse.ResponseType == LoginResponseTypeEnum.ServerNA)
                return new LoginCompletedEventArgs("The server is set to maintence mode.");
            if (authResponse.ResponseType == LoginResponseTypeEnum.UserBanned)
                return new LoginCompletedEventArgs("You are banned.");
            return new LoginCompletedEventArgs("Unknown Error"); //Why is this even called, there is no other possible enum option
        }
    }
}
