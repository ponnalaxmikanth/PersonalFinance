using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities
{
    public class InvestmentValue
    {
        public decimal Investment { get; set; }
        public decimal CurrentValue { get; set; }
    }

    public class InvestmentDetails {
        public InvestmentValue Current { get; set; }
        public InvestmentValue Redeemed { get; set; }
    }
}
