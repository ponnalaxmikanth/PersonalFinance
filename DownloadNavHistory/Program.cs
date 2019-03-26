using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessAccess.MutualFunds;
using BusinessEntities.Entities;
using System.Net;
using System.IO;
using BusinessEntities.Contracts.MutualFunds;
using DataAccess.MutualFunds;
using BusinessEntities.Entities.MutualFunds;

namespace DownloadNavHistory
{
    class Program
    {
        static MutualFundsDataAccess _mfDataAccess = new MutualFundsDataAccess();
        static ICommonDataAccess _CommonDataAccess = new CommonDataAccess();
        static ICommonRepository _CommonRepository = new CommonRepository(_CommonDataAccess);
        static MutualFundsRepository _mutualBusinessAccess = new MutualFundsRepository(_CommonDataAccess, _mfDataAccess);

        static void Main(string[] args)
        {
            DateTime fromDate = DateTime.Now.AddDays(-15).Date;
            DateTime toDate = DateTime.Now.Date.AddDays(-1).Date;

            Console.Title = "Download Mutual Fund History: " + fromDate.ToString("MM/dd/yyyy") + " - " + toDate.ToString("MM/dd/yyyy");
            List<DownloadUrls> urls = new List<DownloadUrls>();
            urls.Add(new DownloadUrls()
            {
                Url = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=1&frmdt=",
                Id = 3,
                Message = " Open Ended - "
            });
            urls.Add(new DownloadUrls()
            {
                Url = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=2&frmdt=",
                Id = 4,
                Message = " Close Ended - "
            });
            urls.Add(new DownloadUrls()
            {
                Url = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=3&frmdt=",
                Id = 5,
                Message = " Interval - "
            });

            //while (fromDate <= toDate)
            //{
            //    Console.WriteLine("[" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffff") + "]:" + " Getting NAV data - " + fromDate.ToString("dd-MMM-yyyy"));
            //    List<NAVData> navData = _mutualBusinessAccess.GetFundsNAV(fromDate);

            //    Parallel.ForEach(urls, u =>
            //    {
            //        DownloadFundNavHistory(fromDate, navData, u.Url + fromDate.ToString("dd-MMM-yyyy") + "&todt=" + fromDate.ToString("dd-MMM-yyyy"), u.Id, u.Message);
            //    });
            //    fromDate = fromDate.AddDays(1).Date;
            //}


            while (toDate > fromDate)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffff") + "]:" + " Getting NAV data - " + toDate.ToString("dd-MMM-yyyy"));
                List<NAVData> navData = _mutualBusinessAccess.GetFundsNAV(toDate);

                Parallel.ForEach(urls, u =>
                {
                    DownloadFundNavHistory(toDate, navData, u.Url + toDate.ToString("dd-MMM-yyyy") + "&todt=" + toDate.ToString("dd-MMM-yyyy"), u.Id, u.Message);
                });
                toDate = toDate.AddDays(-1).Date;
            }
            //Console.WriteLine("Press any key to exit....");
            //Console.ReadKey();
        }

        private static void DownloadFundNavHistory(DateTime date, List<NAVData> navData, string link, int fundType, string message)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // execute the request
                Stream resStream = response.GetResponseStream(); // we will read data via the response stream

                using (var reader = new StreamReader(resStream))
                {
                    UpdateNAVHistoryData(date, reader.ReadToEnd(), navData, fundType, message);
                }
            }
            catch (Exception ex) { }
        }

        private static decimal GetValue(string val)
        {
            decimal retval = -99999999;
            if (decimal.TryParse(val, out retval)) { }
            else { retval = -99999999; }
            return retval;
        }

        private static void UpdateNAVHistoryData(DateTime date, string data, List<NAVData> navData, int fundType, string message)
        {
            string[] navdata = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<NAVData> latestNavData = new List<NAVData>();
            decimal nav;
            decimal repurchasePrice;
            decimal sellPrice;
            for (int i = 0; i < navdata.Length; i++)
            {
                string[] result = navdata[i].Split(';');
                if (result.Length == 6 && result[0].Trim().ToLower() != "Scheme Code".ToLower() && decimal.TryParse(result[2].Trim(), out nav))
                {
                    try
                    {
                        NAVData record = navData.Where(r => r.SchemaCode == Convert.ToInt32(result[0].Trim()) && r.Date == Convert.ToDateTime(result[5].Trim()))
                                                .Select(r => r)
                                                .FirstOrDefault();
                        repurchasePrice = GetValue(result[3].Trim());
                        sellPrice = GetValue(result[4].Trim());
                        if (record == null || record.NAV != Convert.ToDecimal(result[2].Trim()))
                        {
                            NAVData funddata = new NAVData()
                            {
                                SchemaCode = Convert.ToInt32(result[0].Trim()),
                                FundType = fundType,
                                ISINGrowth = string.Empty,
                                ISINDivReinvestment = string.Empty,
                                SchemaName = string.Empty,
                                NAV = nav,
                                RepurchasePrice = repurchasePrice,
                                SellPrice = sellPrice,
                                Date = Convert.ToDateTime(result[5].Trim())
                            };
                            latestNavData.Add(funddata);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine(message + date.ToString("dd-MMM-yyyy") + " Exception: " + ex.Message);
                    }
                }
            }
            //Console.WriteLine(" No Of Records - " + latestNavData.Count());
            Console.WriteLine("[" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffff") + "]:" + message + date.ToString("dd-MMM-yyyy") + "  Count: " + latestNavData.Count());

            if (latestNavData != null && latestNavData.Count() > 0)
            {
                _mfDataAccess.UpdateNAVHistory(latestNavData);
                _CommonRepository.InsertDumpDate(date, fundType, latestNavData.Count());
            }
        }

    }

}
