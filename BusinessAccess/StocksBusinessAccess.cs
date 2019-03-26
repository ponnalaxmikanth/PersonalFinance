using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using System.Data;

namespace BusinessAccess
{
    public class StocksBusinessAccess
    {
        public List<StockPurchases> ToGetStocks(DateTime fromdate, DateTime todate)
        {
            StocksDataAccess sda = new StocksDataAccess();
           List<StockPurchases> sp = new List<StockPurchases>();
            DataTable da = sda.GetToStocks(fromdate, todate);
            sp = MapsStocks(da);
           return sp;
        }
        private List<StockPurchases> MapsStocks(DataTable da)
        {
            List<StockPurchases> lsp = new List<StockPurchases>();
            lsp = (from DataRow dr in da.Rows
                   select new StockPurchases()
                           {
                               NumbersofStocks = Convert.ToInt32(dr["No_of_Stocks"]),
                               StocksId = dr["StockID"].ToString(),
                               PurchaseDate = Convert.ToDateTime(dr["Purchasedatetime"].ToString()),
                               StocksPrice = decimal.Round(Convert.ToDecimal(dr["Stockprice"].ToString()), 3, MidpointRounding.AwayFromZero),

                               //  PurchaseDate = dr["PurchaseDate"].ToString(),
                               //  StocksPrice = dr["StocksPrice"].ToString()
                           }).ToList();       
            return lsp;
        }
    }
}
