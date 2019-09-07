using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class StocksRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Detail { get; set; }
    }

    public class StockPurchases
    {
        public string StocksId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int NumbersofStocks { get; set; }
        public decimal StocksPrice { get; set; }
    }


    public class StocksEntity
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double adjustedclose { get; set; }
        public double volume { get; set; }
        public double dividendamount { get; set; }
        public double MarketPrice { get; set; }
    }

    public class StockValues
    {
        public MetaData MetaData { get; set; }
        ////[JsonProperty("TimeSeriesDaily")]
        public Dictionary<string, StocksEntity> TimeSeriesDaily { get; set; }
    }

    public class MetaData
    {
        ////[JsonProperty("Information")]
        public string Information { get; set; }
        ////[JsonProperty("Symbol")]
        public string Symbol { get; set; }
        //[JsonProperty("Last Refreshed")]
        public string LastRefreshed { get; set; }
        //[JsonProperty("Output Size")]
        public string OutputSize { get; set; }
        //[JsonProperty("Time Zone")]
        public string TimeZone { get; set; }
    }

    public class StocksDesign
    {
    }
    public class Stock
    {
        public string ID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal Dividend { get; set; }
    }

}
