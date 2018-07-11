using Microsoft.AspNetCore.SignalR;
using SignalRWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebAPI
{
    public class SignalrHubs: Hub
    {
        /// <summary>
        /// 创建signalr链接
        /// </summary>
        /// <param name="parentId">pid(作为用户组)</param>
        /// <param name="shopId">sid</param>
        public Task InitUser(string parentId, string shopId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, parentId);
            SignalrGroups.UserGroups.Add(new SignalrGroups()
            {
                ConnectionId = Context.ConnectionId,
                GroupName = parentId,
                ShopId = shopId
            });
            return Clients.All.SendCoreAsync("NoticeOnline", new string[]{ "用户组数据更新完成,新增id为：" + Context.ConnectionId + " pid:" + parentId + "   sid:" + shopId + "" });
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            //掉线移除用户
            var user = SignalrGroups.UserGroups.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                SignalrGroups.UserGroups.Remove(user);
                Groups.RemoveFromGroupAsync(Context.ConnectionId,user.GroupName);
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
