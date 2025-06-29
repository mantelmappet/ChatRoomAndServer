using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppServer.Net.IO
{
    class PacketBuilder
    {
        MemoryStream ms;
        public PacketBuilder()
        {
            ms = new MemoryStream();
        }
        public void WriteOpCode(byte opCode)
        {
            ms.WriteByte(opCode);
        }
        public void WriteMessage(string message)
        {
            var msgLength = message.Length;
            ms.Write(BitConverter.GetBytes(msgLength));
            ms.Write(Encoding.ASCII.GetBytes(message));
        }
        public byte[] GetPacketBytes()
        {
            return ms.ToArray();
        }
    }
}
