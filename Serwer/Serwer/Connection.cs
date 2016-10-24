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
        //private List<Client> _clientSockets;
        private string[] nameTab = { "oko", "autor", "myszka miki", "zdzisiek", "kubek", "scooby doo", "cokolwiek", "foo", "test", "odbiorca" };
        #endregion

        public Connection()
        {
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), _serverPort));
           // _clientSockets = new List<Client>();
            Console.WriteLine("Server wystartowal...");
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
            Random foo = new Random();
            String oko = nameTab[foo.Next(0, nameTab.Length - 1)];
            Client client = new Client(oko, server.EndAccept(result));
            Console.WriteLine("Polaczyl sie " +oko);


            client.BeginReceive();

            server.BeginAccept(AcceptCallback, server); // <- continue accepting connections
        }

        #endregion

        #region Handling Client Methods

        /*public void AddClient(Client client)
        {
            _clientSockets.Add(client);
            //Console.WriteLine("Polaczyl sie klient: " + client.Item1);
        }

        public Client GetClient()
        {
            return _clientSockets[0];
        }

        public void DisconnectClient(Client client)
        {
            _clientSockets.Remove(client);
            //Console.WriteLine("Klient sie rozlaczyl: " + client.Item1);
        }*/

        #endregion


        #endregion
    }
}
