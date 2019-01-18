using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSDevTools
{
    public class Dados
    {
        public Guid IdDoUsuario { get; set; }

        public string NomeDoUsuario { get; set; }

        public Dictionary<string, DateTime> ListaDeVersoes { get; set; }
    }
}
