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
    public partial class ClientView : Form, IClientView
    {
        public event EventHandler<TryToConnectEventArgs> ConnectionTry;
        public event EventHandler<EventArgs> Disconnect;
        public event EventHandler<MessageSendEventArgs> MessageSend;
        public event EventHandler<List<string>> NewConversationStart;
        public event EventHandler<CreateAccountArgs> CreateAccount;

        private CreateAccountView createAccountView = new CreateAccountView();
        private Dictionary<int, ConversationView> conversations = new Dictionary<int, ConversationView>();

        public ClientView()
        {
            InitializeComponent();
            textBoxPassword.Focus();
            createAccountView.createAccount += CreateAccountView_createAccount;

        }
        public void SetLogInSucceeded()
        {

            Invoke((MethodInvoker)(() => { SetLogged(); }));

        }


        public void SetConnectionError()
        {

            Invoke((MethodInvoker)(() => { ClearData(); }));

        }
        public void NewUser(string login, bool logged)
        {

            if (logged)
            {
                Action add = new Action(() => listBoxLogged.Items.Add(login));
                Invoke(add);
            }
            else
            {
                Action add = new Action(() => listBoxNotLogged.Items.Add(login));
                Invoke(add);
            }
        }

        public void UserChanged(string login, bool logged)
        {
            //MessageBox.Show("zmienia"+login);
            if (logged)
            {

                Action add = new Action(() => listBoxLogged.Items.Add(login));
                Invoke(add);
                Action remove = new Action(() => listBoxNotLogged.Items.Remove(login));
                Invoke(remove);


            }
            else
            {

                Action add = new Action(() => listBoxNotLogged.Items.Add(login));
                Invoke(add);
                Action remove = new Action(() => listBoxLogged.Items.Remove(login));
                Invoke(remove);

            }
        }

        public void NewConversation(int id, List<string> logins)
        {



            if (!conversations.ContainsKey(id))
            {
                Action createWindow = new Action(() => conversations.Add(id, new ConversationView(id, logins)));
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



        public void DisplayMessage(string message, string author, int id)
        {
            //MessageBox.Show("wyswietlam");
            conversations[id].DisplayMessage(message, author);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedLogins = new List<string>();
            foreach (var item in listBoxLogged.SelectedItems)
            {
                selectedLogins.Add(item.ToString());
            }
            foreach (var item in listBoxNotLogged.SelectedItems)
            {
                selectedLogins.Add(item.ToString());
            }

            var handler = NewConversationStart;

            if (handler != null && selectedLogins.Count > 0)
            {
                handler(this, selectedLogins);
            }

        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            ClearData();
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
                button3_Click(this, null);
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

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            var selectedLogins = new List<string>();
            foreach (var item in listBoxLogged.SelectedItems)
            {
                selectedLogins.Add(item.ToString());
            }
            var handler = NewConversationStart;

            if (handler != null && selectedLogins.Count == 1)
            {
                handler(this, selectedLogins);
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            var selectedLogins = new List<string>();
            foreach (var item in listBoxNotLogged.SelectedItems)
            {
                selectedLogins.Add(item.ToString());
            }
            var handler = NewConversationStart;

            if (handler != null && selectedLogins.Count == 1)
            {
                handler(this, selectedLogins);
            }
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
                handler(this, e);
            }
        }
        private void SetLogged()
        {
            labelLogin.Text = textBoxLogin.Text;
            labelLogin.Visible = true;
            textBoxPassword.Text = "";
            textBoxLogin.Text = "";
            label1.Visible = false;
            label2.Visible = false;
            buttonLogIn.Visible = false;
            buttonRegister.Visible = false;
            textBoxPassword.Visible = false;
            textBoxLogin.Visible = false;
            buttonLogOut.Visible = true;

        }
        private void ClearData()
        {
            foreach (var item in conversations)
            {
                item.Value.Close();
            }
            conversations = new Dictionary<int, ConversationView>();
            labelLogin.Visible = false;

            label1.Visible = true;
            label2.Visible = true;
            buttonLogIn.Visible = true;
            buttonRegister.Visible = true;
            textBoxPassword.Visible = true;
            textBoxLogin.Visible = true;
            buttonLogOut.Visible = false;
            listBoxLogged.Items.Clear();
            listBoxNotLogged.Items.Clear();
            textBoxPassword.Focus();
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
            if (textBoxPassword.Text == "" || textBoxLogin.Text == "")
                return;
            var args = new TryToConnectEventArgs(textBoxLogin.Text, textBoxPassword.Text);
            var handler = ConnectionTry;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void SetLogInFailed()
        {
            MessageBox.Show("Zły login/hasło");
        }
    }

}
