using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Areas.Admin.Models.CategoryViewModels
{
    public class EditCategoryViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "分类名称")]
        public string CategoryName { get; set; }

        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }

    }
}