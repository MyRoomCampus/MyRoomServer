using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyRoomServer.Extentions
{
    public static class ControllerBaseExtention
    {
        public static string GetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
