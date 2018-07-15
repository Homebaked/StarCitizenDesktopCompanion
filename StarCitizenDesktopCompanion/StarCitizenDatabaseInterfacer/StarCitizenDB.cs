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
            foreach(DBTable table in SCTableDefinitions.AllTables)
            {
                db.CreateTable(table);
            }
        }
    }

    public static class SCTableDefinitions
    {
        public static List<DBTable> AllTables
        {
            get
            {
                List<DBTable> tables = new List<DBTable>();
                tables.Add(CommodityTable);
                tables.Add(TradingPortTable);
                tables.Add(CommodityPriceTable);
                tables.Add(PricePointTable);
                tables.Add(TradingPortCommodityPriceTable);
                return tables;
            }
        }

        #region Model Tables
        public static DBTable CommodityTable
        {
            get
            {
                DBTable table = new DBTable("Commodity");
                table.AddField(DBField.FieldType.TEXT, "Guid", true);
                table.AddField(DBField.FieldType.TEXT, "Name");
                return table;
            }
        }
        public static DBTable TradingPortTable
        {
            get
            {
                DBTable table = new DBTable("TradingPort");
                table.AddField(DBField.FieldType.TEXT, "Guid", true);
                table.AddField(DBField.FieldType.TEXT, "Name");
                return table;
            }
        }
        public static DBTable CommodityPriceTable
        {
            get
            {
                DBTable table = new DBTable("CommodityPrice");
                table.AddField(DBField.FieldType.TEXT, "Guid", true);
                table.AddField(DBField.FieldType.TEXT, "CommodityGuid");
                table.AddField(DBField.FieldType.TEXT, "Type");
                return table;
            }
        }
        public static DBTable PricePointTable
        {
            get
            {
                DBTable table = new DBTable("PricePoint");
                table.AddField(DBField.FieldType.TEXT, "CommodityPriceGuid");
                table.AddField(DBField.FieldType.REAL, "Price");
                table.AddField(DBField.FieldType.TEXT, "DateTime");
                return table;
            }
        }
        #endregion

        #region Relationship Tables
        public static DBTable TradingPortCommodityPriceTable
        {
            get
            {
                DBTable table = new DBTable("TradingPortCommodityPrice");
                table.AddField(DBField.FieldType.TEXT, "TradingPortGuid", true);
                table.AddField(DBField.FieldType.TEXT, "CommodityPriceGuid", true);
                return table;
            }
        }
        #endregion
    }
}
