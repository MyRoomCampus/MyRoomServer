namespace MyRoomServer.Hubs
{
    public partial class ProjectHub
    {
        /// <summary>
        /// 连接的类型
        /// </summary>
        private enum ConnectionType
        {
            /// <summary>
            /// 一般客户
            /// </summary>
            User,

            /// <summary>
            /// 经纪人
            /// </summary>
            Admin
        }

        private record ConnectionInfo(ConnectionType Type, string ConnectionId, string UserName, ulong ProjectId);

        private class ProjectInfo
        {
            public string? AdminConnectionId { get; set; }

            public Dictionary<string, ConnectionInfo> ClientInfos { get; set; } = null!;

            public ProjectInfo(string? adminConnectionId, ConnectionInfo? info)
            {
                AdminConnectionId = adminConnectionId;
                ClientInfos = new Dictionary<string, ConnectionInfo>();
                if (info != null)
                {
                    AddClientInfo(info);
                }
            }

            public void AddClientInfo(ConnectionInfo info)
            {
                ClientInfos.Add(info.ConnectionId, info);
            }

            public void RemoveClientInfo(string connectionId)
            {
                ClientInfos.Remove(connectionId);
            }
        }
    }
}
