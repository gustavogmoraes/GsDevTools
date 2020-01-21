using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace GSDevTools
{
    public partial class frmGlobalizacao : MetroForm
    {
        public frmGlobalizacao()
        {
            InitializeComponent();
        }

        private void MetroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            var cfg = Persistencia.ObtenhaConfiguracao();

            cfg.GlobalizacaoHabilitada = metroToggle1.Checked;

            if (cfg.GlobalizacaoHabilitada)
            {
                cfg.CaminhoLgc = txtCaminhoLGC.Text.Trim();
                cfg.PopUpShowTime = TimeSpan.FromSeconds(Convert.ToDouble(txtShowTime.Text));
                cfg.PopUpFadeOutTime = TimeSpan.FromSeconds(Convert.ToDouble(txtFade.Text));
            }

            Persistencia.AltereConfiguracao(cfg);
        }

        private void FrmGlobalizacao_Load(object sender, EventArgs e)
        {
            var cfg = Persistencia.ObtenhaConfiguracao();

            if (cfg.GlobalizacaoHabilitada)
            {
                metroToggle1.Checked = true;
                txtCaminhoLGC.Text = cfg.CaminhoLgc;
                txtShowTime.Text = cfg.PopUpShowTime.Seconds.ToString();
                txtFade.Text = cfg.PopUpFadeOutTime.Seconds.ToString();

                if (string.IsNullOrEmpty(txtShowTime.Text) || txtShowTime.Text == "0")
                {
                    txtShowTime.Text = "5";
                }

                if (string.IsNullOrEmpty(txtFade.Text) || txtFade.Text == "0")
                {
                    txtShowTime.Text = "3";
                }
            }
        }

        private void TxtShowTime_Click(object sender, EventArgs e)
        {

        }
    }
}
