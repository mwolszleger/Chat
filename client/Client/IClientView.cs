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
        public TryToConnectEventArgs(string login,string password)
        {
           
            Login = login;
            Password = password;
        }
    }
    public class CreateAccountArgs : EventArgs
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public CreateAccountArgs(string login, string password)
        {
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

        void NewConversation(int id, List<string>logins);
        void SetConnectionError();
        void SetConnectionSucceeded();
        void DisplayMessage(string message,string author,int id);
        void newUser(string login, bool logged);
        void UserChanged(string login, bool logged);
        void RegistrationResult(bool e);

        event EventHandler<TryToConnectEventArgs> ConnectionTry;
        event EventHandler<MessageSendEventArgs> MessageSend;
        event EventHandler<EventArgs> Disconnect;
        event EventHandler<List<string>> NewConversationStart;
        event EventHandler<CreateAccountArgs> CreateAccount;
    }
}
