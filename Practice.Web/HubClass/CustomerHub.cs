using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Practice.Web.HubClass
{
    [HubName("CustomerHub")]
    public class CustomerHub : Hub
    {
        public void ShowCustomer()
        {
            Clients.All.showallcustomer();
        }
    }
}