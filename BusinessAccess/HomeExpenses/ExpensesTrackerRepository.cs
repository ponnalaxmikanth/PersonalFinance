using BusinessEntity.Expenses;
using DataAccess;
using Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Utilities;

namespace BusinessAccess.HomeExpenses
{
    public class ExpensesTrackerRepository
    {
        readonly ExpensesTrackerDataAccess _expensesTrackerDataAccess;
        readonly string _application = "BusinessAccess";
        readonly string _component = "ExpensesTrackerRepository";

        public ExpensesTrackerRepository()
        {
            _expensesTrackerDataAccess = new ExpensesTrackerDataAccess();
        }

        public void SetPath(string path)
        {
            _expensesTrackerDataAccess.SetPath(path);
        }

        public List<AccountType> GetAccountTypes()
        {
            List<AccountType> result = null;
            try
            {
                return MapAccountTypes(_expensesTrackerDataAccess.GetAccountTypes());
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        private List<AccountType> MapAccountTypes(DataTable dataTable)
        {
            List<AccountType> result = null;
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    result = (from dr in dataTable.AsEnumerable()
                              select new AccountType()
                              {
                                  Id = int.Parse(dr["AccountTypeId"].ToString()),
                                  Type = dr["AccountType"].ToString()
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }


        public List<AccountDetails> GetAccountDetails()
        {
            List<AccountDetails> result = null;
            try
            {
                return MapAccountDetails(_expensesTrackerDataAccess.GetAccountDetails());
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        private List<AccountDetails> MapAccountDetails(DataTable dataTable)
        {
            List<AccountDetails> result = null;
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    result = (from dr in dataTable.AsEnumerable()
                              select new AccountDetails()
                              {
                                  Id = int.Parse(dr["AccountId"].ToString()),
                                  Name = dr["Name"].ToString(),
                                  OpeningDate = Convert.ToDateTime(dr["OpenDate"]),
                                  DisplayName = dr["DisplayName"].ToString(),
                                  DisplayOrder = int.Parse(dr["DisplayOrder"].ToString()),
                                  AccountType = new AccountType()
                                  {
                                      Id = int.Parse(dr["AccountTypeId"].ToString()),
                                      Type = dr["AccountType"].ToString(),
                                  }

                              }).ToList().OrderBy(r=> r.DisplayOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        public List<ExpenseGroup> GetExpenseGroups()
        {
            try
            {
                return MapExpenseGroups(_expensesTrackerDataAccess.GetExpenseGroups());
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private List<ExpenseGroup> MapExpenseGroups(DataTable dataTable)
        {
            List<ExpenseGroup> result = null;
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    result = (from dr in dataTable.AsEnumerable()
                              select new ExpenseGroup()
                              {
                                  Id = int.Parse(dr["GroupId"].ToString()),
                                  Name = dr["GroupName"].ToString(),
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }


        public List<ExpenseSubGroup> GetExpenseSubGroups()
        {
            try
            {
                return MapExpenseSubGroups(_expensesTrackerDataAccess.GetExpenseSubGroups());
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private List<ExpenseSubGroup> MapExpenseSubGroups(DataTable dataTable)
        {
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return (from dr in dataTable.AsEnumerable()
                            select new ExpenseSubGroup()
                            {
                                GroupId = int.Parse(dr["GroupId"].ToString()),
                                Id = int.Parse(dr["Id"].ToString()),
                                SubGroupName = dr["SubGroupName"].ToString(),
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }


        public bool AddExpenseTransaction(ExpenseTransaction transaction)
        {
            try
            {
                return GetAddExpenseStatus(new ExpensesTrackerDataAccess().AddExpenseTransaction(transaction));
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return false;
        }

        private bool GetAddExpenseStatus(DataTable dataTable)
        {
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return dataTable.Rows[0][0].ToString() == "0" ? true : false;
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return false;
        }


        public List<string> GetItems(string item)
        {
            try
            {
                return MapItems(_expensesTrackerDataAccess.GetItem(item));
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private List<string> MapItems(DataTable dataTable)
        {
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return (from dr in dataTable.AsEnumerable()
                            select dr["Item"].ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }


        public List<string> GetStores(string store)
        {
            try
            {
                return MapStores(_expensesTrackerDataAccess.GetStores(store));
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private List<string> MapStores(DataTable dataTable)
        {
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return (from dr in dataTable.AsEnumerable()
                            select dr["Store"].ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public List<Expenses> GetExpenses(GetExpenses request)
        {
            try
            {
                return MapExpenses(_expensesTrackerDataAccess.GetExpenses(request));
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private List<Expenses> MapExpenses(DataTable dataTable)
        {
            try
            {
                if (dataTable != null)
                {
                    //decimal d = -1;
                    return (from dr in dataTable.AsEnumerable()
                            group dr by new
                            {
                                AccountId = int.Parse(dr["AccountId"].ToString()),
                                AccountName = dr["Name"].ToString(),
                                AccountDisplayName = dr["DisplayName"].ToString(),
                                displayOrder = int.Parse(dr["displayOrder"].ToString()),
                                //AccountLimit = decimal.TryParse(dr["Limit"].ToString()) ? decimal.Parse(dr["Limit"].ToString()) : null,
                                //AccountLimit = (dr["Limit"] != DBNull.Value || decimal.TryParse(dr["Limit"].ToString(), out d)) ? decimal.Parse(dr["Limit"].ToString()) : Decimal.MinValue,
                                AccountLimit = Conversions.ToDecimal(dr["Limit"]),

                                AccountTypeId = int.Parse(dr["AccountTypeId"].ToString()),
                                AccountType = dr["AccountType"].ToString(),
                            } into accountGrp
                            select new Expenses()
                            {
                                //AccountId = accountGrp.Key.AccountId,
                                //AccountName = accountGrp.Key.AccountName,
                                //AccountLimit = accountGrp.Key.AccountLimit,

                                //AccountTypeId = accountGrp.Key.AccountTypeId,
                                //AccountType = accountGrp.Key.AccountType,

                                AccountDetails = new AccountDetails()
                                {
                                    Id  = accountGrp.Key.AccountId,
                                    Name = accountGrp.Key.AccountName,
                                    DisplayName = accountGrp.Key.AccountDisplayName,
                                    DisplayOrder = accountGrp.Key.displayOrder,
                                    Limit = accountGrp.Key.AccountLimit,

                                    AccountType = new AccountType()
                                    {
                                        Id = accountGrp.Key.AccountTypeId,
                                        Type = accountGrp.Key.AccountType
                                    }
                                },
                                Transactions = (from t in accountGrp
                                                select new Transactions()
                                                {
                                                    Id = int.Parse(t["TransactionId"].ToString()),
                                                    Date = DateTime.Parse(t["TransactionDate"].ToString()),
                                                    GroupName = t["GroupName"].ToString(),
                                                    SubGroupName = t["SubGroupName"].ToString(),
                                                    //Item = t["Item"].ToString(),
                                                    Debit = decimal.Parse(t["Debit"].ToString()),
                                                    Credit = decimal.Parse(t["Credit"].ToString()),
                                                    Balance = decimal.Parse(t["Balance"].ToString()),
                                                    Store = t["Store"].ToString(),
                                                    TransactedBy = t["TransactedBy"].ToString(),


                                                    ExpenseGroup = new ExpenseGroup()
                                                    {
                                                        Id = int.Parse(t["GroupId"].ToString()),
                                                        Name = t["GroupName"].ToString(),
                                                    },

                                                    ExpenseSubGroup = new ExpenseSubGroup()
                                                    {
                                                        Id = int.Parse(t["SubGroupId"].ToString()),
                                                        //GroupId = int.Parse(t["GroupId"].ToString()),
                                                        SubGroupName = t["SubGroupName"].ToString(),
                                                    }
                                                }).ToList()
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public ExpenseTracker GetBudget(GetExpenses request)
        {
            try
            {
                return MapBudget(_expensesTrackerDataAccess.GetBudget(request));
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private ExpenseTracker MapBudget(DataTable dataTable)
        {
            ExpenseTracker epenseTracker = new ExpenseTracker();
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    epenseTracker.Expenses = (from dr in dataTable.AsEnumerable()
                            where dr["Group"].ToString().ToUpper() != "TOTAL"
                            select new Budget()
                            {
                                Group = dr["Group"].ToString(),
                                SubGroup = dr["SubGroup"].ToString(),
                                Credit = Conversions.ToDecimal(dr["credit"].ToString(), Convert.ToDecimal(-999999.99)),
                                Debit = decimal.Parse(dr["debit"].ToString()),
                                BudgetAmount = decimal.Parse(dr["Budget"].ToString()),
                                Balance = decimal.Parse(dr["Balance"].ToString()),
                                Level = int.Parse(dr["level"].ToString())
                            }).ToList();

                    epenseTracker.Summary = (from dr in dataTable.AsEnumerable()
                                              where dr["Group"].ToString().ToUpper() == "TOTAL"
                                              select new Budget()
                                              {
                                                  Group = dr["Group"].ToString(),
                                                  SubGroup = dr["SubGroup"].ToString(),
                                                  Credit = Conversions.ToDecimal(dr["credit"].ToString(), Convert.ToDecimal(-999999.99)),
                                                  Debit = decimal.Parse(dr["debit"].ToString()),
                                                  BudgetAmount = decimal.Parse(dr["Budget"].ToString()),
                                                  Balance = decimal.Parse(dr["Balance"].ToString()),
                                                  Level = int.Parse(dr["level"].ToString())
                                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return epenseTracker;
        }
    }
}
