using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Flux
{
    public enum PacketTypeEnum : ushort
    {
        Null = 0,
        LoginRequest = 1,
        LoginResponse = 2
    }

    public enum LoginResponseTypeEnum : ushort
    {
        LoginValidated = 1,
        LoginInvalid = 2,
        ServerNA = 3,
        UserBanned = 4
    }

    public class UserInfo
    {
        public ushort FluxID;
        public LoginResponseTypeEnum LoginResponse;
    }

    public class Ticket
    {
        public uint FluxID;
        public ushort AppID;
        public DateTime LastTouched;
        public ushort AccessLevel;
    }

    public class User
    {
        public Socket Sock;
        public uint FluxID;
        public DateTime LastTouched;
    }
}
