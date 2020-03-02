using BusinessAccess;
using BusinessEntities.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class IncomeController : BaseController
    {
        readonly IncomeRepository incomeRepository;

        public IncomeController()
        {
            incomeRepository = new IncomeRepository();
        }

        [HttpPost]
        [Route("api/Income/GetIncomeDetails")]
        public HttpResponseMessage GetIncomeDetails(IncomeDetailsRequest request)
        {
            //setPath();
            List<Income> result = incomeRepository.GetIncomeDetails(request);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
