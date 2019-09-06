using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using BusinessEntities.Entities.MutualFunds;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.MutualFunds
{
    public class DashboardDataAccess : IDashboardDataAccess
    {
        static string serverPath = string.Empty;
        readonly string _application = "DataAccess.MutualFunds";
        readonly string _component = "DashboardDataAccess";

        public void SetPath(string path)
        {
            serverPath = path;
        }

        public DataTable GetInvestmentDetails(DashboardRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "FromDate", Value = request.FromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "ToDate", Value = request.ToDate });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Portfolio_Value", CommandType.StoredProcedure, parameters);
                if (ds != null)
                {
                    string jsonString = Utilities.Conversions.DataTableToJSON(ds.Tables[0]);
                    Utilities.WriteToFile.Write(serverPath + "\\InvestmentDetails.json", jsonString);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataTable GetUpcomingSipDetails(DashboardRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_SIP_Details", CommandType.StoredProcedure, parameters);
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

        public DataTable GetInvestmentsByMonth(DashboardRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "FromDate", Value = request.FromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "ToDate", Value = request.ToDate });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "portfolioId", Value = request.PortfolioId });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Investments_Details", CommandType.StoredProcedure, parameters);
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


        public DataTable GetIndividualInvestments(DashboardIndividual request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetTransactions", CommandType.StoredProcedure, parameters);
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

        public DataTable GetSectorBreakup(DashboardRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "portfolioId", Value = request.PortfolioId });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetInvestmentsByFundCategory", CommandType.StoredProcedure, parameters);
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


        public DataTable GetInvestments(DashboardIndividual request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "Type", Value = request.Type });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetIndividualTransactions", CommandType.StoredProcedure, parameters);
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

        public DataTable GetULIP()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetULIPValue", CommandType.StoredProcedure, parameters);
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

        public DataTable GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetBenchmarkHistoryValues", CommandType.StoredProcedure, parameters);
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

        public DataTable GetNewGraph(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetMyFundsHistoryValues", CommandType.StoredProcedure, parameters);
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



        public DataTable GetBenchmarkPerformance(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "benchMark", Value = "%" });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetBenchmarkPerformance", CommandType.StoredProcedure, parameters);
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

        public DataTable Insert_mf_daily_tracker(int portfolioId, DateTime trackdate, int period, decimal investValue, decimal currentvalue, decimal profit)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolioId", Value = portfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "trackdate", Value = trackdate });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "period", Value = period });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "investValue", Value = investValue });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "currentvalue", Value = currentvalue });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "profit", Value = profit });

                DataSet ds = SQLHelper.ExecuteProcedure("Investments", "insert_mf_daily_tracker", CommandType.StoredProcedure, parameters);
                //if (ds != null)
                //    return ds.Tables[0];
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

    }
}
