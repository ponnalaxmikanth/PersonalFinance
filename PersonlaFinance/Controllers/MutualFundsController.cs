using BusinessEntities.Contracts;
using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace PersonlaFinance.Controllers
{
    public class MutualFundsController : BaseController
    {
        IMutualFundsBusinessAccess _mutualBusinessAccess;
        public MutualFundsController(IMutualFundsBusinessAccess mutualBusinessAccess)
        {
            _mutualBusinessAccess = mutualBusinessAccess;
        }
        //
        // GET: /MutualFunds/

        public ActionResult Index()
        {
            //_mutualBusinessAccess.GetFundsPerformance();
            return View();
        }

        public JsonResult GetPortfolios()
        {
            return Json(_mutualBusinessAccess.GetMutualFundPortfolios(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactions(GetMFTransactions getMFTransactions)
        {
            //getMFTransactions = new GetMFTransactions()
            //{
            //     PortfolioId = 1,
            //      FromDate = new DateTime(2012,1,9),
            //     ToDate = new DateTime(2012, 1, 9)
            //};
            return Json(_mutualBusinessAccess.GetPortfolioTransactions(getMFTransactions), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFundTransactions(GetMFTransactions getMFTransactions)
        {
            return Json(_mutualBusinessAccess.GetFundTransactions(getMFTransactions), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMutualFundData()
        {
            return Json(_mutualBusinessAccess.GetMFFundDetails(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddFundTransaction(AddMFTransactionRequest _mfTransactionRequest)
        {
            return Json(_mutualBusinessAccess.AddUpdateMFTransaction(_mfTransactionRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDividend(AddDividendRequest _mfDividendRequest)
        {
            _mutualBusinessAccess.AddDividend(_mfDividendRequest);

            return Json("Y", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFundPerformances()
        {
            return Json(_mutualBusinessAccess.GetFundsPerformance(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLastProcessedDetails()
        {
            return Json(_mutualBusinessAccess.GetLastProcessedDetails(), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult DownloadFundNavHistory(DateTime fromDate, DateTime toDate)
        //{
        //    _mutualBusinessAccess.DownloadFundNavHistory(fromDate, toDate);
        //    return Json("Y");
        //}

    }
}
