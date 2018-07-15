using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenDatabaseInterfacer
{
    public class SQLiteDB : IDisposable
    {
        const int SQLITE_VERSION = 3;

        private SQLiteConnection connection;

        public SQLiteDB(string path)
        {
            if (path == "" || path == null) throw new NullReferenceException("StarCitizenSQLiteDB needs non null path.");

            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
            }

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = path;
            builder.Version = SQLITE_VERSION;
            this.connection = new SQLiteConnection(builder.ConnectionString);
            this.connection.Open();

            this.nonQueryCommand("BEGIN TRANSACTION");
        }
        
        public void Dispose()
        {
            this.nonQueryCommand("END TRANSACTION");
            try
            {
                this.connection.Close();
            }
            catch
            {
                Console.WriteLine("Connection already closed at time of deconstruction.");
            }
        }
        
        public void CreateTable(DBTable table)
        {
            string commandString = string.Format("CREATE TABLE '{0}' (", table.Name);

            List<string> fields = new List<string>();
            List<string> primaryKeys = new List<string>();
            foreach (DBField field in table.Fields)
            {
                fields.Add(string.Format("'{0}' {1}", field.Name, field.Type));

                if (field.PrimaryKey)
                {
                    primaryKeys.Add(string.Format("'{0}'", field.Name));
                }
            }

            commandString += convertToCSV(fields);

            if (primaryKeys.Count > 0)
            {
                commandString += string.Format(", PRIMARY KEY({0})", convertToCSV(primaryKeys));
            }

            commandString += ")";

            nonQueryCommand(commandString);
        }

        public bool AddValueToTable(string tableName, Dictionary<string, string> data)
        {
            string commandString = string.Format("INSERT INTO {0} (", tableName);

            List<string> fields = new List<string>();
            List<string> values = new List<string>();
            foreach(KeyValuePair<string, string> fieldValue in data)
            {
                fields.Add(fieldValue.Key);
                values.Add(string.Format("'{0}'", fieldValue.Value));
            }

            commandString += string.Format("{0}) values ({1})", convertToCSV(fields), convertToCSV(values));

            return (nonQueryCommand(commandString) > 0);
        }

        public bool EditValueInTable()
        {
            throw new NotImplementedException();
        }
        
        public bool RemoveValueFromTable()
        {
            throw new NotImplementedException();
        }

        private int nonQueryCommand(string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, this.connection);
            return command.ExecuteNonQuery();
        }
        private static string convertToCSV(List<string> values)
        {
            string result = "";
            foreach (string value in values)
            {
                result += value;
                if (values.IndexOf(value) < (values.Count - 1)) result += ", ";
            }
            return result;
        }
    }
}
