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
            if (t == new AuthPacket().GetType())
                return PacketTypeEnum.LoginRequest;
            return PacketTypeEnum.Null;
        }

        public static Type EnumToPacketType(PacketTypeEnum e)
        {
            if (e == PacketTypeEnum.LoginRequest)
            {
                return new AuthPacket().GetType();
            }
            return new NullPacket().GetType();
        }
    }
}
