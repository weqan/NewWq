using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Areas.Admin.Models.CommodityViewModels
{
    public class CreateCommentViewModel
    {
        public Guid commodityId { get; set; }
        public string content { get; set; }
    }
}