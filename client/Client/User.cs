namespace Client
{
    class User
    {
        public string Login { get; private set; }
        public bool Logged { get; set; }
        public User(string login, bool logged)
        {
            this.Login = login;
            this.Logged = logged;
        }
       
    }
}
