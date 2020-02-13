using NewWq.IDAL;
using NewWq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.DAL
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService() : base(new WebContext())
        {

        }


    }
}
