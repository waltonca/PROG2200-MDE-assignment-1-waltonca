using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Classes
{
    public class Server
    {
        private TcpListener server = null;
        public Server()
        {
            // Set the TcpListener on port 13000.
            Int32 port = 13000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, port);
        }
        public void Start()
        {
            // Start listening for client requests.
            server.Start();
            Console.WriteLine("Waiting for any Client connection... ");

            // Perform a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("Connection established with Client!");

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();
            Thread receiveThread = new Thread(() => ReceiveMessage(stream));
            receiveThread.Start();

            bool inInputMode = false;

            while (true)
            {
                if (inInputMode)
                {
                    Console.Write(">> ");
                    string message = Console.ReadLine();
                    SendMessage(stream, message);
                    if (message.ToLower() == "quit")
                        break;
                }
                else
                {
                    Thread.Sleep(100); // Small delay to prevent high CPU usage
                    if (stream.DataAvailable)
                    {
                        string receivedMessage = ReceiveMessage(stream);
                        Console.WriteLine("Client: " + receivedMessage);
                        if (receivedMessage.ToLower() == "quit")
                            break;
                    }
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.I)
                    {
                        inInputMode = !inInputMode;
                    }
                }
            }
            stream.Close();
            client.Close();
            server.Stop();
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
        public void Stop()
        {
            // Stop listening for new clients.
            server.Stop();
        }
    }
}
