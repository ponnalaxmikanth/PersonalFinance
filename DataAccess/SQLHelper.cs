using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Utilities;
using DataAccess.Logging;
using System.Diagnostics;

namespace DataAccess
{
    public static class SQLHelper
    {
        public static DataSet ExecuteProcedure(string key, string procName, CommandType cmdType, List<SqlParameter> parameters, string component = "")
        {
            Stopwatch watch = new Stopwatch();
            DataSet ds = new DataSet();
            try
            {
                string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                string connstr = Helper.GetConnectionString(key);
                watch.Start();
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    using (SqlCommand cmd = new SqlCommand(procName, conn))
                    {
                        if (parameters != null && parameters.Count > 0)
                            cmd.Parameters.AddRange(parameters.ToArray());
                        cmd.CommandType = cmdType;

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);
                        watch.Stop();
                        if(procName != "LogMetric" && procName != "LogMessage")
                        LoggingDataAccess.LogMetric("Proc:" + procName, appName, "Database" , watch.ElapsedMilliseconds);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return ds;
        }
    }
}
