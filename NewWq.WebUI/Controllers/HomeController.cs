using NewWq.WebUI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewWq.WebUI.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult PartialViewCate()
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var catelist = commodityManager.GetAllCategoriesSync();
            return PartialView(catelist);
        }

        public async Task<ActionResult> Index()
        {
            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();
            var commodityList = await commodityManager.GetAllCommodities(8);
            return View(commodityList);
        }

        public async Task<ActionResult> List(Guid? CategoryId, int pageIndex = 1, int pageSize = 8, bool asc = false)
        {

            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();

            ViewBag.Categories = await commodityManager.GetAllCategories(4);



            var comCount = await commodityManager.GetComCount(CategoryId);

            ViewBag.PageCount = comCount % pageSize == 0 ? comCount / pageSize : comCount / pageSize + 1;
            ViewBag.PageIndex = pageIndex;

            var commodityList = await commodityManager.GetAllCommoditiesByCateId(CategoryId, pageIndex, pageSize, asc);

            return View(commodityList);
        }

        public async Task<ActionResult> Detail(Guid comid)
        {
            if (comid == null)
            {
                return RedirectToAction("List");
            }

            IBLL.ICommodityManager commodityManager = new BLL.CommodityManager();

            ViewBag.Next = await commodityManager.NextOne(comid);
            ViewBag.Before = await commodityManager.BeforeOne(comid);


            var model = await commodityManager.GetOneCommodityById(comid);


            return View(model);
        }


    }
}