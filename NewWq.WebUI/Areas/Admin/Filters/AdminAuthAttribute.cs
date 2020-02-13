using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewWq.WebUI.Areas.Admin.Filters
{
    public class AdminAuthAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        //AuthorizationFilter首先运行
        //Action 动作方法之前或之后运行
        //Result 动作结果被执行之前和之后运行
        //Exception 在另一个过滤器弹出异常时运行
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            //判断是否跳过授权过滤器
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            if (filterContext.HttpContext.Session["loginAdmin"] == null && filterContext.HttpContext.Request.Cookies["loginAdmin"] != null)
            {
                filterContext.HttpContext.Session["loginAdmin"] = filterContext.HttpContext.Request.Cookies["loginAdmin"].Value;
                filterContext.HttpContext.Session["adminId"] = filterContext.HttpContext.Request.Cookies["adminId"].Value;
            }


            if (!(filterContext.HttpContext.Session["loginAdmin"] != null || filterContext.HttpContext.Request.Cookies["loginAdmin"] != null))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary()
                {
                    {"controller","Home" },
                    {"action","Login" }
                });
            }
        }
    }
}