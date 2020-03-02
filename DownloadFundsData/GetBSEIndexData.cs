using BusinessEntities.Entities.MutualFunds;
using BusinessEntities.Entities.Stocks;
using DataAccess.Logging;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using System.Linq;
using Utilities;

namespace DownloadFundsData
{
    public class GetBSEIndexData : BaseClass
    {
        IWebDriver webDriver = null;
        readonly string _application = "DownloadFundsData";
        readonly string _component = "GetBSEIndexData";
        DumpData _dumpData = null;

        public GetBSEIndexData()
        {
            webDriver = new FirefoxDriver();
            _dumpData = new DumpData();
        }

        ~GetBSEIndexData()
        {
            if (webDriver != null)
            {
                webDriver.Close();
                webDriver.Dispose();
            }
        }

        public void GetBSEIndexsData()
        {
            webDriver.Url = "http://www.bseindia.com/sensexview/IndexHighlight.aspx?expandable=2";
        }

        public void GetHistoricalData()
        {
            try
            {
                webDriver = new FirefoxDriver();
                webDriver.Url = "http://www.bseindia.com/indices/IndexArchiveData.aspx";
                //List<string> benchMarks = GetBenchMarks();
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        public void GetBseBenchMarkData(string bseBaseUrl)
        {
            try
            {
                LogMessage("Downloading BSE bench mark data...");
                Supremes.Nodes.Document doc = GetDocument(bseBaseUrl + "sensexview/IndexHighlight.aspx?expandable=2");
                if (doc == null) return;

                var links = doc.Body.Select("#ctl00_ContentPlaceHolder1_ddl_sensex_view");
                int i = 0;
                string benchMark = string.Empty;
                string value = string.Empty;
                string prevClose = string.Empty;
                string open = string.Empty;
                string high = string.Empty;
                string low = string.Empty;
                string closeValue;
                string date;
                // DumpData _dumpData = new DumpData();
                foreach (var link in links[0].Children)
                {
                    i++;
                    if (i == 1) continue;
                    benchMark = link.Text;
                    value = link.Attr("value");

                    //http://www.bseindia.com/SensexView/SensexViewbackPage.aspx?flag=INDEX&indexcode=81
                    var benckmarkDoc = GetDocument(bseBaseUrl + "SensexView/SensexViewbackPage.aspx?flag=INDEX&indexcode=" + value);
                    if (benckmarkDoc != null)
                    {
                        LogMessage("Processing : " + benchMark);
                        string[] values = benckmarkDoc.Body.Text.ToString().Split('@');
                        if (values.Length < 10)
                        {
                            LogMessage("not enough values: " + benckmarkDoc.Body.Text);
                            continue;
                        }
                        open = values[2];
                        high = values[3];
                        low = values[4];
                        closeValue = values[5];
                        prevClose = values[6];
                        date = values[9].Split('|')[0];
                        DateTime dt = Convert.ToDateTime(date);
                        dt = dt.AddHours(int.Parse(values[9].Split('|')[1].Split(':')[0].Trim()));
                        dt = dt.AddMinutes(int.Parse(values[9].Split('|')[1].Split(':')[1].Trim()));
                        _dumpData.DumpBenchMarkData(new BenchMark()
                        {
                            BenchMarkName = benchMark,
                            Close = Conversions.ToDecimal(closeValue, 0),
                            Date = dt,
                            High = Conversions.ToDecimal(high, 0),
                            Low = Conversions.ToDecimal(low, 0),
                            Open = Conversions.ToDecimal(open, 0)
                        });
                    }
                    else { LogMessage("null document"); }
                }
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        internal void GetBseBenchMarkHistoryData(string bseBaseUrl, DateTime fromdate, DateTime todate)
        {
            try
            {
                LogMessage("Downloading BSE bench mark history data...");
                webDriver.Url = bseBaseUrl + "indices/IndexArchiveData.html";
                //Supremes.Nodes.Document doc = GetDocument(bseBaseUrl + "indices/IndexArchiveData.html");
                //if (doc == null) return;

                var index = webDriver.FindElement(By.Id("ddlIndex"));
                var selectElement = new SelectElement(index);
                var options = index.FindElements(By.TagName("option"));
                LogMessage("options: " + options.Count);

                List<string> opts = new List<string>();
                for (int i = 0; i < options.Count; i++)
                {
                    if (i == 0) continue;
                    IWebElement currentOption = options[i];
                    string val = currentOption.GetAttribute("value").Trim();

                    opts.Add(val);
                }

                if (webDriver != null)
                {
                    webDriver.Close();
                    webDriver.Dispose();
                }

                LogMessage("total options: " + opts.Count);
                for (int i = 0; i < opts.Count; i++)
                {
                    if (i == 0) continue;
                    //IWebElement currentOption = options[i];
                    //string val = currentOption.GetAttribute("value");

                    GetIndexHistoricalData(i, opts.Count, opts[i], fromdate, todate);
                }

                //var data = GetElementsById(webDriver, "ddlIndex");
                //List<string> result = new List<string>();
                //var res = data[0].Text;
                //result = res.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
                //if (result.Count > 0)
                //    result.RemoveAt(0);

                //foreach (string i in result)
                //{
                //    new SelectElement(webDriver.FindElement(By.Id("ddlIndex"))).SelectByText(i);
                //}
                //var toDateBox = webDriver.FindElement(By.Id("txtToDt"));
                ////Fill date as mm/dd/yyyy as 09/25/2013
                //toDateBox.SendKeys("01/06/2019");

            }
            catch (Exception ex)
            {
                LogMessage("Exception while GetBseBenchMarkHistoryData Exception: " + ex.Message);
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        private void GetIndexHistoricalData(int index, int total, string val, DateTime fromDate, DateTime toDate)
        {
            try
            {
                DateTime _todate = fromDate.AddDays(15);
                List<BenchMark> historydata = new List<BenchMark>();
                while (_todate <= toDate && fromDate <= _todate)
                {
                    using (var client = new HttpClient())
                    {
                        // DisplayMessage("index: " + val + " " + (index + 1) + "/" + total + " " + fromDate.ToString("dd/MM/yyyy") + " - " + _todate.ToString("dd/MM/yyyy"));
                        string url = "https://api.bseindia.com/BseIndiaAPI/api/IndexArchDaily/w?fmdt=" + fromDate.ToString("dd/MM/yyyy") + "&index=" + val + "&period=D&todt=" + _todate.ToString("dd/MM/yyyy");
                        client.BaseAddress = new Uri(url);
                        var responseTask = client.GetAsync(url);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var jsonString = result.Content.ReadAsStringAsync().Result;
                            BSEHistoricalData dataRes = JsonConvert.DeserializeObject<BSEHistoricalData>(jsonString);
                            LogMessage("index: " + val + " " + (index + 1) + "/" + total + " " 
                                + fromDate.ToString("MM/dd/yyyy") + " - " + _todate.ToString("MM/dd/yyyy") + " records: " + dataRes.Table.Length);
                            if (dataRes == null || dataRes.Table == null) continue;
                            foreach (var data in dataRes.Table)
                            {
                                if (data == null) continue;

                                historydata.Add(new BenchMark()
                                {
                                    BenchMarkName = val,
                                    Date = data.tdate,
                                    Open = Conversions.ToDecimal(data.I_open, 0),
                                    High = Conversions.ToDecimal(data.I_high, 0),
                                    Low = Conversions.ToDecimal(data.I_low, 0),
                                    Close = Conversions.ToDecimal(data.I_close, 0),
                                    SharesTraded = Conversions.Getulong(data.TOTAL_SHARES_TRADED, 0),
                                    TurnOver = Conversions.ToDecimal(data.Turnover, 0)
                                });
                            }
                        }
                        else
                        {
                            LogMessage("index: " + val + " " + (index + 1) + "/" + total + " " 
                                + fromDate.ToString("MM/dd/yyyy") + " - " + _todate.ToString("MM/dd/yyyy") + " Failed to get response, return status code" + result.StatusCode);
                        }
                    }
                    fromDate = _todate.AddDays(1);
                    _todate = fromDate.AddDays(15) > toDate ? toDate : fromDate.AddDays(15);
                }
                if (historydata != null && historydata.Count > 0)
                {
                    string xmlStr = GetXMLString(historydata);
                    _dumpData.DumpBenchMarkData(val, xmlStr);
                }
            }
            catch (Exception ex)
            {
                LogMessage("Exception while GetIndexHistoricalData -  " + val + " Exception: " + ex.Message);
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
        }

        private string GetXMLString(List<BenchMark> data)
        {
            string returnStr = string.Empty;
            try
            {
                returnStr = new XElement("root",
                    (from n in data
                     select
                     new XElement("data",
                           new XElement("open", n.Open),
                           new XElement("high", n.High),
                           new XElement("low", n.Low),
                           new XElement("close", n.Close),
                           new XElement("date", n.Date.ToString("MM/dd/yyyy"))
                           )
                    )
                  ).ToString();
            }
            catch (Exception ex)
            {
                LoggingDataAccess.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return returnStr;
        }
    }
}
