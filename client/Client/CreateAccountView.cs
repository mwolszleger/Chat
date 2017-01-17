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

    public partial class CreateAccountView : Form
    {
        public event EventHandler<CreateAccountArgs> createAccount;
        public CreateAccountView()
        {
            InitializeComponent();
        }

        
        public void registrationSucceded()
        {
            
           
            textBoxPass.Text = "";
            textBoxPass2.Text = "";
            textBoxLogin.Text = "";
            label4.Text = "Rejestracja się udała";
            buttonRegister.Visible = false;

        }

        public void registrationFailed()
        {
            
            label4.Text = "Rejestracja zakończona niepowodzeniem";
        }

        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {
            label4.Text = "";
        }

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {
            label4.Text = "";
        }

        private void textBoxPass2_TextChanged(object sender, EventArgs e)
        {
            label4.Text = "";
        }

        private void CreateAccountView_FormClosing(object sender, FormClosingEventArgs e)
        {
            label4.Text = "";
            textBoxPass.Text = "";
            textBoxPass2.Text = "";
            textBoxLogin.Text = "";
            buttonRegister.Visible = true;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxPass2.Text != textBoxPass.Text)
            {
                //różne hasła 
                label4.Text = "Podane hasła są różne";
                return;
            }
            if (textBoxPass.Text == "")
            {
                label4.Text = "Podano puste hasło";
                return;
            }
            if (textBoxLogin.Text == "")
            {
                label4.Text = "Podano pusty login";
                return;
            }
            var args = new CreateAccountArgs(textBoxLogin.Text, textBoxPass.Text);
            var handler = createAccount;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void textBoxPass2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar ==(char) Keys.Enter)
            {
                button3_Click(this, null);
                e.Handled = true;
            }
        }
    }
}
