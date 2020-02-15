using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewWq.WebUI.Models.Api
{
    public class CategoryViewModel
    {
        public string Label { get; set; }
        public bool Active { get; set; }
        public Guid CateId { get; set; }
    }
}