using NewWq.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.DAL
{
    public class CommodityService : BaseService<Models.Commodity>, ICommodityService
    {
        public CommodityService() : base(new Models.WebContext())
        {

        }
    }
}
