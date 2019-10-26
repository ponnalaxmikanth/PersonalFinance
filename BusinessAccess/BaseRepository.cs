using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessAccess
{
    public class BaseRepository
    {
        protected InvestmentDetails MapInvestmentDetails(DataSet dataSet)
        {
            InvestmentDetails result = new InvestmentDetails()
            {
                Current = new InvestmentValue(),
                Redeemed = new InvestmentValue()
            };

            if (dataSet != null || dataSet.Tables.Count != 0 || dataSet.Tables[0].Rows.Count != 0)
            {
                result.Current = GetDetails(dataSet.Tables[0], "C");
                result.Redeemed = GetDetails(dataSet.Tables[0], "R");
            }

            return result;
        }

        private InvestmentValue GetDetails(DataTable dataTable, string type)
        {
            InvestmentValue result = new InvestmentValue();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                result = (from dr in dataTable.AsEnumerable()
                          where dr["Type"].ToString() == type
                          select new InvestmentValue()
                          {
                              CurrentValue = Conversions.ToDecimal(dr["CurrentValue"], 0),
                              Investment = Conversions.ToDecimal(dr["Investment"], 0),
                          }
                         ).FirstOrDefault();
            }

            return result;
        }

    }
}
