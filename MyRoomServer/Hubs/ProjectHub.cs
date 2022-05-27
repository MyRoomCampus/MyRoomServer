using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyRoomServer.Entities;
using MyRoomServer.Extentions;
using MyRoomServer.Models;

namespace MyRoomServer.Hubs
{
    [Authorize(Policy = IdentityPolicyNames.CommonUser)]
    public class ProjectHub : Hub
    {
        private readonly IMemoryCache cache;
        private readonly MyRoomDbContext dbContext;

        /// <summary>
        /// 客户端用于接收消息的方法
        /// </summary>
        private static class ReceiveMethods
        {
            public const string ReceiveMessage = nameof(ReceiveMessage);
            public const string ReceiceIceCandidate = nameof(ReceiceIceCandidate);
            public const string ReceiveOffer = nameof(ReceiveOffer);
            public const string ReveiveAnswer = nameof(ReveiveAnswer);
            public const string ReceiveVisit = nameof(ReceiveVisit);
            public const string ReceiveVideoReject = nameof(ReceiveVideoReject);
        }

        public ProjectHub(IMemoryCache cache, MyRoomDbContext dbContext)
        {
            this.cache = cache;
            this.dbContext = dbContext;
        }

        public async Task SendMessage(string message, string connectId)
        {
            var userName = this.GetUserName();
            await Clients.All.SendAsync(ReceiveMethods.ReceiveMessage, userName, message);
        }

        public async Task SendIceCandidate(string user, string candidate)
        {
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

        /// <summary>
        /// 需要重复访问来确保缓存不会失效
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task SendVisit(string projectId)
        {
            // todo 可以考虑在这里给用户发点项目的消息
            await Groups.AddToGroupAsync(Context.ConnectionId, projectId);
            var ok = cache.TryGetValue(projectId, out string managerConnectId);
            if (ok)
            {
                // todo 定义需要发送的数据结构
                await Clients.Client(managerConnectId).SendAsync(ReceiveMethods.ReceiveVisit, Context.ConnectionId);
            }
        }

        /// <summary>
        /// 项目管理者访问以获取该项目当前的访客
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task SendObserve(long projectId)
        {
            var project = await dbContext.Projects.FindAsync(projectId);
            // todo 这里考虑需不需要给用户点反馈
            if (project == null)
            {
                return;
            }
            cache.Set(projectId, Context.ConnectionId);
        }

        public async Task SendVideoReject(string uid)
        {
            await Clients.Client(uid).SendAsync(ReceiveMethods.ReceiveVideoReject, uid);
            throw new NotImplementedException();
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
