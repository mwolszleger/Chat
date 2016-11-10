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
            Connection server = new Connection();
            server.Listen();
            server.BeginAccept();
            Console.Read();
        }
    }//end Main class
}//end namespace
