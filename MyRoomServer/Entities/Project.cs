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
        public ulong Id { get; init; }

        [JsonIgnore]
        [ForeignKey(nameof(Id))]
        public AgentHouse House { get; init; } = null!;

        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonPropertyName("projectName")]
        public string Name { get; set; } = null!;

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
