using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRoomServer.Entities
{
    public class UserClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ClaimType { get; set; } = null!;

        [Required]
        public string ClaimValue { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User User { get; } = null!;

        public Guid UserId { get; set; }
    }
}