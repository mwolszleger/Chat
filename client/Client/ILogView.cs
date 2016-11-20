using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TryToLogInEventArgs : EventArgs
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public TryToLogInEventArgs(string login,string password)
        {
            Login = login;
            Password = password;
        }

    }

    interface ILogView
    {

    }
}
