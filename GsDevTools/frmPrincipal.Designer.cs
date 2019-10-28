namespace GsDevTools
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.formatarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encontrarArquivoErroNG = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirClipboardTool = new System.Windows.Forms.ToolStripMenuItem();
            this.sair = new System.Windows.Forms.ToolStripMenuItem();
            this.lblVar = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ucBorders1 = new GsDevTools.ucBorders();
            this.abrirGlobalizacao = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "GsDevTools";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formatarToolStripMenuItem,
            this.encontrarArquivoErroNG,
            this.abrirClipboardTool,
            this.abrirGlobalizacao,
            this.sair});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(229, 136);
            // 
            // formatarToolStripMenuItem
            // 
            this.formatarToolStripMenuItem.Name = "formatarToolStripMenuItem";
            this.formatarToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.formatarToolStripMenuItem.Text = "Formatar SQL";
            this.formatarToolStripMenuItem.Click += new System.EventHandler(this.formatarToolStripMenuItem_Click);
            // 
            // encontrarArquivoErroNG
            // 
            this.encontrarArquivoErroNG.Name = "encontrarArquivoErroNG";
            this.encontrarArquivoErroNG.Size = new System.Drawing.Size(228, 22);
            this.encontrarArquivoErroNG.Text = "Encontrar arquivo de erro NG";
            this.encontrarArquivoErroNG.Click += new System.EventHandler(this.EncontrarArquivoErroNG_Click);
            // 
            // abrirClipboardTool
            // 
            this.abrirClipboardTool.Name = "abrirClipboardTool";
            this.abrirClipboardTool.Size = new System.Drawing.Size(228, 22);
            this.abrirClipboardTool.Text = "Abrir Clipboard Tool";
            this.abrirClipboardTool.Click += new System.EventHandler(this.abrirClipboardTool_Click);
            // 
            // sair
            // 
            this.sair.Name = "sair";
            this.sair.Size = new System.Drawing.Size(228, 22);
            this.sair.Text = "Sair";
            this.sair.Click += new System.EventHandler(this.sair_Click);
            // 
            // lblVar
            // 
            this.lblVar.AutoSize = true;
            this.lblVar.Font = new System.Drawing.Font("Segoe UI Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVar.ForeColor = System.Drawing.Color.White;
            this.lblVar.Location = new System.Drawing.Point(217, 85);
            this.lblVar.Name = "lblVar";
            this.lblVar.Size = new System.Drawing.Size(41, 32);
            this.lblVar.TabIndex = 2;
            this.lblVar.Text = "{0}";
            this.lblVar.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "DevTools";
            // 
            // ucBorders1
            // 
            this.ucBorders1.BackColor = System.Drawing.Color.DimGray;
            this.ucBorders1.Location = new System.Drawing.Point(0, -6);
            this.ucBorders1.Name = "ucBorders1";
            this.ucBorders1.Size = new System.Drawing.Size(831, 30);
            this.ucBorders1.TabIndex = 0;
            // 
            // abrirGlobalizacao
            // 
            this.abrirGlobalizacao.Name = "abrirGlobalizacao";
            this.abrirGlobalizacao.Size = new System.Drawing.Size(228, 22);
            this.abrirGlobalizacao.Text = "Abrir Globalização";
            this.abrirGlobalizacao.Click += new System.EventHandler(this.AbrirGlobalizacao_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(832, 163);
            this.Controls.Add(this.lblVar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ucBorders1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucBorders ucBorders1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem formatarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sair;
        private System.Windows.Forms.ToolStripMenuItem abrirClipboardTool;
        private System.Windows.Forms.Label lblVar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem encontrarArquivoErroNG;
        private System.Windows.Forms.ToolStripMenuItem abrirGlobalizacao;
    }
}

