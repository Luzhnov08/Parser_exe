using System;
using System.IO;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ParserTask
{
    public static class RecordToDB
    {        
        public static void InitializeDatabase() //Создаём базу и таблицу
        {
            try
            {
                string dbpath = Path.Combine(Directory.GetCurrentDirectory(), "SimbirSoft.db"); //В папке с приложением + вынести в интерфейс, другая база данных
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    String tableCommand = "CREATE TABLE IF NOT " +
                        "EXISTS Words (Primary_Key INTEGER PRIMARY KEY, " +
                        "Word TEXT NULL, Count INTEGER NULL)";

                    SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                    createTable.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter(Program.writePath))
                {
                    sw.WriteLine("Не удалось создать БД " + e.InnerException);
                    throw;
                }
            }
        }
        public static void AddData(DataTable DTable) //Записываем данные в таблицу
        {
            try
            {
                string dbpath = Path.Combine(Directory.GetCurrentDirectory(), "SimbirSoft.db"); //повторяется переменная const
                using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        var command = db.CreateCommand();
                        command.CommandText = @"INSERT INTO Words VALUES (NULL, $Word, $Count)";

                        var parameter1 = command.CreateParameter();
                        parameter1.ParameterName = "$Word";
                        command.Parameters.Add(parameter1);

                        var parameter2 = command.CreateParameter();
                        parameter2.ParameterName = "$Count";
                        command.Parameters.Add(parameter2);

                        for (var i = 0; i < DTable.Rows.Count; i++) //не нужен DTable
                        {
                            parameter1.Value = DTable.Rows[i][0];
                            parameter2.Value = DTable.Rows[i][1];
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    db.Close();
                }
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter(Program.writePath))
                {
                    sw.WriteLine("Не удалось записать данные в БД " + e.InnerException);
                    throw;
                }
            }
        }
    }
}