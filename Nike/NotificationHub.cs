using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nike
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        public static void SendNoti(String message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.notiOrderPage(message);
        }
    }
}