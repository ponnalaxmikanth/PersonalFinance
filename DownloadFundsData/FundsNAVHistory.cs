using BusinessAccess.MutualFunds;
using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using DataAccess.MutualFunds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DownloadFundsData
{
    public class FundsNAVHistory : BaseClass
    {
        static MutualFundsDataAccess _mfDataAccess = new MutualFundsDataAccess();
        static ICommonDataAccess _CommonDataAccess = new CommonDataAccess();
        static ICommonRepository _CommonRepository = new CommonRepository(_CommonDataAccess);
        static MutualFundsRepository _mutualBusinessAccess = new MutualFundsRepository(_CommonDataAccess, _mfDataAccess);

        public void DownloadData(int noOfDays = 30)
        {
            List<DownloadUrls> urls = new List<DownloadUrls>();
            urls.Add(new DownloadUrls() {
                Url = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=1&frmdt=", Id = 3, Message = " Open Ended - "
            });
            urls.Add(new DownloadUrls() {
                Url = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=2&frmdt=", Id = 4, Message = " Close Ended - "
            });
            urls.Add(new DownloadUrls() {
                Url = "http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=3&frmdt=", Id = 5, Message = " Interval - "
            });

            DateTime fromDate = DateTime.Now.AddDays(-noOfDays).Date;
            DateTime toDate = DateTime.Now.Date.AddDays(-1).Date;

            ParallelOptions optns = new ParallelOptions() { MaxDegreeOfParallelism = 1 };


            while (toDate > fromDate)
            {
                DisplayMessage("[" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffff") + "]:" + " Getting NAV data - " + toDate.ToString("dd-MMM-yyyy"));
                List<NAVData> navData = _mutualBusinessAccess.GetFundsNAV(toDate);

                Parallel.ForEach(urls, optns, u =>
                {
                    DownloadFundNavHistory(toDate, navData, u.Url + toDate.ToString("dd-MMM-yyyy") + "&todt=" + toDate.ToString("dd-MMM-yyyy"), u.Id, u.Message);
                });
                toDate = toDate.AddDays(-1).Date;
            }
        }

        private void DownloadFundNavHistory(DateTime date, List<NAVData> navData, string link, int fundType, string message)
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

        private void UpdateNAVHistoryData(DateTime date, string data, List<NAVData> navData, int fundType, string message)
        {
            string[] navdata = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<NAVData> latestNavData = new List<NAVData>();
            decimal nav;
            decimal repurchasePrice;
            decimal sellPrice;
            string fundHouse = string.Empty;

            for (int i = 0; i < navdata.Length; i++)
            {
                string[] result = navdata[i].Split(';');
                if (result.Length == 8 && result[0].Trim().ToLower() != "Scheme Code".ToLower() && decimal.TryParse(result[4].Trim(), out nav))
                {
                    try
                    {
                        NAVData record = navData.Where(r => r.SchemaCode == Convert.ToInt32(result[0].Trim()) && r.Date == Convert.ToDateTime(result[7].Trim()))
                                                .Select(r => r)
                                                .FirstOrDefault();
                        repurchasePrice = GetDecimalValue(result[5].Trim());
                        sellPrice = GetDecimalValue(result[6].Trim());
                        if (record == null || record.NAV != Convert.ToDecimal(result[4].Trim()))
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
                                Date = Convert.ToDateTime(result[7].Trim())
                            };
                            latestNavData.Add(funddata);
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("");
                        DisplayMessage(message + date.ToString("dd-MMM-yyyy") + " Exception: " + ex.Message);
                    }
                }
                else if (navdata[i].Trim().Length > 0)
                {
                    fundHouse = navdata[i];
                }
            }
            //DisplayMessage(" No Of Records - " + latestNavData.Count());
            DisplayMessage("[" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss.ffff") + "]:" + message + date.ToString("dd-MMM-yyyy") + "  Count: " + latestNavData.Count());

            if (latestNavData != null && latestNavData.Count() > 0)
            {
                _mfDataAccess.UpdateNAVHistory(latestNavData);
                string xml = GetXMLString(latestNavData);
                _mfDataAccess.UpdateNAVHistory(xml);
                _CommonRepository.InsertDumpDate(date, fundType, latestNavData.Count());
            }
        }

        private string GetXMLString(List<NAVData> latestNavData)
        {
            string returnStr = string.Empty;
            try
            {
                returnStr = new XElement("root",
                    (from n in latestNavData
                     select
                     new XElement("fund",
                           new XElement("code", n.SchemaCode),
                           new XElement("nav", n.NAV),
                           new XElement("type", n.FundType),
                           new XElement("date", n.Date.ToString("MM/dd/yyyy"))
                           )
                    )
                  ).ToString();
            }
            catch (Exception ex)
            {

            }
            return returnStr;
        }
    }
}
