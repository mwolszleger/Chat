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
            textBox4.Focus();
            createAccountView.createAccount += CreateAccountView_createAccount;
            //do testów
            //conversation.Add(new Form2());
            //conversation[0].Show();
            //conversation[0].MessageSend += ClientView_MessageSend;

        }

        private void CreateAccountView_createAccount(object sender, CreateAccountArgs e)
        {
            
            var handler = CreateAccount;
            if (handler != null)
            {
                handler(this, e);
            }
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

        private Dictionary<int,ConversationView> conversations = new Dictionary<int,ConversationView>();
        public event EventHandler<TryToConnectEventArgs> ConnectionTry;
        public event EventHandler<EventArgs> Disconnect;
        public event EventHandler<MessageSendEventArgs> MessageSend;
        public event EventHandler<List<string>> NewConversationStart;
        public event EventHandler<CreateAccountArgs> CreateAccount;

        private CreateAccountView createAccountView=new CreateAccountView();

       
        
        public void SetConnectionSucceeded()
        {
            //Show();
           // MessageBox.Show("udalo sie");
            Invoke((MethodInvoker)(() => { setLogged(); }));
           
        }
        private void setLogged()
        {
            label5.Text = textBox5.Text;
            label5.Visible = true;
            textBox4.Text = "";
            textBox5.Text = "";
            label1.Visible = false;
            label2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            button2.Visible = true;
           
        }

        public void SetConnectionError()
        {

            Invoke((MethodInvoker)(() => { clearData(); }));

        }
        private void clearData()
        {
            foreach (var item in conversations)
            {
                item.Value.Close();
            }
            conversations = new Dictionary<int, ConversationView>();
            label5.Visible = false;

            label1.Visible = true;
            label2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            button2.Visible = false;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox4.Focus();
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
            var args = new TryToConnectEventArgs( textBox5.Text, textBox4.Text);
            var handler = ConnectionTry;
            if (handler != null)
            {
                handler(this, args);
            }
        }

       

       
        public void newUser(string login, bool logged)
        {
           
            if (logged)
            {
                Action add = new Action(() => listBox1.Items.Add(login));
                Invoke(add);
            }
            else
            {
                Action add = new Action(() => listBox2.Items.Add(login));
                Invoke(add);
            }
        }

        public void UserChanged(string login, bool logged)
        {
            //MessageBox.Show("zmienia"+login);
            if (logged)
            {
                
                Action add = new Action(() => listBox1.Items.Add(login));
                Invoke(add);
                Action remove = new Action(() => listBox2.Items.Remove(login));
                Invoke(remove);
               
                
            }
            else
            {

                Action add = new Action(() => listBox2.Items.Add(login));
                Invoke(add);
                Action remove = new Action(() => listBox1.Items.Remove(login));
                Invoke(remove);
             
            }
        }

        public void NewConversation(int id, List<string> logins)
        {


            
            if (!conversations.ContainsKey(id))
            {
                Action createWindow = new Action(() => conversations.Add(id,new ConversationView(id,logins)));
                Invoke(createWindow);
                //conversation.Add(new Form2(id));
                //IntPtr handle = conversations[id].Handle;

                Action newWindow = new Action(() => conversations[id].Show());
               Invoke(newWindow);

                Action newEvent = new Action(() => conversations[id].MessageSend += ClientView_MessageSend);

                Invoke(newEvent);

                //conversation[id].MessageSend += ClientView_MessageSend;
            }
            else
            {
                Action newWindow = new Action(() => conversations[id].Show());
                Invoke(newWindow);
            }
            
        }

        

        public void DisplayMessage(string message,string author, int id)
        {
            //MessageBox.Show("wyswietlam");
            conversations[id].DisplayMessage(message,author);
           
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedLogins = new List<string>();
            foreach (var item in listBox1.SelectedItems)
            {
                selectedLogins.Add(item.ToString());
            }
            foreach (var item in listBox2.SelectedItems)
            {
                selectedLogins.Add(item.ToString());
            }
           
            var handler = NewConversationStart;
            
            if (handler != null&&selectedLogins.Count>0)
            {
                handler(this,selectedLogins);
            }

        }

       
        private void button2_Click_1(object sender, EventArgs e)
        {
            clearData();
            var handler = Disconnect;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }

        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_Click(this,null);
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            createAccountView.Show();
        }

        public void RegistrationResult(bool e)
        {
            if (e)
            {
                Invoke((MethodInvoker)(() => { createAccountView.registrationSucceded(); }));
            }
            else
            {
                Invoke((MethodInvoker)(() => { createAccountView.registrationFailed(); }));
            }
        }
    }
   
}
