namespace BookALookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MThirdMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Salon_SalonId", "dbo.Salons");
            DropIndex("dbo.Categories", new[] { "Salon_SalonId" });
            CreateTable(
                "dbo.CategorySalons",
                c => new
                    {
                        Category_CategoryId = c.Int(nullable: false),
                        Salon_SalonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_CategoryId, t.Salon_SalonId })
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Salons", t => t.Salon_SalonId, cascadeDelete: true)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.Salon_SalonId);
            
            DropColumn("dbo.Categories", "Salon_SalonId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "Salon_SalonId", c => c.Int());
            DropForeignKey("dbo.CategorySalons", "Salon_SalonId", "dbo.Salons");
            DropForeignKey("dbo.CategorySalons", "Category_CategoryId", "dbo.Categories");
            DropIndex("dbo.CategorySalons", new[] { "Salon_SalonId" });
            DropIndex("dbo.CategorySalons", new[] { "Category_CategoryId" });
            DropTable("dbo.CategorySalons");
            CreateIndex("dbo.Categories", "Salon_SalonId");
            AddForeignKey("dbo.Categories", "Salon_SalonId", "dbo.Salons", "SalonId");
        }
    }
}
