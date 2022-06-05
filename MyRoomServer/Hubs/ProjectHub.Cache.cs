using Microsoft.Extensions.Caching.Memory;

namespace MyRoomServer.Hubs
{
    public partial class ProjectHub
    {
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

        private ConnectionInfo? GetConnectionInfo(string connectionId)
        {
            return cache.Get(CacheKeys.GetConnectionInfo(connectionId)) as ConnectionInfo;
        }

        private void SetProjectInfo(ulong projectId, in ProjectInfo info)
        {
            cache.Set(CacheKeys.GetProjectInfo(projectId), info, new TimeSpan(0, 10, 0));
        }

        private ProjectInfo? GetProjectInfo(ulong projectId)
        {
            return cache.Get(CacheKeys.GetProjectInfo(projectId)) as ProjectInfo;
        }

        private ProjectInfo? RemoveConnectionInfo(string connectionId)
        {
            var connectionInfo = GetConnectionInfo(connectionId);
            if (connectionInfo == null)
            {
                return null;
            }
            cache.Remove(CacheKeys.GetConnectionInfo(connectionId));

            var projectInfo = GetProjectInfo(connectionInfo.ProjectId);
            if (projectInfo == null)
            {
                return null;
            }

            if (connectionInfo.Type == ConnectionType.Admin)
            {
                projectInfo.AdminConnectionId = null;
                SetProjectInfo(connectionInfo.ProjectId, projectInfo);
                return null;
            }
            else
            {
                projectInfo.ClientInfos.Remove(connectionId);
                SetProjectInfo(connectionInfo.ProjectId, projectInfo);
                return projectInfo;
            }
        }
    }
}
