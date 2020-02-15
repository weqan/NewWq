using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NewWq.BLL;
using NewWq.WebUI.Filters;
using NewWq.WebUI.Models.Api;
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

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.IUserManager userManager = new BLL.UserManager();
                if (await userManager.GetUserByEmail(model.Email) == null)
                {
                    await userManager.Register(model.Email, model.LoginPwd);
                    return this.SendData("注册成功");
                }
                return this.ErrorData("邮箱已注册");
            }

            return this.ErrorData("输入数据不合法");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("banner")]
        public IHttpActionResult GetBanner()
        {
            var list = new List<BannerViewModel>() {
                new BannerViewModel(){Url="http://www.weqan.cn",Image="//webapp.didistatic.com/static/webapp/shield/cube-ui-examples-slide01.png"},
                new BannerViewModel(){Url="http://www.weqan.cn",Image="//webapp.didistatic.com/static/webapp/shield/cube-ui-examples-slide02.png"},
                new BannerViewModel(){Url="http://www.weqan.cn",Image="//webapp.didistatic.com/static/webapp/shield/cube-ui-examples-slide03.png"}
            };

            //设置序列化时key为驼峰样式  
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            return Json(list, settings);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("rollinglist")]
        public IHttpActionResult GetRollinglist()
        {
            string weburl = System.Configuration.ConfigurationManager.AppSettings["webUrl"];

            var list1 = new List<RollingListViewModel>() {
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_h5.png",Label="HTML5"},
            };

            var list2 = new List<RollingListViewModel>() {
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
                new RollingListViewModel(){Url="http://www.weqan.cn",Image=weburl+"CateImg/about_java.png",Label="Java"},
            };

            List<List<RollingListViewModel>> list = new List<List<RollingListViewModel>>();
            list.Add(list1);
            list.Add(list2);

            //设置序列化时key为驼峰样式  
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            return Json(list, settings);
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("category")]
        public async Task<IHttpActionResult> GetCategory()
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var catelist = await commodityManager.GetAllCategories();
            List<CategoryViewModel> list = new List<CategoryViewModel>();

            foreach (var cate in catelist)
            {
                list.Add(new CategoryViewModel() { Active = false, Label = cate.CategoryName, CateId = cate.CategoryId });
            }

            //设置序列化时key为驼峰样式  
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            return Json(list, settings);
        }

        [HttpGet()]
        [AllowAnonymous]
        [Route("classify")]
        public async Task<IHttpActionResult> GetClassify(string cateId)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var comlist = await commodityManager.GetAllCommoditiesByCateId(cateId);
            List<CommodityViewModel> list = new List<CommodityViewModel>();
            string weburl = System.Configuration.ConfigurationManager.AppSettings["webUrl"];
            foreach (var com in comlist)
            {
                list.Add(new CommodityViewModel() { ComId = com.Id, Image = weburl + com.MainImage, Label = com.Title.Substring(0, 5) });
            }

            //设置序列化时key为驼峰样式  
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;

            return Json(list, settings);

        }



    }
}
