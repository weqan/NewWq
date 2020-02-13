using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Areas.Admin.Models.CommodityViewModels
{
    public class CreateCategoryViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        [Display(Name ="分类名称")]
        public string CategoryName { get; set; }
    }
}