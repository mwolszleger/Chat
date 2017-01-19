using System;
using System.Collections.Generic;
using System.Linq;
namespace Client
{
    class Conversation
    {
        public List<User> users { get; private set; }
        //public User user;
        public Conversation(List<User> users)
        {
            this.users = users;
        }
        public bool IsSingleConversation()
        {
            return users.Count == 1;
        }
        public bool IsTheSame(List<string> logins)
        {
            List<string> conversationsMembersLogins = new List<string>();
            foreach (var item in users)
            {
                conversationsMembersLogins.Add(item.Login);
            }
            return (logins.Count == conversationsMembersLogins.Count) && !logins.Except(conversationsMembersLogins).Any();
        }

    }
}
