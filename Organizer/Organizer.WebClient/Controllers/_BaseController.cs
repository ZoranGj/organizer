using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Organizer.WebClient.Controllers
{
    public class BaseController : Controller
    {
        public int UserId { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var userId = identity.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                     .Select(c => c.Value).SingleOrDefault();
            if (!string.IsNullOrEmpty(userId))
            {
                UserId = int.Parse(userId);
            }
        }

        protected new JsonResult Json(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}