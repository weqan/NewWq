using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Models
{
    public class ComtoCategory:BaseEntity
    {
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey(nameof(Commodity))]
        public Guid CommodityId { get; set; }
        public Commodity Commodity { get; set; }
    }
}
