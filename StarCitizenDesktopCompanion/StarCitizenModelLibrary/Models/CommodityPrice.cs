using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCitizenModelLibrary.Models
{
    public class CommodityPrice : BaseModel
    {
        public Commodity Commodity { get; }
        public PriceType Type { get; }

        public RelationshipCollection<PricePoint> PriceHistory { get; }

        public CommodityPrice(Commodity commodity, PriceType type, Guid guid = new Guid()) : base(guid)
        {
            this.Commodity = commodity;
            this.Type = type;
            this.PriceHistory = new RelationshipCollection<PricePoint>(this);
        }

        public void AddPrice(double price)
        {
            AddPrice(price, DateTime.UtcNow);
        }
        public void AddPrice(double price, DateTime dateTime)
        {
            this.PriceHistory.Add(new PricePoint(price, dateTime));
        }
    }

    public class PricePoint
    {
        public double Price { get; }
        public DateTime DateTime { get; }

        public PricePoint(double price, DateTime dateTime)
        {
            this.Price = price;
            this.DateTime = dateTime;
        }
    }

    public enum PriceType { Buy = 1, Sell }

    public static class PriceTypeUtils
    {
        public static string ToString(this PriceType type)
        {
            if (type == PriceType.Buy)
            {
                return "Buy";
            }
            else if (type == PriceType.Sell)
            {
                return "Sell";
            }
            Console.WriteLine("Unrecognized PriceType");
            return "";
        }

        public static PriceType ConvertString(string type)
        {
            switch (type)
            {
                case "Buy":
                    return PriceType.Buy;
                case "Sell":
                    return PriceType.Sell;
                default:
                    return 0;
            }
        }
    }

    
}
