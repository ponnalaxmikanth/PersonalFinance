using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities
{
    public partial class MF_Folios
    {
        public MF_Folios()
        {
            //this.MF_Transactions = new HashSet<MF_Transactions>();
        }

        public int FolioId { get; set; }
        public int FundHouseId { get; set; }
        public string FolioNumber { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PortfolioId { get; set; }
        public bool isDefaultFolio { get; set; }

        //public virtual ICollection<MF_Transactions> MF_Transactions { get; set; }
    }

    public partial class MF_FundCategory
    {
        public MF_FundCategory()
        {
            //this.MF_Funds = new HashSet<MF_Funds>();
        }

        public int FundClassId { get; set; }
        public string FundClass { get; set; }
        public Nullable<bool> IsSectorCategory { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual ICollection<MF_Funds> MF_Funds { get; set; }
    }

    public partial class MF_FundHouses
    {
        public MF_FundHouses()
        {
            //this.MF_Funds = new HashSet<MF_Funds>();
        }

        public int FundHouseId { get; set; }
        public string FundHouseName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual ICollection<MF_Funds> MF_Funds { get; set; }
    }

    public partial class MF_FundOptions
    {
        public MF_FundOptions()
        {
            //this.MF_Funds = new HashSet<MF_Funds>();
        }

        public int OptionId { get; set; }
        public string FundOption { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual ICollection<MF_Funds> MF_Funds { get; set; }
    }

    public partial class MF_Funds
    {
        public MF_Funds()
        {
            //this.MF_Transactions = new HashSet<MF_Transactions>();
        }

        public int PortfolioId { get; set; }

        public int FundId { get; set; }

        public MF_FundHouses FundHouse { get; set; }
        public MF_FundTypes FundType { get; set; }
        public MF_FundCategory FundCategory { get; set; }
        public MF_FundOptions FundOptions { get; set; }

        //public int FundHouseId { get; set; }
        //public int FundTypeId { get; set; }
        //public int FundClassId { get; set; }
        //public int FundOptionId { get; set; }
        public string FundName { get; set; }
        public int SchemaCode { get; set; }
        public string GrowthISIN { get; set; }
        public string DividendReInvestISIN { get; set; }
        public int GrowthSchemaCode { get; set; }
        public string GrowthFundName { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual MF_FundCategory MF_FundCategory { get; set; }
        //public virtual MF_FundHouses MF_FundHouses { get; set; }
        //public virtual MF_FundOptions MF_FundOptions { get; set; }
        //public virtual MF_FundTypes MF_FundTypes { get; set; }
        //public virtual ICollection<MF_Transactions> MF_Transactions { get; set; }
    }

    public partial class MF_FundTypes
    {
        public MF_FundTypes()
        {
            //this.MF_Funds = new HashSet<MF_Funds>();
        }

        public int FundTypeId { get; set; }
        public string FundType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual ICollection<MF_Funds> MF_Funds { get; set; }
    }

    public partial class MF_Portfolio
    {
        public MF_Portfolio()
        {
            //this.MF_Transactions = new HashSet<MF_Transactions>();
        }

        public int PortfolioId { get; set; }
        public string Portfolio { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual ICollection<MF_Transactions> MF_Transactions { get; set; }
    }

    //public partial class MF_Transactions
    //{
    //    public int TransactionId { get; set; }
    //    public int PortfolioId { get; set; }
    //    public int FolioId { get; set; }
    //    public int FundId { get; set; }
    //    public DateTime PurchaseDate { get; set; }
    //    public decimal Amount { get; set; }
    //    public decimal PurchaseNAV { get; set; }
    //    public decimal Units { get; set; }
    //    public decimal Dividend { get; set; }
    //    public Nullable<decimal> ActualNAV { get; set; }
    //    public Nullable<DateTime> SellDate { get; set; }
    //    public Nullable<decimal> SellNAV { get; set; }
    //    public Nullable<decimal> SellSTT { get; set; }
    //    public DateTime CreateDate { get; set; }

    //    //public virtual MF_Folios MF_Folios { get; set; }
    //    //public virtual MF_Funds MF_Funds { get; set; }
    //    //public virtual MF_Portfolios MF_Portfolios { get; set; }
    //}

    public partial class GetMFTransactions
    {
        public string PortfolioId { get; set; }
        public int FundId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class DashboardIndividual : GetMFTransactions
    {
        public string Type { get; set; }
    }

    public partial class MF_Transactions
    {
        public int TransactionId { get; set; }

        public string Portfolio { get; set; }
        public int PortfolioId { get; set; }
        
        public int FolioId { get; set; }
        public string FolioNumber { get; set; }
        
        public string FundHouseName { get; set; }
        
        public int FundId { get; set; }
        public string FundName { get; set; }
        
        public string FundOption { get; set; }
        public string FundClass { get; set; }
        public bool IsSector { get; set; }
        public string FundType { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public decimal PurchaseNAV { get; set; }
        public decimal PurchaseUnits { get; set; }
        public decimal Dividend { get; set; }
        public decimal ActualNAV { get; set; }

        public Nullable<decimal> SellUnits { get; set; }
        public Nullable<DateTime> SellDate { get; set; }
        public Nullable<decimal> SellNAV { get; set; }
        public Nullable<decimal> STT { get; set; }

        public decimal LatestNAV { get; set; }
        public DateTime LatestNAVDate { get; set; }

    }


    public partial class MFFundDetails
    {
        public List<MF_Folios> LstFolios { get; set; }
        public List<MF_FundCategory> LstFundCategory { get; set; }
        public List<MF_FundHouses> LstFundHouses { get; set; }
        public List<MF_FundOptions> LstFundOptions { get; set; }
        public List<MF_Funds> LstFunds { get; set; }
        public List<MF_FundTypes> LstFundTypes { get; set; }
        public List<MF_Portfolio> LstPortfolios { get; set; }
    }

    public partial class AddDividendRequest
    {
        public int FundId { get; set; }
        public DateTime DividendDate { get; set; }
        public decimal NAV { get; set; }
        public decimal Dividend { get; set; }
    }

    public partial class AddMFTransactionRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TransactionId { get; set; }

        public string TransactionType { get; set; }

        public int PortfolioId { get; set; }
        public int FolioId { get; set; }
        public int FundId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public decimal PurchaseNAV { get; set; }
        //public decimal Units { get; set; }
        public decimal Dividend { get; set; }
        public decimal ActualNAV { get; set; }
        public Nullable<DateTime> SellDate { get; set; }
        public Nullable<decimal> SellUnits { get; set; }
        public Nullable<decimal> SellNAV { get; set; }
        public Nullable<decimal> STT { get; set; }

        public bool IsSIP { get; set; }

        public int FundHouseId { get; set; }
        public int FundTypeId { get; set; }
        public int FundCategoryId { get; set; }
        public int FundOptionsId { get; set; }
        public int SchemaCode { get; set; }
        public int GrowthSchemaCode { get; set; }
        public string FundName { get; set; }
        public decimal Units { get; set; }
    }

    public partial class AddMFTransactionResponse
    {
        public int ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }

    public partial class NAVData
    {
        public string FundHouse { get; set; }
        public int SchemaCode { get; set; }
        public string ISINGrowth { get; set; }
        public string ISINDivReinvestment { get; set; }
        public string SchemaName { get; set; }
        public decimal NAV { get; set; }
        public decimal RepurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public DateTime Date { get; set; }
        public int FundType { get; set; }

        public int FundOption { get; set; }
        public int Fund_Type { get; set; }
    }

    public partial class MFPortfolioData
    {
        public int PortfolioId { get; set; }
        public int FundId { get; set; }
        public string FundName { get; set; }
        public decimal Amount { get; set; }
        public decimal? Dividend { get; set; }
        public decimal Units { get; set; }
        public decimal? SellUnits { get; set; }
        public decimal PurchaseNAV { get; set; }
        public decimal? SellAmount { get; set; }
        public decimal CurrentNAV { get; set; }
        public DateTime LatestNAVDate { get; set; }
    }

    public class FundPerformance
    {
        public DateTime LatestDate { get; set; }
        public decimal LatestNAV { get; set; }

        public string SchemaName { get; set; }
        public string SchemaCode { get; set; }
        public List<FundHistory> Performance { get; set; }
    }

    public class FundHistory
    {
        public int Level { get; set; }
        public DateTime HistoryDate { get; set; }
        public decimal HistoryNAV { get; set; }
    }

    public class DownloadUrls
    {
        public string Url { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class GetMyFundsRequest
    {
        public string Type { get; set; }
        public int PortfolioId { get; set; }
    }

    public partial class GetFundNavRequest
    {
        public int SchemaCode { get; set; }
        public DateTime Date { get; set; }
    }

    public partial class GetFundValueRequst
    {
        public int PortfolioId { get; set; }
        public int SchemaCode { get; set; }
        public int FolioId { get; set; }
        public int OptionId { get; set; }
    }

    public partial class FundValueResponse
    {
        public int SchemaCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Units { get; set; }
        public decimal Dividend { get; set; }
        public decimal AvgNav { get; set; }
        public decimal LatestNav { get; set; }
        public DateTime Date { get; set; }
        public decimal LatestValue { get; set; }
    }

    public class GetMFFundInvestmentsRequest
    {
        public int PortfolioId { get; set; }
        public int FolioId { get; set; }
        public int FundId { get; set; }
    }

    public class MFTransactions
    {
        public int TransactionId { get; set; }

        public int FolioId { get; set; }
        public string FolioNumber { get; set; }

        public MF_FundHouses FundHouses { get; set; }

        public MF_Funds FundDetails { get; set; }

        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Units { get; set; }
        public decimal DividendPerNAV { get; set; }
        public decimal Dividend { get; set; }

        public decimal CurrentValue { get; set; }
        public DateTime SellDate { get; set; }

        public string Type { get; set; }
    }

    public class DailyMFTracker
    {
        public int PortfolioId { get; set; }
        public DateTime Date { get; set; }
        public decimal Investment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Profit { get; set; }
    }

    public class GetMFDailyTracker
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }

    public class Investments
    {
        //public MF_Portfolio Portfolio { get; set; }
        public string FundName { get; set; }
        public DateTime Date { get; set; }
        public decimal Investment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Profit { get; set; }
        public decimal RedeemInvest { get; set; }
        public decimal Value { get; set; }

        public decimal CurrentProfit { get; set; }
        public decimal Dividend { get; set; }

        public decimal ProfitPer { get; set; }
        public decimal RedeemValue { get; set; }

        public decimal AgePer { get; set; }
        public string Type { get; set; }
    }

    public class FundsPerformance
    {
        public DateTime Date { get; set; }
        public int Period { get; set; }
        public double Investment { get; set; }
        public double CurrentValue { get; set; }
        public double Profit { get; set; }
    }

    public class FundsDetails
    {
        public int FundId { get; set; }
        public int SchemaCode { get; set; }
        public string FundName { get; set; }
        public string Category { get; set; }
        public string FundType { get; set; }
        public string FundOption { get; set; }
        public int averageDays { get; set; }
        public double Units { get; set; }
        public double Dividend { get; set; }
        public double PurchaseNAV { get; set; }
        public double ActualNAV { get; set; }
        public double NAV { get; set; }
        public double Amount { get; set; }
        public double CurrentValue { get; set; }
    }

}
