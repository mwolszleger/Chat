using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class ConnectionChangedEventArgs : EventArgs
    {
        public bool Connected { get; private set; }

        public ConnectionChangedEventArgs(bool connected)
        {
            Connected = connected;
        }
    }
    public class MessageRecievedEventArgs : EventArgs
    {
        public string Message { get; private set; }
        public int Id { get; private set; }
        public MessageRecievedEventArgs(string message,int id)
        {
            Message = message;
        }
    }
    public class UserEventArgs : EventArgs
    {
        public string Login { get; private set; }
        public bool Logged { get; private set; }
        public UserEventArgs(string login,bool logged)
        {
            Login = login;
            Logged = logged;

        }

    }
    public class ConversationArgs : EventArgs
    {
        public string Login { get; private set; }
        public int Id { get; private set; }
        public ConversationArgs(string login, int id)
        {
            Login = login;
            Id = id;
        }

    }
    class Client
    {
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public event EventHandler<UserEventArgs> NewUser;
        public event EventHandler<UserEventArgs> ChangedUser;
        public event EventHandler<ConversationArgs> ConversationStart;
        private bool Connected = false;
        private bool Logged = false;
        public string Login { get; private set; }

      

        #region Zmiana
        //zmien pozniej na prywatny i zeby nie byl statyczny
       public static Socket clientSocket;
        #endregion


       private NetworkStream serverStream = default(NetworkStream);
      
        private List<User> users = new List<User>();
        private List<Conversation> conversations = new List<Conversation>();

        #region Tymczasowy bufer
        private byte[] _buffer;
        #endregion
        public void connectToServer(String ip, int port, string login, string password)
        {
            try
            {
                _buffer = new byte[1024];
                
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                clientSocket.Connect(ip, port);
                serverStream = default(NetworkStream);
                Connected = true;
                

                #region Zmiana
               
                sendMessage("login:" + login + ":" + password);

                //rozpoczyna asynchroniczne nasluchiwanie serwera (bez robienia dodatkowego watku, to druga opcja, ale wtedy
                //synchronicznie sluchasz)
                BeginReceive();
                #endregion
               
                
                
                Login = login;

               
              


                //tymczasowo
                newUser("t1",true);
                newUser("testniezalogowany1",false);
                newUser("t2", true);
               
                newUser("t3", true);
            }
            catch (Exception e)
            {

            
                Connected = false;
                var args = new ConnectionChangedEventArgs(false);
                //ConnectionChanged?.Invoke(this, args);
                var handler = ConnectionChanged;
                if (handler != null)
                {
                    handler(this, args);
                }
            }
        }


        #region Client Socket Receive Methods

        public void BeginReceive()
        {
            try
            {
                clientSocket.BeginReceive(_buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Socket mySock = (Socket)result.AsyncState;
            try
            {
                if (!(mySock.Poll(1000, SelectMode.SelectRead) && mySock.Available == 0))
                {
                    
                    BeginReceive();
                    //this.FireMessageReceivedEvent(CreateStringFromByteArray(_buffer));

                    //tutaj faktycznie odbierasz jakas wiadomosc
                    int count = _buffer.Count(bt => bt != 0); // find the first null
                    string message = Encoding.ASCII.GetString(_buffer, 0, count);
                    processRecievedMessage(message);
                
                    // MessageBox.Show(ASCIIEncoding.ASCII.GetString(_buffer));

                    Array.Clear(_buffer, 0, _buffer.Length);
                }
                else
                {
                    Console.WriteLine("Stracono polaczenie z ");
                    //FireDisconnectedEvent(myClient.Name);
                }            
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion

        public void close()
        {

            if (Connected)
            {
                //serverStream.Close();
                clientSocket.Close();
            }
            Connected = false;

        }
        public void sendMessage(string message)
        {
            try
            {
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(message));
            }
            catch (Exception e)
            {
                Connected = false;
            }
        }
        public void SendTextMessage(string message, int id)
        {
            string msg = "sendMsg:" + Login + ":" + conversations[id].user.login + ":" + message;
            sendMessage(msg);
        }
        private void newUser(string login,bool logged)
        {
            users.Add(new User(login,logged));
            var args = new UserEventArgs(login,logged);
            //ConnectionChanged?.Invoke(this, args);
            var handler = NewUser;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        public void changeUser(string login, bool logged)
        {
            foreach (var item in users)
            {
                if (item.login == login)
                {
                    item.logged = logged;
                    break;
                }
            }
            var args = new UserEventArgs(login, logged);
            //ConnectionChanged?.Invoke(this, args);
            var handler = ChangedUser;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        private void LoginSucceeded()
        {
            Logged = true;
            var args = new ConnectionChangedEventArgs(true);
            //ConnectionChanged?.Invoke(this, args);
            var handler = ConnectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }


        public void NewConversationStart(string login)
        {
            int index=-1;
            for (int i=0;i<conversations.Count; i++)
            {
                if (conversations[i].user.login == login)
                {
                    index = i;
                    break;
                }
            }
           
            if (index == -1)
            {
                foreach (var item in users)
                {
                    if (item.login == login)
                    {
                        conversations.Add(new Conversation(item));
                        index = conversations.Count - 1;
                    }
                }
            }
            
            var args = new ConversationArgs(login,index);
            //MessageRecieved?.Invoke(this, args);
            var handler = ConversationStart;
            if (handler != null)
            {
                handler(this, args);
            }




        }


        private void processRecievedMessage(string message)
        {
          
           // MessageBox.Show(message);
            if (!Logged)
            {
                if (message=="logged")
                {
                    LoginSucceeded();
                    return;
                }
                
            }
            var splitted = message.Split(':');

            switch (splitted[0])
            {
                case "sendMsg":

                    //TODO: Obsluga wiadomosci z dwukropkiem
                    recievedMessage(splitted[1], splitted[2], splitted[3]);
                    break;
                case "logged":
                    newUser(splitted[1],true);
                    break;                       
                default:
                    break;
            }

         
        }

        private void recievedMessage(string author, string reciever, string content)
        {
            NewConversationStart(author);
            int index=-1;
            for (int i = 0; i < conversations.Count; i++)
            {
                if (conversations[i].user.login == author)
                {
                    index = i;
                    break;
                }
            }

            var args = new MessageRecievedEventArgs(content, index);
            //MessageRecieved?.Invoke(this, args);
            var handler = MessageRecieved;
            if (handler != null)
            {
                handler(this, args);
            }

        }
    }
}
