using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenDatabaseInterfacer
{
    public static class StarCitizenDB
    {
        public static void SaveNew(string path, SCDataManager dataManager)
        {
            using (SQLiteDB db = new SQLiteDB(path))
            {
                createDatabaseTables(db);
                throw new NotImplementedException();
            }
        }

        public static void SaveDelta(string path, SCDeltaManager deltaManager)
        {
            using (SQLiteDB db = new SQLiteDB(path))
            {
                throw new NotImplementedException();
            }
        }

        public static SCDataManager Load(string path)
        {
            using (SQLiteDB db = new SQLiteDB(path))
            {
                throw new NotImplementedException();
            }
        }

        private static void createDatabaseTables(SQLiteDB db)
        {

        }
    }
}
