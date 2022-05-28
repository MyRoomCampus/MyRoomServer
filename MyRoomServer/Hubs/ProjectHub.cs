using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using MyRoomServer.Extentions;
using MyRoomServer.Models;

namespace MyRoomServer.Hubs
{
    [Authorize(Policy = IdentityPolicyNames.CommonUser)]
    public partial class ProjectHub : Hub
    {
        /// <summary>
        /// 需要重复访问来确保缓存不会失效
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task SendVisit(long projectId)
        {
            SetConnectionInfo(
                Context.ConnectionId,
                new ConnectionInfo(ConnectionType.User, projectId));

            // TODO 线程不安全
            if (TryGetProjectInfo(projectId, out var info))
            {
                if (info.AdminConnectionId != null)
                {
                    // TODO 这里需要仔细考虑应该给 admin 哪些信息
                    await Clients.Client(info.AdminConnectionId).SendAsync(ReceiveMethods.ReceiveVisit, Context.ConnectionId);
                }
                info.ClientConnectionIds.Add(Context.ConnectionId);
                SetProjectInfo(projectId, info);
            }
            else
            {
                SetProjectInfo(projectId, new ProjectInfo(null, new HashSet<string> { Context.ConnectionId }));
            }
        }

        /// <summary>
        /// 项目管理者访问以获取该项目当前的访客
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task SendObserve(long projectId)
        {
            var identifier = Context.UserIdentifier!;
            var hasProject = (from item in dbContext.Projects
                              where item.Id == projectId
                              where item.UserId == Guid.Parse(identifier)
                              select item).Any();

            if (!hasProject)
            {
                // TODO 这里需要给用户通知吗？
                return;
            }

            SetConnectionInfo(
                Context.ConnectionId,
                new ConnectionInfo(ConnectionType.Admin, projectId));

            // TODO 线程不安全
            if (TryGetProjectInfo(projectId, out var info))
            {
                if (info.AdminConnectionId != null)
                {
                    // TODO 这里需要仔细考虑应该给 admin 哪些信息
                    await Clients.Client(info.AdminConnectionId).SendAsync(ReceiveMethods.ReceiveVisit, Context.ConnectionId);
                }
                info.ClientConnectionIds.Add(Context.ConnectionId);
                SetProjectInfo(projectId, info);
            }
            else
            {
                SetProjectInfo(projectId, new ProjectInfo(null, new HashSet<string> { Context.ConnectionId }));
            }
        }

        public async Task SendMessage(string message, string connectId)
        {
            var userName = this.GetUserName();
            await Clients.All.SendAsync(ReceiveMethods.ReceiveMessage, userName, message);
        }

        /// <summary>
        /// 协商是否进行此次通话
        /// </summary>
        /// <param name="user">客户的用户名</param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public async Task SendPreOffer(string user, string offerKey)
        {
            // todo 从 connectionId 获取 projectId
            // todo 从 project 的客户中找出 user 对应的connectionId
            // todo 发送消息
            throw new NotImplementedException();
        }

        /// <summary>
        /// 客户对是否进行此次通话进行应答
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="agree">是否同意进行通话</param>
        /// <returns></returns>
        public async Task SendPreAnswer(string offerKey, bool agree)
        {
            // 找出 connectionId 对应的 project
            // 从 project 找出对应的 经纪人 connectionId
            throw new NotImplementedException();
        }

        public async Task SendIceCandidate(string user, string candidate)
        {
            // todo 从 connectionId 获取 projectId
            // todo 从 project 的客户中找出 user 对应的connectionId

            // 
            await Clients.All.SendAsync(ReceiveMethods.ReceiceIceCandidate, user, candidate);
        }

        public async Task SendOffer(string user, string offer)
        {
            await Clients.All.SendAsync(ReceiveMethods.ReceiveOffer, offer);
        }

        public async Task SendAnswer(string user, string answer)
        {
            await Clients.All.SendAsync(ReceiveMethods.ReveiveAnswer, answer);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var ok = cache.TryGetValue(Context.UserIdentifier, out var user);
            if (ok)
            {
                cache.Remove(Context.UserIdentifier);
                cache.Remove(user);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
