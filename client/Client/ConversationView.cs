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
            if (textBoxMessage.Text == "")
                return;
            textBoxConversation.AppendText("Ja: " + textBoxMessage.Text + Environment.NewLine);
            var args = new MessageSendEventArgs(textBoxMessage.Text, id);

            var handler = MessageSend;
            if (handler != null)
            {
                handler(this, args);
            }

            textBoxMessage.Clear();
        }

        public void DisplayMessage(string message, string author)
        {

            string text = author + ": " + message;
            Action<string> updateAction = new Action<string>((value) => textBoxConversation.AppendText(value));
            textBoxConversation.Invoke(updateAction, text);
            textBoxConversation.Invoke(updateAction, Environment.NewLine);
        }
        public ConversationView(int id, List<string> logins) : this()
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



        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, null);
                e.Handled = true;
            }
        }
    }
}
