using BusinessEntities.Entities.Accounts;
using DataAccess.Accounts;
using System.Data;
using System.Linq;
using Utilities;

namespace BusinessAccess.Accounts
{
    public class AccountsRepository : BaseRepository
    {
        readonly AccountsDataAccess _accountsDataAccess;

        public AccountsRepository()
        {
            _accountsDataAccess = new AccountsDataAccess();
        }

        public AccountsCurrentStatus GetAccountStatusDetails(int Details = 0)
        {
            return MapAccountStatusDetails(_accountsDataAccess.GetAccountStatusDetails(Details));
        }

        private AccountsCurrentStatus MapAccountStatusDetails(DataSet dataSet)
        {
            AccountsCurrentStatus result = new AccountsCurrentStatus();
            result.Savings = MapAccountStatus(dataSet, "Savings");
            result.Credit = MapAccountStatus(dataSet, "Credit");
            return result;
        }

        private AccountStatus MapAccountStatus(DataSet dataSet, string type)
        {
            AccountStatus result = null;

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                result = (from dr in dataSet.Tables[0].AsEnumerable()
                          where dr["AccountType"].ToString() == type
                          select new AccountStatus()
                          {
                              Credit = Conversions.ToDecimal(dr["Credit"], 0),
                              Debit = Conversions.ToDecimal(dr["Debit"], 0),
                              Total = Conversions.ToDecimal(dr["Total"], 0),
                              AccountType = type
                          }).FirstOrDefault();
            }
            return result;
        }
    }
}
