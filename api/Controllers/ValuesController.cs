using BusinessAccess;
using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]DateTime? fromdate)
        {
            //DateTime? fromdate = DateTime.Now.AddDays(-30).Date;
            DateTime? todate = DateTime.Now.Date;

            StocksBusinessAccess fs = new StocksBusinessAccess();
            List<StockPurchases> lsp = new List<StockPurchases>();
            lsp = fs.ToGetStocks(fromdate == null ? DateTime.Now.AddDays(-30).Date : fromdate.Value, todate == null ? DateTime.Now.Date : todate.Value);
            return Request.CreateResponse(HttpStatusCode.Created, lsp);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}