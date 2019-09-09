using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Contracts.MutualFunds
{
    public interface ICommonDataAccess
    {
        void SetPath(string path);

        DataTable Get_MF_DataDumpDates(DateTime fromDate, DateTime toDate);

        void InsertDumpDate(DateTime date, int fundType, int count);

    }
}
