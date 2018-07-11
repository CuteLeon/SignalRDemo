using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRWebAPI
{
    [Produces("application/json")]
    [Route("api/MRSoftPush")]
    public class MRSoftPushController : Controller
    {
        private IHubContext<SignalrHubs> hubContext;

        public MRSoftPushController(IServiceProvider service)
        {
            hubContext = service.GetService<IHubContext<SignalrHubs>>();
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
