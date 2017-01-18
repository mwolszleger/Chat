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
            view.CreateAccount += View_CreateAccount;
            model.ConnectionChanged += Model_ConnectionChanged;
            model.MessageRecieved += Model_MessageRecieved;
            model.NewUserAdded += Model_NewUser;
            model.ChangedUser += Model_ChangedUser;
            model.ConversationStart += Model_ConversationStart;
            model.RegistrationResulted += Model_RegistrationResult;
            model.LogResult += Model_LogResult;
            
        }

        private void Model_LogResult(object sender, LogResultEventArgs e)
        {
            if (e.Result)
            {
                view.SetLogInSucceeded(e.Login);
            }
            else
            {
                view.SetLogInFailed();
            }
        }

        private void View_CreateAccount(object sender, CreateAccountArgs e)
        {
            model.RegisterUser(e.Login,e.Password);
        }

        private void Model_RegistrationResult(object sender, bool e)
        {
            view.RegistrationResult(e);
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
            view.NewUser(e.Login,e.Logged);
        }

        private void Model_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
           
            view.DisplayMessage(e.Message,e.Author,e.Id);
        }

        private void Model_ConnectionChanged(object sender, bool e)
        {
            if (e)
            {
               
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
            model.Close();
        }

        private void View_ConnectionTry(object sender, TryToConnectEventArgs e)
        {
            model.ConnectToServer(e.Login,e.Password);
        }
    }
}
