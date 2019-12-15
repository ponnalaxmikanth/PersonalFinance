using BusinessEntities.Contracts.MutualFunds;
using BusinessEntity;
using DataAccess;
using DataAccess.Stocks;
using Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities;

namespace BusinessAccess.Stocks
{
    public class StocksRepository
    {
        
        ICommonDataAccess _commonDataAccess;
        StocksDataAccess _stocksDataAccess;
        readonly string _application = "BusinessAccess";
        readonly string _component = "CommonRepository";

        public StocksRepository()
        {
            _commonDataAccess = new CommonDataAccess();
            _stocksDataAccess = new StocksDataAccess();
        }

        public StocksRepository(ICommonDataAccess common, StocksDataAccess stocksDataAccess)
        {
            _commonDataAccess = common;
            _stocksDataAccess = stocksDataAccess;
        }

        public void SetPath(string path)
        {
            _stocksDataAccess.SetPath(path);
        }

        //public List<Stock> GetToStocks()
        //{
        //    return MapStocks(_stocksDataAccess.GetToStocks());
        //}

        //private List<Stock> MapStocks(DataTable dataTable)
        //{
        //    List<Stock> lstStocks = null;

        //    try
        //    {
        //        if (dataTable != null && dataTable.Rows.Count > 0)
        //        {
        //            lstStocks = dataTable.AsEnumerable().Select(r => new Stock()
        //            {
        //                ID = r.Field<string>("StockID"),
        //                Dividend = r.Field<decimal>("Dividend"),
        //                MarketPrice = r.Field<decimal>("MarketPrice"),
        //                Price = r.Field<decimal>("Price"),
        //                PurchaseDate = r.Field<DateTime>("PurchaseDate"),
        //                Quantity = r.Field<int>("Quantity")
        //            }).ToList();
        //        }
        //        else if (dataTable == null || dataTable.Rows.Count == 0)
        //        {
        //            lstStocks = new List<Stock>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return lstStocks;
        //}

        public List<StocksEntity> GetStocks(DateTime fromdate, DateTime todate, int Detail)
        {
            return MapStocks(_stocksDataAccess.GetStocks(fromdate, todate, Detail));
        }

        private List<StocksEntity> MapStocks(DataTable da)
        {
            try
            {
                return (from DataRow dr in da.Rows
                        select new StocksEntity()
                        {
                            Symbol = dr["StockID"].ToString(),
                            Date = DateTime.Parse(dr["PurchaseDate"].ToString()),
                            volume = Conversions.ToDouble(dr["Quantity"].ToString(), 0),
                            close = Conversions.ToDouble(dr["Price"].ToString(), 0),
                            dividendamount = Conversions.ToDouble(dr["Dividend"].ToString(), 0),
                            MarketPrice = Conversions.ToDouble(dr["MarketPrice"].ToString(), 0),
                        }).ToList();
            }
            catch (Exception ex) {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }
    }
}
