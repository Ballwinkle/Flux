using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux
{
    public class AuthPacket : BasePacket
    {
        public PacketTypeEnum PacketType = PacketTypeEnum.LoginRequest;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _username;

        public string Password
        {
            get { return _password; }
            set { _password = Hashing.sha256encrypt(value); }
        }
        private string _password;
    }
}
