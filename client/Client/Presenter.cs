using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Presenter
    {
        private IClientView view;
        private Client model;
        public Presenter(IClientView view, Client model)
        {
            this.view = view;
            this.model = model;
            view.ConnectionTry +=View_ConnectionTry;
            view.Disconnect += View_Disconnect;
            view.MessageSend += View_MessageSend;
            view.NewConversationStart += View_NewConversationStart;
            model.ConnectionChanged += Model_ConnectionChanged;
            model.MessageRecieved += Model_MessageRecieved;
            model.NewUser += Model_NewUser;
            model.ChangedUser += Model_ChangedUser;
            model.ConversationStart += Model_ConversationStart;   
        }

        private void Model_ConversationStart(object sender, ConversationArgs e)
        {
            view.NewConversation(e.Id,e.Logins);
        }

        private void View_NewConversationStart(object sender, List<string> e)
        {
            model.NewConversationStart(e);
        }

        private void Model_ChangedUser(object sender, UserEventArgs e)
        {
            view.UserChanged(e.Login,e.Logged);
        }

        private void Model_NewUser(object sender, UserEventArgs e)
        {
            view.newUser(e.Login,e.Logged);
        }

        private void Model_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
           
            view.DisplayMessage(e.Message,e.Author,e.Id);
        }

        private void Model_ConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            if (e.Connected)
            {
               view.SetConnectionSucceeded();
            }
            else
            {
                view.SetConnectionError();
            }
        }

        private void View_MessageSend(object sender, MessageSendEventArgs e)
        {
            model.SendTextMessage(e.Message,e.Id);
        }

        private void View_Disconnect(object sender, EventArgs e)
        {
            model.close();
        }

        private void View_ConnectionTry(object sender, TryToConnectEventArgs e)
        {
            model.connectToServer(e.Ip,e.Port,e.Login,e.Password);
        }
    }
}
