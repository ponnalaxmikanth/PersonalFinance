using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities.Accounts
{
    public class AccountStatus
    {
        public int AccountId { get; set; }
        public string AccountType { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Total { get; set; }
        public string DisplayName { get; set; }
    }

    public class AccountsCurrentStatus {
        public AccountStatus Savings { get; set; }
        public AccountStatus Credit { get; set; }
    }
}
