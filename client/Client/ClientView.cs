﻿using System;
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
    public partial class ClientView : Form,IClientView
    {

        private LogView f;     
        public ClientView()
        {
            InitializeComponent();
           
            f = new LogView();
            f.Hide();
            f.LogInTry += F_LogInTry;
        }

        private void F_LogInTry(object sender, TryToLogInEventArgs e)
        {
            var args = new TryToConnectEventArgs("127.0.0.1", 1024, e.Login, e.Password);
            var handler = ConnectionTry;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event EventHandler<TryToConnectEventArgs> ConnectionTry;
        public event EventHandler<EventArgs> Disconnect;
        public event EventHandler<MessageSendEventArgs> MessageSend;

        public void DisplayMessage(string message)
        {

            string text = "Serwer: " + message;
            Action<string> updateAction = new Action<string>((value) => textBox1.AppendText(value));
            textBox1.Invoke(updateAction,text);
            textBox1.Invoke(updateAction, Environment.NewLine);
        }
        public void SetConnectionSucceeded()
        {
            //Show();
            button1.Invoke((MethodInvoker)(() => { button1.Visible = true; }));
            button2.Invoke((MethodInvoker)(() => { button2.Visible = false; }));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;
            textBox1.AppendText("Ja: "+textBox2.Text + Environment.NewLine);            
            var args = new MessageSendEventArgs(textBox2.Text,textBox3.Text);
            //MessageSend?.Invoke(this, args);
            var handler = MessageSend;
            if (handler != null)
            {
                handler(this, args);
            }
            textBox2.Clear();
        }

        public void SetConnectionError()
        {

            //Show();
            button1.Invoke((MethodInvoker)(() => { button1.Visible = false; }));
            button2.Invoke((MethodInvoker)(() => { button2.Visible = true; }));

            //MessageBoxButtons buttons = MessageBoxButtons.RetryCancel;
            //string message = "Błąd połączenia z serwerem";
            //string caption = "Błąd połączenia";
            //DialogResult result;
            //result = MessageBox.Show(message, caption, buttons);
            //if (result == DialogResult.Retry)
            //{
            //    logIn();
            //}
            //if (result == DialogResult.Cancel)
            //{
            //    return;
            //}        
        }
      
        private void connect()
        {
            
            
        }
        private void logIn()
        {
            //Hide();
            f.Show();
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            logIn();
        }

        private void ClientView_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Disconnect?.Invoke(this, EventArgs.Empty);
            var handler = Disconnect;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }




      




    }
}
