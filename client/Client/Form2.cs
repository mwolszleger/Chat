using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form2 : Form
    {

        public event EventHandler<MessageSendEventArgs> MessageSend;
        private int id;
        public Form2()
        {
            InitializeComponent();
        }
        public void appendMessage(string message)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;
            textBox1.AppendText("Ja: " + textBox2.Text + Environment.NewLine);
            var args = new MessageSendEventArgs(textBox2.Text, id);

            //#region Zmiana
            ////tutaj chyba powinienes wysylac to do serwera
            //Client.clientSocket.Send(System.Text.Encoding.ASCII.GetBytes("sendMsg:foo:foo:" + textBox2.Text));
            //#endregion

            //MessageSend?.Invoke(this, args);
            var handler = MessageSend;
            if (handler != null)
            {
                handler(this, args);
            }
            
            textBox2.Clear();
        }

        public void DisplayMessage(string message,string author)
        {

            string text = author + ": "+message;
            Action<string> updateAction = new Action<string>((value) => textBox1.AppendText(value));
            textBox1.Invoke(updateAction, text);
            textBox1.Invoke(updateAction, Environment.NewLine);
        }
        public Form2(int id):this()
        {
            this.id = id;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
