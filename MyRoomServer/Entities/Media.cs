using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRoomServer.Entities
{
    public class Media
    {
        /// <summary>
        /// 资源标识
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 资源大小
        /// </summary>
        [Required]
        public int Size { get; set; }

        /// <summary>
        /// 资源内容
        /// </summary>
        [Required]
        public byte[] Content { get; set; } = null!;

        /// <summary>
        /// 更新时间
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 资源类型，例如 "image/png"
        /// </summary>
        public string Type { get; set; } = null!;
    }
}
