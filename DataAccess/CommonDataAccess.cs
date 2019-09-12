using BusinessEntities.Contracts.MutualFunds;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class CommonDataAccess : ICommonDataAccess
    {
        readonly string _application = "DataAccess";
        readonly string _component = "CommonDataAccess";

        static string serverPath = string.Empty;
        public void SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(serverPath))
                serverPath = path + "\\Common";
        }

        public DataTable Get_MF_DataDumpDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fromDate", Value = fromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

                DataSet ds = SQLHelper.ExecuteProcedure("Investments", "Get_MF_DataDumpDates", CommandType.StoredProcedure, parameters);
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


        public void InsertDumpDate(DateTime date, int fundType, int count)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "Date", Value = date.ToString("MM/dd/yyyy") });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundType", Value = fundType });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "Count", Value = count });

                SQLHelper.ExecuteProcedure("Investments", "Insetrt_MF_DataDumpDates", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return;
        }

    }
}
