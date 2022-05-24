using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyRoomServer.Models;

namespace MyRoomServer.Hubs
{
    [Authorize(Policy = IdentityPolicyNames.CommonUser)]
    public class VideoHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendIceCandidate(string user, string candidate)
        {
            await Clients.All.SendAsync("ReceiveIceCandidate", user, candidate);
        }

        public async Task SendOffer(string user, string offer)
        {
            await Clients.All.SendAsync("ReceiveOffer", offer);
        }

        public async Task SendAnswer(string user, string offer)
        {
            await Clients.All.SendAsync("ReceiveAnswer", offer);
        }
    }
}
