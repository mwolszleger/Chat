using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class User
    {
        public string login { get; private set; }
        public bool logged { get; set; }
        public User(string login, bool logged)
        {
            this.login = login;
            this.logged = logged;
        }
    }
}
