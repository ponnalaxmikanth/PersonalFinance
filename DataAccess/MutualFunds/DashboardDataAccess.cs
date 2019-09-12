using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using BusinessEntities.Entities.MutualFunds;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.MutualFunds
{
    public class DashboardDataAccess : BaseDataAccess, IDashboardDataAccess
    {
        readonly string serverPath = "\\MFDashboard\\";
        readonly string _application = "DataAccess.MutualFunds";
        readonly string _component = "DashboardDataAccess";
        //static string DataStorePath = ConfigurationManager.AppSettings["DataStorePath"];

        //public void SetPath(string path)
        //{
        //    if(string.IsNullOrWhiteSpace(serverPath))
        //        serverPath = path + "\\MutualFunds\\";
        //}

        //string GetDataSourceOath() {
        //    string path = ConfigurationManager.AppSettings["DataStorePath"];
        //    return path;
        //}

        public DataSet GetInvestmentDetails(DashboardRequest request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "InvestmentDetails.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "FromDate", Value = request.FromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "ToDate", Value = request.ToDate });
                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Portfolio_Value", CommandType.StoredProcedure, parameters);
                }
                if (ds != null && ds.Tables.Count > 0 && !UseMockData && EnableStoreDataAsJson)
                {
                    Utilities.FileOperations.Write(DataStorePath + "\\InvestmentDetails.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetUpcomingSipDetails(DashboardRequest request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "UpcomingSipDetails.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_SIP_Details", CommandType.StoredProcedure, parameters);
                }
                if (ds != null && ds.Tables.Count > 0 && !UseMockData && EnableStoreDataAsJson)
                {
                    Utilities.FileOperations.Write(serverPath + "\\UpcomingSipDetails.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetInvestmentsByMonth(DashboardRequest request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "InvestmentsByMonth.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "FromDate", Value = request.FromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "ToDate", Value = request.ToDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "portfolioId", Value = request.PortfolioId });
                    ds = SQLHelper.ExecuteProcedure("Investments", "Get_Investments_Details", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\InvestmentsByMonth.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetIndividualInvestments(DashboardIndividual request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "IndividualInvestments.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });

                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetTransactions", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\IndividualInvestments.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetSectorBreakup(DashboardRequest request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "SectorBreakup.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "portfolioId", Value = request.PortfolioId });
                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetInvestmentsByFundCategory", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\SectorBreakup.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetInvestments(DashboardIndividual request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "Investments.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = request.PortfolioId });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "Type", Value = request.Type });

                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetIndividualTransactions", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\Investments.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetULIP()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "ULIP.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetULIPValue", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\ULIP.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "ULIP.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetBenchmarkHistoryValues", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\BenchmarkHistoryValues.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetNewGraph(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "ULIP.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });

                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetMyFundsHistoryValues", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\NewGraph.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetBenchmarkPerformance(DateTime fromDate, DateTime toDate)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "ULIP.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = fromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = toDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "benchMark", Value = "%" });

                    ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetBenchmarkPerformance", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(serverPath + "\\BenchmarkPerformance.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet Insert_mf_daily_tracker(int portfolioId, DateTime trackdate, int period, decimal investValue, decimal currentvalue, decimal profit)
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
