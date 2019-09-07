using BusinessEntities.Entities.MutualFunds;
using DataAccess.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class GetNSEBenchmarkData : BaseClass
    {
        IWebDriver webDriver = null;
        DumpData _dumpData = null;
        readonly string _application = "DownloadFundsData";
        readonly string _component = "GetNSEBenchmarkData";

        public GetNSEBenchmarkData()
        {
            webDriver = new FirefoxDriver();
            _dumpData = new DumpData();
        }

        ~GetNSEBenchmarkData()
        {
            if (webDriver != null)
            {
                webDriver.Close();
                webDriver.Dispose();
            }
        }

        public void DownloadNSEBenchMarkHistory(string fromDate, string toDate)
        {
            try
            {
                List<string> benchMarks = GetBenchMarks();
                //DateTime startDate = new DateTime(2008, 1, 1);
                //DateTime date = startDate;
                //foreach (string benchmark in benchMarks)
                //{
                //    date = startDate;
                //    while (date.Year <= DateTime.Now.Year || DateTime.Now.Month <= date.Month)
                //    {
                //        GetHistoryData(date, date.AddMonths(1).AddDays(-1), benchmark);
                //        date = date.AddMonths(1);
                //    }
                //}

                DateTime date = DateTime.Parse(toDate);
                DateTime startDate = DateTime.Parse(fromDate);
                while (date > startDate)
                {
                    
                    //foreach (string benchmark in benchMarks)
                    for (int i = 0; i < benchMarks.Count; i++)
                    {
                        //if (date < startDate)
                        //    break;
                        if (date == DateTime.Today)
                            GetHistoryData(date.AddDays(-15), date, benchMarks[i], i, benchMarks.Count - 1);
                        else
                            GetHistoryData(new DateTime(date.Year, date.Month, 1), date, benchMarks[i], i, benchMarks.Count - 1);
                    }
                    date = date.AddDays(-date.Day);
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public void DownloadNSEBenchMarkData()
        {
            try
            {
                List<string> benchMarks = GetBenchMarks();
                //DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //DateTime date = startDate;
                //foreach (string benchmark in benchMarks)
                for (int i = 0; i < benchMarks.Count; i++)
                {
                    //date = startDate;
                    //GetHistoryData(date, date.AddMonths(1).AddDays(-1), benchmark);
                    GetHistoryData(DateTime.Now.Date.AddDays(-15), DateTime.Now.Date, benchMarks[i], i, benchMarks.Count);
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        private List<string> GetBenchMarks()
        {
            List<string> result = new List<string>();
            try
            {
                webDriver.Url = "https://nseindia.com/products/content/equities/indices/historical_index_data.htm";
                //var data = webDriver.FindElements(By.XPath("html/body/div[2]/div[3]/div[2]/div[1]/div[4]/div/div[1]/div/div[2]/select"));
                var data = GetElementsByXPath(webDriver, "html/body/div[2]/div[3]/div[2]/div[1]/div[4]/div/div[1]/div/div[2]/select");

                var res = data[0].Text;
                result = res.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
                if (result.Count > 0)
                    result.RemoveAt(0);
            }
            catch (Exception ex) {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        public List<BenchMark> GetHistoryData(DateTime fromDate, DateTime toDate, string indexType, int index, int count)
        {
            DisplayMessage("From: " + fromDate.ToString("MM/dd/yyyy") + " To: " + toDate.ToString("MM/dd/yyyy") + " (" + index + "/" + count + ") Benchmark: " + indexType);
            List<BenchMark> result = null;
            try
            {
                //webDriver.Url = "https://nseindia.com/products/dynaContent/equities/indices/historicalindices.jsp?indexType=NIFTY%2050&fromDate=01-01-2017&toDate=31-01-2017";
                webDriver.Url = "https://nseindia.com/products/dynaContent/equities/indices/historicalindices.jsp?indexType=" + indexType 
                                        + "&fromDate=" + fromDate.ToString("dd-MM-yyyy") + "&toDate=" + toDate.ToString("dd-MM-yyyy");

                //var data = webDriver.FindElements(By.XPath("html/body/table/tbody/tr"));
                var data = GetElementsByXPath(webDriver, "html/body/table/tbody/tr");
                for (int i = 3; i < data.Count - 1; i++)
                {
                    var eles = data[i].FindElements(By.TagName("td"));
                    _dumpData.DumpBenchMarkData(new BenchMark()
                    {
                        BenchMarkName = indexType,
                        Date = Convert.ToDateTime(eles[0].Text),
                        Open = GetDecimalValue(eles[1].Text),
                        High = GetDecimalValue(eles[2].Text),
                        Low = GetDecimalValue(eles[3].Text),
                        Close = GetDecimalValue(eles[4].Text),
                        SharesTraded = GetUInt64(eles[5].Text),
                        TurnOver = GetDecimalValue(eles[6].Text)
                    });
                }
            }
            catch(Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        private ulong? GetUInt64(string val)
        {
            ulong retValue = 0;
            try
            {
                if (!ulong.TryParse(val, out retValue))
                    return null;
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return retValue;
        }

        //private decimal? GetDecimalValue(string val)
        //{
        //    decimal retValue = 0;
        //    try
        //    {
        //        if(!decimal.TryParse(val, out retValue))
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
        //    }
        //    return retValue;
        //}

    }
}
