using BusinessEntities.Entities;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess
{
    public class DigitalCurrencyRepository
    {
        DigitalCurrencyDataAccess dcDataAccess;

        readonly string _application = "BusinessAccess";
        readonly string _component = "DigitalCurrencyRepository";

        public DigitalCurrencyRepository()
        {
            dcDataAccess = new DigitalCurrencyDataAccess();
        }

        public InvestmentDetails GetInvestmentDetails()
        {
            return MapInvestmentDetails(dcDataAccess.GetInvestmentDetails());
        }

        private InvestmentDetails MapInvestmentDetails(DataSet dataSet)
        {
            InvestmentDetails result = new InvestmentDetails()
            {
                Current = new InvestmentValue(),
                Redeemed = new InvestmentValue()
            };

            if (dataSet != null || dataSet.Tables.Count != 0 || dataSet.Tables[0].Rows.Count != 0)
            {
                var res = (from dr in dataSet.Tables[0].AsEnumerable()
                 select new
                 {
                     size = decimal.Parse(dr["Size"].ToString()),
                     Price = decimal.Parse(dr["Price"].ToString()),
                     Fees = decimal.Parse(dr["Fees"].ToString()),
                     CurrentPrice = decimal.Parse(dr["CurrentPrice"].ToString()),
                     Amount = (decimal.Parse(dr["Size"].ToString()) * decimal.Parse(dr["Price"].ToString())) + decimal.Parse(dr["Fees"].ToString())
                 }).ToList();

                result.Current.Investment = res.Select(r => r.Amount).Sum();
                result.Current.CurrentValue = res.Select(r => r.CurrentPrice).Sum();
            }
            return result;
        }
    }
}
