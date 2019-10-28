using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSDevTools
{
    public class Configuracao
    {
        //LiteDB
        public Guid Id { get; set; }

        public bool ClipboardHabilitada { get; set; }

        public bool GlobalizacaoHabilitada { get; set; }

        public string CaminhoLGC { get; set; }
    }
}
