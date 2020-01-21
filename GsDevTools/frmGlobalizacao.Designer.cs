namespace GSDevTools
{
    partial class frmGlobalizacao
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
            this.metroToggle1 = new MetroFramework.Controls.MetroToggle();
            this.htmlToolTip1 = new MetroFramework.Drawing.Html.HtmlToolTip();
            this.txtCaminhoLGC = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.txtShowTime = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.txtFade = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroToggle1
            // 
            this.metroToggle1.AutoSize = true;
            this.metroToggle1.Location = new System.Drawing.Point(23, 170);
            this.metroToggle1.Name = "metroToggle1";
            this.metroToggle1.Size = new System.Drawing.Size(80, 17);
            this.metroToggle1.TabIndex = 0;
            this.metroToggle1.Text = "Off";
            this.metroToggle1.UseSelectable = true;
            this.metroToggle1.CheckedChanged += new System.EventHandler(this.MetroToggle1_CheckedChanged);
            // 
            // htmlToolTip1
            // 
            this.htmlToolTip1.OwnerDraw = true;
            // 
            // txtCaminhoLGC
            // 
            // 
            // 
            // 
            this.txtCaminhoLGC.CustomButton.Image = null;
            this.txtCaminhoLGC.CustomButton.Location = new System.Drawing.Point(525, 1);
            this.txtCaminhoLGC.CustomButton.Name = "";
            this.txtCaminhoLGC.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtCaminhoLGC.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCaminhoLGC.CustomButton.TabIndex = 1;
            this.txtCaminhoLGC.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCaminhoLGC.CustomButton.UseSelectable = true;
            this.txtCaminhoLGC.CustomButton.Visible = false;
            this.txtCaminhoLGC.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtCaminhoLGC.Lines = new string[] {
        "C:\\Program Files (x86)\\LG Informatica\\LGComponentes\\"};
            this.txtCaminhoLGC.Location = new System.Drawing.Point(23, 88);
            this.txtCaminhoLGC.MaxLength = 32767;
            this.txtCaminhoLGC.Name = "txtCaminhoLGC";
            this.txtCaminhoLGC.PasswordChar = '\0';
            this.txtCaminhoLGC.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCaminhoLGC.SelectedText = "";
            this.txtCaminhoLGC.SelectionLength = 0;
            this.txtCaminhoLGC.SelectionStart = 0;
            this.txtCaminhoLGC.ShortcutsEnabled = true;
            this.txtCaminhoLGC.Size = new System.Drawing.Size(547, 23);
            this.txtCaminhoLGC.TabIndex = 1;
            this.txtCaminhoLGC.Text = "C:\\Program Files (x86)\\LG Informatica\\LGComponentes\\";
            this.txtCaminhoLGC.UseSelectable = true;
            this.txtCaminhoLGC.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCaminhoLGC.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.Location = new System.Drawing.Point(19, 60);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(211, 25);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Caminho LGComponentes";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel2.Location = new System.Drawing.Point(19, 142);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(75, 25);
            this.metroLabel2.TabIndex = 3;
            this.metroLabel2.Text = "Habilitar";
            // 
            // txtShowTime
            // 
            // 
            // 
            // 
            this.txtShowTime.CustomButton.Image = null;
            this.txtShowTime.CustomButton.Location = new System.Drawing.Point(10, 1);
            this.txtShowTime.CustomButton.Name = "";
            this.txtShowTime.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtShowTime.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtShowTime.CustomButton.TabIndex = 1;
            this.txtShowTime.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtShowTime.CustomButton.UseSelectable = true;
            this.txtShowTime.CustomButton.Visible = false;
            this.txtShowTime.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtShowTime.Lines = new string[] {
        "5"};
            this.txtShowTime.Location = new System.Drawing.Point(279, 143);
            this.txtShowTime.MaxLength = 32767;
            this.txtShowTime.Name = "txtShowTime";
            this.txtShowTime.PasswordChar = '\0';
            this.txtShowTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtShowTime.SelectedText = "";
            this.txtShowTime.SelectionLength = 0;
            this.txtShowTime.SelectionStart = 0;
            this.txtShowTime.ShortcutsEnabled = true;
            this.txtShowTime.Size = new System.Drawing.Size(32, 23);
            this.txtShowTime.TabIndex = 4;
            this.txtShowTime.Text = "5";
            this.txtShowTime.UseSelectable = true;
            this.txtShowTime.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtShowTime.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtShowTime.Click += new System.EventHandler(this.TxtShowTime_Click);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel3.Location = new System.Drawing.Point(125, 142);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(269, 25);
            this.metroLabel3.TabIndex = 5;
            this.metroLabel3.Text = "Mostrar popup por        segundos";
            // 
            // txtFade
            // 
            // 
            // 
            // 
            this.txtFade.CustomButton.Image = null;
            this.txtFade.CustomButton.Location = new System.Drawing.Point(10, 1);
            this.txtFade.CustomButton.Name = "";
            this.txtFade.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtFade.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFade.CustomButton.TabIndex = 1;
            this.txtFade.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFade.CustomButton.UseSelectable = true;
            this.txtFade.CustomButton.Visible = false;
            this.txtFade.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtFade.Lines = new string[] {
        "5"};
            this.txtFade.Location = new System.Drawing.Point(257, 181);
            this.txtFade.MaxLength = 32767;
            this.txtFade.Name = "txtFade";
            this.txtFade.PasswordChar = '\0';
            this.txtFade.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFade.SelectedText = "";
            this.txtFade.SelectionLength = 0;
            this.txtFade.SelectionStart = 0;
            this.txtFade.ShortcutsEnabled = true;
            this.txtFade.Size = new System.Drawing.Size(32, 23);
            this.txtFade.TabIndex = 6;
            this.txtFade.Text = "5";
            this.txtFade.UseSelectable = true;
            this.txtFade.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFade.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel4.Location = new System.Drawing.Point(125, 179);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(246, 25);
            this.metroLabel4.TabIndex = 7;
            this.metroLabel4.Text = "Fade popup por        segundos";
            // 
            // frmGlobalizacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 212);
            this.Controls.Add(this.txtFade);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.txtShowTime);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.txtCaminhoLGC);
            this.Controls.Add(this.metroToggle1);
            this.Name = "frmGlobalizacao";
            this.Text = "Globalização";
            this.Load += new System.EventHandler(this.FrmGlobalizacao_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroToggle metroToggle1;
        private MetroFramework.Drawing.Html.HtmlToolTip htmlToolTip1;
        private MetroFramework.Controls.MetroTextBox txtCaminhoLGC;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox txtShowTime;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox txtFade;
        private MetroFramework.Controls.MetroLabel metroLabel4;
    }
}