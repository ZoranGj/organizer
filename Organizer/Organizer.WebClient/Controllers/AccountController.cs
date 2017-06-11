using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Organizer.Model;
using Organizer.Model.DataProviders;
using Organizer.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Organizer.WebClient.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private UsersProvider _usersProvider;
        private DataContext _dbContext;

        public AccountController()
        {
            _dbContext = new DataContext();
            _usersProvider = new UsersProvider(_dbContext);
        }

        [HttpPost]
        public ActionResult Register(string username, string email, string password)
        {
            var user = _usersProvider.GetByEmail(email);
            if (user != null) throw new Exception("User with that email already exists.");

            user = new User
            {
                Username = username,
                Email = email,
                Password = password,
                DateJoined = DateTime.Now
            };
            try
            {
                _usersProvider.Insert(user);
                _usersProvider.Save();
            }
            catch (Exception exception)
            {
                //decide if additional format should be returned
                throw;
            }

            var token = IdentitySignin(new UserDto(user));
            return Json (new { accessToken = token, userName = username });
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _usersProvider.Get(email, password);
            if (user == null) throw new Exception("User does not exists.");

            var userDto = new UserDto(user);
            var token = IdentitySignin(userDto);
            return Json(new { accessToken = token, userName = userDto.Name });
        }

        [HttpPost]
        public void LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public string IdentitySignin(UserDto user, string providerKey = null, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // add user data as claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            // add all user data serialized as a claim
            claims.Add(new Claim("userState", user.ToString()));

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            var authProperties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };
            AuthenticationManager.SignIn(authProperties, identity);

            AuthenticationTicket ticket = new AuthenticationTicket(identity, authProperties);
            return string.Format("ticket:", ticket.ToString());
        }

        public void IdentitySignout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
    }
}
