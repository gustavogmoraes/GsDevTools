using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GsDevTools
{
    public partial class ucBorders : UserControl
    {
        #region Draggable Form

        public const int WM_NCLBUTTONDOWN = 0xA1;

        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void ucBorders_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.ParentForm.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        public ucBorders()
        {
            InitializeComponent();
        }

        private frmPrincipal _formPai => (ParentForm as frmPrincipal);

        public void btnMinimizar_Click(object sender, EventArgs e)
        {
            _formPai.InicieClipboardTool();
            _formPai.ColoqueAppNaSystemTray();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
