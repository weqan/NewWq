using NewWq.WebUI.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NewWq.WebUI.Filters
{
    public class WebApiAuthAttribute : Attribute, IAuthorizationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            //当某个特定的action加入了AllowAnonymous特性时跳过检查
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Count > 0)
            {
                return await continuation();
            }

            //获取request->headers->token
            IEnumerable<string> headers;
            if (actionContext.Request.Headers.TryGetValues("token", out headers))
            {
                //如果获取到了headers里的token
                //token
                string LoginName = JwtTools.Decoder(headers.First())["username"].ToString();
                Guid UserId = Guid.Parse(JwtTools.Decoder(headers.First())["userid"].ToString());

                (actionContext.ControllerContext.Controller as ApiController).User = new ApplicationUser(LoginName, UserId);

                return await continuation();
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

        }
    }
}