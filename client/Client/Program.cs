using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            Client model = new Client();
            ClientView view = new ClientView();
            Presenter presenter = new Presenter(view, model);


            Application.Run(view);


        }
    }
}
