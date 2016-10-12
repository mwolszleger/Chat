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
        static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        static Socket internalSocket;
        static Socket internalSocket2;
        static byte[] recBuffer = new byte[256];

        static void Main(string[] args)
        {
            
            Thread th, th1;

            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1024));
            int counter = 0;

            Console.WriteLine("foo");

            while (true)
            {
                serverSocket.Listen(1);
                counter++;


                //serverStream = clientSocket.GetStream();

                recBuffer = System.Text.Encoding.ASCII.GetBytes("Michal" + "$");
                
                //serverStream.Write(recBuffer, 0, recBuffer.Length);
                //serverStream.Flush();


                if (counter == 1)
                {
                    internalSocket = serverSocket.Accept();
                    
                    th = new Thread(writing);
                    th.Start();
                }
                else
                {
                    internalSocket2 = serverSocket.Accept();
                    th1 = new Thread(writing2);
                    th1.Start();
                }
            }

           
            Console.Read();
        }


        public static void writing()
        {
            while (true)
            {

                bool part1 = internalSocket.Poll(1000, SelectMode.SelectRead);
                bool part2 = (internalSocket.Available == 0);
                bool part3;               
                if (part1 && part2)
                    part3 = false;
                else
                    part3 = true;


                if (part3 == true)
                {
                    internalSocket.Receive(recBuffer);

                    byte[] bytes = new byte[("Server here: ").Length * sizeof(char)];
                    System.Buffer.BlockCopy(("Server here: ").ToCharArray(), 0, bytes, 0, bytes.Length);
                    byte[] rv = new byte[bytes.Length + recBuffer.Length];
                    System.Buffer.BlockCopy(bytes, 0, rv, 0, bytes.Length);
                    System.Buffer.BlockCopy(recBuffer, 0, rv, bytes.Length, recBuffer.Length);

                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(rv));

                    internalSocket.Send(recBuffer);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(recBuffer));
                    Array.Clear(recBuffer, 0, recBuffer.Length);
                }
                else break;
            }
            Console.WriteLine("okoko");
        }


        public static void writing2()
        {
            while (true)
            {

                bool part1 = internalSocket2.Poll(1000, SelectMode.SelectRead);
                bool part2 = (internalSocket2.Available == 0);
                bool part3;
                if (part1 && part2)
                    part3 = false;
                else
                    part3 = true;


                if (part3 == true)
                {
                    internalSocket2.Receive(recBuffer);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(recBuffer));
                    Array.Clear(recBuffer, 0, recBuffer.Length);
                }
                else break;
            }
            Console.WriteLine("okoko");
        }

    }//end Main class

  



}//end namespace
