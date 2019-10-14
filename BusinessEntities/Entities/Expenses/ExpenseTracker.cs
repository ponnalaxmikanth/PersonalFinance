using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity.Expenses
{
    public class AccountType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class AccountDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime OpeningDate { get; set; }
        public Decimal? Limit { get; set; }
        public DateTime? LimitIncreaseDate { get; set; }
        public string LimitIncreaseStatus { get; set; }
        public AccountType AccountType { get; set; }
    }

    //public class ExpenseGroup
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    //public class ExpenseSubGroup
    //{
    //    public int Id { get; set; }
    //    public string SubGroupName { get; set; }
    //    public int GroupId { get; set; }
    //}

    public class ExpenseTransaction
    {
        public DateTime Date { get; set; }
        public int GroupId { get; set; }
        public int SubGroupId { get; set; }
        public string Item { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public string TransactedBy { get; set; }
        public string Store { get; set; }
    }

    public class GetExpenses
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int AccountId { get; set; }
    }

    public class Expenses
    {
        public AccountDetails AccountDetails { get; set; }
        //public int AccountId { get; set; }
        //public string AccountName { get; set; }
        //public decimal AccountLimit { get; set; }

        //public int AccountTypeId { get; set; }
        //public string AccountType { get; set; }

        public List<Transactions> Transactions { get; set; }
    }

    public class Transactions
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public string Item { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string Store { get; set; }
        public string TransactedBy { get; set; }

        //public ExpenseGroup ExpenseGroup { get; set; }
        //public ExpenseSubGroup ExpenseSubGroup { get; set; }
    }

    public class Budget
    {
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public decimal BudgetAmount { get; set; }
        public int Level { get; set; }
    }

    public class ExpensesChartData
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal Expense { get; set; }
        public decimal Credit { get; set; }
        public decimal Budget { get; set; }
    }

    public class ExpenseTracker {
        public List<Budget> Expenses { get; set; }
        public Budget Summary { get; set; }
    }

}
