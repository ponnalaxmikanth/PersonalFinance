using BusinessAccess.MutualFunds;
using BusinessEntities.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public partial class MutualFundsController : ApiController
    {
        MutualFundsRepository _mutualFundsRepository = new MutualFundsRepository();

        public MutualFundsController()
        {

        }

        [HttpGet]
        [Route("api/MutualFunds/GetFundHouses")]
        public HttpResponseMessage GetFundHouses()
        {
            List<MF_FundHouses> result = _mutualFundsRepository.GetFundHouses();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/MutualFunds/GetFundTypes")]
        public HttpResponseMessage GetFundTypes()
        {
            List<MF_FundTypes> result = _mutualFundsRepository.GetFundTypes();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/MutualFunds/GetFundCategory")]
        public HttpResponseMessage GetFundCategory()
        {
            List<MF_FundCategory> result = _mutualFundsRepository.GetFundCategory();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpGet]
        [Route("api/MutualFunds/GetFundOptions")]
        public HttpResponseMessage GetFundOptions()
        {
            List<MF_FundOptions> result = _mutualFundsRepository.GetFundOptions();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        [HttpGet]
        [Route("api/MutualFunds/GetFunds")]
        public HttpResponseMessage GetFunds()
        {
            List<MF_Funds> result = _mutualFundsRepository.GetFunds();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/MutualFunds/GetFolios")]
        public HttpResponseMessage GetFolios()
        {
            List<MF_Folios> result = _mutualFundsRepository.GetFolios();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/MutualFunds/GetPortfolios")]
        public HttpResponseMessage GetPortfolios()
        {
            List<MF_Portfolio> result = _mutualFundsRepository.GetPortfolios();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetMyFunds")]
        public HttpResponseMessage GetMyFunds(GetMyFundsRequest request)
        {
            List<MF_Funds> result = _mutualFundsRepository.GetMyFunds(request);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("api/MutualFunds/AddTransaction")]
        public HttpResponseMessage AddTransaction(AddMFTransactionRequest _mfTransactionRequest)
        {
            AddMFTransactionResponse response = _mutualFundsRepository.AddTransaction(_mfTransactionRequest);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetFundNav")]
        public HttpResponseMessage GetFundNav(GetFundNavRequest _getFundNavRequest)
        {
            decimal value = _mutualFundsRepository.GetFundNav(_getFundNavRequest);
            return Request.CreateResponse(HttpStatusCode.OK, value);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetFundValue")]
        public HttpResponseMessage GetFundValue(GetFundValueRequst _getFundValueRequest)
        {
            FundValueResponse response = _mutualFundsRepository.GetFundValue(_getFundValueRequest);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetMFFundInvestments")]
        public HttpResponseMessage GetMFFundInvestments(GetMFFundInvestmentsRequest _getMFFundInvestmentsRequest)
        {
            List<MFTransactions> response = _mutualFundsRepository.GetMFFundInvestments(_getMFFundInvestmentsRequest);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetMFDdailyTracker")]
        public HttpResponseMessage GetMFDdailyTracker(GetMFDailyTracker _request)
        {
            List<DailyMFTracker> response = _mutualFundsRepository.GetMFDdailyTracker(_request);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetFundTransactions")]
        public HttpResponseMessage GetFundTransactions(GetMFTransactions _getFundTransactions)
        {
            List<MF_Transactions> response = _mutualFundsRepository.GetFundTransactions(_getFundTransactions);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("api/MutualFunds/GetIndividualInvestments")]
        public HttpResponseMessage GetIndividualInvestments(DashboardIndividual request)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _mutualFundsRepository.GetIndividualInvestments(request));
        }

    }
}
