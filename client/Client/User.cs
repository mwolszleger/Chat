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
