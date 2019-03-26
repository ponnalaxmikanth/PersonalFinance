using BusinessAccess.MutualFunds;
using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using DataAccess.MutualFunds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloadFundsData
{
    public class FundsNAV : BaseClass
    {
        MutualFundsDataAccess _mfDataAccess;
        ICommonDataAccess _CommonDataAccess;

        public FundsNAV()
        {
            _mfDataAccess = new MutualFundsDataAccess();
            _CommonDataAccess = new CommonDataAccess();
        }
        public void DownloadNAVData()
        {
            try
            {
                ICommonRepository _CommonRepository = new CommonRepository(new CommonDataAccess());

                List<DownloadUrls> urls = new List<DownloadUrls>();
                urls.Add(new DownloadUrls() { Id = 3, Url = "http://www.amfiindia.com/spages/NAVOpen.txt", Message = "Downloading Open Funds Data" });
                urls.Add(new DownloadUrls() { Id = 4, Url = "http://www.amfiindia.com/spages/NAVClose.txt", Message = "Downloading Closed Funds Data" });
                urls.Add(new DownloadUrls() { Id = 5, Url = "http://www.amfiindia.com/spages/NAVInterval.txt", Message = "Downloading Interval Funds Data" });

                foreach (var u in urls)
                {
                    DisplayMessage(u.Message + " : " + DateTime.Now.Date.ToString("MM/dd/yyyy"));
                    DownloadNAVData(u.Url, u.Id, DateTime.Now.Date);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //public async Task<bool> DownloadNAVData(string link, int fundType, DateTime date)
        public bool DownloadNAVData(string link, int fundType, DateTime date)
        {
            try
            {
                //await Task.Delay(1);
                StringBuilder sb = new StringBuilder();
                byte[] buf = new byte[8192];
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // execute the request
                Stream resStream = response.GetResponseStream(); // we will read data via the response stream

                using (var reader = new StreamReader(resStream))
                {
                    UpdateNAVData(date, reader.ReadToEnd(), fundType);
                }
            }
            catch (Exception ex) {
                DisplayMessage("Exception occurred: " + ex.Message);
            }
            return true;
        }

        private void UpdateNAVData(DateTime date, string data, int fundType)
        {
            string[] navdata = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<NAVData> latestNavData = new List<NAVData>();
            decimal nav;
            decimal repurchasePrice = -99999999;
            decimal sellPrice = -99999999;
            string fundsHouse = string.Empty;
            for (int i = 0; i < navdata.Length; i++)
            {
                string[] result = navdata[i].Split(';');
                //if (result.Length == 8 && result[0].Trim().ToLower() != "Scheme Code".ToLower() && decimal.TryParse(result[4].Trim(), out nav)
                //                       && Convert.ToDateTime(result[7].Trim()).Date > DateTime.Now.AddDays(-7).Date)

                if (result.Length == 6 && result[0].Trim().ToLower() != "Scheme Code".ToLower() && decimal.TryParse(result[4].Trim(), out nav)
                                       && Convert.ToDateTime(result[5].Trim()).Date > DateTime.Now.AddDays(-7).Date)
                {
                    try
                    {
                        //repurchasePrice = GetValue(result[5].Trim());
                        //sellPrice = GetValue(result[6].Trim());

                        NAVData funddata = new NAVData()
                        {
                            FundHouse = fundsHouse,
                            SchemaCode = Convert.ToInt32(result[0].Trim()),
                            ISINGrowth = result[1].Trim(),
                            ISINDivReinvestment = result[2].Trim(),
                            SchemaName = result[3].Trim().ToUpper().Replace("ICICI PRUDENTIAL".ToUpper(), "ICICI")
                                                                   .Replace("ADITYA BIRLA SUN LIFE", "ABSL")
                                                                   .Replace("DIRECT PLAN", "Direct")
                                                                   .Replace("(Direct)".ToUpper(), "Direct")
                                                                   .Replace("REGULAR PLAN", "Regular")
                                                                   .Replace("(REGULAR)", "Regular")
                                                                   .Replace("Growth Option".ToUpper(), "(G)")
                                                                   .Replace("GROWTH", "(G)")
                                                                   .Replace("Dividend Option".ToUpper(), "(D)")
                                                                   .Replace("DIVIDEND", "(D)")
                                                                   .Replace("FUND", ""),
                            FundOption = result[3].Trim().ToUpper().Contains("GROWTH") ? 1 : 2,
                            Fund_Type = result[3].Trim().ToUpper().Contains("DIRECT") ? 2 : 1,
                            FundType = fundType,
                            NAV = nav,
                            RepurchasePrice = repurchasePrice,
                            SellPrice = sellPrice,
                            Date = Convert.ToDateTime(result[5].Trim())
                        };
                        latestNavData.Add(funddata);

                        DisplayMessage(fundsHouse + " : " + funddata.SchemaName);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (navdata[i].Trim().Length > 0
                    && navdata[i].ToLower() != "Open Ended Schemes(Balanced)".ToLower()
                    && result[0].Trim().ToLower() != "Scheme Code".ToLower()
                    && !navdata[i].Contains(';'))
                {
                    fundsHouse = navdata[i].Replace(" Mutual Fund", "");
                }
            }
            if (latestNavData != null && latestNavData.Count() > 0)
            {
                _mfDataAccess.UpdateLatestNAV(latestNavData);
                _CommonDataAccess.InsertDumpDate(date, fundType, latestNavData.Count());
            }
        }

        private decimal GetValue(string val)
        {
            decimal retval = -99999999;
            if (decimal.TryParse(val, out retval))
            {
            }
            else
            {
                retval = -99999999;
            }
            return retval;
        }
    }
}
