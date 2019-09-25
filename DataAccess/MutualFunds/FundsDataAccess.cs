using DataAccess.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MutualFunds
{
    public partial class MutualFundsDataAccess
    {
        public DataSet GetInvestmentDetails()
        {
            DataSet ds = null;
            try
            {
                if (UseMockData)
                {
                    string fileContent = Utilities.FileOperations.ReadFileContent(DataStorePath + serverPath + "FundsInvestmentDetails.json");
                    ds = Utilities.Conversions.JSONToDataSet(fileContent);
                }
                else
                {
                    ds = SQLHelper.ExecuteProcedure("Investments", "GetFundsInvestmentDetails", CommandType.StoredProcedure, null);
                }
                if (ds != null)
                {
                    Utilities.FileOperations.Write(DataStorePath + serverPath + "\\FundsInvestmentDetails.json", Utilities.Conversions.DataTableToJSON(ds.Tables[0]));
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
