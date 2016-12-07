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
    public partial class ClientView : Form,IClientView
    {

     
        public ClientView()
        {
            InitializeComponent();

            //do testów
            //conversation.Add(new Form2());
            //conversation[0].Show();
            //conversation[0].MessageSend += ClientView_MessageSend;

        }

        private void ClientView_MessageSend(object sender, MessageSendEventArgs e)
        {
          
            //MessageSend?.Invoke(this, args);
            var handler = MessageSend;
            if (handler != null)
            {
                handler(this,e);
            }
        }

        private List<Form2> conversation = new List<Form2>();
        public event EventHandler<TryToConnectEventArgs> ConnectionTry;
        public event EventHandler<EventArgs> Disconnect;
        public event EventHandler<MessageSendEventArgs> MessageSend;
        public event EventHandler<string> NewConversationStart;

       
        
        public void SetConnectionSucceeded()
        {
            //Show();
            MessageBox.Show("udalo sie");
           // button1.Invoke((MethodInvoker)(() => { button1.Visible = true; }));
           // button2.Invoke((MethodInvoker)(() => { button2.Visible = false; }));
        }
       

        public void SetConnectionError()
        {

            //Show();
            //button1.Invoke((MethodInvoker)(() => { button1.Visible = false; }));
            //button2.Invoke((MethodInvoker)(() => { button2.Visible = true; }));

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

        private void button3_Click(object sender, EventArgs e)
        {
            var args = new TryToConnectEventArgs("127.0.0.1", 1024, textBox5.Text, textBox4.Text);
            var handler = ConnectionTry;
            if (handler != null)
            {
                handler(this, args);
            }
        }

       

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string login = listBox1.SelectedItem.ToString() ;
            var handler = NewConversationStart;
            if (handler != null)
            {
                handler(this,login);
            }
        }

        public void newUser(string login, bool logged)
        {
            if (logged)
            {
                listBox1.Items.Add(login);
            }
            else
            {
                listBox2.Items.Add(login);
            }
        }

        public void UserChanged(string login, bool logged)
        {
            if (logged)
            {
                listBox1.Items.Add(login);
                listBox2.Items.Remove(login);
            }
            else
            {
                listBox2.Items.Add(login);
                listBox1.Items.Remove(login);
            }
        }

        public void NewConversation(int id, string login)
        {
            Action newWindow = new Action(() => conversation[id].Show());
            if (id >= conversation.Count)
            {
                Action createWindow = new Action(() => conversation.Add(new Form2(id)));
                BeginInvoke(createWindow);
                //conversation.Add(new Form2(id));
                //IntPtr handle = conversation[id].Handle;
                

                BeginInvoke(newWindow);

                Action newEvent = new Action(() => conversation[id].MessageSend += ClientView_MessageSend);

                BeginInvoke(newEvent);

                //conversation[id].MessageSend += ClientView_MessageSend;
            }
            else
            {
                
                BeginInvoke(newWindow);
            }
        }

        

        public void DisplayMessage(string message, int id)
        {
            //MessageBox.Show("wyswietlam");
            conversation[id].DisplayMessage(message);
           
        }

       
    }
}
