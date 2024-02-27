﻿using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment1ChatProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Dectect if the program is running as a server or client
            // Use the -server argument to run as a server
            // Use the no argument to run as a client
            if(args.Contains("-server")) {
                // Code to run as a server
                Server server = new Server();
                server.Start();
            }
            else
            {
                //Once the client has connected, you can begin sending messages

                Console.WriteLine("Client is connected to Server!");
                // Code to run as a client
                //Client client = new Client();
                //Hard coded message to send to the server
                Connect("127.0.0.1", "Hi Server");
                //Ask the user for a message to send to the server
                

            }

            /*
            
                while (true)
                {
                    //Part 1
                    //Run as Client vs Server
                    Console.WriteLine("Listening for messages");

                    if (Console.KeyAvailable)
                    {
                        //User input mode: when user press "I" key.
                        //Console.ReadLine();
                        ConsoleKeyInfo userKey = Console.ReadKey();

                        if (userKey.Key == ConsoleKey.I)
                        {
                            Console.Write("'I' is PRESSED >>");
                            Thread.Sleep(2000);
                            Console.ReadLine();

                        }
                        else if (userKey.Key == ConsoleKey.Escape) 
                        { 
                            break; // Close the client node object.
                        }
                        else
                        {
                            Console.WriteLine($"You typed {userKey.Key}");
                            Thread.Sleep(2000);
                        }
                    
                    }
                }
                

            */
        }

        //The Connect Method:
        static void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;

                // Prefer a using declaration to ensure the instance is Disposed later.
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the server response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Explicit close is not necessary since TcpClient.Dispose() will be
                // called automatically.
                // stream.Close();
                // client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
