using MyRoomServer.Entities;
using System.Text.Json.Serialization;

namespace MyRoomServer.Controllers
{
    public partial class ProjectController
    {
        public class TransferProject
        {
            /// <summary>
            /// 房产Id（此房产信息属于该用户且未建立项目）
            /// </summary>
            public ulong HouseId { get; init; }

            /// <summary>
            /// 项目名称
            /// </summary>
            public string Name { get; set; } = null!;

            /// <summary>
            /// 组件数据
            /// </summary>
            public string? Data { get; set; }
        }
    }
}
