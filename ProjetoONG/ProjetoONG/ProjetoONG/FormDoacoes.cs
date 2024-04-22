using CsvHelper;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ProjetoONG;
using CadDoadores = ProjetoONG.FormCadDoador;

namespace ProjetoONG
{
    public partial class FormDoacoes : Form
    {
        private string diretorioPadrao = "C:\\Doador";
        private string nomeArquivo = "DadosDoador.csv";
        private string CaminhoDoArquivo => Path.Combine(diretorioPadrao, nomeArquivo);

        //Lista para armazenar os doadores
        private List<FormCadDoador.Doadores> doadores = new List<FormCadDoador.Doadores>();

        public FormDoacoes()
        {
            InitializeComponent();
            ValoresMonetarios.Inicializar();
            AtualizarTotalAtivos();

            //Adiciona as colunas ao DataGridView
            dgvDadosDoador.Columns.Add("ID", "ID");
            dgvDadosDoador.Columns.Add("Doador", "Doador");
            dgvDadosDoador.Columns.Add("Endereco", "Endereco");
            dgvDadosDoador.Columns.Add("Telefone", "Telefone");
            dgvDadosDoador.Columns.Add("CPF", "CPF");
            dgvDadosDoador.Columns.Add("CNPJ", "CNPJ");
            dgvDadosDoador.Columns.Add("TipoDePessoa", "TipoDePesssoa");
            dgvDadosDoador.Columns.Add("Valor", "Valor");

            CarregarDoadores();

        }

