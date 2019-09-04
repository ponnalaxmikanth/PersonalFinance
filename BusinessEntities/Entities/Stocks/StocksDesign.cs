using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
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

    public class StocksEntity
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string close { get; set; }
        public string adjustedclose { get; set; }
        public string volume { get; set; }
        public double dividendamount { get; set; }
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

}
