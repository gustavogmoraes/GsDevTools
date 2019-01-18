using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GsDevTools
{
    public class Servico : IDisposable
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> Formate(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return string.Empty;
            }

            if (texto.StartsWith("exec sp_executesql N'") && texto.Contains("@p0"))
            {
                texto = FormateHibernate(texto);
            }

            var values = 
                new Dictionary<string, string>
                {
                    { "sql", texto },
                    { "reindent", "1" },
                    { "keyword_case", "upper" }
                };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://sqlformat.org/api/v1/format", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(responseString);
            var result = json["result"].ToString();

            return result;
        }

        private string FormateHibernate(string texto)
        {
            var splitted = texto.Split(new string[] { "N'" }, StringSplitOptions.None);

            var setDasVariaveis = RemovaEspacosBrancos(ObtenhaSetVariaveis(texto.Substring(texto.LastIndexOf("@p0"))));

            var declare = RemovaEspacosBrancos(Trate(splitted.Last()));
            var select = RemovaEspacosBrancos(Trate(splitted[1]));
            var textoParaFormatar = $"DECLARE {declare.Remove(declare.LastIndexOf("@p0"))}\n" +
                                    $"{setDasVariaveis}\n" +
                                    $"{select.Replace("''", "'")}";

            return textoParaFormatar;
        }

        private string RemovaEspacosBrancos(string str)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            str = regex.Replace(str, @" ");

            return str;
        }

        private string ObtenhaSetVariaveis(string str)
        {
            var retorno = string.Empty;

            var nStr = str.Split(',');
            foreach (var item in nStr)
            {
                var casoEspecial = false;
                var aux = string.Empty;
                if (item.Where(x => x == "'".ToArray().FirstOrDefault()).Count() == 2)
                {
                    casoEspecial = true;
                    var valor = Between(item, "'", "'").Trim();
                    var nomeVariavel = item.Split('=').FirstOrDefault();

                    aux = $@"{nomeVariavel}='{valor}'";
                }

                retorno += $"SET {(casoEspecial ? aux : item.Trim())}\n";
            }

            retorno = retorno.Replace("go", string.Empty)
                             .Replace("GO", string.Empty)
                             .Replace("Go", string.Empty);

            return retorno;
        }

        private string Trate(string text)
        {
            return text.Replace("',", string.Empty)
                       .Replace("N'", string.Empty);
        }

        private static string Between(string input, string FirstString, string LastString)
        {
            return input.Split(new string[] { FirstString }, StringSplitOptions.None)[1]
                        .Split(new string[] { LastString }, StringSplitOptions.None)[0]
                        .Trim();
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
        // ~Servico() {
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
