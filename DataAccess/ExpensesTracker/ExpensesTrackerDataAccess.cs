using BusinessEntity.Expenses;
using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ExpensesTrackerDataAccess
    {
        readonly string _application = "DataAccess";
        readonly string _component = "ExpensesTrackerDataAccess";
        public DataTable GetAccountTypes()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetAccountTypes", CommandType.StoredProcedure, parameters);
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

        public DataTable GetAccountDetails()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetAccountDetails", CommandType.StoredProcedure, parameters);
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

        public DataTable GetExpenseGroups()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetExpenseGroups", CommandType.StoredProcedure, parameters);
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

        public DataTable GetExpenseSubGroups()
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetExpenseSubGroups", CommandType.StoredProcedure, parameters);
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

        public DataTable GetStores()
        {
            try { 
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetStore", CommandType.StoredProcedure, parameters);
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

        public DataTable GetItem(string item)
        {
            try { 
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "item", Value = item });

            DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetItem", CommandType.StoredProcedure, parameters);
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

        public DataTable GetStores(string store)
        {
            try { 
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter() { DbType = DbType.String, ParameterName = "store", Value = store });

            DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetStore", CommandType.StoredProcedure, parameters);
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


        public DataTable AddExpenseTransaction(ExpenseTransaction transaction)
        {
            try { 
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
                return ds.Tables[0];
            }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataTable GetExpenses(GetExpenses request)
        {
            try { 
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = request.FromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = request.ToDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Int32, ParameterName = "accountId", Value = request.AccountId });

            DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetHomeTransactions", CommandType.StoredProcedure, parameters);
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

        public DataTable GetBudget(GetExpenses request)
        {
            try { 
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "fromDate", Value = request.FromDate });
            parameters.Add(new SqlParameter() { DbType = DbType.Date, ParameterName = "toDate", Value = request.ToDate });

            DataSet ds = SQLHelper.ExecuteProcedure("HomeTransactions", "GetBudgetTransactions", CommandType.StoredProcedure, parameters);
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
