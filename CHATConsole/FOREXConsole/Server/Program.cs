using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    object _lock = new Object();
    List<Task> _connections = new List<Task>();
    static Dictionary<string, double> CurrencyRates = new Dictionary<string, double>();
    private int clientsCount;
    private static int maxClients; 
    private static int maxRequestsNumber; 
    private static Dictionary<TcpClient, int> maxRequests= new Dictionary<TcpClient, int>();

    private async Task StartListener()
    {
        bool onHold = false;
        var tcpListener = TcpListener.Create(8000);
        tcpListener.Start();
        Console.WriteLine("Listener started on: " + tcpListener.LocalEndpoint);
        while (!onHold)
        {
            if (clientsCount > maxClients)
            {
                Console.WriteLine("Max clients number reached!");
                onHold = true;
                tcpListener.Stop();
            }
            else
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("[Server] Client " + clientsCount++ + " has connected");
                maxRequests.Add(tcpClient, maxRequestsNumber);
                var task = StartHandleConnectionAsync(tcpClient, clientsCount);
                if (task.IsFaulted)
                    await task;
            }
        }

        if (onHold)
        {
            Console.WriteLine("Waiting for clients to disconnect...");
            while (true)
            {
                if (clientsCount < maxClients)
                {
                    onHold = false;
                    StartListener();
                }
            }
        }
    }

    private async Task StartHandleConnectionAsync(TcpClient tcpClient, int clientsCount)
    {
        var connectionTask = HandleConnectionAsync(tcpClient, clientsCount);
        
        lock (_lock)
            _connections.Add(connectionTask);
        try
        {
            await connectionTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            lock (_lock)
                _connections.Remove(connectionTask);
        }
    }
    
    private async Task HandleConnectionAsync(TcpClient tcpClient , int clientsCount)
    {
        await Task.Yield();
        
        using (StreamWriter file = new StreamWriter("logs.txt", append: true))
        {
            file.WriteLineAsync("[Client " + clientsCount + "] Connected on : " + DateTime.Now);
        }

        maxRequests[tcpClient] = 0;

        using (var networkStream = tcpClient.GetStream())
        {
            string dataReceived = "";
            
            while (dataReceived != "Abort")
            {
                byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
                int bytesRead = networkStream.Read(buffer, 0, tcpClient.ReceiveBufferSize);
                dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("[Client " + clientsCount + "] Received : " + dataReceived);
                
                if (maxRequests[tcpClient] == maxRequestsNumber)
                {
                    Console.WriteLine("[Client " + clientsCount + "] Reached Max Queries!");
                    tcpClient.Close();
                    Thread.Sleep(10000);
                    maxRequests[tcpClient] = 0;
                }
                
                double dataToBeSent = 0;
                switch (dataReceived)
                {
                    case "EURUSD":
                        dataToBeSent = CurrencyRates["EURUSD"];
                        maxRequests[tcpClient]++;
                        break;
                    case "USDCAD":
                        dataToBeSent = CurrencyRates["USDCAD"];
                        maxRequests[tcpClient]++;
                        break;
                    case "EURJPY":
                        dataToBeSent = CurrencyRates["EURJPY"];
                        maxRequests[tcpClient]++;
                        break;
                    case "USDGBP":
                        dataToBeSent = CurrencyRates["USDGBP"];
                        maxRequests[tcpClient]++;
                        break;
                    case "USDCHF":
                        dataToBeSent = CurrencyRates["USDCHF"];
                        maxRequests[tcpClient]++;
                        break;
                    case "BTCUSD":
                        dataToBeSent = CurrencyRates["BTCUSD"];
                        maxRequests[tcpClient]++;
                        break;
                    case "Abort":
                        maxRequests.Remove(tcpClient);
                        tcpClient.Close();
                        this.clientsCount--;
                        Console.WriteLine("[Client " + clientsCount + "] Disconnected!");
                        using (StreamWriter file = new StreamWriter("logs.txt", append: true))
                        {
                            file.WriteLineAsync("[Client " + clientsCount + "] Disconnected on : " + DateTime.Now);
                        }  
                        return;
                    default:
                        Console.WriteLine("Wrong message form the Client");
                        break;
                }
                Console.WriteLine("[Client " + clientsCount + "] Sending : " + dataToBeSent);
                byte[] buffer1 = Encoding.ASCII.GetBytes(dataToBeSent.ToString());
                networkStream.Write(buffer1, 0, buffer1.Length);
            }
            using (StreamWriter file = new StreamWriter("logs.txt", append: true))
            {
                file.WriteLineAsync("[Client " + clientsCount + "] Disconnected on : " + DateTime.Now);
                file.WriteLineAsync("[Client " + clientsCount + "] Made total queries : " + maxRequests[tcpClient]);
            }  
            tcpClient.Close();
        }
    }

    static async Task Main(string[] args)
    {
        CurrencyRates.Add("EURUSD", 1.32);
        CurrencyRates.Add("USDCAD", 1.18);
        CurrencyRates.Add("EURJPY", 123.44);
        CurrencyRates.Add("USDGBP", 0.87);
        CurrencyRates.Add("USDCHF", 0.74);
        CurrencyRates.Add("BTCUSD", 38694.43);

        using (StreamWriter file = new StreamWriter("logs.txt", append: true))
        {
            file.WriteLineAsync("Currency Rates Start");
            foreach (var var in CurrencyRates)
            {
                file.WriteLineAsync(var.Key + " | " + var.Value);
            }
            file.WriteLineAsync("Currency Rates End");
        }

        Console.WriteLine("Insert below the max amount of clients with a number");
        maxClients = int.Parse(Console.ReadLine());
        maxClients--;
        Console.WriteLine("Insert below the max amount of requests for a client with a number");
        maxRequestsNumber = int.Parse(Console.ReadLine());
        
        await new Program().StartListener();
        
    }
}