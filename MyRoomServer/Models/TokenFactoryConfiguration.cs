namespace MyRoomServer.Models
{
    public class TokenFactoryConfiguration
    {
        /// <summary>
        /// AccessToken有效期（分钟）
        /// </summary>
        public int AccessTokenExpire { get; set; } = 60;

        /// <summary>
        /// </summary>
        public int RefreshTokenExpire { get; set; } = 10080;

        /// <summary>
        /// 在RefreshToken过期前多久自动刷新RefreshToken
        /// </summary>
        public int RefreshTokenBefore { get; set; } = 1440;

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public string SigningKey { get; set; } = null!;
    }
}
