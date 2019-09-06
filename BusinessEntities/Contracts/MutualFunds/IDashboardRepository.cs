using BusinessEntities.Entities;
using BusinessEntities.Entities.MutualFunds;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Contracts.MutualFunds
{
    public interface IDashboardRepository
    {
        void SetPath(string path);

        DashboardResponse GetDashboardData(DashboardRequest request);

        List<InvestmentsByMonth> GetDashboardChartData(DashboardRequest request);

        Individual GetIndividualData(DashboardRequest request);

        List<Investments> GetIndividualInvestments(DashboardIndividual request);

        List<SectorInvestments> GetSectorBreakup(DashboardRequest request);

        List<Investments> GetPerfOfMoreThanYear(DashboardIndividual request);

        ULIPValue GetULIP();

        List<BenchmarkHistory> GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate);

        DataTable GetNewDashboard(DateTime fromDate, DateTime toDate);

        object GetBenchMarks(DateTime fromDate, DateTime toDate);

        //object GetInvestmentProgress(DashboardIndividual request);
    }
}
