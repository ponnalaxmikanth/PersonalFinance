using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Accounts
{
    public class AccountsDataAccess : BaseDataAccess
    {
        readonly string _application = "DataAccess";
        readonly string _component = "AccountsDataAccess";

        public DataSet GetAccountStatusDetails(int Details = 0)
        {
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "detail", Value = Details });

                ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetAccountsCurrentStatus", CommandType.StoredProcedure, parameters, _component);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }
    }
}
