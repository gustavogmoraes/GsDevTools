using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using VisualEffects;
using VisualEffects.Animations.Effects;
using VisualEffects.Easing;

namespace GSDevTools
{
    public partial class frmPopupGlobalizacao : Form
    {
        public frmPopupGlobalizacao(string text)
        {
            InitializeComponent();
            txtText.Text = text;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private void FadeOutIn(TimeSpan time)
        {
            var animator = Animator.Animate(this, new FormFadeEffect(), EasingFunctions.Linear, 0, Convert.ToInt32(time.TotalMilliseconds), 50);

            Task.Run(() => 
            {
                while (animator.ElapsedMilliseconds < Convert.ToInt32(time.TotalMilliseconds))
                {
                    Thread.Sleep(50);
                }

                Invoke((MethodInvoker) delegate
                {
                    Visible = false;
                    Close();
                });
            });
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtText.Text);
        }

        private void AnimateIn(Point locationToBe)
        {
            Focus();

            var animacao = Animator.Animate(this, new XLocationEffect(), EasingFunctions.QuintEaseOut, locationToBe.X, 1000, 50);

            Task.Run(() =>
            {
                while (animacao.ElapsedMilliseconds < 250)
                {
                    Thread.Sleep(50);
                }
            });
        }

        private void FrmPopupGlobalizacao_Load(object sender, EventArgs e)
        {
            var larguraTela = Screen.PrimaryScreen.WorkingArea.Width;
            var alturaTela = Screen.PrimaryScreen.WorkingArea.Height;

            var larguraForm = Width;
            var alturaForm = Height;

            Opacity = 100;
            Location = new Point(larguraTela, alturaTela - alturaForm);

            var locationToBe = new Point(larguraTela - larguraForm, alturaTela - alturaForm);
            AnimateIn(locationToBe);

            Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                FadeOutIn(TimeSpan.FromSeconds(3));
            });
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtText.Text);
        }
    }
}
