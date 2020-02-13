using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Models
{
    public class Fans:BaseEntity
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(FocusUser))]
        public Guid FocusUserId { get; set; }
        public User FocusUser { get; set; }//导航属性


    }
}
