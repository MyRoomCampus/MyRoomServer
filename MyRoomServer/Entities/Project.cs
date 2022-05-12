using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Entities
{
    public class Project
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
