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
            //Carregar doações monetárias do arquivo doações
            CarregarDoacoesMonetarias();

            //Carregar outras receitas do arquivo OutrasReceitas
            CarregarOutrasReceitas();
        }

        public static void CarregarDoacoesMonetarias()
        {
            string caminhoDoacoes = "C:\\Doacoes\\Doacoes.csv";
            if (File.Exists(caminhoDoacoes))
            {
                doacoesMonetarias = CarregarValoresDoacoes(caminhoDoacoes);
                Console.WriteLine("Doações monetárias carregadas com sucesso.");
            }
        }

        public static void CarregarOutrasReceitas()
        {
            string caminhoOutrasReceitas = "C:\\Doacoes\\OutrasReceitas.csv";
            if (File.Exists(caminhoOutrasReceitas))
            {
                outrasReceitas = CarregarValoresOutrasReceitas(caminhoOutrasReceitas);
                Console.WriteLine("Outras receitas carregadas com sucesso.");
            }
        }

        public static void AdicionarDoacaoMonetaria(decimal valor)
        {
            doacoesMonetarias.Add(valor);
            Console.WriteLine("Doação monetária adicionada com sucesso.");
        }

        public static void AdicionarOutraReceita(decimal valor)
        {
            outrasReceitas.Add(valor);
            Console.WriteLine("Outra receita adicionada com sucesso.");
        }


        public static decimal CalcularTotalAtivos()
        {
            decimal totalDoacoesMonetarias = doacoesMonetarias.Sum();
            decimal totalOutrasReceitas = outrasReceitas.Sum();
            return totalDoacoesMonetarias + totalOutrasReceitas;
        }

        private static List<decimal> CarregarValoresDoacoes(string caminhoArquivo)
        {
            List<decimal> valoresDoacoes = new List<decimal>();

            using (var reader = new StreamReader(caminhoArquivo))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //lê cada linha do arquivo e extrai o valor monetário
                while (csv.Read())
                {
                    //verifica se a linha possui dados suficientes para extrair o valor monetário
                    if (csv.TryGetField<decimal>("Valor", out decimal valor))
                    {
                        valoresDoacoes.Add(valor);
                    }
                }
            }
            return valoresDoacoes;
        }

        private static List<decimal> CarregarValoresOutrasReceitas(string caminhoArquivo)
        {
            List<decimal> valoresOutrasReceitas = new List<decimal>();

            using (var reader = new StreamReader(caminhoArquivo))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Lê cada linha do arquivo e extrai o valor monetário
                while (csv.Read())
                {
                    // Verifica se a linha possui dados suficientes para extrair o valor monetário
                    if (csv.TryGetField<decimal>(1, out decimal valor)) //O valor decimal está na segunda coluna (índice 1)
                    {
                        valoresOutrasReceitas.Add(valor);
                    }
                }
            }
            return valoresOutrasReceitas;
        }
    }
}
