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
    //[Transaction(Web = true)]
    //[Trace]
    public class BaseController : ApiController
    {
        //NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction;
        //transaction.AddCustomAttribute("Discount Code", "Summer Super Sale");

        //ITransaction transaction = agent;

        //IAgent agent = NewRelic.Api.Agent.NewRelic.GetAgent();
        //ITransaction transaction = agent.CurrentTransaction;

        //var logger = LogManager.GetLogger("NewRelicLog");
        ////logger.Info("Hello, New Relic!");
        //Logger.Debug("Hello {FirstName}, you are number {nbr} on my list", "Bob", 32);


        public void newRelicLog()
        {
            //transaction.AddCustomAttribute("Discount Code", "Summer Super Sale");
        }
    }
}
