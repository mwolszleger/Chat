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

            model.ConnectionChanged += Model_ConnectionChanged;
            model.MessageRecieved += Model_MessageRecieved;
        }

        private void Model_MessageRecieved(object sender, MessageRecievedEventArgs e)
        {
            view.DisplayMessage(e.Message);
        }

        private void Model_ConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            if (e.Connected)
                view.SetConnectionSucceeded();
            else
                view.SetConnectionError();
        }

        private void View_MessageSend(object sender, MessageSendEventArgs e)
        {
            model.SendTextMessage(e.Message,e.Reciever);
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
