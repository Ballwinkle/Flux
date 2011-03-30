using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flux
{
    static class PacketReg
    {
        public static PacketTypeEnum TypeToEnum(Type t)
        {
            if (t == new AuthRequest().GetType())
                return PacketTypeEnum.LoginRequest;
            if (t == new AuthResponse().GetType())
                return PacketTypeEnum.LoginResponse;
            return PacketTypeEnum.Null;
        }

        public static Type EnumToPacketType(PacketTypeEnum e)
        {
            if (e == PacketTypeEnum.LoginRequest)
                return new AuthRequest().GetType();
            if (e == PacketTypeEnum.LoginResponse)
                return new AuthResponse().GetType();
            return new NullPacket().GetType();
        }
    }
}
