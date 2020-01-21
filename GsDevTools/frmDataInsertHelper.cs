using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace GSDevTools
{
    public partial class frmDataInsertHelper : MetroForm
    {
        public frmDataInsertHelper()
        {
            InitializeComponent();
        }

        private void BtnInsert_Click(object sender, EventArgs e)
        {
            var stopwatch = new Stopwatch();

            Task.Run(() =>
            {
                ServicoDataInsert.KeepTimeRunning(stopwatch, this);
            });

            Task.Run(() =>
            {
                stopwatch.Start();
                ServicoDataInsert.Insert(this);
                stopwatch.Stop();
            });
        }
    }
}
