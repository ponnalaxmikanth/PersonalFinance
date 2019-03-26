using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.Entities.MutualFunds
{
    public class FundHouse
    {
        public string FamilyName { get; set; }
        public string Uri { get; set; }
        public List<FundLinks> FundLinks { get; set; }

        public FundHouse()
        {
            FundLinks = new List<FundLinks>();
        }
    }

    public class FundLinks
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public FundsDetails FundDetails { get; set; }
    }

    public class FundsDetails
    {
        public int SchemaCode { get; set; }
        public string Category { get; set; }
        public decimal ExpenseRatio { get; set; }
        public string Status { get; set; }
        public decimal TurnOver { get; set; }
        public decimal MinimumInvest { get; set; }
        public string Benchmark { get; set; }
        public string MSCategory { get; set; }
        public decimal TotalAsset { get; set; }
    }

    public class BenchMark : BenchmarkDetails
    {
        public string BenchMarkName { get; set; }
    }

    public class BenchmarkDetails
    {
        public DateTime Date { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public UInt64? SharesTraded { get; set; }
        public decimal? TurnOver { get; set; }
    }

    public class BenchmarkHistory
    {
        public string BenchMark { get; set; }
        public List<BenchmarkDetails> HistoryDetails { get; set; }
    }


}
