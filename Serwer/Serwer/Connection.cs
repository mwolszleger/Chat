using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Serwer
{
    using StringSocketPair = Tuple<string, Socket>;

    class Connection
    {
        #region Private Variables
        private Socket serverSocket_;
        private int serverPort_ = 1024;
        private List<StringSocketPair> clientSockets_;
        #endregion

        public Connection()
        {
            serverSocket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            serverSocket_.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort_));
            clientSockets_ = new List<StringSocketPair>();
            Console.WriteLine("Server wystartowal...");
        }

        #region Methods
        #region Server Methods
        public void Listen()
        {
            serverSocket_.Listen(1);
            Console.WriteLine("Serwer czeka na polaczenie.");
        }

        public Socket Accept()
        {
            return serverSocket_.Accept();
        }
        #endregion

        #region Handling Client Methods

        public void AddClient(StringSocketPair client)
        {
            clientSockets_.Add(client);
            Console.WriteLine("Polaczyl sie klient: " + client.Item1);
        }
        
        public StringSocketPair GetClient()
        {
            return clientSockets_[0];
        }
        
        public void DisconnectClient(StringSocketPair client)
        {
            clientSockets_.Remove(client);
            Console.WriteLine("Klient sie rozlaczyl: " + client.Item1);
        }

        #endregion

        #region Communication Methods

        public bool SendMessage(string author, string receiver, string content)
        {
            try
            {
                Socket client = clientSockets_[clientSockets_.FindIndex(t => t.Item1 == receiver)].Item2;
                //client.Send(CreateByteArray("author"));
                //client.Send(ConcatenateByteArrays(new List<byte[]> { CreateByteArray("author"), CreateByteArray("foo") }));
                Console.WriteLine("Wyslano wiadomosc do:" + receiver);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public void ReceiveMessage(string str)
        {
            string[] temp = str.Split(':');
            //temp[0] = order
            //temp[1] = author/login
            //temp[2] = receiver/password
            //temp[3] = content
        }

        private static byte[] CreateByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        private static byte[] ConcatenateByteArrays(List<byte[]> listOfBytes)
        {
            byte[] concatenation = new byte[listOfBytes.Sum(item => item.Length)];
            int leng = 0;
            foreach(byte[] array in listOfBytes)
            {
                System.Buffer.BlockCopy(array, 0, concatenation, leng, array.Length);
                leng += array.Length;
            }
            return concatenation;
        }

        #endregion

        #endregion
    }
}
