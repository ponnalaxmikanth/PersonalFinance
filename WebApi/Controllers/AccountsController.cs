using BusinessAccess.Accounts;
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
        public AccountsController()
        {
            _accountsRepository = new AccountsRepository();
        }

        [HttpGet]
        [Route("api/Accounts/GetAccountStatusDetails")]
        public HttpResponseMessage GetAccountStatusDetails()
        {
            AccountsCurrentStatus result = _accountsRepository.GetAccountStatusDetails();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
