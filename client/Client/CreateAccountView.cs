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

        private void CreateAccountView_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                return;
            }
            if (textBoxPass.Text == "")
            {
                //puste hasło
                return;
            }
            if (textBoxLogin.Text == "")
            {
                //pusty login
                return;
            }
            var args = new CreateAccountArgs(textBoxLogin.Text,textBoxPass.Text);           
            var handler = createAccount;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
