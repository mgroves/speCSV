using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace specsv
{
    public class SqlServer
    {
        private SqlConnection _db;

        public SqlServer(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public void Process(CommandInput input)
        {
            _db.Open();
            // get list of tables
            var tables = GetListOfTables();
            if(tables.Count <= 0)
                throw new Exception("No tables found.");

            foreach (var table in tables)
            {
                ExportTable(table);
            }

            _db.Close();
            _db.Dispose();
        }

        private void ExportTable(string tableName)
        {
            File.Create(tableName + ".csv").Close();
            WriteHeaders(tableName);
            var command = new SqlCommand($"SELECT * FROM {tableName}", _db);
            using (var reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                    while (reader.Read())
                        WriteRowToCsv(reader, tableName);
            }
        }

        private void WriteHeaders(string tableName)
        {
            var command = new SqlCommand("select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA + '.' + TABLE_NAME = 'dbo.Invoices'", _db);
            var columnHeaders = new List<string>();
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                    while (reader.Read())
                        columnHeaders.Add(reader.GetString(0));
            }

            File.AppendAllText(tableName + ".csv", string.Join(',', columnHeaders) + "\r\n");
        }

        private void WriteRowToCsv(SqlDataReader reader, string tableName)
        {
            var count = reader.FieldCount;
            var vals = new List<string>();
            for (int i = 0; i < count; i++)
                vals.Add(reader[i].ToString());
            File.AppendAllText(tableName + ".csv", string.Join(',', vals) + "\r\n");
        }

        private List<string> GetListOfTables()
        {
            var list = new List<string>();
            var command = new SqlCommand("SELECT TABLE_SCHEMA, TABLE_NAME from INFORMATION_SCHEMA.TABLES", _db);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                        list.Add(reader.GetString(0) + '.' + reader.GetString(1));
                }
            }

            return list;
        }
    }
}