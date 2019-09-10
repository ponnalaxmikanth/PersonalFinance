using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using BusinessEntities.Entities.MutualFunds;
using Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessAccess.MutualFunds
{
    public class DashboardRepository : IDashboardRepository
    {
        IDashboardDataAccess dashboardDataAccess;
        string _application = "BusinessAccess";
        string _component = "DashboardRepository";

        //public void SetPath(string path)
        //{
        //    dashboardDataAccess.SetPath(path);
        //}

        public DashboardRepository(IDashboardDataAccess dashbrdDataAccess)
        {
            dashboardDataAccess = dashbrdDataAccess;
        }

        public DashboardResponse GetDashboardData(DashboardRequest request)
        {
            DashboardResponse response = null;

            try
            {
                DataTable dtSipDetails = new DataTable();
                DataTable dtInvestments = new DataTable();
                DataTable dtInvByMonth = new DataTable();
                DataTable dtInvests = new DataTable();

                dtInvestments = dashboardDataAccess.GetInvestmentDetails(request).Tables[0];
                //dtInvestments = dashboardDataAccess.GetIndividualInvestments(request);
                dtSipDetails = dashboardDataAccess.GetUpcomingSipDetails(request).Tables[0];
                //dtInvByMonth = dashboardDataAccess.GetInvestmentsByMonth(request);

                dtInvests = dashboardDataAccess.GetIndividualInvestments(new DashboardIndividual()
                    {
                        FromDate = request.FromDate,
                        PortfolioId = request.PortfolioId.ToString(),
                        ToDate = request.ToDate,
                        Type = ""
                    }).Tables[0];

                response = MapDasbhoardResponse(request, dtInvestments, dtSipDetails, dtInvests);
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return response;
        }

        private DashboardResponse MapDasbhoardResponse(DashboardRequest request, DataTable dtInvestments, DataTable dtSipDetails, DataTable dtInvests)
        {
            DashboardResponse response = new DashboardResponse();

            try
            {
                response.Investments = MapInvestments(dtInvestments);

                if (dtSipDetails != null && dtSipDetails.Rows.Count > 0)
                {
                    response.SipDetails = (from dr in dtSipDetails.AsEnumerable()
                                           select new UpcomingSipDetails()
                                           {
                                               PortfolioId = Convert.ToInt32(dr["PortfolioId"].ToString()),
                                               PortfolioName = dr["Portfolio"].ToString(),

                                               NextSipDate = GetNextSipDate(Convert.ToInt32(dr["SIPDate"].ToString())
                                                                          , Convert.ToDateTime(dr["FromDate"].ToString())
                                                                          , Convert.ToDateTime(dr["ToDate"].ToString())),

                                               FundDetails = new FundDetails()
                                               {
                                                   FundName = dr["SchemaName"].ToString(),
                                                   NAV = Convert.ToDecimal(dr["CurrentNAV"].ToString()),
                                               },
                                               SipDate = Convert.ToInt32(dr["SIPDate"].ToString()),
                                               SipStartDate = Convert.ToDateTime(dr["FromDate"].ToString()),
                                               SipEndDate = Convert.ToDateTime(dr["ToDate"].ToString()),
                                               Investment = new SipInvestmnetDetails()
                                               {
                                                   SIPAmount = Convert.ToDecimal(dr["SIPAmount"].ToString()),
                                                   Investment = Convert.ToDecimal(dr["Amount"].ToString()),
                                                   AvgNAV = Convert.ToDecimal(dr["AvgNAV"].ToString()),
                                                   Dividend = Convert.ToDecimal(dr["Dividend"].ToString()),
                                                   Units = Convert.ToDecimal(dr["Units"].ToString()),
                                               },
                                           }).ToList().OrderBy(r => r.NextSipDate).ToList();

                    response.Overall = GetInvestments(GetDate(1), dtInvests);
                    response.YTD = GetInvestments(GetDate(2), dtInvests);
                    response.QTD = GetInvestments(GetDate(3), dtInvests);
                    response.MTD = GetInvestments(GetDate(4), dtInvests);

                    response.InvestGrowth = MapInvestGrowth(dtInvests);

                    dashboardDataAccess.Insert_mf_daily_tracker(request.PortfolioId, DateTime.Now, -1, response.Investments.Investment, response.Investments.CurrentValue, response.Investments.Profit);
                    dashboardDataAccess.Insert_mf_daily_tracker(request.PortfolioId, DateTime.Now, 1, response.MTD.Amount, response.MTD.CurrentValue, response.MTD.Profit);
                    dashboardDataAccess.Insert_mf_daily_tracker(request.PortfolioId, DateTime.Now, 3, response.QTD.Amount, response.QTD.CurrentValue, response.QTD.Profit);
                    dashboardDataAccess.Insert_mf_daily_tracker(request.PortfolioId, DateTime.Now, 12, response.YTD.Amount, response.YTD.CurrentValue, response.YTD.Profit);

                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }


            return response;
        }

        private List<investGrowth> MapInvestGrowth(DataTable dtInvests)
        {
            List<investGrowth> result = null;
            try
            {
                if (dtInvests != null && dtInvests.Rows.Count > 0)
                {
                    result = (from dr in dtInvests.AsEnumerable()
                              where dr["Type"].ToString() == "I"
                              group dr by new { date = dr["PurchaseDate"].ToString() } into dategrp
                              select new investGrowth()
                              {
                                  Amount = dategrp.Sum(r => decimal.Parse(r["Amount"].ToString())),
                                  CurrentValue = dategrp.Sum(r => decimal.Parse(r["CurrentValue"].ToString())),
                                  Date = DateTime.Parse(dategrp.Key.date),
                                  Profit = dategrp.Sum(r => decimal.Parse(r["CurrentValue"].ToString())) - dategrp.Sum(r => decimal.Parse(r["Amount"].ToString())),
                              }).ToList();

                    result = result.OrderBy(r => r.Date).ToList();

                    for (int r = 0; r < result.Count; r++)
                    {
                        if (r == 0)
                        {
                            result[r].CumulativeAmount = result[r].Amount;
                            result[r].CumulativeCurrentValue = result[r].CurrentValue;
                            result[r].CumulativeProfit = result[r].Profit;
                        }
                        else
                        {
                            result[r].CumulativeAmount = result[r - 1].CumulativeAmount + result[r].Amount;
                            result[r].CumulativeCurrentValue = result[r - 1].CumulativeCurrentValue + result[r].CurrentValue;
                            result[r].CumulativeProfit = result[r - 1].CumulativeProfit + result[r].Profit;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        private Investment GetInvestments(DateTime fromDate, DataTable dtInvests)
        {
            Investment invest = new Investment();
            try
            {
                if (dtInvests != null)
                {
                    var filterRecs = dtInvests.AsEnumerable().Where(r => Convert.ToDateTime(r["PurchaseDate"].ToString()) >= fromDate);

                    invest.Amount = decimal.Round(filterRecs.Where(r => r["Type"].ToString() == "I").Sum(r => Convert.ToDecimal(r["Amount"].ToString())), 2, MidpointRounding.AwayFromZero);
                    invest.CurrentValue = decimal.Round(filterRecs.Where(r => r["Type"].ToString() == "I").Sum(r => Convert.ToDecimal(r["CurrentValue"].ToString())), 2, MidpointRounding.AwayFromZero);

                    invest.Profit = invest.CurrentValue - invest.Amount;
                    invest.ProfitPer = decimal.Round(invest.Amount == 0 ? 0 : (invest.CurrentValue - invest.Amount) * 100 / invest.Amount, 2, MidpointRounding.AwayFromZero);

                    invest.Redeem = decimal.Round(filterRecs.Where(r => r["Type"].ToString() == "R").Sum(r => Convert.ToDecimal(r["RedeemAmount"].ToString())), 2, MidpointRounding.AwayFromZero);
                    invest.RedeemValue = decimal.Round(filterRecs.Where(r => r["Type"].ToString() == "R").Sum(r => Convert.ToDecimal(r["RedeemValue"].ToString())), 2, MidpointRounding.AwayFromZero);

                    invest.RedeemProfit = invest.RedeemValue - invest.Redeem;
                    invest.RedeemProfitPer = decimal.Round(invest.Amount == 0 ? 0 : (invest.RedeemValue - invest.Redeem) * 100 / invest.Redeem, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return invest;
        }

        private DateTime GetDate(int type)
        {
            DateTime dt = new DateTime(2008, 1, 1);
            switch (type)
            {
                case 1:
                    break;
                case 2:
                    //dt = new DateTime(DateTime.Now.Year, 1, 1);
                    dt = DateTime.Now.AddYears(-1).Date;
                    break;
                case 3:
                    //dt = new DateTime(DateTime.Now.Year, (3 * (int)Math.Ceiling(DateTime.Now.Month / 3.0)) - 2, 1);
                    dt = DateTime.Now.AddMonths(-3).Date;
                    break;
                case 4:
                    dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    break;
            }
            return dt;
        }

        private static DateTime GetNextSipDate(int sipDate, DateTime fromDate, DateTime toDate)
        {
            DateTime nextSipDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, sipDate);

            if (nextSipDate > toDate)
                nextSipDate = toDate;
            else if ((DateTime.Now.Date).Day > sipDate)
                nextSipDate = nextSipDate.AddMonths(1);

            //return (DateTime.Now.Date).Day > sipDate
            //                                                        ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, sipDate).AddMonths(1)
            //                                                        : new DateTime(DateTime.Now.Year, DateTime.Now.Month, sipDate);

            return nextSipDate;
        }

        private Investments MapInvestments(DataTable dtInvestments)
        {
            Investments response = new Investments();
            if (dtInvestments != null && dtInvestments.Rows.Count > 0)
            {
                dtInvestments.Columns.Add("Profit", typeof(decimal), "(NAV*Units) -(PurchaseNAV*Units)");
                dtInvestments.Columns.Add("CurrentValue", typeof(decimal), "(NAV*Units)");

                response.Dividend = decimal.Round(dtInvestments.AsEnumerable().Sum(r => Convert.ToDecimal(r["Dividend"].ToString())), 2, MidpointRounding.AwayFromZero);
                response.Investment = decimal.Round(dtInvestments.AsEnumerable().Sum(r => Convert.ToDecimal(r["Amount"].ToString())), 2, MidpointRounding.AwayFromZero);
                response.CurrentValue = decimal.Round(dtInvestments.AsEnumerable().Sum(r => Convert.ToDecimal(r["CurrentValue"].ToString())), 2, MidpointRounding.AwayFromZero);
                response.Profit = decimal.Round(dtInvestments.AsEnumerable().Sum(r => Convert.ToDecimal(r["Profit"].ToString())), 2, MidpointRounding.AwayFromZero);
                response.ProfitPer = decimal.Round((dtInvestments.AsEnumerable().Sum(r => Convert.ToDecimal(r["Profit"].ToString())) / response.Investment) * 100, 2, MidpointRounding.AwayFromZero);
            }
            return response;
        }

        private decimal CalculateProfitPer(decimal redeemamount, decimal redeemVal)
        {
            decimal retvalue = 0;
            try
            {
                if (redeemamount != 0)
                    retvalue = decimal.Round((redeemVal - redeemamount) * 100 / redeemamount, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return retvalue;
        }

        private decimal CalculateProfit(decimal redeemamount, decimal redeem)
        {
            decimal retvalue = 0;
            try
            {
                retvalue = decimal.Round(redeem - redeemamount, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return retvalue;
        }

        public List<InvestmentsByMonth> GetDashboardChartData(DashboardRequest request)
        {
            List<InvestmentsByMonth> result = null;
            try
            {
                result = MapDasbhoardChartResponse(dashboardDataAccess.GetInvestmentsByMonth(request).Tables[0]);
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        private List<InvestmentsByMonth> MapDasbhoardChartResponse(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return (from dr in dataTable.AsEnumerable()
                        group dr by new { date = dr["Date"].ToString() } into dategrp
                        select new InvestmentsByMonth()
                        {
                            Date = Convert.ToDateTime(dategrp.Key.date),
                            Investments = new Investments()
                            {
                                Investment = decimal.Round(dategrp.Sum(r => decimal.Parse(r["Amount"].ToString())), 2, MidpointRounding.AwayFromZero),
                                CurrentValue = decimal.Round(dategrp.Sum(r => decimal.Parse(r["CurrentValue"].ToString())), 2, MidpointRounding.AwayFromZero),

                                CurrentProfit = CalculateProfitPer(dategrp.Sum(r => decimal.Parse(r["Amount"].ToString())), dategrp.Sum(r => decimal.Parse(r["CurrentValue"].ToString()))),

                                //(dategrp.Sum(r => decimal.Parse(r["CurrentValue"].ToString())) - dategrp.Sum(r => decimal.Parse(r["Amount"].ToString()))) * 100 / 
                                //                 dategrp.Sum(r => decimal.Parse(r["Amount"].ToString())),

                                Dividend = decimal.Round(dategrp.Sum(r => decimal.Parse(r["Dividend"].ToString())), 2, MidpointRounding.AwayFromZero),
                                RedeemInvest = decimal.Round(dategrp.Sum(r => decimal.Parse(r["RedeemAmount"].ToString())), 2, MidpointRounding.AwayFromZero),
                                RedeemValue = decimal.Round(dategrp.Sum(r => decimal.Parse(r["RedeemValue"].ToString())), 2, MidpointRounding.AwayFromZero),

                                Profit = CalculateProfit(decimal.Round(dategrp.Sum(r => decimal.Parse(r["RedeemAmount"].ToString())), 2, MidpointRounding.AwayFromZero), decimal.Round(dategrp.Sum(r => decimal.Parse(r["RedeemValue"].ToString())), 2, MidpointRounding.AwayFromZero)),
                                ProfitPer = CalculateProfitPer(decimal.Round(dategrp.Sum(r => decimal.Parse(r["RedeemAmount"].ToString())), 2, MidpointRounding.AwayFromZero), decimal.Round(dategrp.Sum(r => decimal.Parse(r["RedeemValue"].ToString())), 2, MidpointRounding.AwayFromZero)),
                            }
                        }).ToList();
            }
            return null;
        }

        public Individual GetIndividualData(DashboardRequest request)
        {
            Individual response = null;

            try
            {
                DataTable dtInvestments = new DataTable();

                dtInvestments = dashboardDataAccess.GetInvestmentDetails(request).Tables[0];

                response.Investments = MapInvestments(dtInvestments);
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return response;
        }


        public List<Investments> GetIndividualInvestments(DashboardIndividual request)
        {
            return MapIndividualInvestments(request, dashboardDataAccess.GetInvestments(request).Tables[0]);
        }

        private List<Investments> MapIndividualInvestments(DashboardIndividual request, DataTable dataTable)
        {
            List<Investments> result = null;
            try
            {
                if (dataTable != null)
                {
                    result = (from dr in dataTable.AsEnumerable()
                              //where (dr["Type"].ToString() == "I")
                              select new Investments()
                              {
                                  Date = (dr["Type"].ToString() == "R") ? Convert.ToDateTime(dr["SellDate"].ToString()) : Convert.ToDateTime(dr["PurchaseDate"].ToString()),
                                  FundName = dr["FundName"].ToString(),
                                  Investment = decimal.Round(Convert.ToDecimal(dr["Amount"].ToString()), 2, MidpointRounding.AwayFromZero),
                                  CurrentValue = decimal.Round(Convert.ToDecimal(dr["CurrentValue"].ToString()), 2, MidpointRounding.AwayFromZero),
                                  Profit = GetProfitPer(dr),
                                  //RedeemInvest = decimal.Round(Convert.ToDecimal(dr["RedeemAmount"].ToString()), 2, MidpointRounding.AwayFromZero),

                                  AgePer = CalculatePercentAgeGrowth(dr),
                                  Type = dr["Type"].ToString()
                              }).ToList();

                    //var redeemTrans = (from dr in dataTable.AsEnumerable()
                    //                   where (dr["Type"].ToString() == "R")
                    //                   select new Investments()
                    //                   {
                    //                       Date = Convert.ToDateTime(dr["SellDate"].ToString()),
                    //                       FundName = dr["FundName"].ToString(),
                    //                       Investment = decimal.Round(Convert.ToDecimal(dr["RedeemAmount"].ToString()), 2, MidpointRounding.AwayFromZero),
                    //                       CurrentValue = decimal.Round(Convert.ToDecimal(dr["RedeemValue"].ToString()), 2, MidpointRounding.AwayFromZero),
                    //                       Profit = GetProfitPer(dr),
                    //                       RedeemInvest = decimal.Round(Convert.ToDecimal(dr["RedeemValue"].ToString()), 2, MidpointRounding.AwayFromZero),
                    //                       AgePer = CalculatePercentAgeGrowth(dr),
                    //                       Type = "R"
                    //                   });

                    //if (redeemTrans != null && redeemTrans.Count() > 0)
                    //    result = result.Union(redeemTrans).ToList();

                    //var sellTrans = (from dr in dataTable.AsEnumerable()
                    //                 where (dr["Type"].ToString() == "S")
                    //                 select new Investments()
                    //                 {
                    //                     Date = Convert.ToDateTime(dr["PurchaseDate"].ToString()),
                    //                     FundName = dr["FundName"].ToString(),
                    //                     Investment = decimal.Round(Convert.ToDecimal(dr["Amount"].ToString()), 2, MidpointRounding.AwayFromZero),
                    //                     Type = "S"
                    //                 });
                    //if (redeemTrans != null && redeemTrans.Count() > 0)
                    //    result = result.Union(sellTrans).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result.OrderByDescending(r => r.Date).ToList();
        }

        private decimal GetProfitPer(DataRow dr)
        {
            decimal result = 0;
            try
            {
                if (dr["Type"].ToString() == "I" || dr["Type"].ToString() == "R")
                {
                    result = (Convert.ToDecimal(dr["CurrentValue"].ToString()) - Convert.ToDecimal(dr["Amount"].ToString())) / Convert.ToDecimal(dr["Amount"].ToString());
                }
                //else if (dr["Type"].ToString() == "R")
                //{
                //    result = (Convert.ToDecimal(dr["RedeemValue"].ToString()) - Convert.ToDecimal(dr["RedeemAmount"].ToString())) / Convert.ToDecimal(dr["RedeemAmount"].ToString());
                //}
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return Math.Round(result * 100, 2, MidpointRounding.AwayFromZero);
        }

        private decimal CalculatePercentAgeGrowth(DataRow dr)
        {
            decimal result = 0;
            try
            {
                DateTime PurchaseDate = Convert.ToDateTime(dr["PurchaseDate"].ToString());
                DateTime SellDate = dr["Type"].ToString() == "I" ? DateTime.Now.Date : Convert.ToDateTime(dr["SellDate"].ToString());
                if (dr["Type"].ToString() == "I" || dr["Type"].ToString() == "R")
                {
                    double Amount = Convert.ToDouble(dr["Amount"].ToString());
                    double daysAge = 365 / (SellDate - PurchaseDate.Date).TotalDays;
                    double profit = Amount != 0 ? Convert.ToDouble(dr["CurrentValue"].ToString()) / Amount : 0;
                    result = (decimal)Math.Round((Math.Pow(profit, daysAge) - 1) * 100, 2, MidpointRounding.AwayFromZero);
                }
                //if (dr["Type"].ToString() == "I")
                //{
                //    double Amount = Convert.ToDouble(dr["Amount"].ToString());

                //    double daysAge = 365 / (DateTime.Now.Date - PurchaseDate.Date).TotalDays;
                //    double profit = Amount != 0 ? Convert.ToDouble(dr["CurrentValue"].ToString()) / Amount : 0;

                //    result = (decimal)Math.Round((Math.Pow(profit, daysAge) - 1) * 100, 2, MidpointRounding.AwayFromZero);
                //}
                //else if (dr["Type"].ToString() == "R")
                //{

                //    double Amount = Convert.ToDouble(dr["Amount"].ToString());

                //    double daysAge = 365 / (Convert.ToDateTime(dr["SellDate"].ToString()) - PurchaseDate.Date).TotalDays;
                //    double profit = Amount != 0 ? Convert.ToDouble(dr["CurrentValue"].ToString()) / Amount : 0;

                //    result = (decimal)Math.Round((Math.Pow(profit, daysAge) - 1) * 100, 2, MidpointRounding.AwayFromZero);
                //}
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        public List<SectorInvestments> GetSectorBreakup(DashboardRequest request)
        {
            return MapSectorBreakup(dashboardDataAccess.GetSectorBreakup(request).Tables[0]);
        }

        private List<SectorInvestments> MapSectorBreakup(DataTable dataTable)
        {
            List<SectorInvestments> result = null;

            try
            {
                result = (from dr in dataTable.AsEnumerable()
                          select new SectorInvestments()
                          {
                              Sector = dr["FundClass"].ToString(),
                              Amount = decimal.Round(Convert.ToDecimal(dr["Amount"].ToString()), 2, MidpointRounding.AwayFromZero),
                              CurrentValue = decimal.Round(Convert.ToDecimal(dr["currentValue"].ToString()), 2, MidpointRounding.AwayFromZero),
                              Profit = decimal.Round(Convert.ToDecimal(dr["Profit"].ToString()), 2, MidpointRounding.AwayFromZero),
                              //InvestPer = Convert.ToDecimal(dr["InvestPer"].ToString()),
                              //CurrentPer = Convert.ToDecimal(dr["CurrentPer"].ToString()),
                          }).ToList();
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;

        }

        public List<Investments> GetPerfOfMoreThanYear(DashboardIndividual request)
        {
            return MapPerfOfMoreThanYear(request, dashboardDataAccess.GetIndividualInvestments(request).Tables[0]);
        }

        private List<Investments> MapPerfOfMoreThanYear(DashboardIndividual request, DataTable dataTable)
        {
            List<Investments> result = null;
            try
            {
                result = (from dr in dataTable.AsEnumerable()
                          where (dr["Type"].ToString() == "I" && dr["PurchaseDate"] != DBNull.Value && DateTime.Parse(dr["PurchaseDate"].ToString()) < DateTime.Now.AddYears(-1))
                          select new Investments()
                          {
                              Date = Convert.ToDateTime(dr["PurchaseDate"].ToString()),
                              FundName = dr["FundName"].ToString(),
                              Investment = decimal.Round(Convert.ToDecimal(dr["Amount"].ToString()), 2, MidpointRounding.AwayFromZero),
                              CurrentValue = decimal.Round(Convert.ToDecimal(dr["CurrentValue"].ToString()), 2, MidpointRounding.AwayFromZero),
                              Profit = GetProfitPer(dr),
                              //RedeemInvest = decimal.Round(Convert.ToDecimal(dr["RedeemAmount"].ToString()), 2, MidpointRounding.AwayFromZero),
                              AgePer = CalculatePercentAgeGrowth(dr)
                          }).ToList();
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return result;
        }

        public ULIPValue GetULIP()
        {
            ULIPValue result = null;

            result = MapULIP(dashboardDataAccess.GetULIP().Tables[0]);
            return result;
        }

        private ULIPValue MapULIP(DataTable dataTable)
        {
            ULIPValue result = null;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                result = (from dr in dataTable.AsEnumerable()
                          select new ULIPValue()
                          {
                              CurrentValue = decimal.Parse(dr["CurrentValue"].ToString()),
                              Invest = decimal.Parse(dr["Invest"].ToString()),
                              NAV = decimal.Parse(dr["NAV"].ToString()),
                              Units = decimal.Parse(dr["Units"].ToString())
                          }).FirstOrDefault();

            }
            return result;
        }

        public List<BenchmarkHistory> GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate)
        {
            List<BenchmarkHistory> result = MapBenchMarkHistoryValues(dashboardDataAccess.GetBenchmarkHistoryValues(fromDate, toDate).Tables[0]);

            return result;
        }

        private List<BenchmarkHistory> MapBenchMarkHistoryValues(DataTable dataTable)
        {
            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return (from dr in dataTable.AsEnumerable()
                            group dr by new { benchMark = dr["BenchMark"].ToString() } into benchMarkgrp
                            select new BenchmarkHistory()
                            {
                                BenchMark = benchMarkgrp.Key.benchMark,
                                HistoryDetails = (from h in benchMarkgrp
                                                  select new BenchmarkDetails()
                                                  {
                                                      Close = Conversions.GetDecimalValue(h["CloseValue"]),
                                                      High = Conversions.GetDecimalValue(h["HighValue"]),
                                                      Low = Conversions.GetDecimalValue(h["LowValue"]),
                                                      Open = Conversions.GetDecimalValue(h["OpenValue"]),
                                                      TurnOver = Conversions.GetDecimalValue(h["TurnOver"]),
                                                      SharesTraded = Conversions.Getulong(h["SharesTraded"]),
                                                      Date = Conversions.ToDateTime(h["Date"], DateTime.Now)
                                                  }).ToList().OrderBy(r => r.Date).ToList()
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        public DataTable GetNewDashboard(DateTime fromDate, DateTime toDate)
        {
            return (dashboardDataAccess.GetNewGraph(fromDate, toDate).Tables[0]);
        }

        public object GetBenchMarks(DateTime fromDate, DateTime toDate)
        {
            return MapBenchMarks(dashboardDataAccess.GetBenchmarkPerformance(fromDate, toDate).Tables[0]);

        }

        private object MapBenchMarks(DataTable dataTable)
        {
            // List<BenchmarkPerformance> result = null;

            try
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return (from dr in dataTable.AsEnumerable()
                            group dr by new { benchMark = dr["BenchMark"].ToString() } into benchMarkgrp
                            select new
                          {
                              Benchmark = benchMarkgrp.Key.benchMark,
                              Latest = GetBenchmarkValues(benchMarkgrp.AsEnumerable(), "Latest"),
                              High = GetBenchmarkValues(benchMarkgrp.AsEnumerable(), "High"),
                              Low = GetBenchmarkValues(benchMarkgrp.AsEnumerable(), "Low"),
                          }).ToList();
                }
            }
            catch (Exception ex)
            {
                DBLogging.LogException(_application, _component, ex.Message, ex.StackTrace);
            }
            return null;
        }

        private object GetBenchmarkValues(IEnumerable<DataRow> enumerable, string p)
        {
            BenchmarkDetails result = null;

            result = (from b in enumerable
                      where b["Type"].ToString() == p
                      select new BenchmarkDetails()
                      {
                          Close = Conversions.GetDecimalValue(b["CloseValue"], -1),
                          High = Conversions.GetDecimalValue(b["CloseValue"], -1),
                          Low = Conversions.GetDecimalValue(b["CloseValue"], -1),
                          Open = Conversions.GetDecimalValue(b["CloseValue"], -1),
                          Date = DateTime.Parse(b["Date"].ToString())
                      }).FirstOrDefault();

            return result;
        }

    }
}
