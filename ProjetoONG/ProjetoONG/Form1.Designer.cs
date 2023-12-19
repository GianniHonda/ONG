namespace ProjetoONG
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            internoToolStripMenuItem = new ToolStripMenuItem();
            cadastroDeProfissionaisToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { internoToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(684, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // internoToolStripMenuItem
            // 
            internoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cadastroDeProfissionaisToolStripMenuItem });
            internoToolStripMenuItem.Name = "internoToolStripMenuItem";
            internoToolStripMenuItem.Size = new Size(57, 20);
            internoToolStripMenuItem.Text = "Interno";
            // 
            // cadastroDeProfissionaisToolStripMenuItem
            // 
            cadastroDeProfissionaisToolStripMenuItem.Name = "cadastroDeProfissionaisToolStripMenuItem";
            cadastroDeProfissionaisToolStripMenuItem.Size = new Size(206, 22);
            cadastroDeProfissionaisToolStripMenuItem.Text = "Cadastro de Profissionais";
            cadastroDeProfissionaisToolStripMenuItem.Click += cadastroDeProfissionaisToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(684, 450);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ONG";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem internoToolStripMenuItem;
        private ToolStripMenuItem cadastroDeProfissionaisToolStripMenuItem;
    }
}
