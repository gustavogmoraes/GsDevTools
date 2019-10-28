using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WK.Libraries.SharpClipboardNS;

namespace GSDevTools
{
    public class ClipboardItem
    {
        //LiteDB
        public Guid Id { get; set; }

        public SharpClipboard.ContentTypes TipoDeDados { get; set; }

        public DateTime Horario { get; set; }

        public object Dados { get; set; }

        public ClipboardItem(SharpClipboard.ContentTypes contentType, SharpClipboard clipboard)
        {
            Id = Guid.NewGuid();
            Horario = DateTime.Now;
            TipoDeDados = contentType;

            switch (contentType)
            {
                case SharpClipboard.ContentTypes.Text:
                    Dados = clipboard.ClipboardText;
                    break;
                case SharpClipboard.ContentTypes.Image:
                    Dados = Persistencia.SaveImageAndGetPath(Id, clipboard.ClipboardImage);
                    break;
                case SharpClipboard.ContentTypes.Files:
                    Dados = clipboard.ClipboardFiles;
                    break;
                case SharpClipboard.ContentTypes.Other:
                    Dados = clipboard.ClipboardObject;
                    break;
            }
        }

        // Não apagar
        public ClipboardItem()
        {
            
        }
    }
}
