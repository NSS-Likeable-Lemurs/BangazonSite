using System;
using System.Collections.Generic;
using System.Text;
using Bangazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bangazon.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            base.OnModelCreating (modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            modelBuilder.Entity<Order> ()
                .Property (b => b.DateCreated)
                .HasDefaultValueSql ("GETDATE()");

            // Restrict deletion of related order when OrderProducts entry is removed
            modelBuilder.Entity<Order> ()
                .HasMany (o => o.OrderProducts)
                .WithOne (l => l.Order)
                .OnDelete (DeleteBehavior.Restrict);

            modelBuilder.Entity<Product> ()
                .Property (b => b.DateCreated)
                .HasDefaultValueSql ("GETDATE()");

            // Restrict deletion of related product when OrderProducts entry is removed
            modelBuilder.Entity<Product> ()
                .HasMany (o => o.OrderProducts)
                .WithOne (l => l.Product)
                .OnDelete (DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentType> ()
                .Property (b => b.DateCreated)
                .HasDefaultValueSql ("GETDATE()");

            ApplicationUser user = new ApplicationUser {
                FirstName = "admin",
                LastName = "admin",
                StreetAddress = "123 Infinity Way",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid ().ToString ("D")
            };
            var passwordHash = new PasswordHasher<ApplicationUser> ();
            user.PasswordHash = passwordHash.HashPassword (user, "Admin8*");
            modelBuilder.Entity<ApplicationUser> ().HasData (user);

            modelBuilder.Entity<PaymentType> ().HasData (
                new PaymentType () {
                    PaymentTypeId = 1,
                        UserId = user.Id,
                        Description = "American Express",
                        AccountNumber = "86753095551212"
                },
                new PaymentType () {
                    PaymentTypeId = 2,
                        UserId = user.Id,
                        Description = "Discover",
                        AccountNumber = "4102948572991"
                }
            );

            modelBuilder.Entity<ProductType> ().HasData (
                new ProductType () {
                    ProductTypeId = 1,
                        Label = "Sporting Goods"
                },
                new ProductType () {
                    ProductTypeId = 2,
                        Label = "Appliances"
                },
                new ProductType()
                {
                    ProductTypeId = 3,
                    Label = "Electronics"
                },
                new ProductType()
                {
                    ProductTypeId = 4,
                    Label = "Health"
                }

            );

            modelBuilder.Entity<Product> ().HasData (
                new Product () {
                    ProductId = 1,
                        ProductTypeId = 1,
                        UserId = user.Id,
                        Description = "It flies high",
                        Title = "Kite",
                        Quantity = 100,
                        Price = 2.99,
                        City="Franklin",
                        ImagePath="www.google.com"
                },
                new Product () {
                    ProductId = 2,
                        ProductTypeId = 2,
                        UserId = user.Id,
                        Description = "It rolls fast",
                        Title = "Wheelbarrow",
                        Quantity = 5,
                        Price = 29.99,
                        City = "Nashville",
                        ImagePath = "www.google.com"
                },
                new Product()
                {
                    ProductId = 3,
                    ProductTypeId = 3,
                    UserId = user.Id,
                    Description = "Black- not much RAM",
                    Title = "MacBook Air",
                    Quantity = 5,
                    Price = 1200.00
                },

                new Product()
                 {
                    ProductId = 4,
                    ProductTypeId = 3,
                    UserId = user.Id,
                    Description = "White TON OF RAM",
                    Title = "MacBook Pro",
                    Quantity = 5,
                    Price = 2000.00
                },
                new Product()
                {
                    ProductId = 5,
                    ProductTypeId = 3,
                    UserId = user.Id,
                    Description = "Black - Cute",
                    Title = "Dell",
                    Quantity = 5,
                    Price = 1000.00
                },
                new Product()
                {
                    ProductId = 6,
                    ProductTypeId = 3,
                    UserId = user.Id,
                    Description = "Orange",
                    Title = "Linux",
                    Quantity = 5,
                    Price = 500.00
                },
                new Product()
                {
                    ProductId = 7,
                    ProductTypeId = 3,
                    UserId = user.Id,
                    Description = "Not much RAM",
                    Title = "HP",
                    Quantity = 5,
                    Price = 300.00
                },
                new Product()
                {
                    ProductId = 8,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Chocolate",
                    Title = "Protein Powder - Chocolate",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 9,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Vanilla",
                    Title = "Protein Powder - Vanilla",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 10,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Grape",
                    Title = "Protein Powder- Grape ",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 11,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Strawberry",
                    Title = "Protein Powder - Strawberry",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 12,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Black",
                    Title = "Yoga Pants - Black",
                    Quantity = 5,
                    Price = 29.99
                }, 
                new Product()
                {
                    ProductId = 13,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "White",
                    Title = "Yoga Pants - White",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 14,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Red",
                    Title = "Yoga Pants - REd",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 15,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Green",
                    Title = "Yoga Pants - Green",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 16,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Purple",
                    Title = "Yoga Pants - Purple",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 17,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "5 lb",
                    Title = "Weights -5lb",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 18,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "10 lb",
                    Title = "Weights -10 lb",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 19,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "15 lb",
                    Title = "Weights -15 lb",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 20,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "8 lb",
                    Title = "Weights -8 lb",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 21,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "20 lb",
                    Title = "Weights -20lb",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 22,
                    ProductTypeId = 2,
                    UserId = user.Id,
                    Description = "25 lb",
                    Title = "Weights -25 lb",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 23,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "round and yellow",
                    Title = "Tennis Ball",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 24,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Sleek",
                    Title = "Babolat Tennis Racket",
                    Quantity = 5,
                    Price = 29.99
                },
                new Product()
                {
                    ProductId = 25,
                    ProductTypeId = 4,
                    UserId = user.Id,
                    Description = "Rounded",
                    Title = "Wilson Tennis Racket",
                    Quantity = 5,
                    Price = 29.99
                }


             );
            modelBuilder.Entity<Order> ().HasData (
                new Order () {
                    OrderId = 1,
                    UserId = user.Id,
                    PaymentTypeId = null
                }
            );

            modelBuilder.Entity<OrderProduct> ().HasData (
                new OrderProduct () {
                    OrderProductId = 1,
                    OrderId = 1,
                    ProductId = 1
                }
            );

            modelBuilder.Entity<OrderProduct> ().HasData (
                new OrderProduct () {
                    OrderProductId = 2,
                    OrderId = 1,
                    ProductId = 2
                }
            );

        }
    }
}