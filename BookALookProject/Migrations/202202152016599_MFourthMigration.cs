namespace BookALookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MFourthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Salons", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salons", "ImagePath");
        }
    }
}
