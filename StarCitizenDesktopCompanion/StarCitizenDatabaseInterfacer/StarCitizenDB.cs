using StarCitizenModelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenDatabaseInterfacer
{
    public static class StarCitizenDB
    {
        public static bool SaveNew(string path, SCDataManager dataManager)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("File exists at SaveNew() path. Path: {0}", path);
                return false;
            }
            using (SQLiteDB db = new SQLiteDB(path))
            {
                createDatabaseTables(db);

                foreach(Commodity commodity in dataManager.Commodities)
                {
                    addCommodity(db, commodity);
                }
                foreach(TradingPort port in dataManager.TradingPorts)
                {
                    addTradingPort(db, port);
                    foreach(CommodityPrice price in port.Prices)
                    {
                        addCommodityPrice(db, price);
                        addTradingPortCommodityPrice(db, port, price);
                        foreach(PricePoint point in price.PriceHistory)
                        {
                            addPricePoint(db, point, price);
                        }
                    }
                }
            }
            return true;
        }

        public static void SaveDelta(string path, SCDeltaManager deltaManager)
        {
            using (SQLiteDB db = new SQLiteDB(path))
            {
                foreach(DataDelta delta in deltaManager.Deltas)
                {
                    if (delta.PropertyChangedArgs != null)
                    {
                        //Edit
                        PropertyChangedEventArgs args = delta.PropertyChangedArgs;
                        if (delta.Sender is Commodity commodity)
                        {
                            //Commodity
                            if (args.PropertyName == "Name")
                            {
                                db.EditProperty("Name", commodity.Guid.ToString(), commodity.Name);
                            }
                        }
                        else if (delta.Sender is TradingPort port)
                        {
                            //TradingPort
                            if (args.PropertyName == "Name")
                            {
                                db.EditProperty("Name", port.Guid.ToString(), port.Name);
                            }
                        }
                    }
                    else if (delta.RelationshipChangedArgs != null)
                    {
                        RelationshipChangedArgs args = delta.RelationshipChangedArgs;
                        NotifyCollectionChangedEventArgs notifyArgs = args.Args;
                        //Add/Remove
                        if (notifyArgs.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach(object item in notifyArgs.NewItems)
                            {
                                if (item is Commodity commodity)
                                {
                                    addCommodity(db, commodity);
                                }
                                else if (item is TradingPort port)
                                {
                                    addTradingPort(db, port);
                                }
                                else if (item is CommodityPrice price)
                                {
                                    addCommodityPrice(db, price);
                                    if (args.Parent is TradingPort priceParent)
                                    {
                                        addTradingPortCommodityPrice(db, priceParent, price);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No parent to TradingPort");
                                    }
                                }
                                else if (item is PricePoint point)
                                {
                                    if (args.Parent is CommodityPrice pointParent)
                                    {
                                        addPricePoint(db, point, pointParent);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No parent to PricePoint.");
                                    }
                                }
                            }
                        }
                        else if (notifyArgs.Action == NotifyCollectionChangedAction.Remove)
                        {
                            foreach(object item in notifyArgs.OldItems)
                            {
                                if (item is Commodity commodity)
                                {
                                    removeCommodity(db, commodity);
                                }
                                else if (item is TradingPort port)
                                {
                                    removeTradingPort(db, port);
                                }
                                else if (item is CommodityPrice price)
                                {
                                    removeCommodityPrice(db, price);
                                    removePricePointsInCommodityPrice(db, price);
                                    if (args.Parent is TradingPort priceParent)
                                    {
                                        removeTradingPortCommodityPrice(db, priceParent, price);
                                    }
                                    else
                                    {
                                        Console.WriteLine("No parent to Trading Port");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Unhandled Collection Changed Action: {0}", notifyArgs.Action);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Empty delta. Sender: {0}", delta.Sender);
                    }
                }
            }
        }

        public static SCDataManager Load(string path)
        {
            using (SQLiteDB db = new SQLiteDB(path))
            {
                SCDataManager dataManager = new SCDataManager();

                //Commodity
                DataTable commodityTable = db.GetDataFromTable("Commodity");
                foreach(DataRow row in commodityTable.Rows)
                {
                    Guid guid = new Guid(row.ItemArray[0].ToString());
                    string name = row.ItemArray[1].ToString();

                    Commodity commodity = new Commodity(name, guid);
                    dataManager.Commodities.Add(commodity);
                }
                Dictionary<Guid, Commodity> commodityDict = createGuidDictionary(dataManager.Commodities);

                //TradingPort
                DataTable tradingPortTable = db.GetDataFromTable("TradingPort");
                foreach (DataRow row in tradingPortTable.Rows)
                {
                    Guid guid = new Guid(row.ItemArray[0].ToString());
                    string name = row.ItemArray[1].ToString();

                    TradingPort port = new TradingPort(name, guid);
                    dataManager.TradingPorts.Add(port);
                }
                Dictionary<Guid, TradingPort> portDict = createGuidDictionary(dataManager.TradingPorts);

                //TradingPortComodityPrice
                DataTable portPriceTable = db.GetDataFromTable("TradingPortCommodityPrice");
                Dictionary<Guid, TradingPort> priceToPort = new Dictionary<Guid, TradingPort>();
                foreach (DataRow row in portPriceTable.Rows)
                {
                    Guid portGuid = new Guid(row.ItemArray[0].ToString());
                    Guid priceGuid = new Guid(row.ItemArray[1].ToString());
                    priceToPort.Add(priceGuid, portDict[portGuid]);
                }

                //CommodityPrice
                DataTable priceTable = db.GetDataFromTable("CommodityPrice");
                List<CommodityPrice> allPrices = new List<CommodityPrice>();
                foreach(DataRow row in priceTable.Rows)
                {
                    Guid guid = new Guid(row.ItemArray[0].ToString());
                    Guid commodityGuid = new Guid(row.ItemArray[1].ToString());
                    Commodity commodity = commodityDict[commodityGuid];
                    PriceType type = PriceTypeUtils.ConvertString(row.ItemArray[2].ToString());

                    CommodityPrice price = new CommodityPrice(commodity, type, guid);

                    TradingPort port = priceToPort[price.Guid];
                    port.Prices.Add(price);

                    allPrices.Add(price);
                }
                Dictionary<Guid, CommodityPrice> priceDict = createGuidDictionary(allPrices);

                //PricePoint
                DataTable pointTable = db.GetDataFromTable("PricePoint");
                foreach(DataRow row in pointTable.Rows)
                {
                    Guid commodityPriceGuid = new Guid(row.ItemArray[0].ToString());
                    double priceDouble = (double)row.ItemArray[1];
                    DateTime dateTime = new DateTime(Convert.ToInt64(row.ItemArray[2]));

                    CommodityPrice commodityPrice = priceDict[commodityPriceGuid];
                    commodityPrice.AddPrice(priceDouble, dateTime);
                }

                return dataManager;
            }
        }

        private static void createDatabaseTables(SQLiteDB db)
        {
            foreach(DBTable table in SCTableDefinitions.AllTables)
            {
                db.CreateTable(table);
            }
        }

        private static void addCommodity(SQLiteDB db, Commodity commodity)
        {
            Dictionary<string, string> commodityFieldValues = new Dictionary<string, string>();
            commodityFieldValues.Add("Guid", commodity.Guid.ToString());
            commodityFieldValues.Add("Name", commodity.Name);
            db.AddValueToTable("Commodity", commodityFieldValues);
        }
        private static void addTradingPort(SQLiteDB db, TradingPort port)
        {
            Dictionary<string, string> portFieldValues = new Dictionary<string, string>();
            portFieldValues.Add("Guid", port.Guid.ToString());
            portFieldValues.Add("Name", port.Name);
            db.AddValueToTable("TradingPort", portFieldValues);
        }
        private static void addCommodityPrice(SQLiteDB db, CommodityPrice price)
        {
            Dictionary<string, string> priceFieldValues = new Dictionary<string, string>();
            priceFieldValues.Add("Guid", price.Guid.ToString());
            priceFieldValues.Add("CommodityGuid", price.Commodity.Guid.ToString());
            priceFieldValues.Add("Type", price.Type.ToString());
            db.AddValueToTable("CommodityPrice", priceFieldValues);
        }
        private static void addPricePoint(SQLiteDB db, PricePoint point, CommodityPrice price)
        {
            Dictionary<string, string> pointFieldValues = new Dictionary<string, string>();
            pointFieldValues.Add("CommodityPriceGuid", price.Guid.ToString());
            pointFieldValues.Add("Price", point.Price.ToString());
            pointFieldValues.Add("DateTime", point.DateTime.Ticks.ToString());
            db.AddValueToTable("PricePoint", pointFieldValues);
        }

        private static void addTradingPortCommodityPrice(SQLiteDB db, TradingPort port, CommodityPrice price)
        {
            Dictionary<string, string> portPriceGuids = new Dictionary<string, string>();
            portPriceGuids.Add("TradingPortGuid", port.Guid.ToString());
            portPriceGuids.Add("CommodityPriceGuid", price.Guid.ToString());
            db.AddValueToTable("TradingPortCommodityPrice", portPriceGuids);
        }

        private static void removeCommodity(SQLiteDB db, Commodity commodity)
        {
            Dictionary<string, string> id = new Dictionary<string, string>();
            id.Add("Guid", commodity.Guid.ToString());
            db.RemoveValueFromTable("Commodity", id);
        }
        private static void removeTradingPort(SQLiteDB db, TradingPort port)
        {
            Dictionary<string, string> id = new Dictionary<string, string>();
            id.Add("Guid", port.Guid.ToString());
            db.RemoveValueFromTable("TradingPort", id);

            id = new Dictionary<string, string>();
            id.Add("TradingPortGuid", port.Guid.ToString());
            db.RemoveValueFromTable("TradingPortCommodityPrice", id);
        }
        private static void removeCommodityPrice(SQLiteDB db, CommodityPrice price)
        {
            Dictionary<string, string> id = new Dictionary<string, string>();
            id.Add("Guid", price.Guid.ToString());
            db.RemoveValueFromTable("CommodityPrice", id);
        }
        private static void removePricePointsInCommodityPrice(SQLiteDB db, CommodityPrice price)
        {
            Dictionary<string, string> priceID = new Dictionary<string, string>();
            priceID.Add("CommodityPriceID", price.Guid.ToString());
            db.RemoveValueFromTable("PricePoint", priceID);
        }

        private static void removeTradingPortCommodityPrice(SQLiteDB db, TradingPort port, CommodityPrice price)
        {
            Dictionary<string, string> portPriceGuids = new Dictionary<string, string>();
            portPriceGuids.Add("TradingPortGuid", port.Guid.ToString());
            portPriceGuids.Add("CommodityPriceGuid", price.Guid.ToString());
            db.RemoveValueFromTable("TradingPortCommodityPriceTable", portPriceGuids);
        }

        private static Dictionary<Guid, T> createGuidDictionary<T>(IEnumerable<T> collection) where T : BaseModel
        {
            Dictionary<Guid, T> dictionary = new Dictionary<Guid, T>();
            foreach(T item in collection)
            {
                dictionary.Add(item.Guid, item);
            }
            return dictionary;
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
