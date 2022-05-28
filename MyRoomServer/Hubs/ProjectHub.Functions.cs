using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using MyRoomServer.Entities.Contexts;

namespace MyRoomServer.Hubs
{
    public partial class ProjectHub : Hub
    {
        private readonly IMemoryCache cache;
        private readonly MyRoomDbContext dbContext;

        /// <summary>
        /// 客户端用于接收消息的方法
        /// </summary>
        private static class ReceiveMethods
        {
            public const string ReceiveVisit = nameof(ReceiveVisit);
            public const string ReceiveMessage = nameof(ReceiveMessage);
            public const string ReceivePreoffer = nameof(ReceivePreoffer);
            public const string ReceivePreanswer = nameof(ReceivePreanswer);
            public const string ReceiceIceCandidate = nameof(ReceiceIceCandidate);
            public const string ReceiveOffer = nameof(ReceiveOffer);
            public const string ReveiveAnswer = nameof(ReveiveAnswer);
        }

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

        private record ConnectionInfo(ConnectionType Type, long ProjectId);

        private record ProjectInfo(string? AdminConnectionId, HashSet<string> ClientConnectionIds);

        /// <summary>
        /// 用于缓存的键
        /// </summary>
        private static class CacheKeys
        {
            const string KeyConnectionInfo = $"_{nameof(KeyConnectionInfo)}_";
            const string KeyProjectInfo = $"_${nameof(KeyProjectInfo)}_";

            public static string GetConnectionInfo(string connectionId)
            {
                return $"{KeyConnectionInfo}{connectionId}";
            }

            public static string GetProjectInfo(long projectId)
            {
                return $"{KeyProjectInfo}{projectId}";
            }
        }

        private void SetConnectionInfo(string connectionId, ConnectionInfo info)
        {
            cache.Set(CacheKeys.GetConnectionInfo(connectionId), info, new TimeSpan(0, 10, 0));
        }

        private bool TryGetConnectionInfo(string connectionId, out ConnectionInfo value)
        {
            return cache.TryGetValue(CacheKeys.GetConnectionInfo(connectionId), out value);
        }

        private void SetProjectInfo(long projectId, ProjectInfo info)
        {
            cache.Set(CacheKeys.GetProjectInfo(projectId), info, new TimeSpan(0, 10, 0));
        }

        private bool TryGetProjectInfo(long projectId, out ProjectInfo info)
        {
            return cache.TryGetValue(CacheKeys.GetProjectInfo(projectId), out info);
        }

        public ProjectHub(IMemoryCache cache, MyRoomDbContext dbContext)
        {
            this.cache = cache;
            this.dbContext = dbContext;
        }
    }
}
