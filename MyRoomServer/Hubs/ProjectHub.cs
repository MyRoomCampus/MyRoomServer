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

            // 当 project 信息不存在时, 新建 project, 当 admin 在线时, 给 admin 发送上线消息
            var info = GetProjectInfo(houseId);
            if (info != null)
            {
                if (info.AdminConnectionId != null)
                {
                    await SendDebugToCaller("admin is online.");
                    await SendVisitToClient(info);
                }
                else
                {
                    await SendDebugToCaller("admin is offline.");
                }
                info.AddClientInfo(connectionInfo);
            }
            else
            {
                info = new ProjectInfo(null, connectionInfo);
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
                await SendDebugToCaller("don't have the specific project.");
                return;
            }

            var connectionInfo = new ConnectionInfo(ConnectionType.Admin, Context.ConnectionId, this.GetUserName(), houseId);
            SetConnectionInfo(Context.ConnectionId, in connectionInfo);

            var info = GetProjectInfo(houseId);
            if (info != null)
            {
                info.AdminConnectionId = Context.ConnectionId;
            }
            else
            {
                info = new ProjectInfo(Context.ConnectionId, null);
            }
            await SendDebugToCaller("send observe successful.");
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
        /// <param name="connectionId">客户的connectionId</param>
        /// <param name="offerKey"></param>
        /// <returns></returns>
        public async Task SendPreOffer(string offerKey, string connectionId)
        {
            SetOfferInfo(offerKey, Context.ConnectionId, connectionId);
            await Clients.Client(connectionId).SendAsync(ReceiveMethods.ReceivePreOffer, offerKey);
        }

        /// <summary>
        /// 客户对是否进行此次通话进行应答
        /// </summary>
        /// <param name="offerKey"></param>
        /// <param name="agree">是否同意进行通话</param>
        /// <returns></returns>
        public async Task SendPreAnswer(string offerKey, bool agree)
        {
            var offerInfo = GetOfferInfo(offerKey);
            if(offerInfo == null)
            {
                await SendDebugToCaller("offerKey is null.");
                return;
            }
            await Clients.Client(offerInfo.Admin).SendAsync(ReceiveMethods.ReceivePreAnswer, offerKey, agree);
        }

        public async Task SendIceCandidateOffer(string offerKey, string candidate)
        {
            var offerInfo = GetOfferInfo(offerKey);
            if(offerInfo == null)
            {
                await SendDebugToCaller("offerKey is null");
                return;
            }
            await Clients.Client(offerInfo.Client).SendAsync(ReceiveMethods.ReceiceIceCandidate, offerKey, candidate);
        }

        public async Task SendIceCandidateAnser(string offerKey, string candidate)
        {
            var offerInfo = GetOfferInfo(offerKey);
            if (offerInfo == null)
            {
                await SendDebugToCaller("offerKey is null");
                return;
            }
            await Clients.Client(offerInfo.Admin).SendAsync(ReceiveMethods.ReceiceIceCandidate, offerKey, candidate);
        }

        public async Task SendOffer(string offerKey, string offer)
        {
            var offerInfo = GetOfferInfo(offerKey);
            if (offerInfo == null)
            {
                await SendDebugToCaller("offerKey is null");
                return;
            }
            await Clients.Client(offerInfo.Client).SendAsync(ReceiveMethods.ReceiveOffer, offerKey, offer);
        }

        public async Task SendAnswer(string offerKey, string answer)
        {
            var offerInfo = GetOfferInfo(offerKey);
            if (offerInfo == null)
            {
                await SendDebugToCaller("offerKey is null");
                return;
            }
            await Clients.Client(offerInfo.Admin).SendAsync(ReceiveMethods.ReveiveAnswer, offerKey, answer);
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
            if (info != null)
            {
                _ = SendVisitToClient(info);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
