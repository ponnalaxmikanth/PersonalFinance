using BusinessAccess.MutualFunds;
using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities.MutualFunds;
using DataAccess.MutualFunds;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Supremes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class GetMorningStarMFData : BaseClass
    {
        IWebDriver webDriver = null;
        List<FundHouse> FundHouses;
        public string MorningStarBaseurl;

        ICommonDataAccess _CommonDataAccess = null;
        ICommonRepository _CommonRepository = null;
        MutualFundsDataAccess _mfDataAccess = null;
        MutualFundsRepository _mutualBusinessAccess = null;

        StreamWriter SrwFundLinks = null;
        StreamWriter SrwMFFundDetails = null;

        public GetMorningStarMFData()
        {
            Console.Title = "Downloading Morning Star : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            webDriver = new FirefoxDriver();
            FundHouses = new List<FundHouse>();
            MorningStarBaseurl = "http://morningstar.in/";

            _mfDataAccess = new MutualFundsDataAccess();
            _CommonDataAccess = new CommonDataAccess();
            _CommonRepository = new CommonRepository(_CommonDataAccess);
            _mutualBusinessAccess = new MutualFundsRepository(_CommonDataAccess, _mfDataAccess);
            SrwFundLinks = new StreamWriter(@"..\..\" + DateTime.Now.ToString("yyyy.MM.dd_HHmmss_") + "MFFundLinks.txt", false);
            SrwFundLinks.AutoFlush = true;
            SrwMFFundDetails = new StreamWriter(@"..\..\" + DateTime.Now.ToString("yyyy.MM.dd_HHmmss_") + "MFFundDetails.txt", false);
            SrwMFFundDetails.AutoFlush = true;
        }

        ~GetMorningStarMFData()
        {
            if (webDriver != null)
                webDriver.Close();
        }

        public void GetMFDataFromMorningStar()
        {
            webDriver.Url = "http://morningstar.in/default.aspx";
            GetMSFundHouseLinks(webDriver, FundHouses);
            GetMSFundsLinks(FundHouses);


            GetMSFundsData(FundHouses);
            //DumpMSDataToFile(FundHouses);
            //RefreshMFFundsData(FundHouses);
            //webDriver.Close();
        }

        private void GetMSFundHouseLinks(IWebDriver webDriver, List<FundHouse> fundHouse)
        {
            try
            {
                //html/body/form/div[4]/div[3]/div[2]/div/div[2] gives mutual funds div
                //IWebElement fundHouses = webDriver.FindElement(By.XPath("html/body/form/div[4]/div[3]/div[2]/div/div[2]/div"));
                IWebElement fhWrapper = webDriver.FindElement(By.ClassName("mfshortnames_links_wrapper"));
                if (fhWrapper != null)
                {
                    var fundHouses = fhWrapper.FindElements(By.TagName("a"));
                    foreach (var eachHouse in fundHouses)
                    {
                        var fhLink = eachHouse.GetAttribute("href");
                        var fhouse = eachHouse.Text;

                        if (fhLink != null)
                        {
                            DisplayMessage("Adding: " + fhouse);
                            fundHouse.Add(new FundHouse() { FamilyName = fhouse, FundLinks = new List<FundLinks>(), Uri = fhLink });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetMSFundsLinks(List<FundHouse> fundHouses)
        {
            IWebElement nextButton = null;
            IWebElement alink = null;
            string ahref = string.Empty;
            IWebElement category = null;
            ReadOnlyCollection<IWebElement> fundslist = null;
            IWebElement fundsCount = null;
            int totalCount = 0;
            try
            {
                foreach (FundHouse eachHouse in fundHouses)
                {
                    try
                    {
                        webDriver.Url = eachHouse.Uri;
                        DisplayMessage("Processing: " + eachHouse.FamilyName);
                        fundsCount = GetElementById(webDriver, "ctl00_ContentPlaceHolder1_lblTotalCount");
                        nextButton = GetElementsByXPath(webDriver, "html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[3]");
                        totalCount = int.Parse(fundsCount.Text);
                        int pageCount = 1;
                        int count = 1;
                        do
                        {
                            DisplayMessage("Downloading page: " + pageCount++);
                            try
                            {
                                fundslist = GetFundsList(webDriver);
                                List<FundLinks> links = new List<FundLinks>();
                                foreach (var fund in fundslist)
                                {
                                    try
                                    {
                                        alink = GetElementsByTag(fund, "a"); //fund.FindElement(By.TagName("a"));
                                        if (alink == null) continue;
                                        ahref = alink.GetAttribute("href");
                                        category = GetElementsByXPath(fund, "div[2]");// fund.FindElement(By.XPath("div[2]"));
                                        DisplayMessage("Parsing : " + count++ + " " + alink.Text);
                                        if (eachHouse.FundLinks.Where(l => l.Url == ahref).Count() <= 0)
                                        {
                                            links.Add(new FundLinks()
                                            {
                                                Url = ahref,
                                                Title = alink.Text,
                                                FundDetails = new FundsDetails(),
                                                Category = category != null ? category.Text : string.Empty
                                            });

                                            //FundLinks flink = new FundLinks()
                                            //{
                                            //    Url = ahref,
                                            //    Title = alink.Text,
                                            //    FundDetails = new FundsDetails(),
                                            //    Category = category != null ? category.Text : string.Empty
                                            //};
                                            SrwMFFundDetails.WriteLine("\"" + eachHouse.FamilyName + "\", \"" + alink.Text + "\", " + "\"" + ahref + "\", " + "\"" + (category != null ? category.Text : string.Empty) + "\"");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Thread.Sleep(1000);
                                        fundslist = GetFundsList(webDriver);
                                        nextButton = GetElementsByXPath(webDriver, "html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[3]");
                                        pageCount--;
                                        count = (count / 50) * 50;
                                        break;
                                    }
                                }
                                GetMSFundsData(eachHouse.FamilyName, links);
                                if (nextButton.GetAttribute("disabled") != null)
                                    break;
                                nextButton = GetNextPageData(webDriver);
                            }
                            catch (Exception ex)
                            {
                                Thread.Sleep(1000);
                                fundslist = GetFundsList(webDriver);
                                nextButton = GetElementsByXPath(webDriver, "html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[3]");
                            }
                            //} while (nextButton != null && nextButton.GetAttribute("disabled") == null);
                        } while (nextButton != null);
                    }
                    catch (Exception ex) { Thread.Sleep(1000); }
                    //GetMSFundsData(eachHouse);
                }
            }
            catch (Exception ex) { }
        }

        private ReadOnlyCollection<IWebElement> GetFundsList(IWebDriver webDriver)
        {
            ReadOnlyCollection<IWebElement> result = null;
            int count = 10;
            while (webDriver != null && count-- > 0 && result == null)
            {
                try
                {
                    var funds = GetElementById(webDriver, "ctl00_ContentPlaceHolder1_uplFunds");// webDriver.FindElement(By.Id("ctl00_ContentPlaceHolder1_uplFunds"));
                    var fundsDiv = GetElementByClassName(funds, "archive");
                    result = GetElementsByClassName(fundsDiv, "tr");// fundsDiv.FindElements(By.ClassName("tr"));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return result;
        }

        private IWebElement GetElementsByTag(IWebElement fund, string tag)
        {
            IWebElement retEle = null;
            int count = 10;
            while (fund != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = fund.FindElement(By.TagName(tag));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return retEle;
        }

        private IWebElement GetElementsByXPath(IWebElement fund, string xPath)
        {
            IWebElement retEle = null;
            int count = 10;
            while (fund != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = fund.FindElement(By.XPath(xPath));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return retEle;
        }

        private IWebElement GetElementsByXPath(IWebDriver webDriver, string path)
        {
            IWebElement retEle = null;
            int count = 10;
            while (webDriver != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = webDriver.FindElement(By.XPath(path));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return retEle;
        }

        private IWebElement GetElementById(IWebDriver webDriver, string id)
        {
            IWebElement retEle = null;
            int count = 10;
            while (webDriver != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = webDriver.FindElement(By.Id(id));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return retEle;
        }

        private IWebElement GetNextPageData(IWebDriver webDriver)
        {
            IWebElement nextButton = null;
            int count = 10;
            while (count-- > 0 && nextButton == null)
            {
                try
                {
                    IWebElement navigation = GetElementsByXPath(webDriver, "html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[1]");
                    nextButton = GetElementsByXPath(webDriver, "html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[3]");
                    if (nextButton != null && nextButton.GetAttribute("disabled") == null)
                        nextButton.Click();

                    WebDriverWait wait = new WebDriverWait(webDriver, new TimeSpan(0, 0, 15));
                    //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[2]")));
                    //wait.Until(ExpectedConditions.ElementExists(By.ClassName("blockOverlay")));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("blockOverlay")));
                    nextButton = GetElementsByXPath(webDriver, "html/body/form/div[4]/div[2]/div/div/div/div[2]/div[2]/div[2]/div[1]/span/input[3]");
                }
                catch (Exception ex) { Thread.Sleep(30 * 1000); }
            }
            return nextButton;
        }

        private IWebElement GetElementByClassName(IWebDriver webDriver, string className)
        {
            IWebElement retEle = null;
            int count = 10;
            while (webDriver != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = webDriver.FindElement(By.ClassName(className));
                }
                catch (Exception ex) { Thread.Sleep(100); }
            }
            return retEle;
        }

        private IWebElement GetElementByClassName(IWebElement ele, string className)
        {
            IWebElement retEle = null;
            int count = 10;
            while (ele != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = ele.FindElement(By.ClassName(className));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return retEle;
        }

        private ReadOnlyCollection<IWebElement> GetElementsByClassName(IWebElement ele, string className)
        {
            ReadOnlyCollection<IWebElement> retEle = null;
            int count = 10;
            while (ele != null && count-- > 0 && retEle == null)
            {
                try
                {
                    retEle = ele.FindElements(By.ClassName(className));
                }
                catch (Exception ex) { Thread.Sleep(1000); }
            }
            return retEle;
        }

        //private void DisplayMessage(string msg)
        //{
        //    Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "] " + msg);
        //}

        private bool GetMSFundsData(List<FundHouse> fundFamilies)
        {
            try
            {
                //await Task.Delay(1);
                foreach (var house in fundFamilies)
                {
                    ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = 50 };
                    Parallel.ForEach(house.FundLinks, po, fund =>
                    {
                        try
                        {
                            DisplayMessage("Parsing: " + fund.Title);
                            Supremes.Nodes.Document doc = null;
                            Supremes.Nodes.Document docPortfolio = null;

                            Parallel.Invoke(
                                () => doc = GetDocument(fund.Url),
                                () => docPortfolio = GetDocument(fund.Url.Replace("overview.aspx", "portfolio.aspx"))
                                );
                            Supremes.Nodes.Document d1 = docPortfolio == null ? null : GetDocument(docPortfolio.Body.Select(".gr_bodywrap")[0].Children[4].Attr("src"));

                            if (doc != null)
                            {
                                var mflinks = doc.Body.Select("#msqt_title");
                                var gr_table_b1 = doc.Select(".gr_table_b1 > tbody");

                                string schemaCode = string.Empty;
                                string category = string.Empty;
                                string minInvest = string.Empty;
                                string expratio = string.Empty;
                                string status = string.Empty;
                                string turnover = string.Empty;
                                string totAsset = string.Empty;
                                int schCode = 0;
                                Parallel.Invoke(
                                    () => schemaCode = mflinks.Count <= 0 ? string.Empty : mflinks[0].Children[0].Children[1].Children[1].Text,
                                    () => category = gr_table_b1[0].Children.Count <= 4 ? string.Empty :
                                                    gr_table_b1[0].Children[4].Children[1].Children.Count <= 0 ? string.Empty : gr_table_b1[0].Children[4].Children[1].Children[1].Text,
                                    () => minInvest = gr_table_b1.Select(".gr_table_colm21").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm21")[1].Children[1].Text,
                                    () => expratio = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[0].Children[1].Text,
                                    () => status = gr_table_b1.Select(".gr_table_colm27").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm27")[0].Children[1].Text,
                                    () => turnover = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[1].Children[1].Text,
                                    () => totAsset = gr_table_b1.Select(".gr_table_colm16b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm16b")[0].Children[0].Children[1].Children[0].Text
                                );

                                if (int.TryParse(schemaCode.Split('|')[0].Trim(), out schCode))
                                {
                                    fund.FundDetails = new FundsDetails()
                                    {
                                        SchemaCode = int.Parse(schemaCode.Split('|')[0].Trim()),
                                        ExpenseRatio = FormatPercentage(expratio),
                                        Category = category,
                                        MinimumInvest = FormatPercentage(minInvest),
                                        Status = status,
                                        TurnOver = FormatPercentage(turnover),
                                        Benchmark = GetMSBenchmark(d1),
                                        MSCategory = GetMSCategory(d1),
                                        TotalAsset = 0
                                    };

                                    SrwMFFundDetails.WriteLine(house.FamilyName
                                                                    + "," + fund.Title
                                                                    + "," + fund.FundDetails.SchemaCode
                                                                    + "," + fund.FundDetails.ExpenseRatio
                                                                    + "," + fund.FundDetails.Benchmark
                                                                    + "," + fund.FundDetails.MinimumInvest
                                                                    + "," + fund.Category
                                                                    + "," + fund.FundDetails.MSCategory
                                                                    + "," + fund.FundDetails.Status
                                                                    + "," + fund.FundDetails.TotalAsset
                                                                    + "," + fund.FundDetails.TurnOver
                                                                    );
                                }
                            }
                            else
                                DisplayMessage("Failed to load: " + fund.Title);
                        }
                        catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
                    });
                    RefreshMFFundsData(house);
                }
            }
            catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
            return true;
        }

        private async Task<bool> GetMSFundsData(FundHouse house) //private void GetMSFundsData(List<FundHouse> fundFamilies)
        {
            try
            {
                await Task.Delay(1);
                ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = 50 };
                Parallel.ForEach(house.FundLinks, po, fund =>
                {
                    try
                    {
                        DisplayMessage("Parsing: " + fund.Title);
                        Supremes.Nodes.Document doc = null;
                        Supremes.Nodes.Document docPortfolio = null;

                        Parallel.Invoke(
                            () => doc = GetDocument(fund.Url),
                            () => docPortfolio = GetDocument(fund.Url.Replace("overview.aspx", "portfolio.aspx"))
                            );
                        Supremes.Nodes.Document d1 = docPortfolio == null ? null : GetDocument(docPortfolio.Body.Select(".gr_bodywrap")[0].Children[4].Attr("src"));

                        if (doc != null)
                        {
                            var mflinks = doc.Body.Select("#msqt_title");
                            var gr_table_b1 = doc.Select(".gr_table_b1 > tbody");

                            string schemaCode = string.Empty;
                            string category = string.Empty;
                            string minInvest = string.Empty;
                            string expratio = string.Empty;
                            string status = string.Empty;
                            string turnover = string.Empty;
                            string totAsset = string.Empty;
                            int schCode = 0;
                            Parallel.Invoke(
                                () => schemaCode = mflinks.Count <= 0 ? string.Empty : mflinks[0].Children[0].Children[1].Children[1].Text,
                                () => category = gr_table_b1[0].Children.Count <= 4 ? string.Empty :
                                                gr_table_b1[0].Children[4].Children[1].Children.Count <= 0 ? string.Empty : gr_table_b1[0].Children[4].Children[1].Children[1].Text,
                                () => minInvest = gr_table_b1.Select(".gr_table_colm21").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm21")[1].Children[1].Text,
                                () => expratio = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[0].Children[1].Text,
                                () => status = gr_table_b1.Select(".gr_table_colm27").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm27")[0].Children[1].Text,
                                () => turnover = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[1].Children[1].Text,
                                () => totAsset = gr_table_b1.Select(".gr_table_colm16b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm16b")[0].Children[0].Children[1].Children[0].Text
                            );

                            if (int.TryParse(schemaCode.Split('|')[0].Trim(), out schCode))
                            {
                                fund.FundDetails = new FundsDetails()
                                {
                                    SchemaCode = int.Parse(schemaCode.Split('|')[0].Trim()),
                                    ExpenseRatio = FormatPercentage(expratio),
                                    Category = category,
                                    MinimumInvest = FormatPercentage(minInvest),
                                    Status = status,
                                    TurnOver = FormatPercentage(turnover),
                                    Benchmark = GetMSBenchmark(d1),
                                    MSCategory = GetMSCategory(d1),
                                    TotalAsset = 0
                                };
                                SrwMFFundDetails.WriteLine(house.FamilyName
                                                                    + "," + fund.Title
                                                                    + "," + fund.FundDetails.SchemaCode
                                                                    + "," + fund.FundDetails.ExpenseRatio
                                                                    + "," + fund.FundDetails.Benchmark
                                                                    + "," + fund.FundDetails.MinimumInvest
                                                                    + "," + fund.Category
                                                                    + "," + fund.FundDetails.MSCategory
                                                                    + "," + fund.FundDetails.Status
                                                                    + "," + fund.FundDetails.TotalAsset
                                                                    + "," + fund.FundDetails.TurnOver
                                                                    );
                            }
                        }
                        else
                            DisplayMessage("Failed to load: " + fund.Title);
                    }
                    catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
                });
                RefreshMFFundsData(house);
            }
            catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
            return true;
        }

        private bool GetMSFundsData(string familyName, FundLinks fund)
        {
            try
            {
                //await Task.Delay(1);
                try
                {
                    DisplayMessage("Parsing: " + fund.Title);
                    Supremes.Nodes.Document doc = null;
                    Supremes.Nodes.Document docPortfolio = null;

                    Parallel.Invoke(
                        () => doc = GetDocument(fund.Url),
                        () => docPortfolio = GetDocument(fund.Url.Replace("overview.aspx", "portfolio.aspx"))
                        );
                    Supremes.Nodes.Document d1 = docPortfolio == null ? null : GetDocument(docPortfolio.Body.Select(".gr_bodywrap")[0].Children[4].Attr("src"));

                    if (doc != null)
                    {
                        var mflinks = doc.Body.Select("#msqt_title");
                        var gr_table_b1 = doc.Select(".gr_table_b1 > tbody");

                        string schemaCode = string.Empty;
                        string category = string.Empty;
                        string minInvest = string.Empty;
                        string expratio = string.Empty;
                        string status = string.Empty;
                        string turnover = string.Empty;
                        string totAsset = string.Empty;
                        int schCode = 0;
                        Parallel.Invoke(
                            () => schemaCode = mflinks.Count <= 0 ? string.Empty : mflinks[0].Children[0].Children[1].Children[1].Text,
                            () => category = gr_table_b1[0].Children.Count <= 4 ? string.Empty :
                                            gr_table_b1[0].Children[4].Children[1].Children.Count <= 0 ? string.Empty : gr_table_b1[0].Children[4].Children[1].Children[1].Text,
                            () => minInvest = gr_table_b1.Select(".gr_table_colm21").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm21")[1].Children[1].Text,
                            () => expratio = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[0].Children[1].Text,
                            () => status = gr_table_b1.Select(".gr_table_colm27").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm27")[0].Children[1].Text,
                            () => turnover = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[1].Children[1].Text,
                            () => totAsset = gr_table_b1.Select(".gr_table_colm16b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm16b")[0].Children[0].Children[1].Children[0].Text
                        );

                        if (int.TryParse(schemaCode.Split('|')[0].Trim(), out schCode))
                        {
                            fund.FundDetails = new FundsDetails()
                            {
                                SchemaCode = int.Parse(schemaCode.Split('|')[0].Trim()),
                                ExpenseRatio = FormatPercentage(expratio),
                                Category = category,
                                MinimumInvest = FormatPercentage(minInvest),
                                Status = status,
                                TurnOver = FormatPercentage(turnover),
                                Benchmark = GetMSBenchmark(d1),
                                MSCategory = GetMSCategory(d1),
                                TotalAsset = 0
                            };
                            WriteFundDetailsToFile(familyName, fund);
                        }
                    }
                    else
                        DisplayMessage("Failed to load: " + fund.Title);
                }
                catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
                RefreshMFFundsData(familyName, fund);
            }
            catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
            return true;
        }

        private void GetMSFundsData(string familyName, List<FundLinks> links)
        {
            try
            {
                ParallelOptions po = new ParallelOptions() { MaxDegreeOfParallelism = 50 };
                Parallel.ForEach(links, po, fund =>
                {
                    try
                    {
                        DisplayMessage("Parsing: " + fund.Title);
                        Supremes.Nodes.Document doc = null;
                        Supremes.Nodes.Document docPortfolio = null;

                        Parallel.Invoke(
                            () => doc = GetDocument(fund.Url),
                            () => docPortfolio = GetDocument(fund.Url.Replace("overview.aspx", "portfolio.aspx"))
                            );
                        Supremes.Nodes.Document d1 = docPortfolio == null ? null : GetDocument(docPortfolio.Body.Select(".gr_bodywrap")[0].Children[4].Attr("src"));

                        if (doc != null)
                        {
                            var mflinks = doc.Body.Select("#msqt_title");
                            var gr_table_b1 = doc.Select(".gr_table_b1 > tbody");

                            string schemaCode = string.Empty;
                            string category = string.Empty;
                            string minInvest = string.Empty;
                            string expratio = string.Empty;
                            string status = string.Empty;
                            string turnover = string.Empty;
                            string totAsset = string.Empty;
                            int schCode = 0;
                            Parallel.Invoke(
                                () => schemaCode = mflinks.Count <= 0 ? string.Empty : mflinks[0].Children[0].Children[1].Children[1].Text,
                                () => category = gr_table_b1[0].Children.Count <= 4 ? string.Empty :
                                                gr_table_b1[0].Children[4].Children[1].Children.Count <= 0 ? string.Empty : gr_table_b1[0].Children[4].Children[1].Children[1].Text,
                                () => minInvest = gr_table_b1.Select(".gr_table_colm21").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm21")[1].Children[1].Text,
                                () => expratio = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[0].Children[1].Text,
                                () => status = gr_table_b1.Select(".gr_table_colm27").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm27")[0].Children[1].Text,
                                () => turnover = gr_table_b1.Select(".gr_table_colm2b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm2b")[1].Children[1].Text,
                                () => totAsset = gr_table_b1.Select(".gr_table_colm16b").Count <= 0 ? string.Empty : gr_table_b1.Select(".gr_table_colm16b")[0].Children[0].Children[1].Children[0].Text
                            );

                            if (int.TryParse(schemaCode.Split('|')[0].Trim(), out schCode))
                            {
                                fund.FundDetails = new FundsDetails()
                                {
                                    SchemaCode = int.Parse(schemaCode.Split('|')[0].Trim()),
                                    ExpenseRatio = FormatPercentage(expratio),
                                    Category = category,
                                    MinimumInvest = FormatPercentage(minInvest),
                                    Status = status,
                                    TurnOver = FormatPercentage(turnover),
                                    Benchmark = GetMSBenchmark(d1),
                                    MSCategory = GetMSCategory(d1),
                                    TotalAsset = 0
                                };

                                SrwMFFundDetails.WriteLine(familyName
                                                                + "," + fund.Title
                                                                + "," + fund.FundDetails.SchemaCode
                                                                + "," + fund.FundDetails.ExpenseRatio
                                                                + "," + fund.FundDetails.Benchmark
                                                                + "," + fund.FundDetails.MinimumInvest
                                                                + "," + fund.Category
                                                                + "," + fund.FundDetails.MSCategory
                                                                + "," + fund.FundDetails.Status
                                                                + "," + fund.FundDetails.TotalAsset
                                                                + "," + fund.FundDetails.TurnOver
                                                                );
                            }
                            RefreshMFFundsData(familyName, fund);
                        }
                        else
                            DisplayMessage("Failed to load: " + fund.Title);
                    }
                    catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
                });
            }
            catch (Exception ex) { }
        }

        private void WriteFundDetailsToFile(string familyName, FundLinks fund)
        {
            try
            {
                SrwFundLinks.WriteLine(familyName
                                                    + "," + fund.Title
                                                    + "," + fund.FundDetails.SchemaCode
                                                    + "," + fund.FundDetails.ExpenseRatio
                                                    + "," + fund.FundDetails.Benchmark
                                                    + "," + fund.FundDetails.MinimumInvest
                                                    + "," + fund.Category
                                                    + "," + fund.FundDetails.MSCategory
                                                    + "," + fund.FundDetails.Status
                                                    + "," + fund.FundDetails.TotalAsset
                                                    + "," + fund.FundDetails.TurnOver
                                                    );
            }
            catch (Exception ex) { }
        }

        private void DumpMSDataToFile(List<FundHouse> fundFamilies)
        {
            int i = 0;
            int j = 0;
            try
            {
                for (; i < fundFamilies.Count; i++)
                {
                    j = 0;
                    if (fundFamilies[i].FundLinks == null) continue;
                    for (; j < fundFamilies[i].FundLinks.Count; j++)
                    {
                        if (fundFamilies[i].FundLinks[j].FundDetails == null) continue;
                        SrwMFFundDetails.WriteLine(fundFamilies[i].FamilyName
                            + "," + fundFamilies[i].FundLinks[j].Title
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.SchemaCode
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.ExpenseRatio
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.Benchmark
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.MinimumInvest
                            + "," + fundFamilies[i].FundLinks[j].Category
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.MSCategory
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.Status
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.TotalAsset
                            + "," + fundFamilies[i].FundLinks[j].FundDetails.TurnOver
                            );
                    }
                }
            }
            catch (Exception ex) { }
        }

        private string GetMSCategory(Supremes.Nodes.Document d1)
        {
            string result = string.Empty;
            try
            {
                result = d1.Body.Children[0].Children[0].Children[1].Children[0].Children[1].Children[2].Children[0].Select(".categoryName").Text;
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        private string GetMSBenchmark(Supremes.Nodes.Document d1)
        {
            string result = string.Empty;
            try
            {
                result = d1.Body.Children[0].Children[0].Children[1].Children[0].Children[1].Children[2].Children[0].Children[0].Children[0].Children[0].Text.Replace("India INR", "").Replace("TR", "").Trim();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        private Supremes.Nodes.Document GetDocument(string url)
        {
            Supremes.Nodes.Document doc = null;
            int i = 0;
            while (doc == null)
            {
                try { doc = Dcsoup.Parse(new Uri(url), 60 * 1000); }
                catch (Exception ex) { }
                if (i++ > 10) break;
            }
            return doc;
        }

        private decimal FormatPercentage(string expratio)
        {
            decimal retValue = 0;
            try
            {
                if (!string.IsNullOrWhiteSpace(expratio))
                {
                    decimal.TryParse(expratio.Replace("%", "").Replace(",", "").Trim(), out retValue);
                }
            }
            catch (Exception ex) { DisplayMessage("Exception: " + ex.Message + Environment.NewLine + "Stack Trace: " + ex.StackTrace); }
            return retValue;
        }

        private void RefreshMFFundsData(FundHouse fundHouse)
        {
            try
            {
                List<FundHouse> h = new List<FundHouse>();
                h.Add(fundHouse);
                _mfDataAccess.RefreshFundDetails(h);
            }
            catch (Exception ex) { }
        }

        private void RefreshMFFundsData(List<FundHouse> fundFamilies)
        {
            try { _mfDataAccess.RefreshFundDetails(fundFamilies); }
            catch (Exception ex) { }
        }

        private void RefreshMFFundsData(string familyName, FundLinks fund)
        {
            try
            {
                List<FundHouse> h = new List<FundHouse>();
                h.Add(new FundHouse() { FamilyName = familyName, FundLinks = new List<FundLinks> { fund } });
                _mfDataAccess.RefreshFundDetails(h);
            }
            catch (Exception ex) { }
        }

        //private void DownloadMorningStarMFData()
        //{
        //    //GetMSFundHouseLinks(fundFamilies);
        //    //GetMSFundsLinks(fundFamilies);
        //    GetMSFundsData(fundFamilies);
        //    DumpMSDataToFile(fundFamilies);
        //}

        //private static void GetMSFundHouseLinks(List<FundFamilies> fundFamilies)
        //{
        //    try
        //    {
        //        Supremes.Nodes.Document doc = GetDocument(morningStarBaseurl + "funds/");
        //        if (doc == null) return;
        //        var mflinks = doc.Body.Select(".mfshortnames_links_wrapper");
        //        int i = 0;
        //        foreach (var mflink in mflinks)
        //        {
        //            i++;
        //            if (i == 1)
        //            {
        //                foreach (var mf in mflink.Children)
        //                {
        //                    try
        //                    {
        //                        DisplayMessage("Adding: " + mf.Text);
        //                        fundFamilies.Add(new FundFamilies() { FamilyName = mf.Text, Uri = mf.Attr("href") });
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        DisplayMessage("Exception: " + ex.Message);
        //                        DisplayMessage("Stack Trace: " + ex.StackTrace);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage("Exception: " + ex.Message);
        //        DisplayMessage("Stack Trace: " + ex.StackTrace);
        //    }
        //}

        //private static void GetMSFundsLinks(List<FundFamilies> fundFamilies)
        //{
        //    try
        //    {
        //        foreach (var fund in fundFamilies)
        //        {
        //            DisplayMessage("Parsing: " + fund.Uri);
        //            Supremes.Nodes.Document doc = null;
        //            doc = GetDocument(morningStarBaseurl + fund.Uri);
        //            if (doc == null) continue;
        //            var mflinks = doc.Body.Select(".archive");
        //            int i = 0;
        //            if (mflinks == null || mflinks.Count <= 0)
        //                continue;
        //            foreach (var funds in mflinks[0].Children)
        //            {
        //                i++;
        //                try
        //                {
        //                    if (i > 1)
        //                    {
        //                        DisplayMessage("Adding: " + funds.Children[0].Children[0].Text);
        //                        fund.FundLinks.Add(new FundLinks()
        //                        {
        //                            Category = funds.Children[1].Text,
        //                            Title = funds.Children[0].Children[0].Text,
        //                            Url = funds.Children[0].Children[0].Attr("href")
        //                        });
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    DisplayMessage("Exception: " + ex.Message);
        //                    DisplayMessage("Stack Trace: " + ex.StackTrace);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage("Exception: " + ex.Message);
        //        DisplayMessage("Stack Trace: " + ex.StackTrace);
        //    }
        //}

    }
}
