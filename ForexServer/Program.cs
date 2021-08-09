using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FOReignEXchange
{
    internal class Program
    {
        
        public static void Main(string[] args)
        {
            
            string ip = "127.0.0.1";
            int port = 27654;
            
            Dictionary<string, double> CurrencyRates = new Dictionary<string, double>(); 
            CurrencyRates.Add("EURUSD",1.32);
            CurrencyRates.Add("USDCAD",1.18);
            CurrencyRates.Add("EURJPY",123.44);
            CurrencyRates.Add("USDGBP",0.87);
            CurrencyRates.Add("USDCHF",0.74);
            CurrencyRates.Add("BTCUSD",38694.43);
            
            TcpListener listener = TcpListener.Create(port);
            listener.Start();
            Console.WriteLine("Listening started on socket: " + ip + ":" + port);
            Console.WriteLine("Waiting for the client...");
            
            
            
            // async Task StartListener()
            // {
            //     var tcpListener = TcpListener.Create(8000);
            //     tcpListener.Start();
            //     while (true)
            //     {
            //         var tcpClient = await tcpListener.AcceptTcpClientAsync();
            //         Console.WriteLine("[Server] Client has connected");
            //         var task = StartHandleConnectionAsync(tcpClient);
            //         
            //     }
            // }
            //
            // StartListener();
            
            TcpClient client = new TcpClient();
            client = listener.AcceptTcpClient();
            
            NetworkStream nwStream = client.GetStream();

            string dataReceived = "";

            
            while (dataReceived != "Abort")
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); 
                
                
                dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received : " + dataReceived);

                double dataToBeSent = 0;

                switch (dataReceived)
                {
                    case "EURUSD":
                        dataToBeSent = CurrencyRates["EURUSD"];
                        break;
                    case "USDCAD":
                        dataToBeSent = CurrencyRates["USDCAD"];
                        break;
                    case "EURJPY":
                        dataToBeSent = CurrencyRates["EURJPY"];
                        break;
                    case "USDGBP":
                        dataToBeSent = CurrencyRates["USDGBP"];
                        break;
                    case "USDCHF":
                        dataToBeSent = CurrencyRates["USDCHF"];
                        break;
                    case "BTCUSD":
                        dataToBeSent = CurrencyRates["BTCUSD"];
                        break;
                    case "Abort":
                        client.Close();
                        listener.Stop();
                        Console.WriteLine("GoodBye!");
                        return;
                    default:
                        Console.WriteLine("Wrong message form the Client");
                        break;
                }
                Console.WriteLine("Sending back : " + dataToBeSent);
                byte[] buffer1 = Encoding.ASCII.GetBytes(dataToBeSent.ToString());
                nwStream.Write(buffer1, 0, buffer1.Length);
                
            }
            
          
            //
            // dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            // Console.WriteLine("Received : " + dataReceived);
            //
            // double dataToBeSent = 0;
            //
            // switch (dataReceived)
            // {
            //     case "EURUSD":
            //         dataToBeSent = CurrencyRates["EURUSD"];
            //         break;
            //     case "USDCAD":
            //         dataToBeSent = CurrencyRates["USDCAD"];
            //         break;
            //     case "EURJPY":
            //         dataToBeSent = CurrencyRates["EURJPY"];
            //         break;
            //     case "USDGBP":
            //         dataToBeSent = CurrencyRates["USDGBP"];
            //         break;
            //     case "USDCHF":
            //         dataToBeSent = CurrencyRates["USDCHF"];
            //         break;
            //     case "BTCUSD":
            //         dataToBeSent = CurrencyRates["BTCUSD"];
            //         break;
            //     default:
            //         Console.WriteLine("Wrong message form the Client");
            //         break;
            // }
            // Console.WriteLine("Sending back : " + dataToBeSent);
            // byte[] buffer1 = Encoding.ASCII.GetBytes(dataToBeSent.ToString());
            // nwStream.Write(buffer1, 0, buffer1.Length);
            
            client.Close();
            listener.Stop();
            Console.ReadLine();
            
        }
    }
}