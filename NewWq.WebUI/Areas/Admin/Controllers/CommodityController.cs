using NewWq.WebUI.Areas.Admin.Filters;
using NewWq.WebUI.Areas.Admin.Models;
using NewWq.WebUI.Areas.Admin.Models.CategoryViewModels;
using NewWq.WebUI.Areas.Admin.Models.CommodityViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewWq.WebUI.Areas.Admin.Controllers
{
    [AdminAuth]
    public class CommodityController : Controller
    {


        // GET: Admin/Commodity
        public ActionResult Index()
        {
            return View();
        }


        //分类操作
        public ActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
                commodityManager.CreateCategory(model.CategoryName, Guid.Parse(Session["adminId"].ToString()));
                return RedirectToAction("CategoryList");
            }
            else
            {
                ModelState.AddModelError("", "输入信息有误");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> CategoryList()
        {
            IBLL.ICommodityManager categoryManager = new BLL.CommodityManager();
            var categoryList = await categoryManager.GetAllCategories(Guid.Parse(Session["adminId"].ToString()));
            return View(categoryList);
        }

        public async Task<ActionResult> CategoryEdit(Guid id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var cate = await commodityManager.GetCategoryById(id);
            return View(new EditCategoryViewModel()
            {
                CategoryName = cate.CategoryName,
                UserId = cate.UserId,
                CategoryId = cate.CategoryId
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CategoryEdit(EditCategoryViewModel model)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();

            if (ModelState.IsValid)
            {
                await commodityManager.EditCategory(model.CategoryId, model.CategoryName);
                return RedirectToAction("CategoryList");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ActionResult> CategoryDelete(Guid id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            await commodityManager.RemoveCategory(id);
            return RedirectToAction("CategoryList");
        }


        //商品操作
        public async Task<ActionResult> CreateCommodity()
        {
            IBLL.ICommodityManager categoryManager = new BLL.CommodityManager();
            ViewBag.Categories = await categoryManager.GetAllCategories(Guid.Parse(Session["adminId"].ToString()));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> CreateCommodity(CreateCommodityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var imgname = "";
                if (!SaveImage(out imgname))
                {
                    ModelState.AddModelError("", "文件上传失败");
                }
                else
                {
                    model.MainImage = imgname;
                }

                Guid userid = Guid.Parse(Session["adminId"].ToString());
                IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
                await commodityManager.CreateCommodity(model.Title, model.Content, model.CategoryIds, userid, model.MainImage, model.TaobaoUrl);
                return RedirectToAction("CommodityList");
            }
            else
            {
                ModelState.AddModelError("", "添加失败");
            }
            return View(model);
        }

        public async Task<ActionResult> CommodityList(int pageIndex = 1, int pageSize = 5, bool asc = false)
        {
            var comManager = new BLL.CommodityManager();
            Guid userid = Guid.Parse(Session["adminId"].ToString());
            var comCount = await comManager.GetDataCount(userid);
            ViewBag.PageCount = comCount % pageSize == 0 ? comCount / pageSize : comCount / pageSize + 1;
            ViewBag.PageIndex = pageIndex;
            var commodities = await comManager.GetAllCommodities(userid, pageIndex, pageSize, asc);
            return View(commodities);
        }

        public ActionResult UploadImg()
        {

            //上传配置
            string pathbase = "/upload/";//保存路径
            int size = 10;//文件大小限制,单位mb                 
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" }; //文件允许格式
            string callback = Request.QueryString["callback"];
            string editorId = Request.QueryString["editorid"];


            //上传图片
            Hashtable info;
            Uploader up = new Uploader();

            info = up.upFile(pathbase, filetype, size); //获取上传状态
            string json = BuildJson(info);

            Response.ContentType = "text/html";
            if (callback != null)
            {
                return Content(string.Format("<script>{0}(JSON.parse(\"{1}\"));</script>", callback, json));
            }
            else
            {
                return Content(json);
            }
        }
        private string BuildJson(Hashtable info)
        {
            List<string> fields = new List<string>();
            string[] keys = new string[] { "originalName", "name", "url", "size", "state", "type" };
            for (int i = 0; i < keys.Length; i++)
            {
                fields.Add(String.Format("\"{0}\": \"{1}\"", keys[i], info[keys[i]]));
            }
            return "{" + String.Join(",", fields) + "}";
        }
        /// <summary>
        /// 把图片上传到服务器并保存路径到数据库
        /// </summary>
        /// <returns></returns>
        public bool SaveImage(out string imgname)
        {
            //string result = "";
            HttpPostedFileBase imageName = Request.Files["uploadImg"];// 从前台获取文件
            if (imageName == null || imageName.FileName == "")
            {
                imgname = "";
                return true;
            }

            string file = imageName.FileName;

            string fileFormat = file.Split('.')[file.Split('.').Length - 1]; // 以“.”截取，获取“.”后面的文件后缀
            Regex imageFormat = new Regex(@"^(bmp)|(png)|(gif)|(jpg)|(jpeg)"); // 验证文件后缀的表达式（自己写的，不规范别介意哈）
            if (string.IsNullOrEmpty(file) || !imageFormat.IsMatch(fileFormat)) // 验证后缀，判断文件是否是所要上传的格式
            {
                imgname = "";
                //result = "error";
                return false;
            }
            else
            {
                string timeStamp = DateTime.Now.Ticks.ToString(); // 获取当前时间的string类型
                string firstFileName = timeStamp.Substring(0, timeStamp.Length - 4); // 通过截取获得文件名
                string imageStr = "upload/"; // 获取保存图片的项目文件夹
                string uploadPath = Server.MapPath("~/" + imageStr); // 将项目路径与文件夹合并
                string pictureFormat = file.Split('.')[file.Split('.').Length - 1];// 设置文件格式
                string fileName = firstFileName + "." + fileFormat;// 设置完整（文件名+文件格式） 
                string saveFile = uploadPath + fileName;//文件路径
                imageName.SaveAs(saveFile);// 保存文件
                                           // 如果单单是上传，不用保存路径的话，下面这行代码就不需要写了！
                imgname = imageStr + fileName;// 设置数据库保存的路径

                //result = "success";
                return true;
            }

            //return result;
        }

        public async Task<ActionResult> CommodityDetails(Guid? id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            if (id == null || !await commodityManager.ExistsCommodity(id.Value))
            {
                return RedirectToAction(nameof(CommodityList));
            }

            ViewBag.Comments = await commodityManager.GetCommentByCommodityId(id.Value);


            var model = await commodityManager.GetOneCommodityById(id.Value);
            return View(model);
        }




        [HttpGet]
        public async Task<ActionResult> CommodityEdit(Guid id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var comdto = await commodityManager.GetOneCommodityById(id);

            ViewBag.Categories = await commodityManager.GetAllCategories(Guid.Parse(Session["adminId"].ToString()));

            return View(new EditCommodityViewModel()
            {
                CommodityId = comdto.Id,
                Title = comdto.Title,
                Content = comdto.Content,
                MainImage = comdto.MainImage,
                TaobaoUrl = comdto.TaobaoUrl,
                CategoryIds = comdto.CategoryIds
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> CommodityEdit(Models.CommodityViewModels.EditCommodityViewModel model)
        {
            var imgname = "";
            if (!SaveImage(out imgname))
            {
                ModelState.AddModelError("", "文件上传失败");
            }
            else
            {
                if (imgname != "")
                {
                    model.MainImage = imgname;
                }

            }

            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();

            if (ModelState.IsValid)
            {
                await commodityManager.EditCommodity(model.CommodityId, model.Title, model.Content, model.CategoryIds, model.MainImage, model.TaobaoUrl);
                return RedirectToAction("CommodityList");
            }
            else
            {
                ViewBag.Categories = await commodityManager.GetAllCategories(Guid.Parse(Session["adminId"].ToString()));
                return View(model);
            }


        }

        public async Task<ActionResult> CommodityDelete(Guid id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();

            var commodity = await commodityManager.GetOneCommodityById(id);
            if (commodity.MainImage != null || commodity.MainImage != "")
            {
                var FilePath = Server.MapPath("~/" + commodity.MainImage);
                if (System.IO.File.Exists(FilePath))//判断文件是否存在
                {
                    System.IO.File.Delete(FilePath);
                }
            }

            await commodityManager.DeleteAllCategoryIdsByCommodityId(id);

            await commodityManager.RemoveCommodity(id);
            return RedirectToAction("CommodityList");
        }


        //点赞反对
        public async Task<ActionResult> GoodCount(Guid id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            await commodityManager.GoodCount(id);
            return Json(new { result = "ok" });
        }

        public async Task<ActionResult> BadCount(Guid id)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            await commodityManager.BadCount(id);
            return Json(new { result = "ok" });
        }


        //评论
        [HttpPost]
        public async Task<ActionResult> AddComment(CreateCommentViewModel model)
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var adminid = Guid.Parse(Session["adminId"].ToString());
            await commodityManager.CreateComment(adminid, model.commodityId, model.content);
            return Json(new { result = "ok" });
        }
    }
}