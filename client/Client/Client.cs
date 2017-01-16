using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
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
        public event EventHandler<UserEventArgs> NewUserAdded;
        public event EventHandler<UserEventArgs> ChangedUser;
        public event EventHandler<bool> RegistrationResulted;
        public event EventHandler<ConversationArgs> ConversationStart;

        private Socket clientSocket;
        private NetworkStream serverStream = default(NetworkStream);
        private byte[] buffer;
        private string login;
        private string recievedBuffer = "";
        private bool connected = false;
        private bool logged = false;
        private List<User> users = new List<User>();
        private Dictionary<int, Conversation> conversations = new Dictionary<int, Conversation>();


        public void ConnectToServer(string login, string password)
        {
            StartConnection();
            SendMessage("login:" + login + ":" + ComputeHash(password));
            this.login = login;
        }
        public void Close()
        {

            if (connected)
            {
                connected = false;
                clientSocket.Close();
            }

            logged = false;
            conversations = new Dictionary<int, Conversation>();
            users = new List<User>();
            login = "";
            recievedBuffer = "";

        }

        private void StartConnection()
        {

            if (connected)
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
                buffer = new byte[1024];

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                clientSocket.Connect(ip, port);
                serverStream = default(NetworkStream);
                connected = true;

                BeginReceive();

            }
            catch (Exception)
            {

                MessageBox.Show("Brak połączenia z serwerem");
                connected = false;
                logged = false;
                var args = new ConnectionChangedEventArgs(false);
                //ConnectionChanged?.Invoke(this, args);
                var handler = ConnectionChanged;
                if (handler != null)
                {
                    handler(this, args);
                }
            }

        }
        private void BeginReceive()
        {
            try
            {
                clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
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

                    int count = buffer.Count(bt => bt != 0);
                    string message = Encoding.UTF8.GetString(buffer, 0, count);
                    ProcessMessage(message);
                    Array.Clear(buffer, 0, buffer.Length);
                    BeginReceive();
                }
                else
                {
                    Close();
                    MessageBox.Show("Brak połączenia z serwerem");
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
                if (connected)
                    MessageBox.Show(e.Message);
            }
        }

        public void RegisterUser(string login, string password)
        {

            StartConnection();

            SendMessage("register:" + login + ":" + ComputeHash(password));
        }
        public void SendTextMessage(string message, int id)
        {
            string msg;
            if (conversations[id].IsSingleConversation())
            {
                msg = "sendMsg:" + login + ":" + conversations[id].users[0].login + ":" + message;

            }
            else
            {
                msg = "broadcast:" + login + ":";
                for (int i = 0; i < conversations[id].users.Count - 1; i++)
                {
                    msg += conversations[id].users[i].login + ",";
                }
                msg += conversations[id].users.Last().login;
                msg += ":" + message;
            }
            SendMessage(msg);

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
                    usersList.Add(FindUser(item));
                }
                index = NewConversationIndex();
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


        private void ProcessBuffer()
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
                ProcessOrder(recievedBuffer.Substring(length.Length, Int32.Parse(length)));

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
                ProcessBuffer();
        }


        private void ProcessMessage(string message)
        {
            // MessageBox.Show("prztwarzam:"+message);
            Console.WriteLine("dostalem" + message);

            recievedBuffer += message;
            ProcessBuffer();


        }

        private void SendMessage(string message)
        {

            try
            {

                clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(message.Length + message));

                Console.WriteLine("wyslano:" + message.Length + message);
            }
            catch (Exception)
            {
                connected = false;
            }
        }
        private void NewUser(string login, bool logged)
        {
            if (login == this.login)
                return;
            users.Add(new User(login, logged));
            var args = new UserEventArgs(login, logged);
            //ConnectionChanged?.Invoke(this, args);
            var handler = NewUserAdded;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        private void ChangeUser(string login, bool logged)
        {
            if (login == this.login)
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
            logged = true;
            var args = new ConnectionChangedEventArgs(true);
            //ConnectionChanged?.Invoke(this, args);
            var handler = ConnectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
        private void ProcessOrder(string message)
        {

            if (!logged)
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

                    RecievedMessage(splitted[1], list, splitted[3]);
                    break;
                case "broadcast":

                    var recievers = splitted[2].Split(',');
                    var recieversList = recievers.ToList<string>();
                    for (int i = 0; i < recieversList.Count; i++)
                    {
                        recieversList[i] = recieversList[i].Replace(" ", "");

                    }
                    RecievedMessage(splitted[1], recieversList, splitted[3]);
                    break;
                case "logged":
                    int id = FindUserId(splitted[1]);

                    if (id == -1)
                    {
                        NewUser(splitted[1], true);
                    }
                    else
                        ChangeUser(splitted[1], true);
                    break;
                case "loggedOut":
                    int id2 = FindUserId(splitted[1]);
                    if (id2 == -1)
                        NewUser(splitted[1], false);
                    else
                        ChangeUser(splitted[1], false);
                    break;
                case "successfulRegistration":
                    RegistrationResult(true);
                    break;
                case "failToRegister":
                    RegistrationResult(false);
                    break;


                default:
                    break;
            }


        }
        private void RegistrationResult(bool v)
        {

            //ConnectionChanged?.Invoke(this, args);
            var handler = RegistrationResulted;
            if (handler != null)
            {
                handler(this, v);
            }
        }
        private void RecievedMessage(string author, List<string> reciever, string content)
        {
            reciever.Remove(login);
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
        private int NewConversationIndex()
        {
            int index = -1;
            foreach (var it in conversations)
            {
                if (it.Key > index)
                    index = it.Key;

            }
            return ++index;
        }
        private int FindUserId(string login)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].login == login)
                    return i;
            }
            return -1;
        }
        private User FindUser(string login)
        {
            foreach (var item in users)
            {
                if (item.login == login)
                    return item;

            }
            return null;
        }
        private string ComputeHash(string password)
        {
            var alghorithm = SHA256.Create();
            var result = alghorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(result);
        }

    }
}
