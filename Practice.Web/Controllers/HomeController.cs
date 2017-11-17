using Practice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practice.Web.Controllers
{
    public class HomeController : Controller
    {
        IList<CustomerVM> customers;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetList()
        {
            customers = new List<CustomerVM>();
            for (int i = 0; i < 5; i++)
            {
                customers.Add(new CustomerVM { Name = $"C{i}", Surname = $"S{i}" });
            }
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddNewCustomer()
        {
            customers.Add(new CustomerVM { Name = "Client Customer", Surname = "Clientt" });
            return Json(customers, JsonRequestBehavior.AllowGet);
        }
    }
}