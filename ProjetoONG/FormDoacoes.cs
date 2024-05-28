using CsvHelper;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProjetoONG
{
    public class Doadores
    {
        public string ID { get; set; }
        public string Doador { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string TipoDePessoa { get; set; }
        public decimal Valor { get; set; }
    }

    public partial class FormDoacoes : Form
    {
        private BindingList<Doadores> doadores;
        private BindingSource bindingSource;
        private string CaminhoDoArquivo = "C:\\Doacoes\\Doacoes.csv";

        public FormDoacoes()
        {
            InitializeComponent();

            // Inicializa a lista de doadores e o BindingSource
            doadores = new BindingList<Doadores>();
            bindingSource = new BindingSource { DataSource = doadores };

            // Vincula o DataGridView ao BindingSource
            dgvDadosDoador.DataSource = bindingSource;

            // Configura as colunas do DataGridView
            dgvDadosDoador.AutoGenerateColumns = false;
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ID", HeaderText = "ID" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Doador", HeaderText = "Doador" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Endereco", HeaderText = "Endereco" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Telefone", HeaderText = "Telefone" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CPF", HeaderText = "CPF" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CNPJ", HeaderText = "CNPJ" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TipoDePessoa", HeaderText = "Tipo de Pessoa" });
            dgvDadosDoador.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Valor", HeaderText = "Valor" });

            // Vincula os eventos aos métodos
            btnSalvar.Click += btnSalvar_Click;
            btnLimpar.Click += btnLimpar_Click;
        }

        // Método para carregar doadores do arquivo CSV
        private void CarregarDoadores()
        {
            try
            {
                using (var reader = new StreamReader(CaminhoDoArquivo))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Doadores>().ToList();
                    foreach (var record in records)
                    {
                        doadores.Add(record);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Arquivo não encontrado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os doadores: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para salvar os dados no arquivo CSV
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists("C:\\Doacoes"))
                {
                    Directory.CreateDirectory("C:\\Doacoes");
                }

                string caminhoDoacoes = Path.Combine("C:\\Doacoes", "Doacoes.csv");
                bool arquivoExiste = File.Exists(caminhoDoacoes);

                using (var writer = new StreamWriter(caminhoDoacoes, append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    if (!arquivoExiste)
                    {
                        csv.WriteHeader<Doadores>();
                        csv.NextRecord();
                    }

                    foreach (var doador in doadores)
                    {
                        csv.WriteRecord(doador);
                        csv.NextRecord();
                    }

                    writer.WriteLine("# Valores Monetários");
                    writer.WriteLine($"Total de ativos: {ValoresMonetarios.CalcularTotalAtivos()}");

                    csv.WriteField("TipoDeDoacao");
                    foreach (var item in clbTipoDoacao.CheckedItems)
                    {
                        csv.WriteField(item.ToString());
                    }
                    csv.NextRecord();

                    csv.WriteField("Frequencia");
                    foreach (var item in clbFrequencia.CheckedItems)
                    {
                        csv.WriteField(item.ToString());
                    }
                    csv.NextRecord();

                    csv.WriteField("Valor");
                    csv.WriteField(tbValor.Text);
                    csv.NextRecord();

                    csv.WriteField("ItensDoados");
                    csv.WriteField(tbItemDoado.Text);
                    csv.NextRecord();

                    csv.WriteField("FrequenciaDeDoacoes");
                    csv.WriteField(cbFrequenciaDoacoes.SelectedItem?.ToString());
                    csv.NextRecord();
                }

                MessageBox.Show("Dados salvos com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para limpar os campos do formulário
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            tbNomeDoador.Text = string.Empty;
            tbValor.Text = string.Empty;
            tbItemDoado.Text = string.Empty;
            cbFrequenciaDoacoes.SelectedIndex = -1;
            dgvDadosDoador.Rows.Clear();

            LimparSelecoesCheckedListBox(clbTipoDoacao);
            LimparSelecoesCheckedListBox(clbFrequencia);
        }

        // Método auxiliar para limpar seleções de CheckedListBox
        private void LimparSelecoesCheckedListBox(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }
    }
}
