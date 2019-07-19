using BusinessEntities.Contracts;
using BusinessEntities.Entities;
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
    public class MutualFundsDataAccess : IMutualFundDataAccess
    {
        public DataTable GetPortFolios()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Portfolios", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetPortfolioTransactions(GetMFTransactions getMFTransactions)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = getMFTransactions.PortfolioId });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "FromDate", Value = getMFTransactions.FromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "ToDate", Value = getMFTransactions.ToDate });

            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Portfolio_Value", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFolios()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Folios", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFundCategory()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundCategory", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFundHouses()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundHouses", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFundOptions()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundOptions", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFundTypes()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_FundTypes", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFunds()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_MF_Funds", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFundTransactions(GetMFTransactions getMFTransactions)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "PortfolioId", Value = getMFTransactions.PortfolioId });
            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundId", Value = getMFTransactions.FundId });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "FromDate", Value = getMFTransactions.FromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "ToDate", Value = getMFTransactions.ToDate });

            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "Get_Fund_Transactions", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable AddUpdateMFTransaction(AddMFTransactionRequest _mfTransactionRequest)
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
                    return ds.Tables[0];
            }
            else if(_mfTransactionRequest.TransactionType.ToLower() == "Redeem".ToLower())
            {
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "fundid", Value = _mfTransactionRequest.FundId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "portfolio", Value = _mfTransactionRequest.PortfolioId });
                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "selldate", Value = _mfTransactionRequest.SellDate });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "units", Value = _mfTransactionRequest.SellUnits });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "nav", Value = _mfTransactionRequest.PurchaseNAV });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "stt", Value = _mfTransactionRequest.STT });

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "AddRedeemTransaction", CommandType.StoredProcedure, parameters);
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];

            }
            
            return null;
        }

        public DataTable AddDividend(AddDividendRequest _dividendRequest)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "FundId", Value = _dividendRequest.FundId });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "DividendDate", Value = _dividendRequest.DividendDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "Dividend", Value = _dividendRequest.Dividend });
            parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "NAV", Value = _dividendRequest.NAV });

            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "UpdateDividend", CommandType.StoredProcedure, parameters);

            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        public void UpdateNAVHistory(List<NAVData> data)
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

        public void UpdateNAVHistory(string xmlData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "xml", Value = xmlData });
            DataSet ds = SQLHelper.ExecuteProcedure("Investments", "UpdateFundNAVHistory", CommandType.StoredProcedure, parameters);
        }

        public DataTable UpdateLatestNAV(List<NAVData> data)
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
            return null;
        }

        public DataTable UpdateFundsNAV(string strXml, string type)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "xml", Value = strXml });
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "schemaType", Value = type });

            DataSet ds = SQLHelper.ExecuteProcedure("Investments", "DownloadFundsNAV", CommandType.StoredProcedure, parameters);
            return null;
        }

        public void BackUpNAVData()
        {
            SQLHelper.ExecuteProcedure("PersonalFinance", "Backup_FundNavData", CommandType.StoredProcedure, null);
        }

        public DataTable GetFundsPerformance()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetFundsPerformance", CommandType.StoredProcedure, null);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }

        public DataTable GetFundNav(DateTime date)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "NavDate", Value = date.Date });

            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetFundsNav", CommandType.StoredProcedure, parameters);
            if (ds != null)
                return ds.Tables[0];
            return null;
        }


        public string GetLastProcessedDetails()
        {
            DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "GetLastProcessedDetails", CommandType.StoredProcedure, null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["NAVDate"].ToString();
            return null;
        }

        public void RefreshFundDetails(List<FundHouse> fundFamilies)
        {
            for (int i = 0; i < fundFamilies.Count; i++)
            {
                for (int f = 0; f < fundFamilies[i].FundLinks.Count; f++)
                {
                    if (fundFamilies[i] == null 
                        || fundFamilies[i].FundLinks[f] == null 
                        || fundFamilies[i].FundLinks[f].FundDetails == null 
                        || fundFamilies[i].FundLinks[f].FundDetails.SchemaCode == 0) 
                        continue;
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
 
                    }
                }
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
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "OpenValue", Value = benchMark.Open });
                else
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "OpenValue", Value = DBNull.Value });

                if (benchMark.High != null)
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "HighValue", Value = benchMark.High });
                else
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "HighValue", Value = DBNull.Value });


                if (benchMark.Low != null)
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "LowValue", Value = benchMark.Low });
                else
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "LowValue", Value = DBNull.Value });
                

                if (benchMark.Close != null)
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "CloseValue", Value = benchMark.Close });
                else
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "CloseValue", Value = DBNull.Value });
                

                if (benchMark.SharesTraded != null)
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "SharesTraded", Value = benchMark.SharesTraded });
                else
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "SharesTraded", Value = DBNull.Value });
                

                if (benchMark.TurnOver != null)
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "TurnOver", Value = benchMark.TurnOver });
                else
                    parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "TurnOver", Value = DBNull.Value });
                

                DataSet ds = SQLHelper.ExecuteProcedure("PersonalFinance", "DumpBenchMarkData", CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
 
            }
        }
    }
}
