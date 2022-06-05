using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
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
        /// <param name="houseId"></param>
        /// <returns></returns>
        public async Task SendVisit(ulong houseId)
        {
            var connectionInfo = new ConnectionInfo(ConnectionType.User, Context.ConnectionId, this.GetUserName(), houseId);

            SetConnectionInfo(Context.ConnectionId, in connectionInfo);

            // TODO 线程不安全
            // 当 project 信息不存在时, 新建 project, 当 admin 在线时, 给 admin 发送上线消息
            if (TryGetProjectInfo(houseId, out var info))
            {
                if (info.AdminConnectionId != null)
                {
                    await Clients.Caller.SendAsync(ReceiveMethods.ReceiveDebug, $"ConnectionId: {Context.ConnectionId}, admin is online.");
                    await SendVisitToClient(info);
                }
                else
                {
                    await Clients.Caller.SendAsync(ReceiveMethods.ReceiveDebug, $"ConnectionId: {Context.ConnectionId}, admin is offline.");
                }
                info.ClientInfos.Add(Context.ConnectionId, connectionInfo);
            }
            else
            {
                info = new ProjectInfo(null, new Dictionary<string, ConnectionInfo> { { Context.ConnectionId, connectionInfo } });
            }
            SetProjectInfo(houseId, in info);
        }

        /// <summary>
        /// 项目管理者访问以获取该项目当前的访客
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        public async Task SendObserve(ulong houseId)
        {
            var uid = Guid.Parse(Context.UserIdentifier!);
            var hasProject = (from item in dbContext.UserOwns
                              where item.HouseId == houseId
                              where item.UserId == uid
                              select item).Any();

            if (!hasProject)
            {
                await Clients.Caller.SendAsync(ReceiveMethods.ReceiveDebug, $"ConnectionId: {Context.ConnectionId}, don't have the specific project.");
                return;
            }

            var connectionInfo = new ConnectionInfo(ConnectionType.Admin, Context.ConnectionId, this.GetUserName(), houseId);
            SetConnectionInfo(Context.ConnectionId, in connectionInfo);

            // TODO 线程不安全
            if (TryGetProjectInfo(houseId, out var info))
            {
                info = info with { AdminConnectionId = Context.ConnectionId };
            }
            else
            {
                info = new ProjectInfo(Context.ConnectionId, new Dictionary<string, ConnectionInfo>());
            }
            await Clients.Caller.SendAsync(ReceiveMethods.ReceiveDebug, $"ConnectionId: {Context.ConnectionId}, send observe successful.");
            SetProjectInfo(houseId, info);
            await SendVisitToClient(info);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="connectId">连接Id</param>
        /// <returns></returns>
        public async Task SendMessage(string message, string connectId)
        {
            var userName = this.GetUserName();
            await Clients.All.SendAsync(ReceiveMethods.ReceiveMessage, userName, message);
            // todo 区分客户和经纪人的消息
            throw new NotImplementedException();
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

        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"Online event: {Context.ConnectionId}, {this.GetUserName()}, {DateTime.Now}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Offline event: {Context.ConnectionId}, {this.GetUserName()}, {DateTime.Now}");
            var info = RemoveConnectionInfo(Context.ConnectionId);
            if(info != null)
            {
                _ = SendVisitToClient(info);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
