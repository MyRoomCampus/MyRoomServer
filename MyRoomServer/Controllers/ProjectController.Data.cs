namespace MyRoomServer.Controllers
{
    public partial class ProjectController
    {
        public class ProjectPost
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string Name { get; set; } = null!;
        }

        public class ProjectPut
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string Name { get; set; } = null!;
        }
    }
}
