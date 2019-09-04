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

}
