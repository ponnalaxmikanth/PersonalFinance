using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PersonlaFinance.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {
            //foreach (var file in files)
            //{
            //    if (file.ContentLength > 0)
            //    {
            //        var fileName = Path.GetFileName(file.FileName);
            //        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            //        file.SaveAs(path);
            //    }
            //}
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("Index");
        }
    }
}