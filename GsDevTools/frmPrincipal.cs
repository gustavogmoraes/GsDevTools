using GSDevTools;
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

namespace GsDevTools
{
    public partial class frmPrincipal : Form
    {
        // Minimizar e maximizar clicando no ícone na taskbar
        const int WS_MINIMIZEBOX = 0x20000;
        const int CS_DBLCLKS = 0x8;
        protected override CreateParams CreateParams
        {
            get
            {
                var parametrosDeCriacao = base.CreateParams;
                parametrosDeCriacao.Style |= WS_MINIMIZEBOX;
                parametrosDeCriacao.ClassStyle |= CS_DBLCLKS;
                return parametrosDeCriacao;
            }
        }

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void FormateSql(string texto)
        {
            using (var servico = new Servico())
            {
                Task.Run(async () =>
                {
                    var result = await servico.Formate(texto);
                    Invoke((MethodInvoker)delegate
                    {
                        notifyIcon1.BalloonTipText = "Formatado!";
                        notifyIcon1.ShowBalloonTip(3000);

                        Clipboard.SetText(result);
                    });
                });
            }
        }

        public void ColoqueAppNaSystemTray()
        {
            if (WindowState == FormWindowState.Normal || WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Minimized;
            }

            Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
        }

        public async Task VerifiqueSeExistemAtualizacoes()
        {
            var versaoAtual = Meta.Versao;
            using (var servico = new ServicoDeAtualizacao())
            {
                await Task.Run(async () =>
                {
                    Invoke((MethodInvoker)delegate 
                    {
                        lblVar.Text = "Verificando conexão com servidor...";
                    });
                    Thread.Sleep(2000);

                    var conexao = await servico.VerifiqueConexao();
                    if (conexao)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            lblVar.Text = "Verificando se existe atualização de versão...";
                        });

                        Thread.Sleep(2000);
                        var result = await servico.VerifiqueSeExisteAtualizacao(versaoAtual);
                        if (result)
                        {
                            var dialogResult = DialogResult.No;
                            Invoke((MethodInvoker)delegate
                            {
                                dialogResult =
                                MessageBox.Show(
                                    this,
                                    "Existe uma atualização disponível, atualizar?",
                                    "Atualização",
                                    MessageBoxButtons.YesNo);
                            });

                            if (dialogResult == DialogResult.Yes)
                            {
                                Invoke((MethodInvoker)delegate
                                {
                                    lblVar.Text = "Atualizando...";
                                });

                                await servico.AtualizeParaUltimaVersao();
                                Environment.Exit(0);
                            }
                            else if (dialogResult == DialogResult.No)
                            {
                                // Faz nada
                            }
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                lblVar.Text = "Nenhuma atualização disponível...";
                            });

                            Thread.Sleep(2000);

                            return;
                        }
                    }
                });

                return;
            }
        }

        public void InicieClipboardTool()
        {

        }

        private void formatarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var texto = Clipboard.GetText();

            FormateSql(texto);
        }

        private void sair_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void abrirClipboardTool_Click(object sender, EventArgs e)
        {
            new frmClipboardTool().Show();
        }

        private void MudeLabel()
        {
            Invoke((MethodInvoker)delegate
            {
                lblVar.Text = "Iniciando aplicação...\n" +
                              "Disponível na bandeja do sistema";
            });
            
            Thread.Sleep(3000);

            return;
        }

        private async void frmPrincipal_Load(object sender, EventArgs e)
        {
            await VerifiqueSeExistemAtualizacoes();

            await Task.Run(() => MudeLabel());

            ucBorders1.btnMinimizar_Click(this, null);
        }
    }
}
