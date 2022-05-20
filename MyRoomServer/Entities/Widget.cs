using MyRoomServer.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRoomServer.Entities
{
    /// <summary>
    /// 小组件
    /// </summary>
    public class Widget : IAccessData<Widget>
    {
        /// <summary>
        /// 组件Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; init; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; } = null!;

        /// <summary>
        /// 横坐标
        /// </summary>
        public long Abscissa { get; set; }

        /// <summary>
        /// 纵坐标
        /// </summary>
        public long Ordinate { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public long Width { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Data { get; set; } = null!;

        /// <inheritdoc/>
        public object TransferData => new
        {
            Id,
            Abscissa,
            Ordinate,
            Length,
            Width,
            Data
        };

        /// <inheritdoc/>
        public void Update(Widget obj)
        {
            Abscissa = obj.Abscissa;
            Ordinate = obj.Ordinate;
            Length = obj.Length;
            Width = obj.Width;
            Data = obj.Data;
        }
    }
}
