using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Client
    {
        private TcpClient client = null;
        public Client()
        {
            client = new TcpClient();
        }
        public void Start() 
        {
            client.Connect("127.0.0.1",13000);
            Console.WriteLine("Client is connected to Server!");

            // After connection is established, send a message to the server.
            NetworkStream stream = client.GetStream();
            SendMessage(stream, "Hi Server");
            ReceiveMessages(stream);
            stream.Close();
            client.Close();
        }
        private void ReceiveMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Server: " + receivedMessage);
                if (receivedMessage.ToLower() == "quit")
                    break;
            }
        }

        private void SendMessage(NetworkStream stream, string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
 

           
}
