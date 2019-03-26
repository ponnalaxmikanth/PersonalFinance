using BusinessAccess;
using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PersonalAPI.Controllers
{
    public class StocksController : ApiController
    {
        //[Route("api/GetStocks/")]
        [HttpGet, HttpPost]
        //[RoutePrefix("api/Dummy")]
        public HttpResponseMessage Post(DateTime? fromdate)
        {
            //DateTime? fromdate = DateTime.Now.AddDays(-30).Date;
            DateTime? todate = DateTime.Now.Date;

            StocksBusinessAccess fs = new StocksBusinessAccess();
            List<StockPurchases> lsp = new List<StockPurchases>();
            lsp = fs.ToGetStocks(fromdate == null ? DateTime.Now.AddDays(-30).Date : fromdate.Value, todate == null ? DateTime.Now.Date : todate.Value);
            return Request.CreateResponse(HttpStatusCode.Created, lsp);
            //return Ok(lsp);

            //return "Y";
        }
    }
}
