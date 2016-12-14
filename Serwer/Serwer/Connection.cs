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

        public Connection()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), _serverPort));
            _clientSockets = new List<Client>();
            _byteArray = new byte[1024];
            _myDBConnection = new DatabaseConnection();
            _ordersBuff = new List<string>();

            Console.WriteLine("Serwer wystartowal...");
        }

        public void HandleMessageReceivedEvent(object sender, EventArgsWithContent args)
        {
            if (args.Content.Length != 0)
            {
                MakeOrdersFromMessage(args.Content);
                foreach (string item in _ordersBuff)
                {
                    HandleOrder(item, ((Client)sender).ClientSocket);
                }
                _ordersBuff.Clear();
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
            MakeOrdersFromMessage(CreateStringFromByteArray(_byteArray));
            //string order = CreateStringFromByteArray(_byteArray);
            Client client = null;
            if (_ordersBuff.Count > 0)
            {
                client = this.HandleOrder(_ordersBuff[0], clientSocket);
                _ordersBuff.RemoveAt(0);
            }

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

        private void MakeOrdersFromMessage(string order)
        {
            string length = "";

            foreach (char c in order)
            {
                if (c != '\0')
                {
                    if (char.IsDigit(c))
                        length += c;
                    else if (length.Length != 0)
                    {
                        //'global' container for orders
                        _ordersBuff.Add(order.Substring(length.Length, Int32.Parse(length)));
                        MakeOrdersFromMessage(order.Remove(0, length.Length + Int32.Parse(length)));
                        break;
                    }
                }
            }
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
            Console.WriteLine(commands[1] + " wyslal wiadomosc " + commands[3]);
            //[0] = order
            //[1] = author
            //[2] = receiver
            //[3] = content
            if (_clientSockets.FindIndex(a => a.Name == commands[2]) != -1)
            {
                //sends message as order, i.e. sendMsg:authorOfMessage:receiverOfMessage:content
                //if you want to send msg to yourself, create order as "sendMsg:author:author:conent"
                GetClient(commands[2]).ClientSocket.Send(CreateByteArray(commands.Length.ToString() + CreateOrder(commands)));
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
                    GetClient(item).ClientSocket.Send(CreateByteArray(commands.Length.ToString() + CreateOrder(commands)));
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
            Client cl = new Client(nick, client);
            this.AddClient(cl);

            //informs about success
            this.InformSomebody("logged", client);
            //this.InformAboutLoggedUsers(client);
            //send offline messages to him
            while (_myDBConnection.CheckOfflineMessages(nick))
                this.SendMsg(_myDBConnection.GetOfflineMessage(nick));
            //informs everybody that someone logged
            //this.InformEverybody("logged:" + nick);

            return cl;
        }

        private Client Register(string nick, Socket client)
        {
            if (_myDBConnection.UserExists(nick))
            {
                client.Send(CreateByteArray(("failToRegister").Length.ToString() + "failToRegister"));
                return null;
            }
            _myDBConnection.InsertUser(nick);//, splitedOrder[2]);
            return this.Login(nick, client);
        }

        private bool InformSomebody(string message, Socket client)
        {
            client.Send(CreateByteArray(message.Length.ToString() + message));
            return true;
        }

        private void InformEverybody(string message)
        {
            foreach (Client cl in _clientSockets)
                cl.ClientSocket.Send(CreateByteArray(message.Length.ToString() + message));
        }

        private void InformAboutLoggedUsers(Socket loggedClient)
        {
            foreach (Client cl in _clientSockets)
                InformSomebody("logged:" + cl.Name, loggedClient);
        }

        #region Methods to simplify
        private static string CreateStringFromByteArray(byte[] byteArr)
        {
            return ASCIIEncoding.ASCII.GetString(byteArr).TrimEnd(new char[] { (char)0 });
        }

        private static byte[] CreateByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
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

        private static String CreateOrder(String[] commands)
        {
            return commands[0] + ":" + commands[1] + ":" + commands[2] + ":" + commands[3];
        }

        #endregion

        #endregion

        #endregion
    }//end Connection class
}//end namespace
