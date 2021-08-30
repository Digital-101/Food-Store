namespace FoodStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewW : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasketLines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BasketID = c.String(),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductImageMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageNumber = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        ProductImageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.ProductImages", t => t.ProductImageID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ProductImageID);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.FileName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketLines", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductImageMappings", "ProductImageID", "dbo.ProductImages");
            DropForeignKey("dbo.ProductImageMappings", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.ProductImages", new[] { "FileName" });
            DropIndex("dbo.ProductImageMappings", new[] { "ProductImageID" });
            DropIndex("dbo.ProductImageMappings", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.BasketLines", new[] { "ProductID" });
            DropTable("dbo.ProductImages");
            DropTable("dbo.ProductImageMappings");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.BasketLines");
        }
    }
}
