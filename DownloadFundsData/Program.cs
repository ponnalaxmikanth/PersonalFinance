using Supremes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.Entities.MutualFunds;
using DataAccess.MutualFunds;
using BusinessAccess.MutualFunds;
using BusinessEntities.Contracts.MutualFunds;
using System.IO;
using System.Net;
using System.Windows.Forms;
using BusinessEntities.Entities;

namespace DownloadFundsData
{
    class Program
    {
        public static string moneyControlBaseurl;
        public static string nseBaseUrl;
        public static string bseBaseUrl;
        static MutualFundsDataAccess _mfDataAccess = new MutualFundsDataAccess();
        static ICommonDataAccess _CommonDataAccess = new CommonDataAccess();
        static ICommonRepository _CommonRepository = new CommonRepository(_CommonDataAccess);
        static MutualFundsRepository _mutualBusinessAccess = new MutualFundsRepository(_CommonDataAccess, _mfDataAccess);

        [STAThread]
        static void Main(string[] args)
        {
            DisplayMessage("Application started...");
            moneyControlBaseurl = "http://www.moneycontrol.com/";
            
            nseBaseUrl = "https://www.nseindia.com/";
            bseBaseUrl = "http://www.bseindia.com/";

            if (args[0] == "BSE")
            {
                Console.Title = "Downloading BSE Index : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                //new GetBSEIndexData().GetBseBenchMarkData(bseBaseUrl);
                new GetBSEIndexData().GetBseBenchMarkHistoryData(bseBaseUrl);
            }
            if (args[0] == "BSEHistory")
            {
                Console.Title = "Downloading BSE History Index : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                new DownloadBSEIndicesHistory().DownloadBSEHistory();
            }
            if (args[0] == "NSE")
            {
                Console.Title = "Downloading NSE Index : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                new GetNSEBenchmarkData().DownloadNSEBenchMarkData();
            }
            if (args[0] == "NAV")
            {
                int noOfProcesses = args.Length > 1 ? int.Parse(args[1]) : 1;
                Console.Title = "Downloading MF NAV: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                //GetNAVDataFromCAMS();
                new FundsNAV().DownloadNAVData(noOfProcesses);
            }
            if (args[0] == "NAVHistory")
            {
                Console.Title = "Downloading MF NAV History: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                new FundsNAVHistory().DownloadData(args.Length  == 2 ? int.Parse(args[1]) : 30);
            }
            if (args[0] == "NSEHistory")
            {
                Console.WriteLine("Enter From Date: ");
                string fromDate = Console.ReadLine();
                Console.WriteLine("Enter To Date: ");
                string toDate = Console.ReadLine();
                Console.Title = "Downloading NSE Benchmark : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss" + " " + fromDate + " - " + toDate);
                new GetNSEBenchmarkData().DownloadNSEBenchMarkHistory(fromDate, toDate);
            }
            if (args[0] == "MS")
            {
                new GetMorningStarMFData().GetMFDataFromMorningStar();
            }
            if (args[0] == "WSMF")
            {
                Console.Title = "Downloading MF  Data from Wall Street Journal: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                new USMFWallStreetJournal().DownloadData();
            }

            if (args[0] == "Stocks")
            {
                Console.Title = "Downloading Stocks Data: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                new Stocks().GetLatestStockValues();
            }
            //if(args[0] == "MS")
            //    DownloadMorningStarMFData();

            DisplayMessage("Processing completed...");
            DisplayMessage("Press any key to exit...");
            Console.ReadKey();
            DisplayMessage("exiting application...");
            return;
        }

        #region CAMS NAV
        private static void GetNAVDataFromCAMS()
        {
            try
            {
                _mfDataAccess.BackUpNAVData();

                ICommonRepository _CommonRepository = new CommonRepository(new CommonDataAccess());

                List<DownloadUrls> urls = new List<DownloadUrls>();
                urls.Add(new DownloadUrls() { Id = 3, Url = "http://www.amfiindia.com/spages/NAVOpen.txt", Message = "Downloading Open Funds Data" });
                urls.Add(new DownloadUrls() { Id = 4, Url = "http://www.amfiindia.com/spages/NAVClose.txt", Message = "Downloading Closed Funds Data" });
                urls.Add(new DownloadUrls() { Id = 5, Url = "http://www.amfiindia.com/spages/NAVInterval.txt", Message = "Downloading Interval Funds Data" });

                foreach (var u in urls)
                {
                    DisplayMessage(u.Message + " : " + DateTime.Now.Date.ToString("MM/dd/yyyy"));
                    _mutualBusinessAccess.DownloadNAVData(u.Url, u.Id, DateTime.Now.Date).Wait();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region NSE Benckmark Data

        private static void GetNseBenchMarkData()
        {
            try
            {
                //http://www.indiainfoline.com/markets/indices
                //https://nseindia.com/products/dynaContent/equities/indices/historicalindices.jsp?indexType=NIFTY%2050&fromDate=01-10-2017&toDate=17-10-2017
                Supremes.Nodes.Document doc = new BaseClass().GetDocument(nseBaseUrl);//+ "products/content/equities/indices/historical_index_data.htm"
                if (doc == null) return;
                var mflinks = doc.Body.Select("#");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Moneycontrol Data

        private static void GetFundFamilies(List<FundHouse> fundFamilies)
        {
            try
            {
                var doc = Dcsoup.Parse(new Uri(moneyControlBaseurl + "mutual-funds/amc-assets-monitor"), 5000);

                var scoreDiv = doc.Body.Select(".tblfund1");
                var trs = scoreDiv.Select("tr");
                int i = 0;
                foreach (var tr in trs)
                {
                    i++;
                    if (i == 1) continue;
                    try
                    {
                        var tds = tr.Select("td");
                        var a = tds[0].Select("p > a.bl_12");
                        fundFamilies.Add(new FundHouse()
                        {
                            FamilyName = a.Attr("title"),
                            Uri = a.Attr("href")
                        });
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Exception: " + ex.Message);
                        DisplayMessage("Stack Trace: " + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Exception: " + ex.Message);
                DisplayMessage("Stack Trace: " + ex.StackTrace);
            }
        }

        #endregion

        #region Common Methods
        #endregion
        

        //private static void GetFundsData(List<FundFamilies> fundFamilies)
        //{
        //    try
        //    {
        //        foreach (var fund in fundFamilies)
        //        {
        //            var doc = Dcsoup.Parse(new Uri(moneyControlBaseurl + fund.Uri), 5000);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage("Exception: " + ex.Message);
        //        DisplayMessage("Stack Trace: " + ex.StackTrace);
        //    }
        //}

        private static void DisplayMessage(string msg)
        {
            Console.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "] " + msg);
        }
    }
}
