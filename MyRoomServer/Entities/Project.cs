using MyRoomServer.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRoomServer.Entities
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public record Project : IAccessData<Project>
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; } = null!;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// 获取传输对象
        /// </summary>
        public object TransferData => new
        {
            Id,
            Name,
            CreatedAt
        };

        /// <summary>
        /// 更新对象信息
        /// </summary>
        /// <param name="obj">新的对象</param>
        public void Update(Project obj)
        {
            obj.Name = Name;
        }
    }
}
