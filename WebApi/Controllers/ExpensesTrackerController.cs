using BusinessAccess.HomeExpenses;
using BusinessEntity.Expenses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Controllers;

namespace WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ExpensesTrackerController : BaseController
    {
        readonly ExpensesTrackerRepository _expensesTrackerRepository;

        public ExpensesTrackerController()
        {
            _expensesTrackerRepository = new ExpensesTrackerRepository();
        }

        //private void setPath()
        //{
        //    string _path = System.Web.Hosting.HostingEnvironment.MapPath("~");
        //    _expensesTrackerRepository.SetPath(_path + "\\Data\\");
        //}

        [HttpGet]
        [Route("api/Expenses/GetAccountTypes")]
        public HttpResponseMessage GetAccountTypes()
        {
            //setPath();
            List<AccountType> result = _expensesTrackerRepository.GetAccountTypes();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/Expenses/GetAccountDetails")]
        public HttpResponseMessage GetAccountDetails()
        {
            List<AccountDetails> result = _expensesTrackerRepository.GetAccountDetails();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //[HttpGet]
        //[Route("api/Expenses/GetExpenseGroups")]
        //public HttpResponseMessage GetExpenseGroups()
        //{
        //    List<ExpenseGroup> result = _expensesTrackerRepository.GetExpenseGroups();
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        //[HttpGet]
        //[Route("api/Expenses/GetExpenseSubGroups")]
        //public HttpResponseMessage GetExpenseSubGroups()
        //{
        //    List<ExpenseSubGroup> result = _expensesTrackerRepository.GetExpenseSubGroups();
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //}

        [HttpGet]
        [Route("api/Expenses/GetItems/{item}")]
        public HttpResponseMessage GetItems(string item)
        {
            List<string> result = _expensesTrackerRepository.GetItems(item);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/Expenses/GetStores/{store}")]
        public HttpResponseMessage GetStores(string store)
        {
            List<string> result = _expensesTrackerRepository.GetStores(store);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("api/Expenses/AddExpense")]
        public HttpResponseMessage AddTransaction(ExpenseTransaction transaction)
        {
            Boolean result = _expensesTrackerRepository.AddExpenseTransaction(transaction);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("api/Expenses/GetExpenses")]
        public HttpResponseMessage GetTransaction(GetExpenses request)
        {
            List<Expenses> result = _expensesTrackerRepository.GetExpenses(request);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("api/Expenses/GetBudget")]
        public HttpResponseMessage GetBudget(GetExpenses request)
        {
            ExpenseTracker result = _expensesTrackerRepository.GetBudget(request);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("api/Expenses/GetExpensesChartData")]
        public HttpResponseMessage GetExpensesChartData()
        {
            List<ExpensesChartData> result = _expensesTrackerRepository.GetExpensesChartData();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
