namespace MyRoomServer.Models
{
    public class QiniuConfiguration
    {
        public string AccessKey { get; set; } = null!;

        public string SecretKey { get; set; } = null!;
        
        public string Bucket { get; set; } = null!;
    }
}
