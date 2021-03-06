﻿using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Accounts
{
    public class DownloadExcelAccountInfoDataAccess
    {
        readonly string _application = "DataAccess.Accounts";
        readonly string _component = "DownloadExcelAccountInfoDataAccess";

        static string serverPath = string.Empty;
        public void SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(serverPath))
                serverPath = path + "\\Accounts\\";
        }

        public DataTable GetAccountMappingDetails(string connection)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = SQLHelper.ExecuteProcedure(connection, "GetAccountMappingDetails", CommandType.StoredProcedure, parameters);
                if (ds != null)
                    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public void UpdateTransactions(string connection, string xml, int accountId, DateTime _minDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { DbType = DbType.Xml, ParameterName = "xml", Value = xml });
                parameters.Add(new SqlParameter() { DbType = DbType.DateTime, ParameterName = "minDate", Value = _minDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "accountId", Value = accountId }); 
                DataSet ds = SQLHelper.ExecuteProcedure(connection, "Insert_Account_Transactions", CommandType.StoredProcedure, parameters);
            }
            catch(Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }
    }
}
