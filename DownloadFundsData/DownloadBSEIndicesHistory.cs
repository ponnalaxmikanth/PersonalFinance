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
    class DownloadBSEIndicesHistory : BaseClass
    {
        IWebDriver webDriver = null;
        string baseUrl = "https://www.bseindia.com/";
        readonly string _application = "DownloadFundsData";
        readonly string _component = "DownloadBSEIndicesHistory";

        public DownloadBSEIndicesHistory()
        {
            webDriver = new FirefoxDriver();
        }

        public void DownloadBSEHistory()
        {
            List<string> benchMarks = GetBenchMarks();

        }

        private List<string> GetBenchMarks()
        {
            List<string> result = new List<string>();
            try
            {
                webDriver.Url = baseUrl + "indices/IndexArchiveData.html";
                var data = GetElementsById(webDriver, "ddlIndex");
                //var data1 = GetElementsByXPath(webDriver, "//select[@id='ddlIndex']");
                
                if (data != null && data.Count > 0)
                {
                    var elements = data[0].Text.Split(new[] { Environment.NewLine },StringSplitOptions.None);
                    //data.selectByIndex(1);
                }

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
    }
}
