using System;
using System.IO;
using System.Data;

namespace ParserTask
{    
    public class Program
    {
        public static string writePath = Path.Combine(Directory.GetCurrentDirectory(), "Logging.txt"); //вот такое логирование ошибок(
        public static void Main(string[] args)
        {
            string InnerText;
            DataTable DTable;

            Console.WriteLine("Добро пожаловать! \nВведите URL сайта (включая https) для анализа: ");
            InnerText = GetHTMLcontent.GetHTMLtext(Console.ReadLine()); //Ввод http-адреса, парсинг Dom
            DTable = TextAnalyzer.AnalyzeText(InnerText); //Форматирование и Анализ текста
            RecordToDB.InitializeDatabase();
            RecordToDB.AddData(DTable);
            Console.ReadKey();
        }
    }
}