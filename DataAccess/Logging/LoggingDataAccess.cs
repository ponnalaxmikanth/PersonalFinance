using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Logging
{
    static public class LoggingDataAccess
    {
        static public void LogException(string application, string component, string exceptionMessage, string stackTrace)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "shortMessage", Value = exceptionMessage });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "longMessage", Value = stackTrace });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "component", Value = component });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "application", Value = application });

            DataSet ds = SQLHelper.ExecuteProcedure("Logging", "LogMessage", CommandType.StoredProcedure, parameters);
        }


        static public void LogMetric(string message, string application, string component, double metric)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "message", Value = message });
            parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "metric", Value = metric });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "component", Value = component });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "application", Value = application });

            DataSet ds = SQLHelper.ExecuteProcedure("Logging", "LogMetric", CommandType.StoredProcedure, parameters);
        }
    }
}
