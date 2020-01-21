using GSDevTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

            if (Persistencia.ObtenhaConfiguracao().ClipboardHabilitada)
            {
                ServicoClipboard.Habilitado = true;
            }
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

                            switch (dialogResult)
                            {
                                case DialogResult.Yes:
                                    Invoke((MethodInvoker)delegate
                                    {
                                        lblVar.Text = "Atualizando...";
                                    });

                                    await servico.AtualizeParaUltimaVersao();
                                    Environment.Exit(0);
                                    break;
                                case DialogResult.No:
                                    // Faz nada
                                    break;
                            }
                        }
                        else
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                lblVar.Text = "Nenhuma atualização disponível...";
                            });

                            Thread.Sleep(2000);
                        }
                    }
                });
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
            if (Persistencia.FormClipboardTool != null)
            {
                Persistencia.FormClipboardTool.Show();
                return;
            }

            Persistencia.FormClipboardTool = new frmClipboardTool();
            Persistencia.FormClipboardTool.Show();
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

        private void EncontrarArquivoErroNG_Click(object sender, EventArgs e)
        {
            var nomeArquivo = Clipboard.GetText().Trim();
            if (string.IsNullOrEmpty(nomeArquivo) || !nomeArquivo.Contains("_"))
            {
                Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.BalloonTipText = "Não foi encontrado nenhum arquivo correspondente";
                    notifyIcon1.ShowBalloonTip(3000);
                });
                return;
            }

            if (nomeArquivo.EndsWith("."))
            {
                nomeArquivo = nomeArquivo.Remove(nomeArquivo.Length);
            }

            var caminhoErrosLGC = @"C:\Program Files (x86)\LG Informatica\LGComponentes\Erros";
            var caminhoErrosGente = @"C:\Program Files (x86)\LG Informatica\MyWay\Projetos\Gente\Erros";

            var foundList = new List<string>();

            var arquivosLGC = Directory.GetFiles(caminhoErrosLGC);
            var arquivosGente = Directory.GetFiles(caminhoErrosGente);

            var fileList = new List<string>();
            fileList.AddRange(arquivosLGC);
            fileList.AddRange(arquivosGente);

            foreach (var arquivo in fileList)
            {
                if (arquivo.Contains(nomeArquivo))
                {
                    foundList.Add(arquivo);
                }
            }

            if (foundList.Count > 1)
            {
                Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.BalloonTipText = "Mais de um arquivo encontrado, refine a consulta";
                    notifyIcon1.ShowBalloonTip(3000);
                });

                return;
            }

            if (foundList.Count == 0)
            {
                Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.BalloonTipText = "Não foi encontrado nenhum arquivo correspondente";
                    notifyIcon1.ShowBalloonTip(3000);
                });

                return;
            }

            OpenFile(foundList.First());
        }

        private void OpenFile(string path)
        {
            var pathNotePadPP = @"C:\Program Files\Notepad++\notepad++.exe";

            try
            {
                Process.Start(pathNotePadPP, path);
            }
            catch
            {
                Invoke((MethodInvoker)delegate
                {
                    notifyIcon1.BalloonTipText = "Não foi possível abrir o arquivo com Notepad++";
                    notifyIcon1.ShowBalloonTip(3000);
                });
            }
            
        }

        private void AbrirGlobalizacao_Click(object sender, EventArgs e)
        {
            new frmGlobalizacao().Show();
        }

        public void InicieGlobalizacao(string text)
        {
            new frmPopupGlobalizacao(text).Show();
        }

        private void AbrirDataInsertHelper_Click(object sender, EventArgs e)
        {
            new frmDataInsertHelper().Show();
        }

        public void ShortLink(string text)
        {
            Task.Run(() =>
            {
                var shortenResult = ServicoBitLy.EncurteUrl(text);
                if (shortenResult != null)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        Clipboard.SetText(shortenResult);
                        notifyIcon1.BalloonTipText = "Url encurtado por DevTools via bit.ly!";
                        notifyIcon1.ShowBalloonTip(3000);
                    });
                }
            });
        }

        private void Bitly_Click(object sender, EventArgs e)
        {
            var text = Clipboard.GetText();
            ShortLink(text);
        }
    }
}
