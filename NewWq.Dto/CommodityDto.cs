using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Dto
{
    public class CommodityDto
    {
        public Guid Id { get; set; }
        [Display(Name = "商品标题")]
        public string Title { get; set; }
        [Display(Name = "详情")]
        public string Content { get; set; }
        [Display(Name = "商品创建时间")]
        public DateTime CreateTime { get; set; }
        [Display(Name = "创建人")]
        public string Email { get; set; }
        [Display(Name = "赞成次数")]
        public int GoodCount { get; set; }
        [Display(Name = "反对次数")]
        public int BadCount { get; set; }
        [Display(Name = "网站头像")]
        public string ImagePath { get; set; }
        [Display(Name = "商品主图")]
        public string MainImage { get; set; }
        [Display(Name = "淘宝链接")]
        public string TaobaoUrl { get; set; }

        [Display(Name = "分类")]
        public string[] CategoryNames { get; set; }
        public Guid[] CategoryIds { get; set; }
    }
}
