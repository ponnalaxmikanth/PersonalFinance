using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class USMFWallStreetJournal : BaseClass
    {
        IWebDriver webDriver = null;
        DumpData _dumpData = null;
        string baseUrl = string.Empty;

        public USMFWallStreetJournal()
        {
            webDriver = new FirefoxDriver();
            _dumpData = new DumpData();
            baseUrl = "http://www.wsj.com";
        }

        ~USMFWallStreetJournal()
        {
            if (webDriver != null)
            {
                webDriver.Close();
                webDriver.Dispose();
            }
        }

        public void DownloadData()
        {
            try
            {
                string[] alphas = new string [] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

                ///html/body/div[5]/div[3]/div/div[3]/span[2]
                //List<string> links = null;
                StreamWriter swr = null;
                Dictionary<string, string> links = null;

                webDriver.Url = "http://www.wsj.com/mdc/public/page/2_3048-usmfunds_A-usmfunds.html";
                links = GetLinks(webDriver);

                string lastReadFund = string.Empty;
                DateTime date = GetDate();
                if (File.Exists(@"C:\Kanth\Projects\dotNet\USMF\" + date.ToString("MM.dd.yyyy") + "_WSJ FundsNAV.csv"))
                {
                    lastReadFund = GetLastReadFund(@"C:\Kanth\Projects\dotNet\USMF\" + date.ToString("MM.dd.yyyy") + "_WSJ FundsNAV.csv");
                    swr = new StreamWriter(@"C:\Kanth\Projects\dotNet\USMF\" + date.ToString("MM.dd.yyyy") + "_WSJ FundsNAV.csv", true);
                }
                else
                {
                    swr = new StreamWriter(@"C:\Kanth\Projects\dotNet\USMF\" + date.ToString("MM.dd.yyyy") + "_WSJ FundsNAV.csv", true);
                    swr.AutoFlush = true;
                    swr.WriteLine("link,Family,Fund,Symbol,Date,NAV");
                }
                for (int i = 0; i < links.Count; i++)
                //foreach(var link in links)
                {
                    date = GetDate();   

                    if (!string.IsNullOrWhiteSpace(lastReadFund))
                    {
                        webDriver.Url = links[lastReadFund.Split(',')[0]];
                        
                        for (int x = 0; i < links.Count; x++)
                        {
                            if (links.ElementAt(x).Value == links[lastReadFund.Split(',')[0]])
                            {
                                i = x;
                                break;
                            }
                        }
                        lastReadFund = string.Empty;
                    }
                    else
                    {
                        webDriver.Url = links.ElementAt(i).Value;
                    }
                    //var trs = webDriver.FindElements(By.XPath("/html/body/div[5]/div[3]/div/table/tbody/tr"));
                    var trs = GetElementsByXPath(webDriver, "/html/body/div[5]/div[3]/div/table/tbody/tr");
                    string family = string.Empty;
                    string fund = string.Empty;
                    string symbol = string.Empty;
                    string NAV = string.Empty;
                    string change = string.Empty;

                    
                    for (int t = 2; t < trs.Count - 1; t++)
                    {
                        try
                        {
                            var x = GetElementsByXPath(webDriver, "/html/body/div[5]/div[3]/div/table/tbody/tr[" + t + "]/td");
                            if (x.Count <= 0)
                                continue;
                            else if (x.Count == 1)
                            {
                                family = x[0].Text.Trim();
                            }
                            else
                            {
                                fund = x[0].Text.Trim();
                                symbol = x[1].Text.Trim();
                                NAV = x[2].Text.Trim();
                                DisplayMessage(family + " -- " + fund  + " -- " + symbol  + " -- " + NAV);
                                swr.WriteLine(links.ElementAt(i).Key + "," + family + "," + fund + "," + symbol + "," + date.ToString("MM/dd/yyyy") + "," + NAV);
                            }
                        }
                        catch (Exception ex) { 
                            DisplayMessage("USMFWallStreetJournal -- DownloadData: " + t + " -- " + ex.Message);
                            DisplayMessage("USMFWallStreetJournal: " + ex.StackTrace); 
                        }
                    }
                }
                swr.Close();
            }
            catch (Exception ex) { 
                DisplayMessage("USMFWallStreetJournal -- DownloadData " + ex.Message);
                DisplayMessage("USMFWallStreetJournal -- " + ex.StackTrace);
            }
        }

        private string GetLastReadFund(string filePath)
        {
            string lastReadFund = "";
            string[] lines = File.ReadAllLines(filePath);

            lastReadFund = lines[lines.Length - 1];
            return lastReadFund;
        }

        private Dictionary<string, string> GetLinks(IWebDriver webDriver)
        {
            List<string> result = new List<string>();
            Dictionary<string, string> linksDict = new Dictionary<string, string>();
            linksDict.Add("A", "http://www.wsj.com/mdc/public/page/2_3048-usmfunds_A-usmfunds.html");
            try
            {
                //var links = webDriver.FindElements(By.XPath("/html/body/div[5]/div[3]/div/div[3]/span[2]/a"));
                var links = GetElementsByXPath(webDriver, "/html/body/div[5]/div[3]/div/div[3]/span[2]/a");

                for (int i = 0; i < links.Count; i++)
                {
                    result.Add(links[i].GetAttribute("href"));
                    linksDict.Add(links[i].Text, links[i].GetAttribute("href"));
                    
                }
            }
            catch (Exception ex) { }
            return linksDict;
        }

        private DateTime GetDate()
        {
            DateTime dt = new DateTime().Date;
            try
            {
                //var dateEle = webDriver.FindElements(By.XPath("/html/body/div[5]/div[3]/div/div[4]/span[2]"));
                var dateEle = GetElementsByXPath(webDriver, "/html/body/div[5]/div[3]/div/div[4]/span[2]");
                dt = Convert.ToDateTime(dateEle[0].Text);
            }
            catch (Exception ex) { DisplayMessage("USMFWallStreetJournal -- GetDate" + ex.Message); }
            return dt;
        }
    }
}