        //Método para carregar doadores do arquivo CSV
        private void CarregarDoadores()
        {
            try
            {
                using (var reader = new StreamReader(CaminhoDoArquivo))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<FormCadDoador.Doadores>();
                    doadores.AddRange(records);
                    Console.WriteLine("Doadores carregados com sucesso.");
                }

                //Calcula o total de doações monetárias
                decimal totalAtivos = ValoresMonetarios.CalcularTotalAtivos();
                Console.WriteLine($"Total de ativos: {totalAtivos}");

                //Atualiza o total de ativos (doação monetária + ativos adicionais)
                AtualizarTotalAtivos();
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

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtém a palavra-chave de busca
            string palavraChave = tbNomeDoador.Text.Trim();

            // Filtra os doadores com base na palavra-chave de busca
            List<FormCadDoador.Doadores> doadoresFiltrados = doadores.Where(d => d.Doador.Contains(palavraChave)).ToList();

            // Abre o formulário de escolha de doador somente se houver doadores filtrados
            if (doadoresFiltrados.Any())
            {
                FormEscolhaDoador formEscolhaDoador = new FormEscolhaDoador(doadoresFiltrados);

                // Exibe o formulário com o diálogo modal
                if (formEscolhaDoador.ShowDialog() == DialogResult.OK)
                {
                    // Obtém o doador selecionado no formulário Consulta Doador
                    FormCadDoador.Doadores doadorSelecionado = formEscolhaDoador.DoadorSelecionado;

                    // Se o doador selecionado não for nulo, exibe os detalhes no DataGridView
                    if (doadorSelecionado != null)
                    {
                        ExibirDetalhesDoador(new List<FormCadDoador.Doadores> { doadorSelecionado });
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhum doador encontrado com a palavra-chave fornecida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExibirDetalhesDoador(List<FormCadDoador.Doadores> doadoresEncontrados)
        {
            //Limpa o DataGridView antes de adicionar o novo doador
            dgvDadosDoador.Rows.Clear();

            foreach (var doador in doadoresEncontrados)
            {
                //Adiciona os detalhes do doador ao DataGridView
                dgvDadosDoador.Rows.Add(doador.ID, doador.Doador, doador.Endereco, doador.Telefone, doador.CPF, doador.CNPJ, doador.TipoDePessoa);
            }
        }

        private void AtualizarTotalAtivos()
        {
            decimal totalAtivos = ValoresMonetarios.CalcularTotalAtivos();
            lbTotAtivos.Text = totalAtivos.ToString("C");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            decimal valorDoacao;
            if (decimal.TryParse(tbValor.Text, out valorDoacao))
            {
                ValoresMonetarios.AdicionarDoacaoMonetaria(valorDoacao);
                Console.WriteLine("Doação monetária salva com sucesso.");
                AtualizarTotalAtivos();
            }
            else
            {
                MessageBox.Show("Por favor, insira um valor válido.\", \"Erro\", MessageBoxButtons.OK, MessageBoxIcon.Error");
            }
            try
            {
                //Verifica se o diretório de destino existe, se não, cria o diretório
                if (!Directory.Exists("C:\\Doacoes"))
                {
                    Directory.CreateDirectory("C:\\Doacoes");
                }

                //Define o caminho do arquivo de dõações
                string caminhoDoacoes = Path.Combine("C:\\Doacoes", "Doacoes.csv");

                //Verifica se o arquivo já existe
                bool arquivoExiste = File.Exists(caminhoDoacoes);

                //Cria ou abre o arquivo CSV de doações
                using (var writer = new StreamWriter(caminhoDoacoes, append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    //Escreve os cabeçalhos se o arquivo acabou de ser criado
                    if (!arquivoExiste)
                    {
                        csv.WriteHeader<FormCadDoador.Doadores>();
                        csv.NextRecord();
                    }

                    //Escreve os dados dos doadores do DataGridView
                    foreach (DataGridViewRow row in dgvDadosDoador.Rows)
                    {
                        //Cria um objeto do tipo Doadores com os dados da linha do DataGridView
                        var doador = new FormCadDoador.Doadores
                        {
                            ID = row.Cells["ID"].Value?.ToString(),
                            Doador = row.Cells["Doador"].Value?.ToString(),
                            Endereco = row.Cells["Endereco"].Value?.ToString(),
                            Telefone = row.Cells["Telefone"].Value?.ToString(),
                            CPF = row.Cells["CPF"].Value?.ToString(),
                            CNPJ = row.Cells["CNPJ"].Value?.ToString(),
                            TipoDePessoa = row.Cells["TipoDePessoa"].Value?.ToString()
                        };

                        //Verifica se a doação é monetária
                        decimal valorDoacaoMonetaria;
                        if (decimal.TryParse(row.Cells["Valor"].Value?.ToString(), out valorDoacaoMonetaria))
                        {
                            ValoresMonetarios.AdicionarDoacaoMonetaria(valorDoacaoMonetaria);
                        }

                        //Escreve o registro no arquivo CSV
                        csv.WriteRecord(doador);
                        csv.NextRecord();
                    }

                    //Salva os outros dados do formulário
                    //Escreve o tipo de doação
                    csv.WriteField("TipoDeDoacao");
                    foreach (var item in clbTipoDoacao.CheckedItems)
                    {
                        csv.WriteField(item.ToString());
                    }
                    csv.NextRecord();

                    //Escreva a frequência das doações selecionadas
                    csv.WriteField("Frequencia");
                    foreach (var item in clbFrequencia.CheckedItems)
                    {
                        csv.WriteField(item.ToString());
                    }
                    csv.NextRecord();

                    //Escreve o valor da doação
                    decimal valorDoacaoMonetaris;
                    if(decimal.TryParse(tbValor.Text, out valorDoacao))
                    {
                        csv.WriteField("Valor");
                        csv.WriteField(valorDoacao);
                        csv.NextRecord();
                    }

                    //Escreva o item doado
                    csv.WriteField("ItensDoados");
                    csv.WriteField(tbItemDoado.Text);
                    csv.NextRecord();

                    //Escreva a frequência de doações selecionada no comboBox 
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

        private void btnLimpar_Click_1(object sender, EventArgs e)
        {
            tbNomeDoador.Text = string.Empty;
            tbValor.Text = string.Empty;
            tbItemDoado.Text = string.Empty;
            cbFrequenciaDoacoes.SelectedIndex = -1;
            dgvDadosDoador.Rows.Clear();
            for (int i = 0; i < clbTipoDoacao.Items.Count; i++)
            {
                clbTipoDoacao.SetItemChecked(i, false);
            }
            for (int i = 0; i < clbFrequencia.Items.Count; i++)
            {
                clbFrequencia.SetItemChecked(i, false);
            }
        }
    }
}
