namespace BookALookProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MSecondMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        AppointmentService = c.String(),
                        AppointmentCategory = c.String(),
                        Time = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Client_Id = c.String(maxLength: 128),
                        Salon_SalonId = c.Int(),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.AspNetUsers", t => t.Client_Id)
                .ForeignKey("dbo.Salons", t => t.Salon_SalonId)
                .Index(t => t.Client_Id)
                .Index(t => t.Salon_SalonId);
            
            CreateTable(
                "dbo.Salons",
                c => new
                    {
                        SalonId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNum = c.String(),
                        Email = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        WorkingFrom = c.DateTime(nullable: false),
                        WorkingTo = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SalonId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Salon_SalonId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.Salons", t => t.Salon_SalonId)
                .Index(t => t.Salon_SalonId);
            
            CreateTable(
                "dbo.SalonServices",
                c => new
                    {
                        SalonServiceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        Category_CategoryId = c.Int(),
                        Salon_SalonId = c.Int(),
                    })
                .PrimaryKey(t => t.SalonServiceId)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId)
                .ForeignKey("dbo.Salons", t => t.Salon_SalonId)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.Salon_SalonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalonServices", "Salon_SalonId", "dbo.Salons");
            DropForeignKey("dbo.SalonServices", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Appointments", "Salon_SalonId", "dbo.Salons");
            DropForeignKey("dbo.Categories", "Salon_SalonId", "dbo.Salons");
            DropForeignKey("dbo.Appointments", "Client_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SalonServices", new[] { "Salon_SalonId" });
            DropIndex("dbo.SalonServices", new[] { "Category_CategoryId" });
            DropIndex("dbo.Categories", new[] { "Salon_SalonId" });
            DropIndex("dbo.Appointments", new[] { "Salon_SalonId" });
            DropIndex("dbo.Appointments", new[] { "Client_Id" });
            DropTable("dbo.SalonServices");
            DropTable("dbo.Categories");
            DropTable("dbo.Salons");
            DropTable("dbo.Appointments");
        }
    }
}
