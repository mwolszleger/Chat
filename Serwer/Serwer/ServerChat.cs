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
        static byte[] recBuffer = new byte[256];

        static void Main(string[] args)
        {
            Connection server = new Connection();
            Thread th;

            //while (true)
            //{              
                server.Listen();
                server.BeginAccept();
                //server.AddClient(GetStringSocketPair(server.Accept()));
                //server.SendMessage("foo", "author", "foo");
                //th = new Thread(() => writing(GetSocketFromPair(server.GetClient())));
                //th.Start();
           // }     
            Console.Read();
        }

        public static Tuple<string, Socket> GetStringSocketPair(Socket socket)
        {
            return new Tuple<string, Socket>("author", socket);
        }

        public static Socket GetSocketFromPair(Tuple<string, Socket> foo)
        {
            return foo.Item2;
        }

        public static void writing(Socket serv)
        {
            while (true)
            {

                bool part1 = serv.Poll(1000, SelectMode.SelectRead);
                bool part2 = (serv.Available == 0);
                bool part3;               
                if (part1 && part2)
                    part3 = false;
                else
                    part3 = true;


                if (part3 == true)
                {
                    serv.Receive(recBuffer);
                    //Console.WriteLine(serv.GetHashCode());
                    //Console.WriteLine(ASCIIEncoding.ASCII.GetString(recBuffer));

                    //recBuffer = System.Text.Encoding.ASCII.GetBytes("Bardzo dlugi tekst i zastanawiam sie jak bardzo dlugi on moze byc by normalnie dalej to przeszlo ciekawe co nie no to sprawdzimy hehehehehehehehe okoko nienienie");
                    //serv.Send(recBuffer);
                    //recBuffer = System.Text.Encoding.ASCII.GetBytes("Receiver");
                    //serv.Send(recBuffer);


                    //Array.Clear(recBuffer, 0, recBuffer.Length);
                }
                else break;
            }
            Console.WriteLine("okoko");
        }



    }//end Main class
}//end namespace
