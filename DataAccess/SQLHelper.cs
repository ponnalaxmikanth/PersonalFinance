using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Utilities;

namespace DataAccess
{
    public static class SQLHelper
    {
        public static DataSet GetDataFromDB(string key, string procName, CommandType cmdType, List<SqlParameter> parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                string connstr = Helper.GetConnectionString(key);
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
