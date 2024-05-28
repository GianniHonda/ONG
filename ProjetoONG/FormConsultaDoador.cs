using CsvHelper;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CsvHelper.Configuration;

namespace ProjetoONG
{
    public partial class FormConsultaDoador : Form
    {
        public FormConsultaDoador()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            string palavraChave = tbNomeDoador.Text.Trim();

            //Verifica se a palavra-chave não está vazia
            if(string.IsNullOrEmpty(palavraChave) )
            {
                MessageBox.Show("Por favor insira um nome para buscar.");
                return;
            }

            //Diretório onde os arquivos CSV serão procurados
            string diretorio = "C:\\Doador";

                try
                {
                    if (Directory.Exists(diretorio))
                    {
                        string[] arquivos = Directory.GetFiles(diretorio, "*.csv", SearchOption.AllDirectories);

                        foreach (string caminhoDoArquivo in arquivos)
                        {
                            try
                            {
                                using (var reader = new StreamReader(caminhoDoArquivo))
                                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
                                {
                                    // Lê os registros do CSV
                                    var records = csv.GetRecords<Doadores>();

                                    foreach (var doador in records)
                                    {
                                        //Verifica se o nome do doador contém a palavra-chave
                                        if (doador.Doador.Contains(palavraChave))
                                        {
                                        //Abre o arquivo se a palavra-chave for encontrada no campo
                                        if (MessageBox.Show("Deseja abrir o arquivo?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            System.Diagnostics.Process.Start("notepad.exe", caminhoDoArquivo);
                                        }
                                            return; //Encerra a busca se encontrar a palavra chave
                                        }
                                    }
                                   
                                }
                            }
                            catch (Exception ex)
                            {
                            MessageBox.Show($"Erro ao ler arquivo {caminhoDoArquivo}: {ex.Message}");
                            }
                        }
                        MessageBox.Show("A palavra-chave não foi encontrada nos arquivos.");
                    }
                    else
                    {
                       MessageBox.Show($"O diretório {diretorio} não existe.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao acessar o diretório {diretorio}: {ex.Message}");
                }
            }

        private class Doadores
        {
            public string ID { get; set; }
            public string Doador { get; set; }
            public string Endereco { get; set; }
            public string Telefone { get; set; }
            public string CPF { get; set; }
            public string CNPJ { get; set; }
            public string TipoDePessoa { get; set; }
        }
    }
}
