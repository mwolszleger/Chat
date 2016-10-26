using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TryToConnectEventArgs : EventArgs
    {
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public TryToConnectEventArgs(string ip,int port)
        {
            Ip = ip;
            Port = port;
        }
    }
    public class MessageSendEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public MessageSendEventArgs(string message)
        {
            Message = message;
        }
    }
    interface IClientView
    {
        void Show();
        void SetConnectionError();
        void SetConnectionSucceeded();
        void DisplayMessage(string message);
        event EventHandler<TryToConnectEventArgs> ConnectionTry;
        event EventHandler<MessageSendEventArgs> MessageSend;
        event EventHandler<EventArgs> Disconnect;
    }
}
