using Organizer.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Organizer.WebClient.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public async Task<IHttpActionResult> Register(RegisterModel model)
        {
            //todo update
            return null;
        }
    }
}
