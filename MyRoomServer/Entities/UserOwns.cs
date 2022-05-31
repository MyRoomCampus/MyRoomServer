using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyRoomServer.Entities
{
    public class UserOwn
    {
        public ulong Id { get; set; }

        public ulong HouseId { get; set; }

        [ForeignKey(nameof(HouseId))]
        [JsonIgnore]
        public AgentHouse House { get; set; } = null!;

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public User User { get; set; } = null!;

        public ulong? ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        [JsonIgnore]
        public Project? Project { get; set; }
    }
}
