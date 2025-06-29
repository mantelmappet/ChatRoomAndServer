using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatAppServer.Net.IO;

namespace ChatAppServer
{
    class Client
    {
        public String UserName { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }
        PacketReader packetReader;
        public Client(TcpClient client)
        { 
            ClientSocket = client;
            UID = Guid.NewGuid();
            packetReader = new PacketReader(ClientSocket.GetStream());
            var opcode = packetReader.ReadByte();
            UserName = packetReader.ReadMessage();
            Console.WriteLine($"[{DateTime.Now}]: Client has Connected with the username: {UserName}");

            Task.Run(() => Process());
        }

        void Process()
        {
            while(true)
            {
                try
                {
                    var opcode = packetReader.ReadByte();
                    switch(opcode)
                    {
                        case 5:
                            var msg = packetReader.ReadMessage();
                            Console.WriteLine($"{DateTime.Now}: Message recieved! {msg}");
                            Program.BroadcastMessage($"[{DateTime.Now}]: [{UserName}]: {msg}");
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception a)
                {
                    Console.WriteLine($"{UID.ToString()}: Disconnected!");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
