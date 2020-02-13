using NewWq.BLL;
using NewWq.WebUI.Filters;
using NewWq.WebUI.Models.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NewWq.WebUI.Controllers
{
    /*
     * 准备工作
     * 1.ef引用并设置连接串
     * 2.jwt引用
     * 3.Attribute特性用来过滤/校验 登录的合法性
     * 
     * 4.每个控制器要做跨域处理Cors
     * 5.为Action编写ViewModel用来校验提交的数据的合法性
     * 6.为返回的结果编写一个ResponseData处理统一返回的数据
     * 
     * 
     */

    [WebApiAuth]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/user")]
    public class WebApiController : ApiController
    {
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new BLL.UserManager();
                if (userManager.Login(model.Email, model.LoginPwd, out Guid userid))
                {
                    return this.SendData(JwtTools.Encoder(new Dictionary<string, object>() {
                        {"username",model.Email },
                        {"userid",userid }
                    }));
                }
                else
                {
                    return this.ErrorData("用户名密码错误");
                }
            }

            return this.ErrorData("输入数据不合法");
        }



    }
}
