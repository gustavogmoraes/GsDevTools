using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using WK.Libraries.SharpClipboardNS;

namespace GSDevTools
{
    public partial class ucItem : MetroUserControl
    {
        public ucItem()
        {
            InitializeComponent();
        }

        private string ConvertType(SharpClipboard.ContentTypes type)
        {
            switch (type)
            {
                case SharpClipboard.ContentTypes.Text:
                    return "Texto";
                case SharpClipboard.ContentTypes.Image:
                    return "Imagem";
                case SharpClipboard.ContentTypes.Files:
                    return "Arquivo";
                case SharpClipboard.ContentTypes.Other:
                    return "Outro";
            }

            return string.Empty;
        }

        public ClipboardItem Item { get; set; }

        public ucItem(ClipboardItem item)
        {
            InitializeComponent();
            Item = item;

            lblHorario.Text = item.Horario.ToString("dd/MM/yyyy hh:mm:ss");
            lblTipo.Text = ConvertType(item.TipoDeDados);

            if (item.TipoDeDados == SharpClipboard.ContentTypes.Text)
            {
                txtSample.Text = (string)item.Dados;
            }
            else
            {
                txtSample.Text = string.Empty;
            }
        }

        private void BtnCopiar_Click(object sender, EventArgs e)
        {
            ServicoClipboard.Habilitado = false;
            Thread.Sleep(TimeSpan.FromSeconds(1));

            switch (Item.TipoDeDados)
            {
                case SharpClipboard.ContentTypes.Text:
                    Clipboard.SetText((string)Item.Dados);
                    break;
                case SharpClipboard.ContentTypes.Image:
                    Clipboard.SetImage(new Bitmap((string)Item.Dados));
                    break;
                case SharpClipboard.ContentTypes.Files:
                    var collection = new StringCollection();
                    collection.AddRange(((IList)Item.Dados).Cast<string>().ToArray());

                    Clipboard.SetFileDropList(collection);
                    break;
                case SharpClipboard.ContentTypes.Other:
                    Clipboard.SetDataObject(Item.Dados);
                    break;
            }

            ServicoClipboard.Habilitado = true;
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            using (var conexao = Persistencia.AbraConexao())
            {
                var collection = conexao.ObtenhaCollectionClipboardItem();
                collection.Delete(Item.Id);

                Persistencia.ChangedClipboardCollection(Item, true);
            }
        }
    }
}
