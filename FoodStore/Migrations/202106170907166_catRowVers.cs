namespace FoodStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class catRowVers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "RowVersion");
        }
    }
}
