namespace FoodStore.Migrations
{
    using FoodStore.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class StoreConfiguration : DbMigrationsConfiguration<FoodStore.DAL.StoreContext>
    {
        public StoreConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FoodStore.DAL.StoreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var categories = new List<Category>
            {
                new Category {Name = "Full Meal"},
                new Category {Name = "Fast Food"},
                new Category {Name = "Drinks"},
                new Category {Name = "Extras"},
            };
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            //var products = new List<Product>
            //{
            //    new Product {Name = "Phuth n Beef", Description="Whole Meal with Meat", Price=31.99M, CategoryID=categories.Single(c=>c.Name == "Full Meal").ID},
            //    new Product {Name = "Simba Chips", Description="Tasty Potato Chips", Price=16.99M, CategoryID=categories.Single(c=>c.Name == "Extras").ID}
            //};
            //products.ForEach(c => context.Products.AddOrUpdate(p => p.Name, c));
            //context.SaveChanges();

            var images = new List<ProductImage>
            {
                new ProductImage {FileName="candy.jpg"},
                new ProductImage {FileName="fried.jpg"},
                new ProductImage {FileName="simbachips.jpg"},
                new ProductImage {FileName="phuthuNbeef.jpg"},
                new ProductImage {FileName="chips.jpg"}
            };
            images.ForEach(c => context.ProductImages.AddOrUpdate(p => p.FileName, c));
            context.SaveChanges();

            //var imageMappings = new List<ProductImageMapping>
            //{
            //    new ProductImageMapping { ProductImageID= images.Single(i=>i.FileName == "phuthuNbeef.jpg").ID, ProductID = products.Single(c=>c.Name == "Phuthu n Beef").ID, ImageNumber = 0 },
            //    new ProductImageMapping { ProductImageID= images.Single(i=>i.FileName == "simbachips.jpg").ID, ProductID = products.Single(c=>c.Name == "Simba Chips").ID, ImageNumber = 1 }
            //};
            //imageMappings.ForEach(c => context.ProductImageMappings.AddOrUpdate(im => im.ProductImageID, c));
            //context.SaveChanges();

            var orders = new List<Order>
            {
                new Order { DeliveryAddress = new Address {AddressLine1="1 Some Street", Town="Town1", Country="SA", PostCode="4001"}, TotalPrice=5.99M, UserID="user@example.com", DateCreated= new DateTime(2021, 6, 14), DeliveryName="User" },
                new Order { DeliveryAddress = new Address {AddressLine1="2 Some Street", Town="Town2", Country="SA", PostCode="4002"}, TotalPrice=6.99M, UserID="user@example.com", DateCreated= new DateTime(2021, 6, 15), DeliveryName="User1" },
                new Order { DeliveryAddress = new Address {AddressLine1="3 Some Street", Town="Town3", Country="SA", PostCode="4003"}, TotalPrice=7.99M, UserID="user@example.com", DateCreated= new DateTime(2021, 6, 16), DeliveryName="User2" }
            };
            orders.ForEach(o => context.Orders.AddOrUpdate(p => p.DateCreated, o));
            context.SaveChanges();

            //var orderLines = new List<OrderLine>
            //{
            //    new OrderLine {OrderID=1, ProductID=products.Single(c=>c.Name == "Phuthu n Beef").ID, ProductName="Phuthu n Beef", Quantity=1, UnitPrice=products.Single(c=>c.Name == "Phuthu n Beef").Price},
            //    new OrderLine {OrderID=2, ProductID=products.Single(c=>c.Name == "Phuthu n Beef").ID, ProductName="Simba Chips", Quantity=1, UnitPrice=products.Single(c=>c.Name == "Simba Chips").Price}
            //};
        }
    }
}
