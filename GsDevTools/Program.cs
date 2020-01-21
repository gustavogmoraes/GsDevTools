using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GsDevTools
{
    static class Program
    {
        public static frmPrincipal InstanciaPrincipal { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InstanciaPrincipal = new frmPrincipal();

            Application.Run(InstanciaPrincipal);
        }
    }
}
