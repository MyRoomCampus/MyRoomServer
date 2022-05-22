using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MyRoomServer.Hubs
{
    public class VideoHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task VideoOffer(string connectId, string offer)
        {
            await Clients.Caller.SendAsync("VideoOfferCall", offer);
            //await Clients.Client(connectId).SendAsync("VideoOfferCall", offer);
        }
    }
}
