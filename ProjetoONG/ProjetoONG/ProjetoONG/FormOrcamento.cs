using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoONG
{
    public partial class FormOrcamento : Form
    {
        public FormOrcamento()
        {
            InitializeComponent();
            ValoresMonetarios.Inicializar();
            AtualizarTotalAtivos();
        }

        private void btnAdReceita_Click(object sender, EventArgs e)
        {
            //Verifique se o valor inserido é válido
            if (decimal.TryParse(tbAdReceita.Text, out decimal valor))
            {
                //Adicione o valor do ativo
                ValoresMonetarios.AdicionarOutraReceita(valor);
                Console.WriteLine("Outra receita adicionada com sucesso.");

                //Atualize o texto do label que exibe o total de ativos
                AtualizarTotalAtivos();

                //Limpe o textBox
                LimparTextBoxReceita();
            }
            else
            {
                MessageBox.Show("Por favor, insira um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparTextBoxReceita()
        {
            tbAdReceita.Text = string.Empty;
        }

        private void AtualizarTotalAtivos()
        {
            //Soma os valores das doações monetárias e os valores adicionados diretamente no formulário
            decimal totalAtivos = ValoresMonetarios.CalcularTotalAtivos();
            Console.WriteLine($"Total de Ativos: {totalAtivos}");

            //Atualiza o texto do label que exibe o total de ativos
            lbTotAtivos.Text = totalAtivos.ToString("C");
        }
     
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparTextBoxReceita();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                //Declaração da variável antes do primeiro uso
                decimal valorNovaReceita;

                //Verifica se o valor inserido é válido
                if (decimal.TryParse(tbAdReceita.Text, NumberStyles.Currency, CultureInfo.GetCultureInfo("pt-BR"), out valorNovaReceita))
                {
                    //Adiciona o valor da nova receita 
                    ValoresMonetarios.AdicionarOutraReceita(valorNovaReceita);
                    Console.WriteLine("Nova receita adicionada com sucesso.");

                    //Atualiza o texto do label que exibe o total de ativos
                    AtualizarTotalAtivos();

                    //Limpe o textBox
                    LimparTextBoxReceita();
                }
                else
                {
                    MessageBox.Show("Por favor insira um valor válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; //Saia do método se o valor não for válido
                }

                string diretorioDoacoes = "C:\\Doacoes";
                string caminhoOutrasReceitas = Path.Combine(diretorioDoacoes, "OutrasReceitas.csv");

                //Verifica se o diretório de destino existe, se não, cria o diretório
                if (!Directory.Exists(diretorioDoacoes))
                {
                    Directory.CreateDirectory(diretorioDoacoes);
                }

                //Verifica se o ar2quivo de outras receitas já existe
                bool arquivoExiste = File.Exists(caminhoOutrasReceitas);

                //Abre ou cria o arquivo CSV de outras receitas
                using (var writer = new StreamWriter(caminhoOutrasReceitas, append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    //Escreve os cabeçalhos se o arquivo acabou de ser criado
                    if (!arquivoExiste)
                    {
                        //Escreve o cabeçalho necessário
                        csv.WriteField("DataHora");
                        csv.WriteField("Valor");
                        csv.NextRecord();
                    }

                    //Escreve o valor da nova receita
                    csv.WriteField(DateTime.Now);
                    csv.WriteField(valorNovaReceita);
                    csv.NextRecord();

                    MessageBox.Show("Dados salvos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}