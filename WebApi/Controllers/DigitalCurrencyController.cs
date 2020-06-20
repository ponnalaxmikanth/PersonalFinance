using BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DigitalCurrencyController : BaseController
    {
        DigitalCurrencyRepository dcRepository;

        public DigitalCurrencyController()
        {
            dcRepository = new DigitalCurrencyRepository();
        }

        [HttpGet]
        [Route("api/DigitalCurrency/GetInvestmentDetails")]
        public HttpResponseMessage GetCurrentInvestment()
        {
            return Request.CreateResponse(HttpStatusCode.OK, dcRepository.GetInvestmentDetails());
        }
    }
}
