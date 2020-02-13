using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Models
{
    public class Category:BaseEntity
    {
        
        public string CategoryName { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }




    }
}
