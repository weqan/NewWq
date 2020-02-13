using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Areas.Admin.Models
{
    public class AdminListViewModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "头像")]
        public string ImagePath { get; set; }
        [Required]
        [Display(Name = "昵称")]
        public string SiteName { get; set; }
        public int FansCount { get; set; }
        public int FocusCount { get; set; }
        [Required]
        [Display(Name = "权限类别")]
        public int Type { get; set; }
    }
}