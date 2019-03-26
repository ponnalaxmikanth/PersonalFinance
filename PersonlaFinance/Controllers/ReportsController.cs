using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PersonlaFinance.Controllers
{
    public class ReportsController : BaseController
    {
        public ActionResult Index()
        {
            //return View("~/Views/MutualFunds/Dashboard/Index.cshtml");
            return View();
        }

        public ActionResult History()
        {
            return View("~/Views/MutualFunds/Dashboard/History/HistoryView.cshtml");
        }

        public ActionResult GetBenchMarkHistory()
        {
            return View("~/Views/MutualFunds/Dashboard/Benchmark/Index.cshtml");
        }
    }
}
