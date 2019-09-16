using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities.Accounts
{
    public class AccountMappingDetails
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ExcelMapping { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; }
    }

    public class Transactions
    {
        public int rowNumber { get; set; }
        public int AccountId { get; set; }
        public DateTime TransactDate { get; set; }
        public DateTime PostedDate { get; set; }
        public string Description { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Total { get; set; }
        public string TransactedBy { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string Comments { get; set; }
    }
}
