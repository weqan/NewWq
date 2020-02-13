using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewWq.Models
{
    public class WebContext : DbContext
    {
        public WebContext() : base("con")
        {
            //从不创建数据库
            Database.SetInitializer<WebContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Fans> Fans { get; set; }
        public DbSet<ComtoCategory> ComtoCategories { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
