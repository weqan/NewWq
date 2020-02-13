using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Models.UserViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 5)]
        [Display(Name ="邮箱")]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "密码")]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "确认密码")]
        [DataType(dataType: DataType.Password)]
        public string ConfirmPwd { get; set; }
    }
}