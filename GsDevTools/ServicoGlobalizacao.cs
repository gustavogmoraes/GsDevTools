using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsDevTools;

namespace GSDevTools
{
    public static class ServicoGlobalizacao
    {
        public static frmPrincipal PrincipalReference { get; set; }

        public static void TryExposeGlobalization(string text)
        {
            if (text.Trim().Length > 5 || !text.IsConvertibleToInt32()) return;

            var caminhoLgc = Persistencia.ObtenhaConfiguracao().CaminhoLgc;
            if (string.IsNullOrEmpty(caminhoLgc))
            {
                caminhoLgc = @"C:\Program Files (x86)\LG Informatica\LGComponentes";
            }

            if (!Directory.Exists(caminhoLgc)) return;

            var idiomsPath = Path.Combine(caminhoLgc, "Idiomas");
            if (!Directory.Exists(idiomsPath)) return;

            var ptBrPath = Path.Combine(idiomsPath, "pt-BR.xml");
            if (!File.Exists(ptBrPath)) return;

            var xmlLines = File.ReadAllLines(ptBrPath);
            var resultDictionary = xmlLines.Skip(2).TakeWhile(x => x != xmlLines.Last()).ToDictionary(
                x => Convert.ToInt32(x.Between(@"indice=""", @""" ").Trim()),
                y => y.Between(@"descricao=""", @""" "));

            var code = Convert.ToInt32(text.Trim());
            if (resultDictionary.ContainsKey(code))
            {
                new frmPopupGlobalizacao(resultDictionary[code].Replace("&quot;", @"""")).Show();
            }
        }

        private static bool IsConvertibleToInt32(this string text)
        {
            return int.TryParse(text, out _);
        }

        private static string Between(this string input, string firstString, string lastString)
        {
            return input.Split(new[] { firstString }, StringSplitOptions.None)[1]
                        .Split(new[] { lastString }, StringSplitOptions.None)[0]
                        .Trim();
        }
    }
}
