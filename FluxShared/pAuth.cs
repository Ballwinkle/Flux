using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux
{
    public class AuthRequest : BasePacket
    {
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _username = "";

        public string Password
        {
            get { return _password; }
            set { _password = Hashing.sha256encrypt(value); }
        }
        private string _password = "";
    }

    public class AuthResponse : BasePacket
    {
        public ushort FluxID
        {
            get { return _fluxid; }
            set { _fluxid = value; }
        }
        private ushort _fluxid = 0;

        public LoginResponseTypeEnum ResponseType
        {
            get { return (LoginResponseTypeEnum)_responseType; }
            set { _responseType = (ushort)value; }
        }
        private ushort _responseType;
    }
}
