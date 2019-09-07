using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities.MutualFunds;
using DataAccess;
using Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.MutualFunds
{
    public class CommonRepository : ICommonRepository
    {
        ICommonDataAccess CommonDataAccess;
        readonly string _application = "BusinessAccess";
        readonly string _component = "CommonRepository";

        public CommonRepository(ICommonDataAccess common)
        {
            CommonDataAccess = common;
        }

        public void SetPath(string path)
        {
            CommonDataAccess.SetPath(path);
        }

        public List<MFDumpDates> GetDumpData(DateTime fromDate, DateTime toDate)
        {
            return  MapDumpData(CommonDataAccess.Get_MF_DataDumpDates(fromDate, toDate));
        }

        private List<MFDumpDates> MapDumpData(DataTable dataTable)
        {
            List<MFDumpDates> result = null;
            try
            {
                if (dataTable != null)
                {
                    result = (from dr in dataTable.AsEnumerable()
                              select new MFDumpDates()
                              {
                                  Date = Convert.ToDateTime(dr["Date"].ToString()),
                                  FundTypes = Convert.ToInt32(dr["FundType"].ToString())
                              }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        public void InsertDumpDate(DateTime date, int fundType, int count)
        {
            try
            {
                CommonDataAccess.InsertDumpDate(date, fundType, count);
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

    }
}
