namespace BookStore.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAppUserFromOrder : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "user_Id", newName: "AppUser_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_user_Id", newName: "IX_AppUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Orders", name: "IX_AppUser_Id", newName: "IX_user_Id");
            RenameColumn(table: "dbo.Orders", name: "AppUser_Id", newName: "user_Id");
        }
    }
}
