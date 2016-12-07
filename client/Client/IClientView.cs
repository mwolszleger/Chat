﻿using System;
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
        public int Id { get; private set; }
        public MessageSendEventArgs(string message,int id)
        {
            Message = message;
            Id = id;
        }
    }
    interface IClientView
    {

        void NewConversation(int id, string login);
        void SetConnectionError();
        void SetConnectionSucceeded();
        void DisplayMessage(string message,int id);
        void newUser(string login, bool logged);
        void UserChanged(string login, bool logged);
        event EventHandler<TryToConnectEventArgs> ConnectionTry;
        event EventHandler<MessageSendEventArgs> MessageSend;
        event EventHandler<EventArgs> Disconnect;
        event EventHandler<string> NewConversationStart;
    }
}
