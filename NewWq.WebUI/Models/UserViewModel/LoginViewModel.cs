using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Models.UserViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="邮箱是必须的")]
        [EmailAddress(ErrorMessage ="不是有效的电子邮箱")]
        [Display(Name ="电子邮箱")]
        public string Email { get; set; }
        [Required]
        [StringLength(50,MinimumLength =6)]
        [Display(Name = "密码")]
        [DataType(dataType:DataType.Password)]
        public string LoginPwd { get; set; }
        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}