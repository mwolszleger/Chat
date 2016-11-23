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
        public string Login { get; private set; }
        public string Password { get; private set; }
        public TryToConnectEventArgs(string ip,int port,string login,string password)
        {
            Ip = ip;
            Port = port;
            Login = login;
            Password = password;
        }
    }
    public class MessageSendEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public string Reciever { get; private set; }
        public MessageSendEventArgs(string message,string reciever)
        {
            Message = message;
            Reciever = reciever;
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
