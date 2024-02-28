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
        private bool isSending;
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
            //Thread receiveThread = new Thread(() => ReceiveMessage(stream));
            //receiveThread.Start();

            isSending = false;

            bool inInputMode = false;

            // Listening loop to receive all the data sent by the client.
            while (true)
            {
                // Always listening, messages sent from the client are displayed as soon as they are available;
                // if you go into insert mode, you can still receive messages and display.  
                if (stream.DataAvailable)
                {
                    string receivedMessage = ReceiveMessage(stream);
                    Console.WriteLine("Client: "+ receivedMessage);
                    if (receivedMessage.ToLower() == "quit")
                        break;

                }

                // If the server is in input mode, it will send messages to the client.
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


                // Check if the user has pressed I key
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
