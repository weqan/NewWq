using Newtonsoft.Json;
using NewWq.WebUI.Models.UserViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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

        //QQ的APPID
        public string appId = ConfigurationManager.AppSettings["appId"];

        //QQ的appSecret
        public string appSecret = ConfigurationManager.AppSettings["appSecret"];

        //回调地址
        public string redirecturl = ConfigurationManager.AppSettings["redirecturl"];

        /// <summary>
        /// 打开qq授权页面
        /// </summary>
        /// <returns></returns>
        public ActionResult QQAuthorize()
        {
            var url = string.Format(
                     "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&state=State",
                     appId, WebUtility.UrlEncode(redirecturl));
            return new RedirectResult(url);
        }

        public async Task<ActionResult> QQLogin()
        {
            IBLL.IUserManager userManager = new BLL.UserManager();

            var code = Request.QueryString["code"];
            var token = GetAuthorityAccessToken(code);
            var dic = GetAuthorityOpendIdAndUnionId(token);
            var userInfo = GetUserInfo(token, dic["openid"]);

            Dto.UserInformationDto user = await userManager.GetUserByOpenId(dic["openid"], userInfo);


            Response.Cookies.Add(new HttpCookie("userinfo")
            {
                Value = JsonConvert.SerializeObject(user),
                Expires = DateTime.Now.AddDays(7)
            });

            
            return RedirectToAction("Index", "Home");
      
        }

        //获取token
        public virtual string GetAuthorityAccessToken(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            try
            {
                var url =
                             string.Format(
                                 "https://graph.qq.com/oauth2.0/token?client_id={0}&client_secret={1}&code={2}&grant_type=authorization_code&redirect_uri={3}",
                                 appId, appSecret, code, redirecturl);

                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

                HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;

                Stream stream = webResponse.GetResponseStream();

                using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(json))
                        return null;
                    if (!json.Contains("access_token"))
                    {
                        return null;
                    }

                    var dis = json.Split('&').Where(it => it.Contains("access_token")).FirstOrDefault();
                    var accessToken = dis.Split('=')[1];
                    return accessToken;

                }

            }
            catch (Exception)
            {

                return "";
            }

        }

        //获取openid和client_id
        public virtual Dictionary<string, string> GetAuthorityOpendIdAndUnionId(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            var url = $"https://graph.qq.com/oauth2.0/me?access_token={token}&unionid=1";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;

            Stream stream = webResponse.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                var json = reader.ReadToEnd();

                if (string.IsNullOrEmpty(json) || json.Contains("error") || !json.Contains("callback"))
                {
                    return null;
                }

                Regex reg = new Regex(@"\(([^)]*)\)");
                Match m = reg.Match(json);
                var dis = m.Result("$1");
                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(dis);

                return dic;
            }

        }

        //获取用户信息
        public virtual Dictionary<string, string> GetUserInfo(string token, string openId)
        {
            if (string.IsNullOrEmpty(token)) { return null; }

            var url = $"https://graph.qq.com/user/get_user_info?access_token={token}&openid={openId}&oauth_consumer_key={appId}";
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;

            HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
            Stream stream = webResponse.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                var json = reader.ReadToEnd();

                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                if (dic.ContainsKey("ret") && dic["ret"] != "0")
                {
                    return null;
                }

                return dic;
            }

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