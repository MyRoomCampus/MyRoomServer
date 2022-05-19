using MyRoomServer.Models;
using System.ComponentModel.DataAnnotations;

namespace MyRoomServer.Entities
{
    public class User : IUser
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64), MinLength(6)]
        public string UserName { get; set; } = null!;

        [MaxLength(64)]
        public string Password { get; set; } = null!;

        public string UniqueUserId => Id.ToString();

        public IDictionary<string, string> GetUserInfo()
        {
            var result = new Dictionary<string, string>(8)
            {
                { nameof(UserName), UserName }
            };

            return result;
        }
    }
}
