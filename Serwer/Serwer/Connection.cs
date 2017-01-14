using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace Serwer
{
    class Connection
    {
        #region Private Variables
        private Socket _serverSocket;
        private int _serverPort = 1024;
        private byte[] _byteArray;
        private List<Client> _clientSockets;
        private DatabaseConnection _myDBConnection;
        private List<string> _ordersBuff;
        #endregion

        public Connection(string ip)
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), _serverPort));
            _clientSockets = new List<Client>();
            _byteArray = new byte[1024];
            _myDBConnection = new DatabaseConnection();
            _ordersBuff = new List<string>();

            Console.WriteLine("Serwer wystartowal na ip " + ip + " ...");
        }

        public void HandleMessageReceivedEvent(object sender, EventArgsWithContent args)
        {
            if (args.Content.Length != 0)
            {           
                CreateOrdersFromMessage(args.Content);
                int count = _ordersBuff.Count;
                for (int i = 0; i < count;i++)
                {
                    Console.WriteLine(_ordersBuff[0]);
                    this.HandleOrder(_ordersBuff[0], ((Client)sender).ClientSocket);
                    _ordersBuff.RemoveAt(0);
                }                            
            }
        }

        public void HandleDisconnectedEvent(object sender, EventArgsWithContent args)
        {
            DisconnectClient(_clientSockets[_clientSockets.FindIndex(a => a.Name == args.Content)]);
        }

        #region Methods

        #region Server Methods
        public void Listen()
        {
            _serverSocket.Listen(1);
            Console.WriteLine("Serwer czeka na polaczenie.");
        }

        public void BeginAccept()
        {
            _serverSocket.BeginAccept(AcceptCallback, _serverSocket);
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Socket server = (Socket)result.AsyncState;
            Socket clientSocket = server.EndAccept(result);

            //wait for login/register order      
            clientSocket.Receive(_byteArray);
            //handle login/register order, creates new client
            Client client = this.HandleOrder(CreateOrdersFromMessage(CreateStringFromByteArray(_byteArray)), clientSocket);
            _ordersBuff.RemoveAt(0);
            Array.Clear(_byteArray, 0, _byteArray.Length);
            if (client != null)
            {
                // server subscribes to client's MessageReceivedEvent
                client.MessageReceivedEvent += HandleMessageReceivedEvent;
                client.DisconnectedEvent += HandleDisconnectedEvent;

                client.BeginReceive();
            }
            else
                Console.WriteLine("client = null");

            server.BeginAccept(AcceptCallback, server);
        }

        private string CreateOrdersFromMessage(string message)
        {
            string len = "";
            foreach (char c in message)
            {
                if (char.IsDigit(c))
                    len += c;
                else
                    break;
            }
            if (len != "")
            {
                message = message.Remove(0, len.Length);
                if (message.Length > Int32.Parse(len))
                {
                    _ordersBuff.Add(message.Substring(0, Int32.Parse(len)));
                    message = message.Remove(0, Int32.Parse(len));
                    CreateOrdersFromMessage(message);
                }
                else
                {
                    _ordersBuff.Add(message);
                }
                return message;
            }
            return "";
        }

        #endregion

        #region Handling Client Methods

        private void AddClient(Client client)
        {
            _clientSockets.Add(client);
            Console.WriteLine("Polaczyl sie " + client.Name);
        }

        private Client GetClient(string name)
        {
            return _clientSockets[_clientSockets.FindIndex(a => a.Name == name)];
        }

        private void DisconnectClient(Client client)
        {
            _clientSockets.Remove(client);
            //informs all clients that someone disconnected
            this.InformEverybody("loggedOut:" + client.Name);
        }

        #endregion

        #region Private Methods

        private Client HandleOrder(string order, Socket client)
        {
            string[] splitedOrder = order.Split(':');

            switch (splitedOrder[0])
            {
                case "sendMsg":
                    if (SendMsg(order))
                        Console.WriteLine("Udalo sie wyslac wiadomosc");
                    else
                        Console.WriteLine("Nie udalo sie wyslac wiadomosci");
                    break;

                case "broadcast":
                    if (Broadcast(order))
                        Console.WriteLine("Udalo sie wyslac wiadomosci do wszystkich osob");
                    else
                        Console.WriteLine("Nie udalo sie wyslac wiadomosci");
                    break;

                case "login":
                    // if (_myDBConnection.IfUserExists(splitedOrder[1]))//,splitedOrder[2]);   
                    return this.Login(splitedOrder[1], client);
                //InformSomebody("failToLogin", client);

                //login:nickname:shrtpass
                //normal split
                //return null;
                //break;

                case "register":
                    return this.Register(splitedOrder[1], client);

                default:
                    break;
            }
            return new Client();
        }

        private bool SendMsg(string order)
        {
            string[] commands = SplitOrder(order);
            //Console.WriteLine(commands[1] + " wyslal wiadomosc " + commands[3]);
            //[0] = order
            //[1] = author
            //[2] = receiver
            //[3] = content
            if (_clientSockets.FindIndex(a => a.Name == commands[2]) != -1)
            {
                //sends message as order, i.e. sendMsg:authorOfMessage:receiverOfMessage:content
                //if you want to send msg to yourself, create order as "sendMsg:author:author:conent"

                GetClient(commands[2]).ClientSocket.Send(CreateByteArray(AppendLength(CreateOrder(commands))));
                return true;
            }
            else
            {
                _myDBConnection.InsertOfflineMessage(commands);
            }
            return false;
        }

        private bool Broadcast(string order)
        {
            string[] commands = SplitOrder(order);
            //[0] = order
            //[1] = author
            //[2] = receiverS
            //[3] = content
            string[] receivers = commands[2].Split(',');
            bool success = false;
            
            foreach (string item in receivers)
            {
                if (_clientSockets.FindIndex(a => a.Name == item) != -1)
                {
                    //sends message as order, i.e. broadcast:authorOfMessage:receiversOfMessage:content
                    GetClient(item).ClientSocket.Send(CreateByteArray(AppendLength(CreateOrder(commands))));
                    success = true;
                }
                else
                {
                    string tmpReceivers = item + ", ";
                    foreach (string receiver in receivers)
                        if (receiver != item)
                            tmpReceivers += receiver + ", ";
                    tmpReceivers.Substring(tmpReceivers.Length - 2);
                    _myDBConnection.InsertOfflineMessage(commands[0], commands[1], tmpReceivers, commands[3]);
                    success = true;
                }
            }
            return success;
        }

        private Client Login(string nick, Socket client)
        {
            string tmp = "";
            string[] deleteArray;
            Client cl = new Client(nick, client);
            this.AddClient(cl);

            //informs about success
            this.InformSomebody("logged", client);
            this.InformAboutLoggedUsers(client);
            this.InformAboutNotLoggedUsers(client);
            //send offline messages to him
            
            while(true)
            {
                tmp = _myDBConnection.GetOfflineMessage(nick);
                if (tmp != "")
                {
                    client.Send(CreateByteArray(AppendLength(tmp)));
                    deleteArray = SplitOrder(tmp);                 
                    _myDBConnection.DeleteOfflineMessage(deleteArray[0], deleteArray[1], deleteArray[2], deleteArray[3]);
                }
                else
                    break;
            }
            
            //informs everybody that someone logged
            this.InformEverybody("logged:" + nick);

            return cl;
        }

        private Client Register(string nick, Socket client)
        {
            if (_myDBConnection.UserExists(nick))
            {
                client.Send(CreateByteArray(AppendLength("failToRegister")));
                return null;
            }
            _myDBConnection.InsertUser(nick);//, splitedOrder[2]);
            client.Send(CreateByteArray(AppendLength("successfulRegistration")));
            return this.Login(nick, client);
        }

        private bool InformSomebody(string message, Socket client)
        {
            client.Send(CreateByteArray(AppendLength(message)));
            return true;
        }

        private void InformEverybody(string message)
        {
            string client = message.Split(':')[1];
            foreach (Client cl in _clientSockets)
                if(cl.Name != client)
                    InformSomebody(message, cl.ClientSocket);
        }

        private void InformAboutLoggedUsers(Socket loggedClient)
        {
            if(_clientSockets.Count>0)
                foreach (Client cl in _clientSockets)
                    InformSomebody("logged:" + cl.Name, loggedClient);
        }

        private void InformAboutNotLoggedUsers(Socket loggedClient)
        {
            foreach(string user in _myDBConnection.GetUsers())
            {
                if (!_clientSockets.Where(x => x.Name==user).Any())
                    InformSomebody("loggedOut:" + user, loggedClient);
            }
        }

        #region Methods to simplify
        private static string CreateStringFromByteArray(byte[] byteArr)
        {
            return Encoding.UTF8.GetString(byteArr).TrimEnd(new char[] { (char)0 });
        }

        private static byte[] CreateByteArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        private string[] SplitOrder(string order)
        {
            string[] temp = new string[4];
            int[] indexArr = new int[3];
            int i = -1, j = 0;

            while (j < 3 || i == order.Length)
            {
                if (order[++i] == ':')
                    indexArr[j++] = ++i;
            }
            temp[0] = order.Substring(0, indexArr[0] - 1);
            temp[1] = order.Substring(indexArr[0], indexArr[1] - indexArr[0] - 1);
            temp[2] = order.Substring(indexArr[1], indexArr[2] - indexArr[1] - 1);
            temp[3] = order.Substring(indexArr[2]);
            return temp;
        }

        private static string CreateOrder(string[] commands)
        {
            return commands[0] + ":" + commands[1] + ":" + commands[2] + ":" + commands[3];
        }

        private static string AppendLength(string str)
        {
            return str.Length + str;
        }

        #endregion

        #endregion

        #endregion
    }//end Connection class
}//end namespace
