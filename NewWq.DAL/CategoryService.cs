using NewWq.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.DAL
{
    public class CategoryService : BaseService<Models.Category>, ICategoryService
    {
        public CategoryService() : base(new Models.WebContext())
        {

        }
    }
}
