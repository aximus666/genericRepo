using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Practice.Api.Controllers
{
    public class OrdersController : ApiController
    {
        [HttpGet]
        [Authorize]
        public List<string> List()
        {
            List<string> orders = new List<string>();

            orders.Add("Elma");
            orders.Add("Armut");
            orders.Add("Erik");

            return orders;
        }
    }
}
