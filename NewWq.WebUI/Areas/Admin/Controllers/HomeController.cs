using NewWq.WebUI.Areas.Admin.Filters;
using NewWq.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewWq.WebUI.Areas.Admin.Controllers
{
    [AdminAuth]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public async Task<ActionResult> Index()
        {
            var loginname = Session["loginAdmin"].ToString();
            IBLL.IUserManager userManager = new BLL.UserManager();
            var usermodel = await userManager.GetUserByEmail(loginname);

            return View(usermodel);
        }

        //登录
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new BLL.UserManager();
                Guid userId;
                if (userManager.Login(model.Email, model.LoginPwd, out userId))
                {
                    var usermodel = await userManager.GetUserByEmail(model.Email);
                    if (usermodel.Type == 0)
                    {
                        ModelState.AddModelError("", "该账号没有管理权限");
                        return View(model);
                    }
                    //跳转
                    //用session还是用cookie
                    if (model.RememberMe)
                    {
                        Response.Cookies.Add(new HttpCookie("loginAdmin")
                        {
                            Value = model.Email,
                            Expires = DateTime.Now.AddDays(7)
                        });
                        Response.Cookies.Add(new HttpCookie("adminId")
                        {
                            Value = userId.ToString(),
                            Expires = DateTime.Now.AddDays(7)
                        });
                    }
                    else
                    {
                        Session["loginAdmin"] = model.Email;
                        Session["adminId"] = userId;
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

        //管理员管理
        public async Task<ActionResult> AdminList()
        {
            IBLL.IUserManager userManager = new BLL.UserManager();

            return View(await userManager.GetAllUsers());
        }

        public async Task<ActionResult> UserEdit(Guid id)
        {
            IBLL.IUserManager userManager = new BLL.UserManager();
            var user = await userManager.GetOneUserById(id);

            return View(new AdminListViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                ImagePath = user.ImagePath,
                SiteName = user.SiteName,
                FansCount = user.FansCount,
                FocusCount = user.FocusCount,
                Type = user.Type
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(AdminListViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new BLL.UserManager();
                await userManager.UserEdit(model.Id, model.Email, model.ImagePath, model.SiteName, model.Type);
                return RedirectToAction(nameof(AdminList));
            }

            ModelState.AddModelError("", "验证失败");
            return View(model);
        }

        public async Task<ActionResult> UserDelete(Guid id)
        {
            IBLL.IUserManager userManager = new BLL.UserManager();
            await userManager.UserDelete(id);
            return RedirectToAction(nameof(AdminList));
        }


        //退出
        public ActionResult Quit()
        {
            Session.Abandon();

            if (Request.Cookies["adminId"] != null)
            {
                var adminId = new HttpCookie("adminId");
                adminId.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(adminId);
            }

            if (Request.Cookies["loginAdmin"] != null)
            {
                var loginAdmin = new HttpCookie("loginAdmin");
                loginAdmin.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(loginAdmin);
            }

            return RedirectToAction("Login", "Home");
        }
    }
}