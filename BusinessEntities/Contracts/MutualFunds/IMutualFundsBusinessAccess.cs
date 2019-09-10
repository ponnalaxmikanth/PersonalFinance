using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Contracts
{
    public interface IMutualFundsBusinessAccess
    {
        Task<bool> DownloadNAVData(string link, int fundType, DateTime date);
        //bool DownloadNAVData(string link, int fundType);

        List<MF_Portfolio> GetMutualFundPortfolios();
        List<MFPortfolioData> GetPortfolioTransactions(GetMFTransactions getMFTransactions);
        List<MF_Transactions> GetFundTransactions(GetMFTransactions getMFTransactions);

        MFFundDetails GetMFFundDetails();

        List<MF_Transactions> AddUpdateMFTransaction(AddMFTransactionRequest _mfTransactionRequest);
        void AddDividend(AddDividendRequest _dividendRequest);
        void BackUpNAVData();
        List<FundPerformance> GetFundsPerformance();

        List<NAVData> GetFundsNAV(DateTime date);

        //void DownloadFundNavHistory(DateTime fromDate, DateTime toDate);

        //void UpdateNAVHistoryData(DateTime fromDate, DateTime toDate);


        string GetLastProcessedDetails();

        //void SetPath(string path);
    }
}
