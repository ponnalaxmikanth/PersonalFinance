using BusinessEntities.Contracts;
using BusinessEntities.Entities;
using BusinessEntities.Entities.MutualFunds;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.MutualFunds
{
    public class MutualFundsDataAccess : IMutualFundDataAccess
    {
        readonly string _application = "DataAccess.MutualFunds";
        readonly string _component = "MutualFundsDataAccess";
        public DataTable GetPortFolios()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Portfolios", CommandType.StoredProcedure, null);
                if (ds != null)
                {
                    string jsonString = Utilities.Conversions.DataTableToJSON(ds.Tables[0]);
                    Utilities.WriteToFile.Write(@"C:\Temp\PortFolios.json", jsonString);
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataTable GetPortfolioTransactions(GetMFTransactions getMFTransactions)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = getMFTransactions.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "FromDate", Value = getMFTransactions.FromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "ToDate", Value = getMFTransactions.ToDate });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Portfolio_Value", CommandType.StoredProcedure, parameters);
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

        public DataTable GetFolios()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Folios", CommandType.StoredProcedure, null);
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

        public DataTable GetFundCategory()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundCategory", CommandType.StoredProcedure, null);
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

        public DataTable GetFundHouses()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundHouses", CommandType.StoredProcedure, null);
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

        public DataTable GetFundOptions()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundOptions", CommandType.StoredProcedure, null);
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

        public DataTable GetFundTypes()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundTypes", CommandType.StoredProcedure, null);
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

        public DataTable GetFunds()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Funds", CommandType.StoredProcedure, null);
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

        public DataTable GetFundTransactions(GetMFTransactions getMFTransactions)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = getMFTransactions.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundId", Value = getMFTransactions.FundId });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "FromDate", Value = getMFTransactions.FromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "ToDate", Value = getMFTransactions.ToDate });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Fund_Transactions", CommandType.StoredProcedure, parameters);
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

        public DataTable AddUpdateMFTransaction(AddMFTransactionRequest _mfTransactionRequest)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();


                if (_mfTransactionRequest.TransactionType.ToLower() == "Purchase".ToLower())
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "TransactionId", Value = _mfTransactionRequest.TransactionId });
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "PortfolioId", Value = _mfTransactionRequest.PortfolioId });
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FolioId", Value = _mfTransactionRequest.FolioId });
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundId", Value = _mfTransactionRequest.FundId });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "PurchaseDate", Value = _mfTransactionRequest.PurchaseDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Amount", Value = _mfTransactionRequest.Amount });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "PurchaseNAV", Value = _mfTransactionRequest.PurchaseNAV });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Dividend", Value = _mfTransactionRequest.Dividend });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "SIP", Value = _mfTransactionRequest.IsSIP ? "Y" : "N" });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Units", Value = _mfTransactionRequest.SellUnits });

                    DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "AddMFTransaction", CommandType.StoredProcedure, parameters);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                else if (_mfTransactionRequest.TransactionType.ToLower() == "Redeem".ToLower())
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "fundid", Value = _mfTransactionRequest.FundId });
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolio", Value = _mfTransactionRequest.PortfolioId });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "selldate", Value = _mfTransactionRequest.SellDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "units", Value = _mfTransactionRequest.SellUnits });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "nav", Value = _mfTransactionRequest.PurchaseNAV });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "stt", Value = _mfTransactionRequest.STT });

                    DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "AddRedeemTransaction", CommandType.StoredProcedure, parameters);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }

            return null;
        }

        public DataTable AddDividend(AddDividendRequest _dividendRequest)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundId", Value = _dividendRequest.FundId });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "DividendDate", Value = _dividendRequest.DividendDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Dividend", Value = _dividendRequest.Dividend });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "NAV", Value = _dividendRequest.NAV });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "UpdateDividend", CommandType.StoredProcedure, parameters);

                if (ds != null && ds.Tables.Count > 0)
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

        public void UpdateNAVHistory(List<NAVData> data)
        {
            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "schemaCode", Value = data[i].SchemaCode });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "nav", Value = data[i].NAV });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "fundtype", Value = data[i].FundType });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "date", Value = data[i].Date });

                    DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "UpdateFundNAV_History", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public void UpdateNAVHistory(string xmlData)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "xml", Value = xmlData });
                DataSet ds = SQLHelper.ExecuteProcedure("Investments", "UpdateFundNAVHistory", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public DataTable UpdateLatestNAV(List<NAVData> data)
        {
            try
            {
                for (int i = 0; i < data.Count; i++)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fundhouse", Value = data[i].FundHouse });
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "schemaCode", Value = data[i].SchemaCode });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "isinGrowth", Value = data[i].ISINGrowth });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "isinDivReInv", Value = data[i].ISINDivReinvestment });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fundType", Value = data[i].FundType });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "SchemaName", Value = data[i].SchemaName });
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "nav", Value = data[i].NAV });
                    //parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "repurchasePrice", Value = data[i].RepurchasePrice});
                    //parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "salePrice", Value = data[i].SellPrice});

                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fundOption", Value = data[i].FundOption });
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fund_type", Value = data[i].Fund_Type });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "date", Value = data[i].Date });

                    DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "UpdateFundNAV", CommandType.StoredProcedure, parameters);

                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataTable UpdateFundsNAV(string strXml, string type)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "xml", Value = strXml });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "schemaType", Value = type });

                DataSet ds = SQLHelper.ExecuteProcedure("Investments", "DownloadFundsNAV", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public void BackUpNAVData()
        {
            try
            {
                SQLHelper.ExecuteProcedure("PersonalFinance", "Backup_FundNavData", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public DataTable GetFundsPerformance()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetFundsPerformance", CommandType.StoredProcedure, null);
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

        public DataTable GetFundNav(DateTime date)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "NavDate", Value = date.Date });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetFundsNav", CommandType.StoredProcedure, parameters);
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


        public string GetLastProcessedDetails()
        {
            try
            {
                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetLastProcessedDetails", CommandType.StoredProcedure, null);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["NAVDate"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public void RefreshFundDetails(List<FundHouse> fundFamilies)
        {
            try
            {
                for (int i = 0; i < fundFamilies.Count; i++)
                {
                    for (int f = 0; f < fundFamilies[i].FundLinks.Count; f++)
                    {
                        if (fundFamilies[i] == null
                            || fundFamilies[i].FundLinks[f] == null
                            || fundFamilies[i].FundLinks[f].FundDetails == null
                            || fundFamilies[i].FundLinks[f].FundDetails.SchemaCode == 0)
                        {
                            continue;
                        }

                        try
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();

                            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "schemaCode", Value = fundFamilies[i].FundLinks[f].FundDetails.SchemaCode });
                            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "expenseRatio", Value = fundFamilies[i].FundLinks[f].FundDetails.ExpenseRatio });
                            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "category", Value = fundFamilies[i].FundLinks[f].FundDetails.Category });
                            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "status", Value = fundFamilies[i].FundLinks[f].FundDetails.Status });
                            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "tunrover", Value = fundFamilies[i].FundLinks[f].FundDetails.TurnOver });
                            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "minimum", Value = fundFamilies[i].FundLinks[f].FundDetails.MinimumInvest });
                            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "benchmark", Value = fundFamilies[i].FundLinks[f].FundDetails.Benchmark });
                            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "MScategory", Value = fundFamilies[i].FundLinks[f].FundDetails.MSCategory });

                            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "UpdateMFFundDetails", CommandType.StoredProcedure, parameters);
                        }
                        catch (Exception ex)
                        {
                            LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public void DumpBenchMarkData(BenchMark benchMark)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "BenchMark", Value = benchMark.BenchMarkName });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "Date", Value = benchMark.Date });

                if (benchMark.Open != null)
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "OpenValue", Value = benchMark.Open });
                }
                else
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "OpenValue", Value = DBNull.Value });
                }

                if (benchMark.High != null)
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "HighValue", Value = benchMark.High });
                }
                else
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "HighValue", Value = DBNull.Value });
                }

                if (benchMark.Low != null)
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "LowValue", Value = benchMark.Low });
                }
                else
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "LowValue", Value = DBNull.Value });
                }

                if (benchMark.Close != null)
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "CloseValue", Value = benchMark.Close });
                }
                else
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "CloseValue", Value = DBNull.Value });
                }

                if (benchMark.SharesTraded != null)
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "SharesTraded", Value = benchMark.SharesTraded });
                }
                else
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "SharesTraded", Value = DBNull.Value });
                }

                if (benchMark.TurnOver != null)
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "TurnOver", Value = benchMark.TurnOver });
                }
                else
                {
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "TurnOver", Value = DBNull.Value });
                }

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "DumpBenchMarkData", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public DataTable GetPortfolios()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Portfolios", CommandType.StoredProcedure, parameters);
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

        public DataTable GetMyFunds(GetMyFundsRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "transaction", Value = request.Type });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolioId", Value = request.PortfolioId });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetMyFunds", CommandType.StoredProcedure, parameters);
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

        public DataTable AddTransaction(AddMFTransactionRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "PortfolioId", Value = request.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "houseId", Value = request.FundHouseId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "typeId", Value = request.FundTypeId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "categoryId", Value = request.FundCategoryId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "optionsId", Value = request.FundOptionsId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "schemaCode", Value = request.SchemaCode });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "growthschemaCode", Value = request.GrowthSchemaCode });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "fundName", Value = request.FundName });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "PurchaseDate", Value = request.PurchaseDate.Date });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FolioId", Value = request.FolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Amount", Value = request.Amount });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "PurchaseNAV", Value = request.PurchaseNAV });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Units", Value = request.Units });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "SIP", Value = (request.IsSIP == true ? "Y" : "N") });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "AddMFPurchase", CommandType.StoredProcedure, parameters);
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

        public DataTable GetFundNav(GetFundNavRequest getFundNavRequest)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "schemaCode", Value = getFundNavRequest.SchemaCode });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "date", Value = getFundNavRequest.Date.Date });


                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetFundPrice", CommandType.StoredProcedure, parameters);

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

        public DataTable GetFundValue(GetFundValueRequst getFundValueRequest)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolio", Value = getFundValueRequest.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "schemaCode", Value = getFundValueRequest.SchemaCode });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "folioId", Value = getFundValueRequest.FolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "optionId", Value = getFundValueRequest.OptionId });


                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetFundValue", CommandType.StoredProcedure, parameters);

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

        public DataTable GetMyMFFundInvestments(GetMFFundInvestmentsRequest request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolioId", Value = request.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "fundid", Value = request.FundId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "folioId", Value = request.FolioId });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetMyMFFundInvestments", CommandType.StoredProcedure, parameters);
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

        public DataTable GetMFDdailyTracker(GetMFDailyTracker request)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = request.fromDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = request.toDate });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetMFDdailyTracker", CommandType.StoredProcedure, parameters);
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

                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "PortfolioId", Value = request.PortfolioId });
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

    }
}
