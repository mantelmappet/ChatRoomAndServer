using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatApp.NET.IO;

namespace ChatApp.NET
{
    class Server
    {
        TcpClient client;
        public PacketReader PacketReader;
        public event Action connectedEvent;
        public event Action msgRecievedEvent;
        public event Action userDisconnectedEvent;

        public Server()
        {
            client = new TcpClient();
        }
        public void ConnectToServer(String username)
        {
            if (!client.Connected)
            {
                client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var ConnectPacket = new PacketBuilder();
                    ConnectPacket.WriteOpCode(0);
                    ConnectPacket.WriteMessage(username);
                    client.Client.Send(ConnectPacket.GetPacketBytes());
                }
                ReadPackets();
            }
        }
        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while(true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch(opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgRecievedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
        }

    }
}
