using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GSDevTools
{
    public class ServicoDeAtualizacao : IDisposable
    {
        public KeyValuePair<string, string> UltimaVersao { get; private set; }

        public async Task<bool> VerifiqueSeExisteAtualizacao(string versaoDoSistema)
        {
            var request = (HttpWebRequest)WebRequest.Create(Meta.VersionamentoUrl);
            request.Timeout = Meta.VersionamentoTimeout;
            request.Method = "GET"; // Uri do one drive não aceita HEAD

            var retries = 0;
            while (retries < Meta.ConexaoRetries)
            {
                try
                {
                    using (var webClient = new WebClient())
                    {
                        var responseText = await webClient.DownloadStringTaskAsync(Meta.VersionamentoUrl);
                        var ultimaVersao = ObtenhaUltimaVersao(ObtenhaVersionamento(responseText));

                        UltimaVersao = ultimaVersao;

                        return Convert.ToDouble(ultimaVersao.Key) > Convert.ToDouble(Meta.Versao);
                    }

                }
                catch (WebException) { }

                retries++;
            }

            return false;
        }

        private KeyValuePair<string, string> ObtenhaUltimaVersao(Dictionary<string, string> versionamento)
        {
            var maior = versionamento.Keys.Max();

            return versionamento.FirstOrDefault(x => x.Key == maior.ToString(CultureInfo.InvariantCulture));
        }

        private Dictionary<string, string> ObtenhaVersionamento(string texto)
        {
            var linhas = texto.Split('\n');
            var versionamentos = new Dictionary<string, string>();

            foreach(var linha in linhas)
            {
                var splitted = linha.Split('|');
                versionamentos.Add(splitted[0].Trim(), splitted[1].Trim());
            }

            return versionamentos;
        }

        public async Task<bool> VerifiqueConexao()
        {
            var request = (HttpWebRequest)WebRequest.Create(Meta.ConexaoUrl);
            request.Timeout = Meta.ConexaoTimeout;
            request.Method = "GET"; // Uri do one drive não aceita HEAD

            var retries = 0;
            while(retries < Meta.ConexaoRetries)
            {
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        return response.StatusCode == HttpStatusCode.OK;
                    }
                }
                catch (WebException) { }

                retries++;
            }

            return false;
        }

        public async Task AtualizeParaUltimaVersao()
        {
            var diretorioRaiz = AppDomain.CurrentDomain.BaseDirectory;
            var diretorioVersoes = diretorioRaiz + @"\Versoes";
            var arquivoUltimaVersao = diretorioVersoes + $@"\{UltimaVersao.Key}.zip";

            if (!Directory.Exists(diretorioVersoes))
            {
                Directory.CreateDirectory(diretorioVersoes);
            }

            if (File.Exists(arquivoUltimaVersao))
            {
                File.Delete(arquivoUltimaVersao);
            }

            await BaixeArquivo(UltimaVersao.Value, arquivoUltimaVersao);

            ExecuteAppUpdater(diretorioRaiz, arquivoUltimaVersao);
        }

        private void ExecuteAppUpdater(string diretorioRaiz, string arquivoAtualizacao)
        {
            var executavelAppUpdater = $@"{diretorioRaiz}\{Meta.AppUpdaterDiretorio}\{Meta.AppUpdaterExecutavel}";
            var arguments = $@"-c {arquivoAtualizacao} -d {diretorioRaiz} -n {Meta.GSDevToolsExecutavel}";
            var processInfo = new ProcessStartInfo(executavelAppUpdater, arguments) { WindowStyle = ProcessWindowStyle.Hidden };

            Process.Start(processInfo);
        }

        private async Task BaixeArquivo(string uri, string caminho)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Timeout = Meta.AtualizacaoTimeout;
            request.Method = "GET";

            var retries = 0;
            while (retries < Meta.AtualizacaoRetries)
            {
                try
                {
                    using (var response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var client = new WebClient())
                            {
                                await client.DownloadFileTaskAsync(uri, caminho);

                                return;
                            }
                        }
                    }
                }
                catch (WebException) { }

                retries++;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ServicoDeAtualizacao() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
