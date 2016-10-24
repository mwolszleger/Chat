using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public MessageRecievedEventArgs(string message)
        {
            Message = message;
        }

    }
    class Client
    {
        public event EventHandler<ConnectionChangedEventArgs> ConnectionChanged;
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        private bool Connected = false;


        private System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        private NetworkStream serverStream = default(NetworkStream);
        private Thread recievingThread;
        public void connectToServer(String ip, int port)
        {
            try
            {
                clientSocket = new System.Net.Sockets.TcpClient();
            clientSocket.Connect(ip, port);
            serverStream = default(NetworkStream);
            Connected = true;
            recievingThread = new Thread(getMessage);
            recievingThread.Start();
            var args = new ConnectionChangedEventArgs(true);
            ConnectionChanged?.Invoke(this, args);
            }
            catch (Exception e)
            {

                Connected = false;
                var args = new ConnectionChangedEventArgs(false);
                ConnectionChanged?.Invoke(this, args);
            }
        }
        public void close()
        {
            Connected = false;
            serverStream.Close();
            clientSocket.Close();

        }
        public void sendMessage(string message)
        {
            try
            {
                serverStream = clientSocket.GetStream();
                byte[] outStream;
                outStream = System.Text.Encoding.ASCII.GetBytes(message);
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
            catch (Exception e)
            {
                Connected = false;
            }
        }
        private void getMessage()
        {

            while (Connected)
            {
                if (Connected)
                {
                    try
                    {
                        serverStream = clientSocket.GetStream();
                        int buffSize = 0;
                        buffSize = clientSocket.ReceiveBufferSize;
                        byte[] inStream = new byte[buffSize];
                        serverStream.Read(inStream, 0, buffSize);
                        string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                        var args = new MessageRecievedEventArgs(returndata);
                        MessageRecieved?.Invoke(this, args);

                    }
                    catch (Exception e)
                    {
                        if (!Connected)
                            break;
                        Connected = false;
                        clientSocket.Close();
                        var args = new ConnectionChangedEventArgs(false);
                        ConnectionChanged?.Invoke(this, args);
                    }
                }

            }
        }


    }
}
