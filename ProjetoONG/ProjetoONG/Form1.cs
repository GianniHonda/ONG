namespace ProjetoONG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cadastroDeProfissionaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //Cria uma inst�ncia do formul�rio que desejo abrir
            CadastroProfissionais cadastroProfissionais = new CadastroProfissionais();

            //Exibe o formul�rio
            cadastroProfissionais.Show();
        }
    }
}
