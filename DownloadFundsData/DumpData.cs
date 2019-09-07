using BusinessAccess.MutualFunds;
using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities.MutualFunds;
using DataAccess;
using DataAccess.Logging;
using DataAccess.MutualFunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class DumpData : BaseClass
    {
        MutualFundsDataAccess _mfDataAccess = null;
        ICommonDataAccess _CommonDataAccess = null;
        ICommonRepository _CommonRepository = null;
        MutualFundsRepository _mutualBusinessAccess = null;
        readonly string _application = "DownloadFundsData";
        readonly string _component = "DumpData";

        public DumpData()
        {
            _mfDataAccess = new MutualFundsDataAccess();
            _CommonDataAccess = new CommonDataAccess();
            _CommonRepository = new CommonRepository(_CommonDataAccess);
            _mutualBusinessAccess = new MutualFundsRepository(_CommonDataAccess, _mfDataAccess);
        }

        public void DumpBenchMarkData(BenchMark benchMark)
        {
            try
            {
                _mfDataAccess.DumpBenchMarkData(benchMark);
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }
    }
}
