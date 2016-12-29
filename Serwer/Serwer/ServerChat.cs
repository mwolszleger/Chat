using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Net;


namespace Serwer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip= "";
            try
            {   // Open the text file using a stream reader.
                using (System.IO.StreamReader sr = new System.IO.StreamReader("ip.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    ip = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                ip = "127.0.0.1";
            }

            Connection server = new Connection(ip);
            server.Listen();
            server.BeginAccept();
            Console.Read();
        }
    }//end Main class
}//end namespace
