using System;
using System.Collections.Generic;

namespace BusinessEntities.Entities.Stocks
{
    public class BSEHistoricalData
    {
        public BSERecord[] Table { get; set; }
    }

    public class BSERecord
    {
        public int Day { get; set; }
        public string Month { get; set; }
        public string year { get; set; }
        public string Turnover { get; set; }
        public DateTime tdate { get; set; }
        public string I_name { get; set; }
        public float I_open { get; set; }
        public float I_high { get; set; }
        public float I_low { get; set; }
        public float I_close { get; set; }
        public float I_pe { get; set; }
        public float I_pb { get; set; }
        public float I_yl { get; set; }
        public string Turnover_1 { get; set; }
        public string TOTAL_SHARES_TRADED { get; set; }
    }

}
