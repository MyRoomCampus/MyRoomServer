using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyRoomServer.Entities
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class Project
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("projectId")]
        public long Id { get; init; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonPropertyName("projectName")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonPropertyName("author")]
        public Guid UserId { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; } = null!;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// 组件数据
        /// </summary>
        public List<Widget> Data { get; set; } = null!;
    }
}
