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
        public long Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// 获取传输对象
        /// </summary>
        public object TransferData => new
        {
            Id, Name, CreatedAt
        };

        /// <summary>
        /// 绑定用户
        /// </summary>
        /// <param name="guid">用户Id</param>
        public void BindUser(Guid guid)
        {
            UserId = guid;
        }

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
