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
        DataSet GetInvestmentDetails(DashboardRequest request);

        DataSet GetUpcomingSipDetails(DashboardRequest request);

        DataSet GetInvestmentsByMonth(DashboardRequest request);

        DataSet GetIndividualInvestments(DashboardIndividual request);

        DataSet GetSectorBreakup(DashboardRequest request);

        DataSet GetInvestments(DashboardIndividual request);

        DataSet GetULIP();

        DataSet GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate);

        DataSet GetNewGraph(DateTime fromDate, DateTime toDate);

        DataSet GetBenchmarkPerformance(DateTime fromDate, DateTime toDate);

        DataSet Insert_mf_daily_tracker(int portfolioId, DateTime trackdate, int period, decimal investValue, decimal currentvalue, decimal profit);
    }
}
