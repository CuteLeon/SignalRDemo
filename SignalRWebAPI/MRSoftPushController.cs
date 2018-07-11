using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRWebAPI.Models;

namespace SignalRWebAPI
{
    [Produces("application/json")]
    [Route("api/MRSoftPush")]
    public class MRSoftPushController : Controller
    {
        private IHubContext<SignalrHubs> hubContext;

        public MRSoftPushController(IServiceProvider service)
        {
            hubContext = (IHubContext<SignalrHubs>)service.GetService(typeof(IHubContext<SignalrHubs>));
        }

        [HttpGet]
        public string Get()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }

        /// <summary>
        /// 单个connectionid推送
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        [HttpPost, Route("AnyOne")]
        public IActionResult AnyOne([FromBody]IEnumerable<SignalrGroups> groups)
        {
            if (groups != null && groups.Any())
            {
                var ids = groups.Select(c => c.ShopId);
                var list = SignalrGroups.UserGroups.Where(c => ids.Contains(c.ShopId));
                foreach (var item in list)
                    hubContext.Clients.Client(item.ConnectionId).SendCoreAsync("AnyOne", new string[] { $"{item.ConnectionId}: {item.Content}" });
            }
            return Ok();
        }

        /// <summary>
        /// 全部推送
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet, Route("EveryOne")]
        public IActionResult EveryOne(string message)
        {
            hubContext.Clients.All.SendCoreAsync("EveryOne", new string[] { $"{message}" });
            return Ok();
        }

        /// <summary>
        /// 组推送
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost, Route("AnyGroups")]
        public IActionResult AnyGroups([FromBody]SignalrGroups group)
        {
            if (group != null)
            {
                hubContext.Clients.Group(group.GroupName).SendCoreAsync("AnyGroups", new string[] { $"{group.Content}" });
            }
            return Ok();
        }

        /// <summary>
        /// 多参数接收方式
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet, Route("MoreParamsRequest")]
        public IActionResult MoreParamsRequest(string message)
        {
            hubContext.Clients.All.SendCoreAsync("MoreParamsRequest", new string[] { message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") });
            return Ok();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
