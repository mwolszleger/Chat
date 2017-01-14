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
    public partial class ConversationView : Form
    {

        public event EventHandler<MessageSendEventArgs> MessageSend;
        private int id;
        private List<string> Logins;
        public ConversationView()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                return;
            textBox1.AppendText("Ja: " + textBox2.Text + Environment.NewLine);
            var args = new MessageSendEventArgs(textBox2.Text, id);

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
        public ConversationView(int id,List<string>logins):this()
        {
            this.id = id;
            this.Logins = logins;
            for (int i = 0; i < Logins.Count; i++)
            {
                label1.Text += Logins[i];
                if (i != Logins.Count - 1)
                    label1.Text += ", ";
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
