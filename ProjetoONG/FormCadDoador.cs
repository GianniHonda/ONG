using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ProjetoONG
{
    public partial class FormCadDoador : Form
    {
        //define o diretorio padrão para salvar os arquivos
        private string diretorioPadrao = "C:\\Doador";

        //Nome do arquivo único
        private string nomeArquivo = "DadosDoador.csv";

        //Caminho completo do arquivo
        private string CaminhoDoArquivo => Path.Combine(diretorioPadrao, nomeArquivo);
        public FormCadDoador()
        {
            InitializeComponent();

            //verifica se o diretório padrão existe, se não, cria
            if (!Directory.Exists(diretorioPadrao))
            {
                Directory.CreateDirectory(diretorioPadrao);
            }

            //Trunca os IDs existentes para 8 caracteres
            TruncarIDs();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparSelecoesCheckedListBox(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            //dados dos campos
            string idDoador = TruncarID(Guid.NewGuid().ToString()); //gera um ID único para o doador
            string doador = tbDoadorInstituicao.Text;
            string endereco = tbEndereco.Text;
            string telefone = mtbTelefone.Text;
            string CPF = new string(mtbCPF.Text.Where(char.IsDigit).ToArray());
            string CNPJ = new string(mtbCNPJ.Text.Where(char.IsDigit).ToArray());
            List<string> tipoDePessoa = new List<string>();
            //Adiciona apenas os itens não nulos à lista tipoDePessoa
            foreach (var item in clbPessoa.CheckedItems)
            {
                if (item != null)
                {
                    tipoDePessoa.Add(item?.ToString() ?? "");
                }
            }

            //Cria um novo registro de doador
            var newDoador = new Doadores
            {
                ID = idDoador,
                Doador = doador,
                Endereco = endereco,
                Telefone = telefone,
                CPF = CPF,
                CNPJ = CNPJ,
                TipoDePessoa = string.Join(",", tipoDePessoa)
            };

            //Escreve cabeãlho se necessário
            if (!CabecalhoEscrito())
            {
                EscreverCabecalho();
            }

            //Escreva o novo registro no arquivo CSV
            using (var writer = new StreamWriter(CaminhoDoArquivo, true, Encoding.UTF8))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(newDoador);
                csv.NextRecord();
            }

            MessageBox.Show("Dados salvos com sucesso!");

            LimparCampos();
        }

        private bool CabecalhoEscrito()
        {
            //Verifica se o arquivo existe
            if (!File.Exists(CaminhoDoArquivo))
            {
                return false; //O arquivo não existe, portanto o cabeçalho não está presente
            }

            //Lê a primeira linha do arquivo
            string primeiraLinha = File.ReadLines(CaminhoDoArquivo).FirstOrDefault();

            //Verifica se a primeira linha corresponde ao cabeçalho esperado
            return primeiraLinha != null && primeiraLinha.Equals("ID,Doador,Endereco,Telefone,CPF,CNPJ,TipoDePessoa");
        }

        private void EscreverCabecalho()
        {
            //Verifica se o cabeçaljho já foi escrito no arquivo
            if (!CabecalhoEscrito())
            {
                //Escreve o cabeçalho apenas se ele não estiver presente no arquivo
                using (var writer = new StreamWriter(CaminhoDoArquivo, false, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Doadores>();
                    csv.NextRecord();
                }
            }
        }

        private void TruncarIDs()
        {
            //Verifica se o arquivo existe e não está vazio
            if (!File.Exists(CaminhoDoArquivo) || new FileInfo(CaminhoDoArquivo).Length == 0)
            {
                //Se o arquivo não existir não há necessidade de truncar os IDs
                return;
            }

            //Lê todas as linhas do arquivo
            var linhas = File.ReadAllLines(CaminhoDoArquivo).Skip(1).ToList();

            //Truncar os IDs para 8 caracteres, se necesseário, e sobrescreve o arquivo
            using (var writer = new StreamWriter(CaminhoDoArquivo, false, Encoding.UTF8))
            {
                //Escreva o cabeçalho explicitamente
                writer.WriteLine("ID,Doador,Endereco,Telefone,CPF,CNPJ,TipoDePessoa");

                foreach (var linha in linhas)
                {
                    //Divide a linha em campos usando a vírgula como delimitador
                    var campos = linha.Split(',');

                    //Trunca o ID se necessário
                    campos[0] = TruncarID(campos[0]);

                    //Escreve a linha modificada no arquivo
                    writer.WriteLine(string.Join(",", campos));
                }
            }
        }

        private string TruncarID(string idDoador)
        {
            //Define o número de caracteres a serem exibidos no ID
            const int comprimentoDesejado = 8;

            //Verifica se o ID tem mais caracteres do que o comprimento desejado
            if (idDoador.Length > comprimentoDesejado)
            {
                //retorna apenas os primeiros caracteres do ID
                return idDoador.Substring(0, comprimentoDesejado);
            }
            else
            {
                //retorna o ID sem alteração se já for menor ou igual ao comprimento desejado
                return idDoador;
            }

        }

        private void LimparCampos()
        {
            tbDoadorInstituicao.Text = string.Empty;
            tbEndereco.Text = string.Empty;
            mtbTelefone.Clear();
            mtbCPF.Clear();
            mtbCNPJ.Clear();
            LimparSelecoesCheckedListBox(clbPessoa);
        }

        //Definição de classe Doadores para mapeamento do arquivo CSV
        public class Doadores
        {
            public string ID { get; set; }
            public string Doador { get; set; }
            public string Endereco { get; set;}
            public string Telefone { get; set;}
            public string CPF { get; set;}
            public string CNPJ { get; set;}
            public string TipoDePessoa { get; set;}
        }
    }
}
