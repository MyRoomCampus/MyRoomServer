using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Entities
{
    public class Widget
    {
        [Key]
        public long Id { get; set; }

        public long ProjectId { get; set; }

        public string Name { get; set; }

    }
}
