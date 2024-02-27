using Classes;
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
                //
                // Code to run as a Server
                //
                Server server = new Server();
                server.Start();
            }
            else
            {
                //
                // Code to run as a Client
                //
                Client client = new Client();
                client.Start();
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

    }
}
