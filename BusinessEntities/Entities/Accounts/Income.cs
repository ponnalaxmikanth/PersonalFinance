using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities.Accounts
{
    public class Income
    {
        public DateTime PayDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double Billing { get; set; }
        public double Share { get; set; }
        public double Hours { get; set; }
        public double Insurance { get; set; }
        public double FederalTax { get; set; }
        public double SocialTax { get; set; }
        public double Medicare { get; set; }
        public double StateTax { get; set; }
        public double Reimbursment { get; set; }
        public double Miscelaneous { get; set; }
        public string Comments { get; set; }

        public double Vendor { get; set; }
        public double Employer { get; set; }
        public double Gross { get; set; }
        public double NetIncome { get; set; }
        public double Tax { get; set; }
        public double TaxPercent { get; set; }
    }

    public class IncomeDetailsRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
