using BusinessEntities.Contracts;
using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.MutualFunds
{
    public class MutualFundsRepository : IMutualFundsBusinessAccess
    {
        IMutualFundDataAccess _mfDataAccess;
        ICommonDataAccess CommonDataAccess;

        public MutualFundsRepository(ICommonDataAccess common, IMutualFundDataAccess mfDataAccess)
        {
            CommonDataAccess = common;
            _mfDataAccess = mfDataAccess;
        }

        public List<MF_Portfolio> GetMutualFundPortfolios()
        {
            return MapMFPortfolios(_mfDataAccess.GetPortFolios());
        }

        private List<MF_Portfolio> MapMFPortfolios(DataTable dataTable)
        {
            List<MF_Portfolio> lstMFPortFolios = null;

            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    lstMFPortFolios = dataTable.AsEnumerable().Select(r => new MF_Portfolio()
                        {
                            PortfolioId = r.Field<int>("PortfolioId"),
                            Portfolio = r.Field<string>("Portfolio"),
                            Description = r.Field<string>("Description"),
                            CreatedDate = r.Field<DateTime>("CreatedDate")
                        }).ToList();
                }
                else if (dataTable != null && dataTable.Rows.Count == 0)
                {
                    lstMFPortFolios = new List<MF_Portfolio>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstMFPortFolios;
        }


        public List<MFPortfolioData> GetPortfolioTransactions(GetMFTransactions getMFTransactions)
        {
            return MapPortfolioTransactions(_mfDataAccess.GetPortfolioTransactions(getMFTransactions));
        }

        private List<MFPortfolioData> MapPortfolioTransactions(DataTable dataTable)
        {
            List<MFPortfolioData> lstTransactions = null;
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    lstTransactions = dataTable.AsEnumerable().Select(r => new MFPortfolioData()
                    {
                        PortfolioId = r.Field<int>("PortfolioId"),
                        FundId = r.Field<int>("FundId"),
                        Amount = r.Field<decimal>("Amount"),
                        CurrentNAV = r.Field<decimal>("NAV"),
                        Dividend = r.Field<decimal?>("Dividend"),
                        FundName = r.Field<string>("FundName"),
                        LatestNAVDate = r.Field<DateTime>("LatestNAVDate"),
                        PurchaseNAV = r.Field<decimal>("PurchaseNAV"),
                        //SellAmount = r.Field<decimal?>("sellAmount"),
                        //SellUnits = r.Field<decimal?>("SellUnits"),
                        SellAmount = (r["sellAmount"] == null || r["sellAmount"] == DBNull.Value || string.IsNullOrWhiteSpace(r["sellAmount"].ToString()))
                                        ? 0 : decimal.Parse(r["sellAmount"].ToString()),
                        SellUnits = (r["SellUnits"] == null || r["SellUnits"] == DBNull.Value || string.IsNullOrWhiteSpace(r["SellUnits"].ToString()))
                        ? 0 : decimal.Parse(r["SellUnits"].ToString()),
                        Units = r.Field<decimal>("Units"),
                        
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return lstTransactions;
        }

        //private List<MF_Transactions> MapPortfolioTransactions(DataTable dataTable)
        //{
        //    List<MF_Transactions> lstTransactions = null;

        //    try
        //    {
        //        if (dataTable != null && dataTable.Rows.Count > 0)
        //        {
        //            lstTransactions = dataTable.AsEnumerable().Select(r => new MF_Transactions()
        //            {
        //                //TransactionId = r.Field<int>("TransactionId"),
        //                Portfolio = r.Field<string>("Portfolio"),
        //                PortfolioId = r.Field<int>("PortfolioId"),
        //                FolioId = r.Field<int>("FolioId"),
        //                FolioNumber = r.Field<string>("FolioNumber"),
        //                FundHouseName = r.Field<string>("FundHouseName"),
        //                FundId = r.Field<int>("FundId"),
        //                FundName = r.Field<string>("FundName"),
        //                FundOption = r.Field<string>("FundOption"),
        //                FundClass = r.Field<string>("FundClass"),
        //                IsSector = r.Field<bool>("IsSectorCategory"),
        //                FundType = r.Field<string>("FundType"),
        //                //PurchaseDate = r.Field<DateTime>("PurchaseDate"),
        //                Amount = decimal.Round(r.Field<decimal>("Amount"), 3, MidpointRounding.AwayFromZero),
        //                PurchaseNAV = decimal.Round(r.Field<decimal>("PurchaseNAV"), 3, MidpointRounding.AwayFromZero),
        //                PurchaseUnits = decimal.Round(r.Field<decimal>("Units"), 3, MidpointRounding.AwayFromZero),

        //                Dividend = decimal.Round(r.Field<decimal>("Dividend"), 3, MidpointRounding.AwayFromZero),
        //                ActualNAV = decimal.Round(r.Field<decimal>("ActualNAV"), 3, MidpointRounding.AwayFromZero),

        //                SellUnits = r.Field<decimal?>("SellUnits"),
        //                //SellDate = r.Field<DateTime?>("SellDate"),
        //                SellNAV = r.Field<decimal?>("SellNAV"),
        //                STT = r.Field<decimal?>("SellSTT"),
        //            }).ToList();
        //        }
        //        else if (dataTable != null && dataTable.Rows.Count == 0)
        //        {
        //            lstTransactions = new List<MF_Transactions>();
        //        }
        //    }
        //    catch (Exception ex)
        //    { }
        //    return lstTransactions;
        //}


        public MFFundDetails GetMFFundDetails()
        {
            MFFundDetails response = null;

            try
            {
                DataTable dtPortFolios = _mfDataAccess.GetPortFolios();
                DataTable dtFolios = _mfDataAccess.GetFolios();
                DataTable dtFundCategory = _mfDataAccess.GetFundCategory();
                DataTable dtFundHouses = _mfDataAccess.GetFundHouses();
                DataTable dtFundOptions = _mfDataAccess.GetFundOptions();
                DataTable dtFundTypes = _mfDataAccess.GetFundTypes();
                DataTable dtFunds = _mfDataAccess.GetFunds();

                response = new MFFundDetails();
                response.LstPortfolios = MapMFPortfolios(dtPortFolios);
                response.LstFolios = MapMFFolios(dtFolios);
                response.LstFundCategory = MapFundCategory(dtFundCategory);
                response.LstFundHouses = MapFundHouses(dtFundHouses);
                response.LstFundOptions = MapFundOptions(dtFundOptions);
                response.LstFundTypes = MapFundTypes(dtFundTypes);
                response.LstFunds = MapFunds(dtFunds);
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        private List<MF_Funds> MapFunds(DataTable dtFunds)
        {
            List<MF_Funds> lstFunds = null;
            try
            {
                if (dtFunds != null && dtFunds.Rows.Count > 0)
                {
                    lstFunds = dtFunds.AsEnumerable().Select(r => new MF_Funds()
                    {
                        FundClassId = r.Field<int>("FundClassId"),
                        FundHouseId = r.Field<int>("FundHouseId"),
                        FundId = r.Field<int>("FundId"),
                        FundName = r.Field<string>("FundName"),
                        FundOptionId = r.Field<int>("FundOptionId"),
                        FundTypeId = r.Field<int>("FundTypeId"),

                        CreatedDate = r.Field<DateTime>("CreatedDate")
                    }).ToList();
                }
                else if (dtFunds != null && dtFunds.Rows.Count == 0)
                {
                    lstFunds = new List<MF_Funds>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstFunds;
        }

        private List<MF_FundTypes> MapFundTypes(DataTable dtFundTypes)
        {
            List<MF_FundTypes> lstFundTypes = null;
            try
            {
                if (dtFundTypes != null && dtFundTypes.Rows.Count > 0)
                {
                    lstFundTypes = dtFundTypes.AsEnumerable().Select(r => new MF_FundTypes()
                    {
                        FundTypeId = r.Field<int>("FundTypeId"),
                        FundType = r.Field<string>("FundType"),
                        Description = r.Field<string>("Description"),
                        CreatedDate = r.Field<DateTime>("CreatedDate")
                    }).ToList();
                }
                else if (dtFundTypes != null && dtFundTypes.Rows.Count == 0)
                {
                    lstFundTypes = new List<MF_FundTypes>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstFundTypes;
        }

        private List<MF_FundOptions> MapFundOptions(DataTable dtFundOptions)
        {
            List<MF_FundOptions> lstFundOptions = null;
            try
            {
                if (dtFundOptions != null && dtFundOptions.Rows.Count > 0)
                {
                    lstFundOptions = dtFundOptions.AsEnumerable().Select(r => new MF_FundOptions()
                    {
                        FundOption = r.Field<string>("FundOption"),
                        OptionId = r.Field<int>("OptionId"),
                        Description = r.Field<string>("Description"),
                        CreatedDate = r.Field<DateTime>("CreatedDate")
                    }).ToList();
                }
                else if (dtFundOptions != null && dtFundOptions.Rows.Count == 0)
                {
                    lstFundOptions = new List<MF_FundOptions>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstFundOptions;
        }

        private List<MF_FundHouses> MapFundHouses(DataTable dtFundHouses)
        {
            List<MF_FundHouses> lstFundHouses = null;
            try
            {
                if (dtFundHouses != null && dtFundHouses.Rows.Count > 0)
                {
                    lstFundHouses = dtFundHouses.AsEnumerable().Select(r => new MF_FundHouses()
                    {
                        FundHouseId = r.Field<int>("FundHouseId"),
                        FundHouseName = r.Field<string>("FundHouseName"),
                        Description = r.Field<string>("Description"),
                        CreatedDate = r.Field<DateTime>("CreatedDate")
                    }).ToList();
                }
                else if (dtFundHouses != null && dtFundHouses.Rows.Count == 0)
                {
                    lstFundHouses = new List<MF_FundHouses>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstFundHouses;
        }

        private List<MF_FundCategory> MapFundCategory(DataTable dtFundCategory)
        {
            List<MF_FundCategory> lstFundCategory = null;
            try
            {
                if (dtFundCategory != null && dtFundCategory.Rows.Count > 0)
                {
                    lstFundCategory = dtFundCategory.AsEnumerable().Select(r => new MF_FundCategory()
                    {
                        FundClass = r.Field<string>("FundClass"),
                        FundClassId = r.Field<int>("FundClassId"),
                        IsSectorCategory = r.Field<bool>("IsSectorCategory"),
                        Description = r.Field<string>("Description"),
                        CreatedDate = r.Field<DateTime>("CreatedDate")
                    }).ToList();
                }
                else if (dtFundCategory != null && dtFundCategory.Rows.Count == 0)
                {
                    lstFundCategory = new List<MF_FundCategory>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstFundCategory;
        }

        private List<MF_Folios> MapMFFolios(DataTable dtFolios)
        {
            List<MF_Folios> lstMFFolios = null;
            try
            {
                if (dtFolios != null && dtFolios.Rows.Count > 0)
                {
                    lstMFFolios = dtFolios.AsEnumerable().Select(r => new MF_Folios()
                    {
                        FolioId = r.Field<int>("FolioId"),
                        FundHouseId = r.Field<int>("fundhouseid"),
                        FolioNumber = r.Field<string>("FolioNumber"),
                        Description = r.Field<string>("Description"),
                        CreatedDate = r.Field<DateTime>("CreatedDate")
                    }).ToList();
                }
                else if (dtFolios != null && dtFolios.Rows.Count == 0)
                {
                    lstMFFolios = new List<MF_Folios>();
                }
            }
            catch (Exception ex)
            {

            }
            return lstMFFolios;
        }


        public List<MF_Transactions> GetFundTransactions(GetMFTransactions getMFTransactions)
        {
            return MapFundTransactions(_mfDataAccess.GetFundTransactions(getMFTransactions));
        }

        private List<MF_Transactions> MapFundTransactions(DataTable dataTable)
        {
            List<MF_Transactions> lstTransactions = null;

            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    lstTransactions = dataTable.AsEnumerable().Select(r => new MF_Transactions()
                    {
                        FundId = r.Field<int>("FundId"),
                        TransactionId = r.Field<int>("TransactionId"),
                        PurchaseDate = r.Field<DateTime>("PurchaseDate").Date,
                        Amount = decimal.Round(r.Field<decimal>("Amount"), 3, MidpointRounding.AwayFromZero),
                        PurchaseNAV = decimal.Round(r.Field<decimal>("PurchaseNAV"), 3, MidpointRounding.AwayFromZero),
                        PurchaseUnits = decimal.Round(r.Field<decimal>("Units"), 3, MidpointRounding.AwayFromZero),
                        Dividend = decimal.Round(r.Field<decimal>("Dividend"), 3, MidpointRounding.AwayFromZero),
                        ActualNAV = decimal.Round(r.Field<decimal>("ActualNAV"), 3, MidpointRounding.AwayFromZero),
                        //SellDate = r.Field<DateTime?>("SellDate"),
                        //SellUnits = r.Field<decimal?>("SellUnits"),
                        //SellNAV = r.Field<decimal?>("SellNAV"),
                        //STT = r.Field<decimal?>("SellSTT"),
                        LatestNAV = r.Field<decimal>("NAV"),
                        LatestNAVDate = r.Field<DateTime>("LatestNAVDate"),
                    }).ToList().OrderByDescending(x => x.PurchaseDate).ToList();
                }
                else if (dataTable != null && dataTable.Rows.Count == 0)
                {
                    lstTransactions = new List<MF_Transactions>();
                }
            }
            catch (Exception ex)
            { }
            return lstTransactions;
        }


        public List<MF_Transactions> AddUpdateMFTransaction(AddMFTransactionRequest _mfTransactionRequest)
        {
            _mfDataAccess.AddUpdateMFTransaction(_mfTransactionRequest);

            GetMFTransactions getMFTransactions = new GetMFTransactions()
            {
                FromDate = _mfTransactionRequest.FromDate,
                ToDate = _mfTransactionRequest.ToDate,
                FundId = _mfTransactionRequest.FundId,
                PortfolioId = _mfTransactionRequest.PortfolioId.ToString()
            };
            return MapFundTransactions(_mfDataAccess.GetFundTransactions(getMFTransactions));
        }


        public void AddDividend(AddDividendRequest _dividendRequest)
        {
            _mfDataAccess.AddDividend(_dividendRequest);
        }

        public async Task<bool> DownloadNAVData(string link, int fundType, DateTime date)
        {
            try
            {
                await Task.Delay(1);
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
            catch (Exception ex) { }
            return true;
        }

        private void UpdateNAVData(DateTime date, string data, int fundType)
        {
            string[] navdata = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<NAVData> latestNavData = new List<NAVData>();
            decimal nav;
            decimal repurchasePrice;
            decimal sellPrice;
            string fundsHouse = string.Empty;
            for (int i = 0; i < navdata.Length; i++)
            {
                string[] result = navdata[i].Split(';');
                if (result.Length == 8 && result[0].Trim().ToLower() != "Scheme Code".ToLower() && decimal.TryParse(result[4].Trim(), out nav)
                                       && Convert.ToDateTime(result[7].Trim()).Date > DateTime.Now.AddDays(-7).Date)
                {
                    try
                    {
                        repurchasePrice = GetValue(result[5].Trim());
                        sellPrice = GetValue(result[6].Trim());

                        NAVData funddata = new NAVData()
                        {
                            SchemaCode = Convert.ToInt32(result[0].Trim()),
                            ISINGrowth = result[1].Trim(),
                            ISINDivReinvestment = result[2].Trim(),
                            SchemaName = result[3].Trim().ToUpper().Replace("ICICI PRUDENTIAL".ToUpper(), "ICICI PRU")
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
                            Date = Convert.ToDateTime(result[7].Trim())
                        };
                        latestNavData.Add(funddata);


                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (navdata[i].Trim().Length > 0 
                    && navdata[i].ToLower() != "Open Ended Schemes(Balanced)".ToLower() 
                    && result[0].Trim().ToLower() != "Scheme Code".ToLower())
                {
                    fundsHouse = navdata[i];
                }
            }
            if (latestNavData != null && latestNavData.Count() > 0)
            {
                _mfDataAccess.UpdateLatestNAV(latestNavData);
                CommonDataAccess.InsertDumpDate(date, fundType, latestNavData.Count());
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

        public List<FundPerformance> GetFundsPerformance()
        {
            return MapFundsPerformance(_mfDataAccess.GetFundsPerformance());
        }

        private List<FundPerformance> MapFundsPerformance(DataTable dataTable)
        {
            List<FundPerformance> result = null;
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    //var r1 = (from r in dataTable.AsEnumerable()
                    //          group r by new { level = r["Level"].ToString() } into lvlGrp
                    //          select new
                    //          {
                    //              LatestDate = dataTable.AsEnumerable().Select(r => Convert.ToDateTime(r["LatestDate"].ToString())).OrderBy(r => r.Date).First(),
                    //              HistoryDate = lvlGrp.Select(r => Convert.ToDateTime(r["HistoryDate"].ToString())).OrderByDescending(r => r.Date).First(),
                    //              Schemas = (from s in lvlGrp
                    //                         select new
                    //                         {
                    //                             SchemaCode = s["SchemaCode"].ToString(),
                    //                             SchemaName = s["SchemaName"].ToString(),
                    //                             LatestNav = decimal.Parse(s["LatestNAV"].ToString()),
                    //                             HistoricalNav = decimal.Parse(s["HistoryNAV"].ToString()),
                    //                         }).ToList().OrderByDescending(r => (r.LatestNav - r.HistoricalNav))
                    //          }).ToList();

                    result = (from r in dataTable.AsEnumerable()
                              where r["Level"].ToString() == "1"
                              group r by new { SchemaCode = r["SchemaCode"].ToString(), SchemaName = r["SchemaName"].ToString() } into schemaGrp
                              select new FundPerformance
                              {
                                  LatestDate = dataTable.AsEnumerable().Select(r => Convert.ToDateTime(r["LatestDate"].ToString())).OrderBy(r => r.Date).First(),
                                  LatestNAV = dataTable.AsEnumerable().Select(r => Convert.ToDecimal(r["LatestNAV"].ToString())).First(),
                                  //HistoryDate = schemaGrp.Select(r => Convert.ToDateTime(r["HistoryDate"].ToString())).OrderByDescending(r => r.Date).First(),

                                  SchemaName = schemaGrp.Key.SchemaName,
                                  SchemaCode = schemaGrp.Key.SchemaCode,

                                  Performance = (from s in schemaGrp
                                                 select new FundHistory
                                                 {
                                                     Level = int.Parse(s["Level"].ToString()),
                                                     HistoryDate = Convert.ToDateTime(s["HistoryDate"].ToString()),
                                                     HistoryNAV = decimal.Parse(s["HistoryNAV"].ToString())
                                                 }).ToList()

                                  //Schemas = (from 
                                  //          //(from s in schemaGrp
                                  //           select new
                                  //           //{
                                  //               SchemaCode = schemaGrp.Key.SchemaCode,
                                  //               SchemaName = schemaGrp.Key.SchemaName,
                                  //               LatestNav = decimal.Parse(schemaGrp.First()["LatestNAV"].ToString()),
                                  //               LatestDate = decimal.Parse(schemaGrp.First()["LatestDate"].ToString()),

                                  //               HistoricalNav = decimal.Parse(s.Where(hn => hn["HistoryNAV"].ToString())),

                                  //           //}
                                  //           ).ToList().OrderByDescending(r => (r.LatestNav - r.HistoricalNav))
                              }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public void BackUpNAVData()
        {
            _mfDataAccess.BackUpNAVData();
        }


        //public void DownloadFundNavHistory(DateTime fromDate, DateTime toDate)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://portal.amfiindia.com/DownloadNAVHistoryReport_Po.aspx?tp=1&frmdt=" + fromDate.ToString("dd-MMM-yyyy") + "&todt=" + toDate.ToString("dd-MMM-yyyy"));


        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse(); // execute the request
        //    Stream resStream = response.GetResponseStream(); // we will read data via the response stream

        //    using (var reader = new StreamReader(resStream))
        //    {
        //        UpdateNAVHistoryData(reader.ReadToEnd());
        //    }
        //}

        //private void UpdateNAVHistoryData(string data)
        //{
        //    string[] navdata = data.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        //    List<NAVData> latestNavData = new List<NAVData>();
        //    decimal nav;
        //    for (int i = 0; i < navdata.Length; i++)
        //    {
        //        string[] result = navdata[i].Split(';');
        //        if (result.Length == 6 && result[0].Trim().ToLower() != "Scheme Code".ToLower() && decimal.TryParse(result[2].Trim(), out nav)
        //            //&& Convert.ToDateTime(result[7].Trim()).Date > DateTime.Now.AddDays(-7).Date
        //           )
        //        {
        //            try
        //            {
        //                NAVData funddata = new NAVData()
        //                {
        //                    SchemaCode = Convert.ToInt32(result[0].Trim()),
        //                    ISINGrowth = string.Empty,
        //                    ISINDivReinvestment = string.Empty,
        //                    SchemaName = string.Empty,
        //                    NAV = Convert.ToDecimal(result[2].Trim()),
        //                    Date = Convert.ToDateTime(result[5].Trim())
        //                };
        //                latestNavData.Add(funddata);
        //            }
        //            catch (Exception ex)
        //            {
        //            }
        //        }
        //    }
        //    _mfDataAccess.UpdateNAVHistory(latestNavData);
        //}

        public List<NAVData> GetFundsNAV(DateTime date)
        {
            return MapFundsNAV(_mfDataAccess.GetFundNav(date));
        }

        private List<NAVData> MapFundsNAV(DataTable dataTable)
        {
            List<NAVData> navData = null;
            try
            {
                navData = (from n in dataTable.AsEnumerable()
                           select new NAVData()
                           {
                               Date = Convert.ToDateTime(n["Date"].ToString()),
                               ISINDivReinvestment = string.Empty,
                               ISINGrowth = string.Empty,
                               NAV = Convert.ToDecimal(n["NAV"].ToString()),
                               SchemaCode = Convert.ToInt32(n["SchemaCode"].ToString()),
                           }).ToList();
            }
            catch (Exception ex)
            {
 
            }
            return navData;
        }



        public string GetLastProcessedDetails()
        {
            string date = _mfDataAccess.GetLastProcessedDetails();
            if (!string.IsNullOrWhiteSpace(date) && date.Length > 0)
            {
                return Convert.ToDateTime(date).ToString("MM/dd/yyyy");
            }
            return string.Empty;
        }
    }
}
