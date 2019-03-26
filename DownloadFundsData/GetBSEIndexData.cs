using BusinessEntities.Entities.MutualFunds;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class GetBSEIndexData : BaseClass
    {
        IWebDriver webDriver = null;

        public GetBSEIndexData()
        {
            //webDriver = new FirefoxDriver();
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
                List<string> benchMarks = GetBenchMarks();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void GetBseBenchMarkData(string bseBaseUrl)
        {
            try
            {
                DisplayMessage("Downloading BSE bench mark data...");
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
                DumpData _dumpData = new DumpData();
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
                        DisplayMessage("Processing : " + benchMark);
                        string[] values = benckmarkDoc.Body.Text.ToString().Split('@');
                        if (values.Length < 10)
                        {
                            DisplayMessage("not enough values: " + benckmarkDoc.Body.Text);
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
                            Close = GetDecimalValue(closeValue),
                            Date = dt,
                            High = GetDecimalValue(high),
                            Low = GetDecimalValue(low),
                            Open = GetDecimalValue(open)
                        });
                    }
                    else { DisplayMessage("null document"); }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private List<string> GetBenchMarks()
        {
            List<string> result = new List<string>();
            try
            {
            }
            catch (Exception ex)
            {
 
            }
            return result;
        }

    }
}
