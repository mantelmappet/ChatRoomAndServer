using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.NET.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream ns;
        public PacketReader(NetworkStream ns) : base(ns)
        {
            this.ns = ns;
        }
        public string ReadMessage()
        {
            byte[] msgBuffer;
            var length = ReadInt32();
            msgBuffer = new byte[length];
            ns.Read(msgBuffer, 0, length);
            var msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
    }
}
