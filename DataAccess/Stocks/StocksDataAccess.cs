using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Stocks
{
    public class StocksDataAccess
    {
        readonly string _application = "DataAccess.Stocks";
        readonly string _component = "StocksDataAccess";

        static string serverPath = string.Empty;
        public void SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(serverPath))
                serverPath = path + "\\Stocks\\";
        }
        //public DataTable GetStocks()
        //{
        //    try
        //    {
        //        DataSet ds = SQLHelper.ExecuteProcedure("Investments", "GetStocks", CommandType.StoredProcedure, null);
        //        if (ds != null)
        //        {
        //            return ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
        //    }
        //    return null;
        //}

        public DataTable GetStocks(DateTime fromdate, DateTime todate, int Detail)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "details", Value = Detail });

                DataSet ds = SQLHelper.ExecuteProcedure("Investments", "GetStocks", CommandType.StoredProcedure, parameters);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        

        public DataTable SoldStocks()
        {
            return null;
        }

        public DataTable PurchaseStocks()
        {
            return null;
        }
        public DataTable DividendStocks()
        {
            return null;
        }
    }
}
