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
    public interface IDashboardDataAccess
    {
        void SetPath(string path);

        DataTable GetInvestmentDetails(DashboardRequest request);

        DataTable GetUpcomingSipDetails(DashboardRequest request);

        DataTable GetInvestmentsByMonth(DashboardRequest request);

        DataTable GetIndividualInvestments(DashboardIndividual request);

        DataTable GetSectorBreakup(DashboardRequest request);

        DataTable GetInvestments(DashboardIndividual request);

        DataTable GetULIP();

        DataTable GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate);

        DataTable GetNewGraph(DateTime fromDate, DateTime toDate);

        DataTable GetBenchmarkPerformance(DateTime fromDate, DateTime toDate);

        DataTable Insert_mf_daily_tracker(int portfolioId, DateTime trackdate, int period, decimal investValue, decimal currentvalue, decimal profit);
    }
}
