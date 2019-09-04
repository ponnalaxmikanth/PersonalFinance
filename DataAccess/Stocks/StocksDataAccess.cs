using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Stocks
{
    public class StocksDataAccess
    {
        public DataTable GetStocks()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("Investments", "GetStocks", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }
    }
}
