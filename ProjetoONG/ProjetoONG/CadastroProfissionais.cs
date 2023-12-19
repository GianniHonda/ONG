using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoONG
{
    public partial class CadastroProfissionais : Form
    {
        public CadastroProfissionais()
        {
            InitializeComponent();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            tbNome.Text = string.Empty;
            tbEndereco.Text = string.Empty;
            tbArea.Text = string.Empty;

            mtbTelefone.Clear();
            mtbHorario1.Clear();
            mtbHorario2.Clear();

            LimparSelecoesCheckedListBox(clbSemana);
        }

        private void LimparSelecoesCheckedListBox(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }
    }
}
