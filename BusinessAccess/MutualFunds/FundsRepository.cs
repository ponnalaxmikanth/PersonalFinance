using BusinessEntities.Contracts;
using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessAccess.MutualFunds
{
    public partial class MutualFundsRepository : BaseRepository, IMutualFundsBusinessAccess
    {
        public InvestmentDetails GetInvestmentDetails()
        {
            return MapInvestmentDetails(_mfDataAccess.GetInvestmentDetails());
        }
        
    }
}
