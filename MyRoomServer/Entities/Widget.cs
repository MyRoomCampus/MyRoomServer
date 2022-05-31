using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyRoomServer.Entities
{
    /// <summary>
    /// 小组件
    /// </summary>
    public class Widget
    {
        /// <summary>
        /// 组件Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public long Id { get; init; }

        /// <summary>
        /// 项目Id
        /// </summary>
        [JsonIgnore]
        public long ProjectId { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        [ForeignKey("ProjectId")]
        [JsonIgnore]
        public Project Project { get; } = null!;

        /// <summary>
        /// 组件类型
        /// </summary>
        [MaxLength(64)]
        public string Type { get; set; } = null!;

        /// <summary>
        /// 组件名称
        /// </summary>
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 项目内组件 ID
        /// </summary>
        public ulong CurrentId { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        [MaxLength(256)]
        public string Data { get; set; } = null!;

        /// <summary>
        /// 组件样式
        /// </summary>
        public string Style { get; set; } = null!;
    }
}
