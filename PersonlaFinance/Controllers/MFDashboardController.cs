using BusinessEntities.Contracts.MutualFunds;
using BusinessEntities.Entities;
using BusinessEntities.Entities.MutualFunds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonlaFinance.Controllers
{
    public class MFDashboardController : BaseController
    {
        IDashboardRepository dashboardRepository;
        //string path = string.Empty;

        public MFDashboardController(IDashboardRepository dashbrdrepository)
        {
            dashboardRepository = dashbrdrepository;
        }

        //private void setPath() {
        //    string _path = Server.MapPath("");
        //    dashboardRepository.SetPath(_path + "\\Data\\");
        //}

        //
        // GET: /MFDashboard/
        public ActionResult Index()
        {
            //string _path = Server.MapPath("");
            //dashboardRepository.SetPath(_path + "\\Data\\");
            return View("~/Views/MutualFunds/Dashboard/Index.cshtml");
        }

        public PartialViewResult GetView(DashboardRequest request)
        {
            ViewBag.PortfolioId = request.PortfolioId;
            string postsHtml = string.Empty;
            if(request.PortfolioId == -1)
                //postsHtml = viewren.RenderPartialView("~/Views/MutualFunds/Dashboard/_DashboardView.cshtml");
                return PartialView("~/Views/MutualFunds/Dashboard/_DashboardView.cshtml");
            else
                return PartialView("~/Views/MutualFunds/Dashboard/Individual/_Index.cshtml");

            //return Json(new { html = postsHtml });
        }

        public ActionResult GetDashboardData(DashboardRequest request)
        {
            ViewBag.PortfolioId = request.PortfolioId;
            //setPath();
            return Json(dashboardRepository.GetDashboardData(request));
        }

        public ActionResult GetDashboardChartData(DashboardRequest request)
        {
            return Json(dashboardRepository.GetDashboardChartData(request));
        }

        public ActionResult GetIndividualInvestments(DashboardIndividual request)
        {
            ViewBag.PortfolioId = request.PortfolioId;
            return Json(dashboardRepository.GetIndividualInvestments(request));
        }

        public ActionResult GetPerfOfMoreThanYear(DashboardIndividual request)
        {
            return Json(dashboardRepository.GetPerfOfMoreThanYear(request));
        }

        public ActionResult GetSectorBreakup(DashboardRequest request)
        {
            return Json(dashboardRepository.GetSectorBreakup(request));
        }

        //public ActionResult GetULIPValue()
        //{
        //    return Json(dashboardRepository.GetULIP());
        //}

        public ActionResult History()
        {
            return View("~/Views/MutualFunds/Dashboard/History/HistoryView.cshtml");
            //return View("~/Views/MutualFunds/Dashboard/History/Index.cshtml");
        }
        
        public ActionResult GetBenchmarkHistoryValues(DateTime fromDate, DateTime toDate)
        {
            return Json(dashboardRepository.GetBenchmarkHistoryValues(fromDate, toDate));
        }

        public ActionResult GetGraphHistory(DateTime fromDate,DateTime toDate)
        { 
            return Json(dashboardRepository.GetNewDashboard(fromDate,toDate));
        }

    }
}
