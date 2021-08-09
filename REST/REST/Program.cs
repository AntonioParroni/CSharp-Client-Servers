using System;
using System.IO;
using System.Net;
using System.Text;

namespace REST
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string site = "";
            Console.WriteLine("Please enter your website address");
            site = Console.ReadLine();
            site = string.Concat("http://" + site);
            Console.WriteLine(site);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Request");
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            Console.WriteLine(req.Address);
            Console.WriteLine(req.Connection);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Response");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Console.WriteLine(resp.StatusCode.ToString());
            Console.WriteLine(resp.Headers.ToString());

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Content \n");
            using (StreamReader stream = new StreamReader(
                resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                Console.WriteLine(str);
            }
        }
    }
}