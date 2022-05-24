using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MyRoomServer.Entities;

namespace MyRoomServer.Extentions
{
    public static class HubExtention
    {
        public static string GetUserName(this Hub hub)
        {
            var userId = hub.Context.User!.FindFirst(nameof(User.UserName))!.Value;
            return userId;
        }
    }
}
