using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities.MutualFunds;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MutualFunds
{
    public class DashboardDataAccess : IDashboardDataAccess
    {

        public DataTable GetInvestmentDetails(DashboardRequest request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "FromDate", Value = request.FromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "ToDate", Value = request.ToDate });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "Get_Portfolio_Value", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetUpcomingSipDetails(DashboardRequest request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "Get_SIP_Details", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetInvestmentsByMonth(DashboardRequest request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "FromDate", Value = request.FromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "ToDate", Value = request.ToDate });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "portfolioId", Value = request.PortfolioId });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "Get_Investments_Details", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }


        public DataTable GetIndividualInvestments(DashboardIndividual request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetTransactions", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetSectorBreakup(DashboardRequest request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "portfolioId", Value = request.PortfolioId });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetInvestmentsByFundCategory", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }


        public DataTable GetInvestments(DashboardIndividual request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "Type", Value = request.Type });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetIndividualTransactions", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetULIP()
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetULIPValue", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetBenchmarkHistoryValues", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetNewGraph(DateTime fromDate, DateTime toDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetMyFundsHistoryValues", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }



        public DataTable GetBenchmarkPerformance(DateTime fromDate, DateTime toDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "benchMark", Value = "%" });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "GetBenchmarkPerformance", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable Insert_mf_daily_tracker(int portfolioId, DateTime trackdate, decimal investValue, decimal currentvalue, decimal profit)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolioId", Value = portfolioId });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "trackdate", Value = trackdate });
            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "investValue", Value = investValue });
            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "currentvalue", Value = currentvalue });
            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "profit", Value = profit });

            DataSet ds = SQLHelper.GetDataFromDB("PersonalFinance", "insert_mf_daily_tracker", CommandType.StoredProcedure, parameters);
            //if (ds != null)
            //    return ds.Tables[0];
            return null;
        }

    }
}
