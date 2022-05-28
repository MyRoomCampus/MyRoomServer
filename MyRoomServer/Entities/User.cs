using Microsoft.EntityFrameworkCore;
using MyRoomServer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRoomServer.Entities
{
    public record User : IUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; init; }

        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(64), MinLength(6)]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "char(64)")]
        public string Password { get; set; } = null!;

        /// <inheritdoc/>
        public string UniqueUserId => Id.ToString();

        /// <inheritdoc/>
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
