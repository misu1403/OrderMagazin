using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OrderManagement.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; } // Denumirea la plural
        public DbSet<Invoice> Invoices { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurare relație între Order și OrderDetail
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(od => od.Order) // Relația inversă
                .HasForeignKey(od => od.OrderId) // Foreign key: OrderId
                .OnDelete(DeleteBehavior.Cascade); // Ștergerea în cascadă pentru detaliile comenzii

            // Configurare relație între Order și Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithMany() // Un Order poate avea mai multe facturi
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Ștergerea în cascadă pentru facturile asociate

            // Configurare relație între OrderDetail și Product
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany() // Un produs poate fi utilizat în mai multe detalii de comandă
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Restricționează ștergerea unui produs dacă are detalii de comandă asociate
        }
    }
}
