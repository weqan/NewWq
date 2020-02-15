using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Models.Api
{
    public class CommodityViewModel
    {
        public Guid ComId { get; set; }
        public string Image { get; set; }
        public string Label { get; set; }
    }
}