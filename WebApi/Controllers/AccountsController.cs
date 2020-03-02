using BusinessAccess.Accounts;
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
    public class AccountsController : BaseController
    {
        readonly AccountsRepository _accountsRepository;
        readonly IncomeRepository incomeRepository;

        public AccountsController()
        {
            _accountsRepository = new AccountsRepository();
            incomeRepository = new IncomeRepository();
        }

        [HttpGet]
        [Route("api/Accounts/GetAccountStatusDetails")]
        public HttpResponseMessage GetAccountStatusDetails()
        {
            AccountsCurrentStatus result = _accountsRepository.GetAccountStatusDetails();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("api/Home/AddIncome")]
        public HttpResponseMessage AddTransaction(Income incomeDetails)
        {
            Boolean result = incomeRepository.AddIncome(incomeDetails);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
