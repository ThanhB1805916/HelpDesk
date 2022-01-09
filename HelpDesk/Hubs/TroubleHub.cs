using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HelpDesk.Hubs
{
    [HubName("TroubleHub")]
    public class TroubleHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }       

        //public static void Send(string mes)
        //{
        //    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TroubleHub>();
        //    context.Clients.All.SendAll(mes);
        //}

        public static void SentTrouble()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TroubleHub>();
            context.Clients.All.DisplayTrouble();
        }

        public static void NotifyFinish(int idReport, int idTrouble)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TroubleHub>();
            context.Clients.All.DisplayFinish(idReport, idTrouble);
        }

        
    }
}