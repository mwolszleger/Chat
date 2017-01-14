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

        #region Events&Handling
        public event EventHandler<EventArgsWithContent> MessageReceivedEvent;
        public event EventHandler<EventArgsWithContent> DisconnectedEvent;

        private void FireMessageReceivedEvent(string order)
        {
            EventHandler<EventArgsWithContent> handler = MessageReceivedEvent;
            if (handler != null)
            {
                handler(this, new EventArgsWithContent(order));
            }
        }

        private void FireDisconnectedEvent(string name)
        {
            EventHandler<EventArgsWithContent> handler = DisconnectedEvent;
            if (handler != null)
            {
                handler(this, new EventArgsWithContent(name));
            }
        }

        #endregion

        public Client(string name, Socket socket)
        {
            _name = name;
            _clientSocket = socket;
            _buffer = new byte[1024];
        }
        public Client()
        {
        }

        #region Methods

        public void BeginReceive()
        {
            try
            {
                _clientSocket.BeginReceive(_buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), this);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Client myClient = (Client)result.AsyncState;

            if (!(myClient.ClientSocket.Poll(1000, SelectMode.SelectRead) && myClient.ClientSocket.Available == 0))
            {
                string foo = CreateStringFromByteArray(_buffer);
                Array.Clear(_buffer, 0, _buffer.Length);
                //tell server, that i received message
                this.FireMessageReceivedEvent(foo);
                BeginReceive();
            }
            else
            {
                Console.WriteLine("Stracono polaczenie z " + myClient.Name);
                FireDisconnectedEvent(myClient.Name);
            }
        }

        #region StaticMethods
        private static string CreateStringFromByteArray(byte[] byteArr)
        {
            return Encoding.UTF8.GetString(byteArr);
        }

        #endregion

        #endregion
    }//end of Client class

    public class EventArgsWithContent : EventArgs
    {
        private readonly string _content;

        public EventArgsWithContent(string cont)
        {
            this._content = cont.TrimEnd(new char[] { (char)0 });
        }

        public string Content
        {
            get { return this._content; }
        }
    }//end of MessageEventArgs class
}//end of namespace
