using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


public class OrderNotificationHub : Hub
{
    public async Task SendOrderUpdate(int pendingOrders)
    {
        await Clients.All.SendAsync("ReceiveOrderUpdate", pendingOrders);
    }
}


