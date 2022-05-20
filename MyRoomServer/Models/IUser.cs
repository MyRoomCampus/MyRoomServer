namespace MyRoomServer.Models
{
    public interface IUser
    {
        /// <summary>
        /// 用户唯一Id
        /// </summary>
        string UniqueUserId { get; }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetUserInfo();
    }
}
