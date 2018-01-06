using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MESSENGER
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int port = -1;

            string ip = "";

            if (args.Length >= 1)
            {
                if (!args[0].Contains(":"))
                {
                    int.TryParse(args[0].Replace("-", "").Replace(" ", ""), out port);
                }
                else
                {
                    ip = args[0].Replace("-", "");
                }
            }

            if (port != -1)
                Application.Run(new ServerForm(port));
           else
                Application.Run(new MessengerForm(ip));
        }
    }
}
