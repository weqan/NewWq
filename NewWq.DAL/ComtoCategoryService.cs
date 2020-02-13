using NewWq.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.DAL
{
    public class ComtoCategoryService : BaseService<Models.ComtoCategory>, IComtoCategoryService
    {
        public ComtoCategoryService() : base(new Models.WebContext())
        {

        }
    }
}
