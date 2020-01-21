using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualEffects;
using VisualEffects.Animations.Effects;

namespace GSDevTools
{
    public partial class frmClipboardTool : MetroForm
    {
        public frmClipboardTool()
        {
            InitializeComponent();
            flowLayoutPanel1.MouseWheel += FlpMouseWheel;
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
            flowLayoutPanel1.Controls.Clear();

            using (var persistencia = Persistencia.AbraConexao())
            {
                var lista = persistencia.ObtenhaCollectionClipboardItem()
                    .FindAll()
                    .Take(50)
                    .OrderByDescending(x => x.Horario)
                    .ToList();

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

        private void FlpMouseWheel(object sender, MouseEventArgs e)
        {
            HandleScrollDown();
        }

        private void FlowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {
            // Means component is being scrolled down
            if (e.NewValue - e.OldValue > 0)
            {
                HandleScrollDown();
            }
        }

        private void HandleScrollDown()
        {
            // When controlCount is 50, range is 500
            // So range must always be controlCount * 10;
            var controlCount = flowLayoutPanel1.Controls.Count;
            var range = Convert.ToInt32(controlCount * 7.5);
            
            if (IsNear(flowLayoutPanel1.VerticalScroll.Value, flowLayoutPanel1.VerticalScroll.Maximum, range))
            {
                DoubleBuffered = true;
                SuspendLayout();
                using (var persistencia = Persistencia.AbraConexao())
                {
                    var lista = persistencia.ObtenhaCollectionClipboardItem()
                        .FindAll()
                        .Skip(controlCount)
                        .Take(20)
                        .OrderByDescending(x => x.Horario)
                        .ToList();

                    lista.ForEach(item =>
                    {
                        flowLayoutPanel1.Controls.Add(new ucItem(item));
                        //Thread.Sleep(2);
                    });
                }
                ResumeLayout();
            }
        }

        private string GetScrollDirection(ScrollEventArgs e)
        {
            return e.NewValue - e.OldValue > 0 ? "Down" : "Up";
        }

        private bool IsNear(int valueToCheck, int valueToReach, int range)
        {
            return valueToCheck + range >= valueToReach;
        }
    }
}
