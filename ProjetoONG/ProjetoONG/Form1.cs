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
           //Cria uma instância do formulário que desejo abrir
            CadastroProfissionais cadastroProfissionais = new CadastroProfissionais();

            //Exibe o formulário
            cadastroProfissionais.Show();
        }
    }
}
