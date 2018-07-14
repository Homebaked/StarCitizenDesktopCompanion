using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenDatabaseInterfacer
{
    public class DBTable
    {
        public string Name { get; }
        public List<DBField> Fields { get; } = new List<DBField>();

        public DBTable(string name)
        {
            this.Name = name;
        }

        public void AddField(DBField.FieldType type, string name, bool pk = false)
        {
            this.Fields.Add(new DBField(type, name, pk));
        }
    }

    public struct DBField
    {
        public enum FieldType { NULL, INTEGER, REAL, TEXT, BLOB }

        public FieldType Type { get; }
        public string Name { get; }
        public bool PrimaryKey { get; }

        public DBField(FieldType type, string name, bool pk)
        {
            this.Type = type;
            this.Name = name;
            this.PrimaryKey = pk;
        }
    }
}
