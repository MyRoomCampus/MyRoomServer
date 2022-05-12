using MyRoomServer.Models;

namespace MyRoomServer.Services
{
    public interface ITokenFactory
    {
        int RefreshTokenExpireBefore { get; }
        string CreateAccessToken(IUser user);
        string CreateRefreshToken(IUser user);
    }
}
