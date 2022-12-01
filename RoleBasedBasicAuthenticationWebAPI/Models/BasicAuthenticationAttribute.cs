using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RoleBasedBasicAuthenticationWebAPI.Models
{
    public class BasicAuthenticationAttribute:AuthorizationFilterAttribute
    {
        private const string Realm = "My Realm";
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization==null)
            {
                actionContext.Response= actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                if(actionContext.Response.StatusCode== System.Net.HttpStatusCode.Unauthorized )
                {
                    actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm= \"{0}\"", Realm));
                }
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodeAuthToken=Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                string[] userpwdarray = decodeAuthToken.Split(':');
                string username = userpwdarray[0];
                string password = userpwdarray[1];
                if(UserValidate.Login(username, password))
                {
                    var UserDetails= UserValidate.GetUserDetails(username, password);
                    var identity = new GenericIdentity(username);
                    identity.AddClaim(new Claim("Email", UserDetails.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Name, UserDetails.UserName));
                    identity.AddClaim(new Claim("ID", UserDetails.ID.ToString()));

                    IPrincipal principal=new GenericPrincipal(identity,UserDetails.Roles.Split(','));
                    Thread.CurrentPrincipal = principal;
                    if(HttpContext.Current!=null)
                    {
                        HttpContext.Current.User= principal;
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}