using BusinessEntities.Entities.MutualFunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Contracts.MutualFunds
{
    public interface ICommonRepository
    {
        List<MFDumpDates> GetDumpData(DateTime fromDate, DateTime toDate);

        void InsertDumpDate(DateTime date, int fundType, int count);

    }
}
