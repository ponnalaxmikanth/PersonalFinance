using BusinessEntities.Contracts.MutualFunds;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MutualFunds
{
    public class CommonDataAccess : ICommonDataAccess
    {
        public DataTable Get_MF_DataDumpDates(DateTime fromDate, DateTime toDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fromDate", Value = fromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "Get_MF_DataDumpDates", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }


        public void InsertDumpDate(DateTime date, int fundType, int count)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "Date", Value = date.ToString("MM/dd/yyyy") });
            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundType", Value = fundType });
            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "Count", Value = count });

            SQLHelper.GetDataFromDB("PersonalFinance", "Insetrt_MF_DataDumpDates", CommandType.StoredProcedure, parameters);
            
            return;
        }

    }
}
