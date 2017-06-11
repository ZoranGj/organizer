using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Organizer.WebClient.Controllers
{
    public class BaseController : Controller
    {
        public User AppUser { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            // Grab the user's login information from Identity
            //User appUserState = new User();
            //if (User is ClaimsPrincipal)
            //{
            //    var user = User as ClaimsPrincipal;
            //    var claims = user.Claims.ToList();
            //    var userStateString = GetClaim(claims, "userState");

            //    if (!string.IsNullOrEmpty(userStateString))
            //        appUserState.FromString(userStateString);
            //}
            //AppUser = appUserState;

            //ViewData["UserState"] = AppUserState;
            //ViewData["ErrorDisplay"] = ErrorDisplay;
        }

        protected new JsonResult Json(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}