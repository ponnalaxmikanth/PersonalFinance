using BusinessEntity.Expenses;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ExpensesTrackerDataAccess: BaseDataAccess
    {
        readonly string _application = "DataAccess";
        readonly string _component = "ExpensesTrackerDataAccess";

        readonly string serverPath = "\\ExpensesTracker\\";
        //public void SetPath(string path)
        //{
        //    if (string.IsNullOrWhiteSpace(serverPath))
        //        serverPath = "\\ExpensesTracker\\";
        //}

        public DataSet GetAccountTypes()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "AccountTypes.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetAccountTypes", CommandType.StoredProcedure, new List<SqlParameter>(), _component);
                }
                if (ds != null && ds.Tables.Count > 0 && !UseMockData && EnableStoreDataAsJson)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "AccountTypes.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetAccountDetails()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "AccountDetails.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetAccountDetails", CommandType.StoredProcedure, new List<SqlParameter>(), _component);
                    if (ds != null && ds.Tables.Count > 0 && !UseMockData && EnableStoreDataAsJson)
                    {
                        Utilities.FileOperations.Write(DataStorePath + serverPath + "\\AccountDetails.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetExpenseGroups()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "ExpenseGroups.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetExpenseGroups", CommandType.StoredProcedure, new List<SqlParameter>(), _component);
                }
                    if (ds != null && ds.Tables.Count > 0 && !UseMockData && EnableStoreDataAsJson)
                    {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\ExpenseGroups.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataSet GetExpenseSubGroups()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "ExpenseSubGroups.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetExpenseSubGroups", CommandType.StoredProcedure, new List<SqlParameter>(), _component);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\ExpenseSubGroups.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetStores()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "Stores.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetStore", CommandType.StoredProcedure, new List<SqlParameter>(), _component);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\Stores.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetItem(string item)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "Item.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "item", Value = item });

                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetItem", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\Item.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetStores(string store)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "Stores.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "store", Value = store });
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetStore", CommandType.StoredProcedure, parameters);
                }
                
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\Stores.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }


        public DataTable AddExpenseTransaction(ExpenseTransaction transaction)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "transactionDate", Value = transaction.Date });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "groupId", Value = transaction.GroupId });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "subgroupid", Value = transaction.SubGroupId });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "item", Value = transaction.Item });
                parameters.Add(new SqlParameter() { DbType = DbType.Decimal, ParameterName = "amount", Value = transaction.Amount });
                parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "accountid", Value = transaction.AccountId });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "transactedBy", Value = transaction.TransactedBy });
                parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "store", Value = transaction.Store });

                DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "AddHomeTransactions", CommandType.StoredProcedure, parameters);
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\ExpenseTransaction.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataSet GetExpenses(GetExpenses request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "Expenses.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = request.FromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = request.ToDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "accountId", Value = request.AccountId });

                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetHomeTransactions", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\Expenses.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetBudget(GetExpenses request)
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "Budget.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = request.FromDate });
                    parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = request.ToDate });

                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetBudgetTransactions", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\Budget.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                    return ds;
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetExpensesChartData()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "LastYearTransactions.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetLastYearTransactions", CommandType.StoredProcedure, parameters);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\LastYearTransactions.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return ds;
        }
    }
}
