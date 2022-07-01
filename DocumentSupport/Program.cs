using System;
using System.Collections.Generic;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocumentSupport
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
            Application.SetCompatibleTextRenderingDefault(false);

            //DialogResult res;
            //Form1 f1 = new Form1();
            //res = f1.ShowDialog();
            //f1.Dispose();

            //if (res == DialogResult.OK)
            //{
                Application.Run(new MainForm());
            //}
            
        }
    }
}
