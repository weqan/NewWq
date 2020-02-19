namespace NewWq.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newwq : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "OpenId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "OpenId");
        }
    }
}
