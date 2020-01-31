using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Contracts
{
    public interface IMutualFundDataAccess
    {
        DataSet GetFolios();

        DataSet GetFundCategory();

        DataSet GetFundHouses();

        DataSet GetFundOptions();

        DataSet GetFunds();

        DataSet GetFundTypes();

        DataSet GetPortFolios();

        DataSet GetPortfolioTransactions(GetMFTransactions getMFTransactions);

        DataSet GetFundTransactions(GetMFTransactions getMFTransactions);

        DataSet AddUpdateMFTransaction(AddMFTransactionRequest _mfTransactionRequest);

        DataSet AddDividend(AddDividendRequest _dividendRequest);

        DataSet UpdateLatestNAV(List<NAVData> data);

        DataSet GetFundsPerformance();

        void BackUpNAVData();

        //void UpdateNAVHistory(List<NAVData> data);

        DataSet GetFundNav(DateTime date);

        string GetLastProcessedDetails();

        DataSet GetPortfolios();

        DataSet GetMyFunds(GetMyFundsRequest request);

        DataSet AddTransaction(AddMFTransactionRequest request);

        DataSet GetFundNav(GetFundNavRequest getFundNavRequest);

        DataSet GetFundValue(GetFundValueRequst getFundValueRequest);

        DataSet GetMyMFFundInvestments(GetMFFundInvestmentsRequest request);

        DataSet GetMFDdailyTracker(GetMFDailyTracker request);

        DataSet GetInvestments(DashboardIndividual request);

        //void SetPath(string path);

        DataSet GetInvestmentDetails();

        DataSet GetInvestmentPerformance(int portfolioId);

        DataSet GetInvestmentFundsDetails(int portfolioId);

    }
}
