using BusinessEntities.Contracts.MutualFunds;
using BusinessEntity;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Stocks
{
    public class StocksRepository
    {
        
        ICommonDataAccess _commonDataAccess;
        StocksDataAccess _stocksDataAccess;

        public StocksRepository(ICommonDataAccess common, StocksDataAccess stocksDataAccess)
        {
            _commonDataAccess = common;
            _stocksDataAccess = stocksDataAccess;
        }

        public List<Stock> GetToStocks()
        {
            return MapStocks(_stocksDataAccess.GetToStocks());
        }

        private List<Stock> MapStocks(DataTable dataTable)
        {
            List<Stock> lstStocks = null;

            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    lstStocks = dataTable.AsEnumerable().Select(r => new Stock()
                    {
                        ID = r.Field<string>("StockID"),
                        Dividend = r.Field<decimal>("Dividend"),
                        MarketPrice = r.Field<decimal>("MarketPrice"),
                        Price = r.Field<decimal>("Price"),
                        PurchaseDate = r.Field<DateTime>("PurchaseDate"),
                        Quantity = r.Field<int>("Quantity")
                    }).ToList();
                }
                else if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    lstStocks = new List<Stock>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstStocks;
        }
    }
}
