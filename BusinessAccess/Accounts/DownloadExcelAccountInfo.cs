using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entities.Accounts;
using DataAccess.Accounts;
using Logging;

namespace BusinessAccess.Accounts
{
    public class DownloadExcelAccountInfo
    {
        DownloadExcelAccountInfoDataAccess _dataAccess = new DownloadExcelAccountInfoDataAccess();
        readonly string _application = "BusinessAccess";
        readonly string _component = "DownloadExcelAccountInfo";
        public List<AccountMappingDetails> GetAccountMappingDetails()
        {
            return MapAccountMappingDetails(_dataAccess.GetAccountMappingDetails());
        }

        private List<AccountMappingDetails> MapAccountMappingDetails(DataTable dataTable)
        {
            List<AccountMappingDetails> result = null;
            try
            {
                if(dataTable != null)
                {
                    result = (from dr in dataTable.AsEnumerable()
                              select new AccountMappingDetails()
                              {
                                  AccountId = int.Parse(dr["AccountId"].ToString()),
                                  DisplayName = dr["DisplayName"].ToString(),
                                  ExcelMapping = dr["ExcelMapping"].ToString(),
                                  Name = dr["Name"].ToString(),
                                  AccountTypeId = int.Parse(dr["AccountTypeId"].ToString()),
                                  AccountType = dr["AccountType"].ToString()
                              }).ToList();
                }
            }
            catch(Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        public void UpdateTransactions(string xml, int accountId, DateTime _minDate)
        {
            try
            {
                _dataAccess.UpdateTransactions(xml, accountId, _minDate);
            }
            catch(Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }
    }
}
