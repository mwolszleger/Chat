﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Serwer
{
    class Client
    {
        private string _name;
        private Socket _clientSocket;
        private byte[] _buffer;

        #region Getters&Setters
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public Socket ClientSocket
        {
            get
            {
                return _clientSocket;
            }
        }
        #endregion

        #region Events&Handling
        public event EventHandler<EventArgsWithContent> MessageReceivedEvent;
        public event EventHandler<EventArgsWithContent> DisconnectedEvent;

        private void FireMessageReceivedEvent(string order)
        {
            EventHandler<EventArgsWithContent> handler = MessageReceivedEvent;
            if (handler != null)
            {
                handler(this, new EventArgsWithContent(order));
            }
        }

        private void FireDisconnectedEvent(string name)
        {
            EventHandler<EventArgsWithContent> handler = DisconnectedEvent;
            if (handler != null)
            {
                handler(this, new EventArgsWithContent(name));
            }
        }

        #endregion

        public Client(string name, Socket socket)
        {
            _name = name;
            _clientSocket = socket;
            _buffer = new byte[1024];
        }
        public Client()
        {
        }
     
        #region Methods

        public void BeginReceive()
        {
            _clientSocket.BeginReceive(_buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), this);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Client myClient = (Client)result.AsyncState;

            if (!(myClient.ClientSocket.Poll(1, SelectMode.SelectRead) && myClient.ClientSocket.Available == 0))
            {
                BeginReceive();
                //tell server, that i received message
                this.FireMessageReceivedEvent(CreateStringFromByteArray(_buffer));

                Array.Clear(_buffer, 0, _buffer.Length);
            }
            else
            {
                Console.WriteLine("Stracono polaczenie z " + myClient.Name);
                FireDisconnectedEvent(myClient.Name);
            }       
        }

        #region StaticMethods
        private static string CreateStringFromByteArray(byte[] byteArr)
        {
            return ASCIIEncoding.ASCII.GetString(byteArr);
        }


        /*private static byte[] CreateByteArray(string str)
            {
                return System.Text.Encoding.ASCII.GetBytes(str);
            }*/

        /* private static byte[] ConcatenateByteArrays(List<byte[]> listOfBytes)
         {
             byte[] concatenation = new byte[listOfBytes.Sum(item => item.Length)];
             int leng = 0;
             foreach(byte[] array in listOfBytes)
             {
                 System.Buffer.BlockCopy(array, 0, concatenation, leng, array.Length);
                 leng += array.Length;
             }
             return concatenation;
         }*/
        #endregion

        #endregion
    }//end of Client class


    public class EventArgsWithContent : EventArgs
    {
        private readonly string _content;

        public EventArgsWithContent(string cont)
        {
            this._content = cont;
        }

        public string Content
        {
            get { return this._content; }
        }
    }//end of MessageEventArgs class
}//end of namespace
