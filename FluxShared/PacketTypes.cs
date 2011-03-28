using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace Flux
{
    public interface IPacket
    {
        byte[] Write();
        PacketTypeEnum GetPacketType();
    }

    public class BasePacket : IPacket
    {
        public byte[] Write()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);
            FieldInfo[] fi = this.GetType().GetFields();
            writer.Write((ushort)PacketReg.TypeToEnum(this.GetType()));
            try
            {
                foreach (FieldInfo field in fi)
                {
                    if (field.Name.StartsWith("_"))
                    {
                        string s = "";
                        if (field.FieldType == s.GetType())
                        {
                            s = (string)field.GetValue(this);
                            byte[] utfString = Common.StringToByteArray(s);
                            writer.Write((uint)utfString.Length);
                            writer.Write(utfString);
                        }

                        ushort ush = 0;
                        if (field.FieldType == ush.GetType())
                        {
                            ush = (ushort)field.GetValue(this);
                            writer.Write(ush);
                        }

                        short sh = 0;
                        if (field.FieldType == sh.GetType())
                        {
                            sh = (short)field.GetValue(this);
                            writer.Write(sh);
                        }

                        uint uin = 0;
                        if (field.FieldType == uin.GetType())
                        {
                            uin = (uint)field.GetValue(this);
                            writer.Write(uin);
                        }

                        int i = 0;
                        if (field.FieldType == i.GetType())
                        {
                            i = (int)field.GetValue(this);
                            writer.Write(i);
                        }

                        ulong ulo = 0;
                        if (field.FieldType == ulo.GetType())
                        {
                            ulo = (ulong)field.GetValue(this);
                            writer.Write(ulo);
                        }

                        long lo = 0;
                        if (field.FieldType == ulo.GetType())
                        {
                            lo = (long)field.GetValue(this);
                            writer.Write(lo);
                        }

                        byte[] byt = new byte[1024];
                        if (field.GetType() == byt.GetType())
                        {
                            byt = (byte[])field.GetValue(this);
                            writer.Write(byt.Length);
                            writer.Write(byt);
                        }
                    }
                }
                return memoryStream.ToArray();
            }
            catch
            {
                
            }
            return null;
        }

        public static IPacket Read(byte[] packet)
        {
            try
            {
                MemoryStream memStr = new MemoryStream(packet);
                BinaryReader reader = new BinaryReader(memStr);
                PacketTypeEnum type = (PacketTypeEnum)reader.ReadUInt16();
                IPacket p = (IPacket)Activator.CreateInstance(PacketReg.EnumToPacketType(type));
                FieldInfo[] fi = p.GetType().GetFields();

                foreach (FieldInfo field in fi)
                {
                    if (field.Name.StartsWith("_"))
                    {
                        string s = "";
                        if (field.FieldType == s.GetType())
                        {
                            uint length = reader.ReadUInt32();
                            byte[] strArray = reader.ReadBytes((int)length);
                            field.SetValue(p, Common.ByteArrayToString(strArray));
                        }

                        ushort ush = 0;
                        if (field.FieldType == ush.GetType())
                        {
                            field.SetValue(p, reader.ReadUInt16());
                        }

                        short sh = 0;
                        if (field.FieldType == sh.GetType())
                        {
                            field.SetValue(p, reader.ReadInt16());
                        }

                        uint uin = 0;
                        if (field.FieldType == uin.GetType())
                        {
                            field.SetValue(p, reader.ReadUInt32());
                        }

                        int i = 0;
                        if (field.FieldType == i.GetType())
                        {
                            field.SetValue(p, reader.ReadInt32());
                        }

                        ulong ulo = 0;
                        if (field.FieldType == ulo.GetType())
                        {
                            field.SetValue(p, reader.ReadUInt64());
                        }

                        long lo = 0;
                        if (field.FieldType == lo.GetType())
                        {
                            field.SetValue(p, reader.ReadInt64());
                        }

                        byte[] byt = new byte[1024];
                        if (field.GetType() == byt.GetType())
                        {
                            int length = reader.ReadInt32();
                            field.SetValue(p, reader.ReadBytes(length));
                        }
                    }
                }
                return p;
            }
            catch
            {
                return new NullPacket();
            }
        }



        public PacketTypeEnum GetPacketType()
        {
            try
            {
                return PacketReg.TypeToEnum(this.GetType());
            }
            catch
            {
                return PacketTypeEnum.Null;
            }
        }
    }

    public class NullPacket : BasePacket { }
}
