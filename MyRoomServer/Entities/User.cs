using Microsoft.EntityFrameworkCore;
using MyRoomServer.Models;
using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Entities
{
    public class User : IUser
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64)]
        public string NickName { get; set; } = null!;

        [EmailAddress]
        [MaxLength(64)]
        public string Email { get; set; } = null!;

        public long Phone { get; set; }

        [MaxLength(64)]
        public string Password { get; set; } = null!;

        public string UniqueUserId => Id.ToString();

        public IDictionary<string, string> GetUserInfo()
        {
            var result = new Dictionary<string, string>(8)
            {
                { nameof(Email), Email },
                { nameof(NickName), NickName }
            };

            return result;
        }
    }
}
