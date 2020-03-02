using BusinessEntities.Entities.Accounts;
using DataAccess;
using Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess
{
    public class IncomeRepository
    {
        readonly string _application = "BusinessAccess";
        readonly string _component = "IncomeRepository";
        readonly IncomeDataAccess incomeDataAccess;

        public IncomeRepository()
        {
            incomeDataAccess = new IncomeDataAccess();
        }
        public bool AddIncome(Income income)
        {
            return incomeDataAccess.AddIncome(income);
        }

        public List<Income> GetIncomeDetails(IncomeDetailsRequest request)
        {
            return MapIncomeDetails(incomeDataAccess.GetIncomeDetails(request));
        }

        private List<Income> MapIncomeDetails(DataSet dataSet)
        {
            List<Income> result = null;
            try
            {
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    result = (from dr in dataSet.Tables[0].AsEnumerable()
                              select new Income()
                              {
                                  PayDate = Convert.ToDateTime(dr["PayDate"]),
                                  FromDate = Convert.ToDateTime(dr["FromDate"]),
                                  ToDate = Convert.ToDateTime(dr["ToDate"]),
                                  Billing = Convert.ToDouble(dr["Billing"].ToString()),
                                  Comments = string.Empty,
                                  FederalTax = Convert.ToDouble(dr["FederalTax"].ToString()),
                                  Hours = Convert.ToDouble(dr["Hours"].ToString()),
                                  Insurance = Convert.ToDouble(dr["Insurance"].ToString()),
                                  Medicare = Convert.ToDouble(dr["Medicare"].ToString()),
                                  Miscelaneous = Convert.ToDouble(dr["Miscelaneous"].ToString()),
                                  Reimbursment = Convert.ToDouble(dr["Reimbursment"].ToString()),
                                  Share = Convert.ToDouble(dr["Share"].ToString()),
                                  SocialTax = Convert.ToDouble(dr["SocialTax"].ToString()),
                                  StateTax = Convert.ToDouble(dr["StateTax"].ToString()),

                                  Vendor = Convert.ToDouble(dr["Vendor"].ToString()),
                                  Employer = Convert.ToDouble(dr["Employer"].ToString()),
                                  Gross = Convert.ToDouble(dr["Gross"].ToString()),
                                  NetIncome = Convert.ToDouble(dr["NetIncome"].ToString()),
                                  Tax = Convert.ToDouble(dr["Tax"].ToString()),
                                  TaxPercent = Convert.ToDouble(dr["TaxPercent"].ToString()),
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }
    }
}
