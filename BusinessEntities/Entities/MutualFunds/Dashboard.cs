using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities.MutualFunds
{
    public class DashboardRequest
    {
        public int PortfolioId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    //public class DashboardIndividual : DashboardRequest
    //{
    //    public string Type { get; set; }
    //}

    public class DashboardResponse
    {
        public Investments Investments { get; set; }
        public List<UpcomingSipDetails> SipDetails { get; set; }
        public List<InvestmentsByMonth> InvestmentsByMonth { get; set; }

        public Investment Overall { get; set; }
        public Investment YTD { get; set; }
        public Investment QTD { get; set; }
        public Investment MTD { get; set; }
        public List<investGrowth> InvestGrowth { get; set; }
    }

    public class investGrowth
    {
        
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal CumulativeAmount { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal CumulativeCurrentValue { get; set; }
        public decimal Profit { get; set; }
        public decimal CumulativeProfit { get; set; }
        public decimal Dividend { get; set; }
        public decimal CumulativeDividend { get; set; }

    }

    public class Investment
    {
        public decimal Amount { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPer { get; set; }

        public decimal Redeem { get; set; }
        public decimal RedeemValue { get; set; }
        public decimal RedeemProfit { get; set; }
        public decimal RedeemProfitPer { get; set; }
    }

    //public class Investments
    //{
    //    //public MF_Portfolio Portfolio { get; set; }
    //    public string FundName { get; set; }
    //    public DateTime Date { get; set; }
    //    public decimal Investment { get; set; }
    //    public decimal CurrentValue { get; set; }
    //    public decimal Profit { get; set; }
    //    public decimal RedeemInvest { get; set; }
    //    public decimal Value { get; set; }

        
    //    public decimal CurrentProfit { get; set; }
    //    public decimal Dividend { get; set; }
        
    //    public decimal ProfitPer { get; set; }
    //    public decimal RedeemValue { get; set; }

    //    public decimal AgePer { get; set; }
    //    public string Type { get; set; }
    //}

    public class UpcomingSipDetails
    {
        public DateTime NextSipDate { get; set; }
        public int SipDate { get; set; }
        public int PortfolioId { get; set; }
        public string PortfolioName { get; set; }

        public decimal Amount { get; set; }

        public DateTime SipStartDate { get; set; }
        public DateTime SipEndDate { get; set; }

        public FundDetails FundDetails { get; set; }
        public SipInvestmnetDetails Investment { get; set; }

    }

    public class FundDetails 
    {
        public int FundId { get; set; }
        public string FundName { get; set; }
        public DateTime DateTime { get; set; }
        public decimal NAV { get; set; }
    }

    public class SipInvestmnetDetails
    {
        public decimal SIPAmount { get; set; }
        public decimal Investment { get; set; }
        public decimal Dividend { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPer { get; set; }
        public decimal Units { get; set; }
        public decimal AvgNAV { get; set; }
    }

    public class InvestmentsByMonth 
    {
        public DateTime Date { get; set; }
        public Investments Investments { get; set; }
    }

    public class Individual
    {
        public Investments Investments { get; set; }
    }

    public class IndividualInvestments
    {
        public List<Investments> Investments { get; set; }
    }

    public class SectorInvestments
    {
        public string Sector { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Profit { get; set; }
        //public decimal InvestPer { get; set; }
        //public decimal CurrentPer { get; set; }
    }

    public class ULIPValue
    {
        public decimal Invest { get; set; }
        public decimal NAV { get; set; }
        public decimal Units { get; set; }
        public decimal CurrentValue { get; set; }
    }

}
