using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSDevTools
{
    public partial class frmClipboardTool : MetroForm
    {
        public frmClipboardTool()
        {
            InitializeComponent();
        }

        private void MetroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            ServicoClipboard.Habilitado = metroToggle1.Checked;
        }

        private void FrmClipboardTool_Load(object sender, EventArgs e)
        {
            if (Persistencia.ObtenhaConfiguracao().ClipboardHabilitada)
            {
                metroToggle1.Checked = true;
            }

            RefreshList();
        }

        public void RefreshList()
        {
            flowLayoutPanel1.Controls.OfType<Control>().ToList().ForEach(flowLayoutPanel1.Controls.Remove);

            using (var persistencia = Persistencia.AbraConexao())
            {
                var lista = persistencia.ObtenhaCollectionClipboardItem().FindAll().OrderByDescending(x => x.Horario).ToList();

                lista.ForEach(item => flowLayoutPanel1.Controls.Add(new ucItem(item)));
            }
        }

        public void RefreshSingleItem(ClipboardItem item, bool remove = false)
        {
            if (remove)
            {
                var itemToRemove = flowLayoutPanel1.Controls.OfType<ucItem>().FirstOrDefault(x => x.Item.Id == item.Id);
                flowLayoutPanel1.Controls.Remove(itemToRemove);

                return;
            }

            var newUcItem = new ucItem(item);
            flowLayoutPanel1.Controls.Add(newUcItem);
            flowLayoutPanel1.Controls.GetChildIndex(newUcItem);
            flowLayoutPanel1.Controls.SetChildIndex(newUcItem, 0);
        }

        private void BtnLimparConteudo_Click(object sender, EventArgs e)
        {
            using (var persistencia = Persistencia.AbraConexao())
            {
                persistencia.ObtenhaCollectionClipboardItem().Delete(x => true);
            }

            RefreshList();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void FrmClipboardTool_FormClosed(object sender, FormClosedEventArgs e)
        {
            Persistencia.FormClipboardTool.Dispose();
            Persistencia.FormClipboardTool = null;
        }
    }
}
