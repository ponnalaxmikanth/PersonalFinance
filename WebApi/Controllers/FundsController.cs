using BusinessAccess.MutualFunds;
using BusinessEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi.Controllers
{
    
    public partial class FundsController : BaseController
    {
        MutualFundsRepository _mutualFundsRepository = new MutualFundsRepository();

        [HttpGet]
        [Route("api/Funds/GetFundsInvestmentDetails")]
        public HttpResponseMessage GetFundsInvestmentDetails() {
            InvestmentDetails result = _mutualFundsRepository.GetInvestmentDetails();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
