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
    public partial class LogView : Form
    {
        public event EventHandler<TryToLogInEventArgs> LogInTry;
        public LogView()
        {
            InitializeComponent();
        }

        private void LogView_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var args = new TryToLogInEventArgs(textBox1.Text,textBox2.Text);
            textBox1.Text = "";
            textBox2.Text = "";
            var handler = LogInTry;
            if (handler != null)
            {
                handler(this, args);
            }
           Hide();
        }
    }
}
