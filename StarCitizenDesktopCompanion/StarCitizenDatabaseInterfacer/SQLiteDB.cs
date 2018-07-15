using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

            commandString += separateValues(fields);

            if (primaryKeys.Count > 0)
            {
                commandString += string.Format(", PRIMARY KEY({0})", separateValues(primaryKeys));
            }

            commandString += ");";

            nonQueryCommand(commandString);
        }

        public bool AddValueToTable(string tableName, Dictionary<string, string> dataFields)
        {
            string commandString = string.Format("INSERT INTO {0} (", tableName);

            List<string> fields = new List<string>();
            List<string> values = new List<string>();
            foreach(KeyValuePair<string, string> fieldValue in dataFields)
            {
                fields.Add(fieldValue.Key);
                values.Add(string.Format("'{0}'", fieldValue.Value));
            }

            commandString += string.Format("{0}) VALUES ({1});", separateValues(fields), separateValues(values));
            
            if (nonQueryCommand(commandString) > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("AddValueToTable() failed. Command string:");
                Console.WriteLine(commandString);
                return false;
            }
        }
        public bool EditValueInTable(string tableName, Dictionary<string, string> dataFields, Dictionary<string, string> conditionFields)
        {
            string commandString = string.Format("UPDATE {0} SET ", tableName);

            List<string> fieldsToUpdate = new List<string>();
            foreach(KeyValuePair<string, string> fieldValue in dataFields)
            {
                fieldsToUpdate.Add(string.Format("{0} = '{1}'", fieldValue.Key, fieldValue.Value));
            }

            List<string> conditionsToCheck = new List<string>();
            foreach(KeyValuePair<string, string> conditionField in conditionFields)
            {
                conditionsToCheck.Add(string.Format("{0} = '{1}'", conditionField.Key, conditionField.Value));
            }

            commandString += string.Format("{0} WHERE {1}", separateValues(fieldsToUpdate), separateValues(conditionsToCheck, " AND "));

            if (nonQueryCommand(commandString) > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("EditValueInTable() failed. Command string:");
                Console.WriteLine(commandString);
                return false;
            }
        }
        public bool RemoveValueFromTable(string tableName, Dictionary<string, string> conditionFields)
        {
            string commandString = string.Format("DELETE FROM {0} WHERE (", tableName);

            List<string> conditionsToCheck = new List<string>();
            foreach(KeyValuePair<string, string> conditionField in conditionFields)
            {
                conditionsToCheck.Add(string.Format("{0} = '{1}'", conditionField.Key, conditionField.Value));
            }

            commandString += string.Format("{0})", separateValues(conditionsToCheck, " AND "));

            if (nonQueryCommand(commandString) > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("RemoveValueFromTable() failed. Command string:");
                Console.WriteLine(commandString);
                return false;
            }
        }

        public DataTable GetDataFromTable(string tableName)
        {
            string commandString = string.Format("SELECT * FROM {0}", tableName);
            DataTable data = loadDataCommand(commandString);
            
            if (data == null)
            {
                Console.WriteLine("GetDataFromTable() failed. Command string:");
                Console.WriteLine(commandString);
            }

            return data;
        }

        private int nonQueryCommand(string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, this.connection);
            return command.ExecuteNonQuery();
        }
        private DataTable loadDataCommand(string commandText)
        {
            DataTable data = new DataTable();
            SQLiteCommand command = new SQLiteCommand(commandText, this.connection);
            SQLiteDataReader reader = command.ExecuteReader();
            try
            {
                data.Load(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine("loadDataCommand() failed. Exception:");
                Console.WriteLine(e.Message);
                return null;
            }
            reader.Close();
            return data;
        }

        private static string separateValues(List<string> values, string separator = ", ")
        {
            string result = "";
            foreach (string value in values)
            {
                result += value;
                if (values.IndexOf(value) < (values.Count - 1)) result += separator;
            }
            return result;
        }
    }
}
