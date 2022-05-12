namespace MyRoomServer.Models
{
    public enum CaptchaType
    {
        /// <summary>
        /// 注册
        /// </summary>
        Registry = 1,

        /// <summary>
        /// 重置密码
        /// </summary>
        Restore = 2,

        /// <summary>
        /// 注销
        /// </summary>
        cancellation = 3,
    }
}
