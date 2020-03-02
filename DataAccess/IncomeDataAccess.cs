using BusinessEntities.Entities.Accounts;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class IncomeDataAccess
    {
        readonly string _application = "DataAccess";
        readonly string _component = "IncomeAccess";

        public bool AddIncome(Income income)
        {
            try
            {   
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "PayDate", Value = income.PayDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "FromDate", Value = income.FromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "ToDate", Value = income.ToDate });

                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Billing", Value = income.Billing });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Share", Value = income.Share });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Hours", Value = income.Hours });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Insurance", Value = income.Insurance });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "FederalTax", Value = income.FederalTax });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "SocialTax", Value = income.SocialTax });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Medicare", Value = income.Medicare });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "StateTax", Value = income.StateTax });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Reimbursment", Value = income.Reimbursment });
                parameters.Add(new SqlParameter() { DbType = DbType.Double, ParameterName = "Miscelaneous", Value = income.Miscelaneous });

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "Comments", Value = income.Comments });

                SQLHelper.ExecuteProcedure("HomeTransactions", "AddIncome", CommandType.StoredProcedure, parameters, _component);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
                return false;
            }
            return true;
        }

        public DataSet GetIncomeDetails(IncomeDetailsRequest request)
        {
            DataSet ds = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = request.FromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = request.ToDate });

                ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetIncomeDetails", CommandType.StoredProcedure, parameters, _component);    
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }
    }
}
