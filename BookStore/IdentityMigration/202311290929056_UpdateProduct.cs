namespace BookStore.IdentityMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AuthorName", c => c.String());
            DropColumn("dbo.Products", "AuthorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "AuthorId", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "AuthorName");
        }
    }
}
