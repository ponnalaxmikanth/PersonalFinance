using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DigitalCurrencyDataAccess
    {
        readonly string _application = "DataAccess.DigitalCurrency";
        readonly string _component = "DigitalCurrencyDataAccess";

        public DataSet GetInvestmentDetails()
        {
            DataSet ds = null;
            try
            {
                
                    ds = SQLHelper.ExecuteProcedure("Investments", "GetDCTransactions", CommandType.StoredProcedure, null);
               
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }
    }
}
