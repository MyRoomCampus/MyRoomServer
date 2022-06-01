using Microsoft.Extensions.Caching.Memory;

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

        private record ProjectInfo(string? AdminConnectionId, Dictionary<string, ConnectionInfo> ClientInfos);

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

            public static string GetProjectInfo(ulong projectId)
            {
                return $"{KeyProjectInfo}{projectId}";
            }
        }

        private void SetConnectionInfo(string connectionId, in ConnectionInfo info)
        {
            cache.Set(CacheKeys.GetConnectionInfo(connectionId), info, new TimeSpan(0, 10, 0));
        }

        private bool TryGetConnectionInfo(string connectionId, out ConnectionInfo value)
        {
            return cache.TryGetValue(CacheKeys.GetConnectionInfo(connectionId), out value);
        }

        private void RemoveConnectionInfo(string connectionId)
        {
            var hasConnectionInfo = TryGetConnectionInfo(connectionId, out var connectionInfo);
            if (!hasConnectionInfo)
            {
                return;
            }
            cache.Remove(CacheKeys.GetConnectionInfo(connectionId));
            var hasProjectInfo = TryGetProjectInfo(connectionInfo.ProjectId, out var projectInfo);
            if (connectionInfo.Type == ConnectionType.Admin)
            {
                SetProjectInfo(connectionInfo.ProjectId, projectInfo with { AdminConnectionId = null });

            }
            else
            {
                projectInfo.ClientInfos.Remove(connectionId);
                SetProjectInfo(connectionInfo.ProjectId, projectInfo);
            }
        }

        private void SetProjectInfo(ulong projectId, in ProjectInfo info)
        {
            cache.Set(CacheKeys.GetProjectInfo(projectId), info, new TimeSpan(0, 10, 0));
        }

        private bool TryGetProjectInfo(ulong projectId, out ProjectInfo info)
        {
            return cache.TryGetValue(CacheKeys.GetProjectInfo(projectId), out info);
        }
    }
}
