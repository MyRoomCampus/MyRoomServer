namespace MyRoomServer.Models
{
    public interface IUser
    {
        string UniqueUserId { get; }
        IDictionary<string, string> GetUserInfo();
    }
}
