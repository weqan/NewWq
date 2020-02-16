using NewWq.WebUI.Models.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewWq.WebUI.Controllers
{
    public class UserController : Controller
    {
        //public ActionResult PartialViewCate()
        //{
        //    IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
        //    var catelist = commodityManager.GetAllCategoriesSync();
        //    return PartialView(catelist);
        //}
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new BLL.UserManager();
                if (await userManager.GetUserByEmail(model.Email) == null)
                {
                    await userManager.Register(model.Email, model.Password);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "邮箱已存在");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new BLL.UserManager();
                Guid userid;
                if (userManager.Login(model.Email, model.LoginPwd, out userid))
                {
                    //跳转
                    //用session还是用cookie
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("loginName")
                        {
                            Value = model.Email,
                            Expires = DateTime.Now.AddDays(7)
                        });
                        Response.Cookies.Add(new HttpCookie("userId")
                        {
                            Value = userid.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else
                    {
                        Session["loginName"] = model.Email;
                        Session["userId"] = userid;
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "用户名密码错误");
                }
            }
            else
            {
                ModelState.AddModelError("", "您的账号密码有误");
            }
            return View(model);
        }

        //退出
        public ActionResult Quit()
        {
            Session.Abandon();

            if (Request.Cookies["loginName"] != null)
            {
                var loginName = new HttpCookie("loginName");
                loginName.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(loginName);
            }

            if (Request.Cookies["userId"] != null)
            {
                var userId = new HttpCookie("userId");
                userId.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userId);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}