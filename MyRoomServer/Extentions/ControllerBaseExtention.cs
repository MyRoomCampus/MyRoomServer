using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyRoomServer.Extentions
{
    public static class ControllerBaseExtention
    {
        public static string GetUserId(this ControllerBase controller)
        {
            var userId = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new NullReferenceException("Couldn't found user's id claim.");
            }
            return userId;
        }
    }
}
