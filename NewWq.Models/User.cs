using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Models
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(40)]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }
        [Required]
        [StringLength(30)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; }
        [Required,StringLength(300),Column(TypeName ="varchar")]
        public string ImagePath { get; set; }
        public int FansCount { get; set; }
        public int FocusCount { get; set; }
        public string SiteName { get; set; }
        public int Type { get; set; }
        public string OpenId { get; set; }
    }
}
