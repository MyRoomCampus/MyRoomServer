﻿using Microsoft.AspNetCore.SignalR;
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
            public const string ReceivePreOffer = nameof(ReceivePreOffer);
            public const string ReceivePreAnswer = nameof(ReceivePreAnswer);
            public const string ReceiceIceCandidate = nameof(ReceiceIceCandidate);
            public const string ReceiveOffer = nameof(ReceiveOffer);
            public const string ReveiveAnswer = nameof(ReveiveAnswer);
        }

        public ProjectHub(IMemoryCache cache, MyRoomDbContext dbContext)
        {
            this.cache = cache;
            this.dbContext = dbContext;
        }

        private async Task SendVisitToClient(ProjectInfo info)
        {
            var adminConnectionId = info.AdminConnectionId;
            if (adminConnectionId == null)
            {
                return;
            }

            await Clients.Client(adminConnectionId).SendAsync(ReceiveMethods.ReceiveVisit, info.ClientInfos);
        }
    }
}
