using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Data;

namespace ParserTask
{
    public static class TextAnalyzer
    {
        public static DataTable AnalyzeText(string BigString) //расчет кол-ва уникальных слов
        {
            BigString = BigString.ToLower(); //Приводим к общему виду

            string[] WordsSingle = { }; //Массив содержащий слова после разбивки
            string[] uniqueElems = { }; //Массив уникальных слов
            string[] Delimeters = { " ", ",", ".", "! ", "?", ";", ":", "[", "]", "(", ")", "\n", "\r", "\t", "\"" }; //разделители

            try
            {
                WordsSingle = BigString.Split(Delimeters, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку
                uniqueElems = WordsSingle.Distinct().ToArray(); //Уникальные значения
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter(ParserTask.StartProgram.writePath()))
                {
                    sw.WriteLine("Не удалось разбить текст по заданным разделителям " + e.InnerException);
                    throw;
                }
            }

            string Word;
            int matchedItems; //счетчик

            DataTable DTable = new DataTable();
            DTable.Clear();
            DTable.Columns.Add("Word");
            DTable.Columns.Add("Count");

            for (int i = 0; i < uniqueElems.Length; i++)
            {
                Word = uniqueElems[i].ToString();
                matchedItems = Array.FindAll(WordsSingle, Word => Word == WordsSingle[i]).Length; //подсчет кол-ва повторений слова

                DTable.Rows.Add(new object[] { Word, matchedItems.ToString() }); //запись значений
                Console.WriteLine(Word + " " + matchedItems);
            }
            Console.WriteLine("Всего " + uniqueElems.Length.ToString() + " уникальных слов.");

            return DTable;
        }
    }
}