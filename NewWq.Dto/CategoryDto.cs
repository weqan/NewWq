using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Dto
{
    public class CategoryDto
    {
        [Display(Name = "创建者ID")]
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }

        [Display(Name = "分类标签名称")]
        public string CategoryName { get; set; }
    }
}
