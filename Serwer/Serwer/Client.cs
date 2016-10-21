using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Serwer
{
    class Client
    {
        private string _name;
        private Socket _clientSocket;
        private byte[] _buffer;

        public Client(string name, Socket socket)
        {
            _name = name;
            _clientSocket = socket;
            _buffer = new byte[1024];
        }

        #region Getters&Setters
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }    
        public Socket ClientSocket
        {
            get
            {
                return _clientSocket;
            }
        }
        #endregion

        #region Methods
        public void ReceiveMessage()
        {

        }
        public void SendMessage()
        {

        }

        public void BeginReceive()
        {
            _clientSocket.BeginReceive(_buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), this);//_clientSocket);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Client myClient = (Client)result.AsyncState;

            Console.WriteLine(ASCIIEncoding.ASCII.GetString(_buffer));
            //int bytesReceived = EndReceive(myState.ClientSocket);
            Console.WriteLine("odebrano wiadomosc od " + myClient.Name);
            if (!(myClient.ClientSocket.Poll(1, SelectMode.SelectRead) && myClient.ClientSocket.Available == 0))
                BeginReceive();
        }


        /*public void ReceiveMessage(string str)
        {
            string[] temp = str.Split(':');
            //temp[0] = order
            //temp[1] = author/login
            //temp[2] = receiver/password
            //temp[3] = content
        }*/

        /*public bool SendMessage(string author, string receiver, string content)
        {
            try
            {
                //Socket client = clientSockets_[clientSockets_.FindIndex(t => t.Item1 == receiver)].Item2;
                //client.Send(CreateByteArray("author"));
                //client.Send(ConcatenateByteArrays(new List<byte[]> { CreateByteArray("author"), CreateByteArray("foo") }));
                Console.WriteLine("Wyslano wiadomosc do:" + receiver);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }*/
        
        
        /*private static byte[] CreateByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }*/

        /*private static byte[] ConcatenateByteArrays(List<byte[]> listOfBytes)
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
        */

        #endregion
    }
}
