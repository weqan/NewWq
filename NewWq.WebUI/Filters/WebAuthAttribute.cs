using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace NewWq.WebUI.Filters
{
    public class WebAuthAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            //用户存储在cookie中且session数据为空时，把cookie的数据同步到session中
            if (filterContext.HttpContext.Session["loginName"] == null && filterContext.HttpContext.Request.Cookies["loginName"] != null)
            {
                filterContext.HttpContext.Session["loginName"] = filterContext.HttpContext.Request.Cookies["loginName"].Value;
            }



            if (!(filterContext.HttpContext.Session["loginName"] != null || filterContext.HttpContext.Request.Cookies["loginName"] != null))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary()
                {
                    {"controller","User" },
                    {"action","Login" }
                });
            }
        }
    }
}