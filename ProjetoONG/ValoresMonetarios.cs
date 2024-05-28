using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using System.Configuration;

namespace ProjetoONG
{
    public static class ValoresMonetarios
    {
        private static List<decimal> doacoesMonetarias = new List<decimal>();
        private static List<decimal> outrasReceitas = new List<decimal>();

        public static void Inicializar()
        {
            // Limpa as listas antes de carregar novamente para evitar duplicidade
            doacoesMonetarias.Clear();
            outrasReceitas.Clear();

            // Carregar doações monetárias do arquivo doações
            CarregarDoacoesMonetarias();

            // Carregar outras receitas do arquivo OutrasReceitas
            CarregarOutrasReceitas();
        }

        public static void CarregarDoacoesMonetarias()
        {
            string caminhoDoacoes = "C:\\Doacoes\\Doacoes.csv";
            if (File.Exists(caminhoDoacoes))
            {
                doacoesMonetarias = CarregarValores(caminhoDoacoes, "Valor");
            }
        }

        public static void CarregarOutrasReceitas()
        {
            string caminhoOutrasReceitas = "C:\\Doacoes\\OutrasReceitas.csv";
            if (File.Exists(caminhoOutrasReceitas))
            {
                outrasReceitas = CarregarValores(caminhoOutrasReceitas, "Valor");
            }
        }

        public static decimal CalcularTotalAtivos()
        {
            // Soma os valores das doações monetárias e outras receitas
            decimal totalAtivos = doacoesMonetarias.Sum() + outrasReceitas.Sum();
            return totalAtivos;
        }

        public static void AdicionarDoacaoMonetaria(decimal valor)
        {
            doacoesMonetarias.Add(valor);
        }

        public static void AdicionarOutraReceita(decimal valor)
        {
            outrasReceitas.Add(valor);
            Console.WriteLine($"Outra receita adicionada: {valor}");
        }

        private static List<decimal> CarregarValores(string caminhoArquivo, string nomeCampo)
        {
            List<decimal> valores = new List<decimal>();

            try
            {
                using (var reader = new StreamReader(caminhoArquivo))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        if (csv.TryGetField<decimal>(nomeCampo, out decimal valor))
                        {
                            valores.Add(valor);
                            Console.WriteLine($"Valor carregado: {valor}");
                        }
                    }
                }
            }
            catch (CsvHelper.ReaderException ex)
            {
                Console.WriteLine($"Erro ao ler o arquivo CSV: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine($"Total de valores carregados: {valores.Count}");
            return valores;
        }
    }
}
