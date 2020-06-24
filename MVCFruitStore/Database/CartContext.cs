using MVCFruitStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCFruitStore.Database
{

        public class CartContext : DbContext
        {
            public CartContext(DbContextOptions<CartContext> options)
            : base(options)
            { }

            protected override void OnModelCreating(ModelBuilder model)
            {
                model.Entity<Order_Detail>().HasIndex(model => model.ActivationCode).IsUnique();
            }

            public DbSet<Cart> Carts { get; set; }
            public DbSet<Order_Detail> OrderDetails { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<User> Users { get; set; }

        public DbSet<Review> Reviews { get; set; }
        }
    
}
