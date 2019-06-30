using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
     public class StocksDataAccess
    {
        public DataTable GetToStocks(DateTime fromdate, DateTime todate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fromdate", Value = fromdate });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "todate", Value = todate });

            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "getstocks", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
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
