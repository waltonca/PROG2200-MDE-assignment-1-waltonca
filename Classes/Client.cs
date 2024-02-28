using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

            bool inInputMode = false;

            while (true)
            {
                if (stream.DataAvailable)
                {
                    string receivedMessage = ReceiveMessage(stream);
                    Console.WriteLine("Server: " + receivedMessage);
                    if (receivedMessage.ToLower() == "quit")
                        break;

                    
                }

                if (inInputMode)
                {
                    Console.Write("Insertion Mode>> ");
                    string message = Console.ReadLine();
                    // If the user types "quit", stop sending messages
                    if (message.ToLower() == "quit")
                    {
                        break;
                    }  
                    else
                    {
                        SendMessage(stream, message);
                    }
                    // Once the message is sent, reset the input mode into listening mode.
                    inInputMode = false;

                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.I)
                    {
                        inInputMode = !inInputMode;
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("You typed Escape to exit.");
                        break;
                    }
                }
            }

            // Close the stream and the client
            stream.Close();
            client.Close();

            // Show message to the user that the client is closed.
            Console.WriteLine("Disconnected.\nGood Bye!");
        }
        private string ReceiveMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        private void SendMessage(NetworkStream stream, string message)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
    }
 

           
}
