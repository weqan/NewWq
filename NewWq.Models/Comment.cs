using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Models
{
    public class Comment:BaseEntity
    {
        [ForeignKey(nameof(Commodity))]
        public Guid CommodityId { get; set; }
        public Commodity Commodity { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        [StringLength(800)]
        public string Content { get; set; }

    }
}
