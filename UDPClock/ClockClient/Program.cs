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
        private static IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3001);
        
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("UDP Client Started..");
            try
            {
                Thread tRec = new Thread(new ThreadStart(Receiver));
                tRec.Start();

                while (true)
                {
                   Receiver();
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Exception: " + ex + "\n  " + ex.Message);
            }
        }
        
        public static void Receiver()
        {
            UdpClient clTwo = new UdpClient();
            clTwo.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            clTwo.Client.Bind(endPoint);
            try
            {
                while (true)
                {
                    byte[] receiveBytes = clTwo.Receive(
                       ref endPoint);
                    
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