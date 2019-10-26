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

        readonly string serverPath = "\\Accounts\\";

        public DataSet GetAccountStatusDetails(int Details = 0)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "AccountsCurrentStatus.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "detail", Value = Details });

                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetAccountsCurrentStatus", CommandType.StoredProcedure, parameters, _component);
                }
                if (ds != null && ds.Tables.Count > 0 && !UseMockData && EnableStoreDataAsJson)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "AccountsCurrentStatus.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }
    }
}
