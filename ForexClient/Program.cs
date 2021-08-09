using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 27654;
            TcpClient client; 
            try
            {
                client = new TcpClient(ip, port);
            }
            catch (Exception e)
            {
                Console.WriteLine("No Connection to the Server");
                Console.WriteLine("Please try again later!");
                return;
            }
            Console.WriteLine("Connected to the server at: " + client.Client.LocalEndPoint);
            NetworkStream nwStream = client.GetStream();

            bool programIsRunning = true;
            while (programIsRunning)
            {
                Console.WriteLine("Please insert your currency pair");
                Console.WriteLine("1. EURUSD 2.USDCAD 3.EURJPY 4.USDGBP 5.USDCHF 6.BTCUSD 0.EXIT!");
           
                bool properInputGranted = false;
                string messageToBeSent = "";
 
                
                while (!properInputGranted)
                {
                    int switcher = 0;
                    switcher = int.Parse(Console.ReadLine());
                
                    switch (switcher)
                    {
                        case 0:
                            messageToBeSent = "Abort";
                            properInputGranted = true;
                            programIsRunning = false;
                            break;
                        case 1:
                            messageToBeSent = "EURUSD";
                            properInputGranted = true;
                            break;
                        case 2:
                            messageToBeSent = "USDCAD";
                            properInputGranted = true;
                            break;
                        case 3:
                            messageToBeSent = "EURJPY";
                            properInputGranted = true;
                            break; 
                        case 4:
                            messageToBeSent = "USDGBP";
                            properInputGranted = true;
                            break;
                        case 5:
                            messageToBeSent = "USDCHF";
                            properInputGranted = true;
                            break; 
                        case 6:
                            messageToBeSent = "BTCUSD";
                            properInputGranted = true;
                            break;
                        default:
                            Console.WriteLine("Wrong Choice! Try Again!");
                            break;
                    }
                }
                Console.WriteLine(messageToBeSent);
                
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(messageToBeSent);

                Console.WriteLine("Sending : " + messageToBeSent);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                if (messageToBeSent == "Abort")
                {
                    Console.WriteLine("Aborting program!");
                    return;
                }
                
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                Console.WriteLine("Exchange rate of " + messageToBeSent + " is " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead) );
                
            }
           
            
            
            // byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(messageToBeSent);
            //
            // Console.WriteLine("Sending : " + messageToBeSent);
            // nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            //
            // byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            // int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            // Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            // Console.WriteLine("Exchange rate of " + messageToBeSent + " is " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead) );
            //
            
            Console.ReadLine();
            client.Close();
        }
    }
}