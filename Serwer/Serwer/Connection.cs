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
        #endregion

        public Connection()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), _serverPort));
            _clientSockets = new List<Client>();
            _byteArray = new byte[1024];
            Console.WriteLine("Server wystartowal...");
        }

        public void HandleMessageReceivedEvent(object sender, EventArgsWithContent args)
        {
            if (args.Content.Length != 0)
            {
                HandleOrder(args.Content, ((Client)sender).ClientSocket);
                Console.WriteLine(((Client)sender).Name + " wyslal wiadomosc: " + args.Content);
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
            Client client = this.HandleOrder(CreateStringFromByteArray(_byteArray), clientSocket);

            //server subscribes to client's MessageReceivedEvent
            client.MessageReceivedEvent += HandleMessageReceivedEvent;
            client.DisconnectedEvent += HandleDisconnectedEvent;

            client.BeginReceive();
            server.BeginAccept(AcceptCallback, server);
        }

        #endregion

        #region Handling Client Methods

        private void AddClient(Client client)
        {
            _clientSockets.Add(client);
            Console.WriteLine("Polaczyl sie " + client.Name);
            //inform all clients, that someone connected
            //check if there are any awaiting message for this client
        }

        private Client GetClient(string name)
        {
            return _clientSockets[_clientSockets.FindIndex(a => a.Name == name)];
        }

        private void DisconnectClient(Client client)
        {
            _clientSockets.Remove(client);
            //inform all clients that someone disconnected
        }

        #endregion

        #region Private Methods

        private Client HandleOrder(string order, Socket client)
        {
            String[] splitedOrder = order.Split(':');

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
                    //check if correct,
                    Client cl = new Client(splitedOrder[1], client);
                    this.AddClient(cl);
                    return cl;
                //inform about success and inform all clients
                //login:nickname:shrtpass
                //normal split

                case "register":
                    //add record to database and call login
                    //register:nickname:shrtpass
                    //normal split
                    break;

                default:
                    break;
            }
            return new Client();
        }

        private bool SendMsg(string order)
        {
            String[] commands = SplitOrder(order);
            //[0] = order
            //[1] = author
            //[2] = receiver
            //[3] = content
            if (_clientSockets.FindIndex(a => a.Name == commands[2]) != -1)
            {
                //sends message as order, i.e. sendMsg:authorOfMessage:receiverOfMessage:content
                //if you want to send msg to yourself, create order as "sendMsg:author:author:conent"
                GetClient(commands[2]).ClientSocket.Send(CreateByteArray(CreateOrder(commands)));
                return true;
            }
            else
            {
                //write to database
            }
            return false;
        }

        private bool Broadcast(string order)
        {
            String[] commands = SplitOrder(order);
            //[0] = order
            //[1] = author
            //[2] = receiverS
            //[3] = content
            String[] receivers = commands[2].Split(',');
            bool success = false;

            foreach (string item in receivers)
            {
                if (_clientSockets.FindIndex(a => a.Name == item) != -1)
                {
                    //sends message as order, i.e. broadcast:authorOfMessage:receiversOfMessage:content
                    GetClient(item).ClientSocket.Send(CreateByteArray(CreateOrder(commands)));
                    success = true;
                }
                else
                {
                    //przeslij do bazy i wyslij, gdy sie zaloguje
                    success = true;
                }
            }
            if (success)
                return true;
            return false;
        }

        //TO DO
        /*  
        private bool Login(string order)
        {
            return true;
        }

        private bool Register(string order)
        {
            //create login order
            //call Login
            return true;
        }
         private bool CheckAndSendOldMessages(Client client)
        {
            //get message from database and send to the client
            return true;
        }    
         */

        #region Methods to simplify
        private static string CreateStringFromByteArray(byte[] byteArr)
        {
            return ASCIIEncoding.ASCII.GetString(byteArr);
        }

        private static byte[] CreateByteArray(string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        private String[] SplitOrder(string order)
        {
            String[] temp = new String[4];
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
            temp[3] = order.Substring(indexArr[2], order.IndexOf('\0') - indexArr[2]);
            return temp;
        }

        private String CreateOrder(String[] commands)
        {
            return commands[0] + ":" + commands[1] + ":" + commands[2] + ":" + commands[3];
        }

        #endregion

        #endregion

        #endregion
    }//end Connection class
}//end namespace
