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
        public List<Stock> ToGetStocks(DateTime fromdate, DateTime todate)
        {
            StocksDataAccess sda = new StocksDataAccess();
            //List<Stock> sp = new List<Stock>();
            // DataTable da = sda.GetToStocks(fromdate, todate);
            // sp = MapsStocks(da);
            //return sp;
            return null;
        }

        private List<Stock> MapsStocks(DataTable da)
        {
            List<Stock> lsp = new List<Stock>();
            lsp = (from DataRow dr in da.Rows
                   select new Stock()
                           {
                               Quantity = Convert.ToInt32(dr["No_of_Stocks"]),
                               ID = dr["StockID"].ToString(),
                               PurchaseDate = Convert.ToDateTime(dr["Purchasedatetime"].ToString()),
                               Price = decimal.Round(Convert.ToDecimal(dr["Stockprice"].ToString()), 3, MidpointRounding.AwayFromZero),

                               //  PurchaseDate = dr["PurchaseDate"].ToString(),
                               //  StocksPrice = dr["StocksPrice"].ToString()
                           }).ToList();       
            return lsp;
        }
    }
}
