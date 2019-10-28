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

        private void TrateDateTime(IList<Variavel> variaveis)
        {
            foreach (var variavelDt in variaveis.Where(x => x.DbType.Contains("datetime")))
            {
                variavelDt.Valor = $"CONVERT(DATETIME2, '{variavelDt.Valor}')";
            }
        }

        private string FormateHibernate(string texto)
        {
            var splitted = texto.Split(new[] { ",N'" }, StringSplitOptions.None);

            var preSelect = RemovaEspacosBrancos(Trate(splitted.First()));
            var select = preSelect.Remove(preSelect.LastIndexOf("'", StringComparison.Ordinal)).Replace("exec sp_executesql", string.Empty);

            var variaveis = ObtenhaVariaveis(texto);

            TrateDateTime(variaveis);

            foreach (var item in variaveis)
            {
                select = select.Replace(item.Nome, item.Valor);
            }

            var textoParaFormatar = $"{select.Replace("''", "'")}".Trim();

            return textoParaFormatar;
        }

        public class Variavel
        {
            public string DbType { get; set; }

            public string Nome { get; set; }

            public string Valor { get; set; }
        }

        private Dictionary<string, string> ObtenhaDbTypesVariaveis(string texto)
        {
            var splitted = texto.Split(',');

            var retorno = new Dictionary<string, string>();
            foreach (var item in splitted)
            {
                if (item == splitted.Last())
                    continue;
                retorno.Add(item.Split(' ').First().Trim(), item.Split(' ').Last().Trim());
            }

            return retorno;
        }

        public static string[] SplitAt(string source, params int[] index)
        {
            index = index.Distinct().OrderBy(x => x).ToArray();
            string[] output = new string[index.Length + 1];
            int pos = 0;

            for (int i = 0; i < index.Length; pos = index[i++])
                output[i] = source.Substring(pos, index[i] - pos);

            output[index.Length] = source.Substring(pos);
            return output;
        }

        private IList<Variavel> ObtenhaVariaveis(string texto)
        {
            var text = texto.Split(new[] { ",N'" }, StringSplitOptions.None).Last();
            var indicePraSplit = text.LastIndexOf("@p0", StringComparison.Ordinal);
            text = SplitAt(text, indicePraSplit).First();
            var dbTypesVariaveis = ObtenhaDbTypesVariaveis(text);

            var setDasVariaveis = RemovaEspacosBrancos(ObtenhaSetVariaveis(texto.Substring(texto.LastIndexOf("@p0", StringComparison.Ordinal))));
            var lista = setDasVariaveis.Split(new[] { "SET" }, StringSplitOptions.None).ToList();
            lista.RemoveAt(0);

            var variaveis = new List<Variavel>();
            foreach (var item in lista)
            {
                var splitt = item.Split('=');
                variaveis.Add(new Variavel
                {
                    Nome = splitt.First().Trim(),
                    Valor = splitt.Last().Remove(splitt.Last().LastIndexOf('\n')).Trim(),
                    DbType = dbTypesVariaveis[splitt.First().Trim()]
                });
            }

            return variaveis;
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
                if (item.Count(x => x == "'".ToArray().FirstOrDefault()) == 2)
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
