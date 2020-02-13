using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Dto
{
    public class UserInformationDto
    {
        public Guid Id { get; set; }
        [Display(Name = "邮箱")]
        public string Email { get; set; }
        [Display(Name = "头像")]
        public string ImagePath { get; set; }
        [Display(Name = "昵称")]
        public string SiteName { get; set; }
        [Display(Name = "粉丝数")]
        public int FansCount { get; set; }
        [Display(Name = "关注数")]
        public int FocusCount { get; set; }
        [Display(Name = "类型")]
        public int Type { get; set; }
    }
}
