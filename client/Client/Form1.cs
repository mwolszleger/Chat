using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        byte[] outStream;
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public Form1()
        {
            InitializeComponent();
            clientSocket.Connect("127.0.0.1", 1024);


            Thread oko = new Thread(getMessage);
            oko.Start();

        }


        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            //
            textBox1.Text += value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            serverStream = clientSocket.GetStream();

            outStream = System.Text.Encoding.ASCII.GetBytes(textBox2.Text);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
        private void getMessage()
        {
            while (true)
            {
                serverStream = clientSocket.GetStream();
                int buffSize = 0;
                byte[] inStream = new byte[10025];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;
               // Console.WriteLine(readData);
                AppendTextBox(readData);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            serverStream = clientSocket.GetStream();

            outStream = System.Text.Encoding.ASCII.GetBytes("Moja wiadomosc" + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverStream.Close();
            clientSocket.Close();
        }
    }
}
