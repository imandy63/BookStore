namespace BookStore.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateUserInOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "AppUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AppUser_Id" });
            AddColumn("dbo.Orders", "AppUser_Id1", c => c.String(maxLength: 128));
            AlterColumn("dbo.Orders", "AppUser_Id", c => c.String());
            CreateIndex("dbo.Orders", "AppUser_Id1");
            AddForeignKey("dbo.Orders", "AppUser_Id1", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "AppUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "AppUser_Id1" });
            AlterColumn("dbo.Orders", "AppUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Orders", "AppUser_Id1");
            CreateIndex("dbo.Orders", "AppUser_Id");
            AddForeignKey("dbo.Orders", "AppUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
