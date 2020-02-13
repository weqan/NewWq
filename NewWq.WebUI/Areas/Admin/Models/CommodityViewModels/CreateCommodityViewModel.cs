using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Areas.Admin.Models.CommodityViewModels
{
    public class CreateCommodityViewModel
    {
        [Required]
        [Display(Name = "标题")]
        public string Title { get; set; }
        [Display(Name = "内容")]
        public string Content { get; set; }
        [Display(Name = "封面图")]
        public string MainImage { get; set; }

        [Display(Name = "淘宝链接")]
        public string TaobaoUrl { get; set; }

        [Display(Name = "商品分类标签")]
        public Guid[] CategoryIds { get; set; }
    }
}