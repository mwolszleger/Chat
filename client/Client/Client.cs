using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
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
        public string Author { get; private set; }
        public int Id { get; private set; }
        public MessageRecievedEventArgs(string message, string author, int id)
        {
            Message = message;
            Id = id;
            Author = author;
        }
    }
    public class UserEventArgs : EventArgs
    {
        public string Login { get; private set; }
        public bool Logged { get; private set; }
        public UserEventArgs(string login, bool logged)
        {
            Login = login;
            Logged = logged;

        }

    }
    public class ConversationArgs : EventArgs
    {
        public List<string> Logins { get; private set; }
        public int Id { get; private set; }
        public ConversationArgs(List<string> logins, int id)
        {
            Logins = logins;
            Id = id;
        }

    }
    class Client
    {
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public event EventHandler<UserEventArgs> NewUser;
        public event EventHandler<UserEventArgs> ChangedUser;
        public event EventHandler<bool> RegistrationResult;
        public event EventHandler<ConversationArgs> ConversationStart;
        private bool Connected = false;
        private bool Logged = false;
        public string Login { get; private set; }

        private string recievedBuffer = "";



        private Socket clientSocket;



        private NetworkStream serverStream = default(NetworkStream);

        private List<User> users = new List<User>();

        private Dictionary<int, Conversation> conversations = new Dictionary<int, Conversation>();

        private byte[] _buffer;
        public void startConnection()
        {
           
            if (Connected)
               return;
            int port;
            string ip;
            try
            {
                using (StreamReader sr = new StreamReader("ip.txt"))
                {
                    ip = sr.ReadLine();
                    port = Convert.ToInt32(sr.ReadLine());

                }
            }
            catch (Exception)
            {

                ip = "127.0.0.1";
                port = 1024;
            }
         
            try
            {
                _buffer = new byte[1024];

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                clientSocket.Connect(ip, port);
                serverStream = default(NetworkStream);
                Connected = true;
                
                BeginReceive();
               
            }
            catch (Exception)
            {


                Connected = false;
                Logged = false;
                var args = new ConnectionChangedEventArgs(false);
                //ConnectionChanged?.Invoke(this, args);
                var handler = ConnectionChanged;
                if (handler != null)
                {
                    handler(this, args);
                }
            }

        }

        public void connectToServer(string login, string password)
        {
            startConnection();
            sendMessage("login:" + login + ":" + computeHash(password));
            Login = login;
        }
        
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

                    int count = _buffer.Count(bt => bt != 0);
                    string message = Encoding.UTF8.GetString(_buffer, 0, count);
                    processMessage(message);
                    Array.Clear(_buffer, 0, _buffer.Length);
                    BeginReceive();
                }
                else
                {
                    close();

                    var args = new ConnectionChangedEventArgs(false);
                    var handler = ConnectionChanged;
                    if (handler != null)
                    {
                        handler(this, args);
                    }

                }
            }
            catch (Exception e)
            {
                if (Connected)
                    MessageBox.Show(e.Message);
            }
        }
        private void processBuffer()
        {
            string length = "";
            foreach (var item in recievedBuffer)
            {
                if (char.IsDigit(item))
                    length += item;
                else
                    break;
            }
            if (recievedBuffer.Length >= length.Length + Int32.Parse(length))
            {
                processOrder(recievedBuffer.Substring(length.Length, Int32.Parse(length)));

            }
            else
                return;
            if (recievedBuffer.Length > length.Length + Int32.Parse(length))
            {
                recievedBuffer = recievedBuffer.Substring(length.Length + Int32.Parse(length));


            }
            else
            {
                recievedBuffer = "";
                //MessageBox.Show("nic");
            }

            if (recievedBuffer != "")
                processBuffer();
        }
        private void processMessage(string message)
        {
            // MessageBox.Show("prztwarzam:"+message);
            Console.WriteLine("dostalem" + message);

            recievedBuffer += message;
            processBuffer();


        }

        public void close()
        {

            if (Connected)
            {
                //serverStream.Close();
                Connected = false;
                clientSocket.Close();
            }

            Logged = false;
            conversations = new Dictionary<int, Conversation>();
            users = new List<User>();
            Login = "";
            recievedBuffer = "";

        }
        public void sendMessage(string message)
        {

            try
            {

                clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(message.Length + message));

                Console.WriteLine("wyslano:" + message.Length + message);
            }
            catch (Exception)
            {
                Connected = false;
            }
        }
        public void registerUser(string login, string password)
        {
            
            startConnection();
           
            sendMessage("register:" + login + ":" + computeHash(password));
        }
        public void SendTextMessage(string message, int id)
        {
            string msg;
            if (conversations[id].IsSingleConversation())
            {
                msg = "sendMsg:" + Login + ":" + conversations[id].users[0].login + ":" + message;

            }
            else
            {
                msg = "broadcast:" + Login + ":";
                for (int i = 0; i < conversations[id].users.Count - 1; i++)
                {
                    msg += conversations[id].users[i].login + ",";
                }
                msg += conversations[id].users.Last().login;
                msg += ":" + message;
            }
            sendMessage(msg);

        }
        private void newUser(string login, bool logged)
        {
            if (login == Login)
                return;
            users.Add(new User(login, logged));
            var args = new UserEventArgs(login, logged);
            //ConnectionChanged?.Invoke(this, args);
            var handler = NewUser;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        public void changeUser(string login, bool logged)
        {
            if (login == Login)
                return;
            foreach (var item in users)
            {
                if (item.login == login)
                {
                    if (item.logged == logged)
                        return;
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


        public void NewConversationStart(List<string> logins)
        {

            int index = -1;

            foreach (var item in conversations)
            {

                if (item.Value.IsTheSame(logins))
                {
                    index = item.Key;
                    break;
                }
            }

            if (index == -1)
            {
                var usersList = new List<User>();
                foreach (var item in logins)
                {
                    usersList.Add(findUser(item));
                }
                index = newConversationIndex();
                conversations.Add(index, new Conversation(usersList));

            }
            Console.WriteLine("nadano" + index);
            var args = new ConversationArgs(logins, index);
            //MessageRecieved?.Invoke(this, args);
            var handler = ConversationStart;
            if (handler != null)
            {
                handler(this, args);
            }
            //MessageBox.Show(index.ToString());



        }


        private void processOrder(string message)
        {

            if (!Logged)
            {
                if (message == "logged")
                {
                    LoginSucceeded();
                    return;
                }

            }
            var splitted = message.Split(':');
            if (splitted.Length > 4)
            {
                for (int i = 4; i < splitted.Length; i++)
                {
                    splitted[3] += ":";
                    splitted[3] += splitted[i];
                }
            }
            var list = new List<string>();
            switch (splitted[0])
            {
                case "sendMsg":


                    list.Add(splitted[2]);

                    recievedMessage(splitted[1], list, splitted[3]);
                    break;
                case "broadcast":

                    var recievers = splitted[2].Split(',');
                    var recieversList = recievers.ToList<string>();
                    for (int i = 0; i < recieversList.Count; i++)
                    {
                        recieversList[i] = recieversList[i].Replace(" ", "");

                    }
                    recievedMessage(splitted[1], recieversList, splitted[3]);
                    break;
                case "logged":
                    int id = findUserId(splitted[1]);

                    if (id == -1)
                    {
                        newUser(splitted[1], true);
                    }
                    else
                        changeUser(splitted[1], true);
                    break;
                case "loggedOut":
                    int id2 = findUserId(splitted[1]);
                    if (id2 == -1)
                        newUser(splitted[1], false);
                    else
                        changeUser(splitted[1], false);
                    break;
                case "registrationSuceeded":
                    registrationResult(true);
                    break;
                case "registrationFailed":
                    registrationResult(false);
                    break;


                default:
                    break;
            }


        }

        private void registrationResult(bool v)
        {
           
            //ConnectionChanged?.Invoke(this, args);
            var handler = RegistrationResult;
            if (handler != null)
            {
                handler(this, v);
            }
        }

        private void recievedMessage(string author, List<string> reciever, string content)
        {
            reciever.Remove(Login);
            reciever.Add(author);
            NewConversationStart(reciever);
            int index = -1;
            foreach (var item in conversations)
            {
                if (item.Value.IsTheSame(reciever))
                {
                    index = item.Key;
                    break;
                }
            }

            //                                                                                                                                                          MessageBox.Show(author);

            var args = new MessageRecievedEventArgs(content, author, index);
            //MessageRecieved?.Invoke(this, args);
            var handler = MessageRecieved;
            if (handler != null)
            {
                handler(this, args);
            }

        }
        private int newConversationIndex()
        {
            int index = -1;
            foreach (var it in conversations)
            {
                if (it.Key > index)
                    index = it.Key;

            }
            return ++index;
        }
        private int findUserId(string login)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].login == login)
                    return i;
            }
            return -1;
        }

        private User findUser(string login)
        {
            foreach (var item in users)
            {
                if (item.login == login)
                    return item;

            }
            return null;
        }
        private string computeHash(string password)
        {
            var alghorithm = SHA256.Create();
            var result = alghorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(result);
        }

    }
}
