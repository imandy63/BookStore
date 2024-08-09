namespace BookStore.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUserFromOrder : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Orders", new[] { "AppUser_Id1" });
            DropColumn("dbo.Orders", "AppUser_Id");
            RenameColumn(table: "dbo.Orders", name: "AppUser_Id1", newName: "AppUser_Id");
            AlterColumn("dbo.Orders", "AppUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "AppUser_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Orders", new[] { "AppUser_Id" });
            AlterColumn("dbo.Orders", "AppUser_Id", c => c.String());
            RenameColumn(table: "dbo.Orders", name: "AppUser_Id", newName: "AppUser_Id1");
            AddColumn("dbo.Orders", "AppUser_Id", c => c.String());
            CreateIndex("dbo.Orders", "AppUser_Id1");
        }
    }
}
