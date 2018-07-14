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
        
        public bool CreateTable()
        {
            throw new NotImplementedException();
        }

        public bool AddRow(string tableName, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }

        private void nonQueryCommand(string commandText)
        {
            SQLiteCommand command = new SQLiteCommand(commandText, this.connection);
            command.ExecuteNonQuery();
        }
    }
}
