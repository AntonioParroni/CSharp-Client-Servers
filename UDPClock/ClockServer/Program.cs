// Написать сетевые часы с использованием дейтаграмного протокола. На сервере устанавливается служба
// отправки пакетов, содержащих информацию о текущем времени в локальную сеть. Клиенты отображают
// текущее время. Опционально можете сделать систему
// звонков.


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpSample
{
    class Clock
    {
        
        private static IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 3001);
        private static IPEndPoint localPoint = new IPEndPoint(IPAddress.Any, 3001);

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Server Started...");
            try
            {
                while (true)
                {
                    Console.WriteLine("Sending current time : " + DateTime.Now + " to " + endPoint);
                    Send(DateTime.Now.ToString());
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex + "\n  " + ex.Message);
            }
        }

        private static void Send(string datagram)
        {
            UdpClient clOne = new UdpClient();
            clOne.ExclusiveAddressUse = false;
            clOne.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            
            clOne.Client.Bind(localPoint);
            
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(datagram);
                clOne.Send(bytes, bytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex + "\n  " + ex.Message);
            }
            finally
            {
                clOne.Close();
            }
        }
    }
}