// Напишите систему обмена сообщениями между клиентами. Система должна использовать центральный
//     сервер, к которому подключаются клиенты и оставляют на нём сообщения. Клиент, которому адресованы
//     сообщения, получает их с сервера по запросу. Система
//     должна использовать протокол, ориентированный на
// соединение (TCP).

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpSample
{
    class Chat
    {
        private static IPAddress remoteIPAddress = IPAddress.Broadcast;
        private static int remotePort;
        private static int localPort;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Local Port");
                localPort = Convert.ToInt16(Console.ReadLine());

                Console.WriteLine("Remote Port");
                remotePort = Convert.ToInt16(Console.ReadLine());
                
                
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();

                while (true)
                {
                    Send(Console.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex + "\n  " + ex.Message);
            }
        }

        private static void Send(string datagram)
        {
            UdpClient sender = new UdpClient();
            
            IPEndPoint endPoint = new IPEndPoint(remoteIPAddress, remotePort);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(datagram);
                
                sender.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex + "\n  " + ex.Message);
            }
            finally
            {
                sender.Close();
            }
        }

        public static void Receiver()
        {
            UdpClient receivingUdpClient = new UdpClient(localPort);

            IPEndPoint RemoteIpEndPoint = null;

            try
            {
                Console.WriteLine("Welcome!");

                while (true)
                {
                    byte[] receiveBytes = receivingUdpClient.Receive(
                       ref RemoteIpEndPoint);
                    
                    string returnData = Encoding.UTF8.GetString(receiveBytes);
                    Console.WriteLine(" --> " + returnData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex + "\n  " + ex.Message);
            }
        }
    }
}