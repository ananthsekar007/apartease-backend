using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using apartease_backend.Models;

namespace apartease_backend.Data
{
    public class ApartEaseContext : DbContext
    {
        public ApartEaseContext (DbContextOptions<ApartEaseContext> options)
            : base(options)
        {
        }

        public DbSet<Manager> Manager { get; set; } = default!;

        public DbSet<Apartment> Apartment { get; set; } = default!;

        public DbSet<Resident> Resident { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>()
                .HasOne(u => u.Apartment)
                .WithOne(p => p.Manager)
                .HasForeignKey<Apartment>(p => p.ManagerId);

            modelBuilder.Entity<Apartment>()
                .HasMany(a => a.Residents)
                .WithOne(r => r.Apartment)
                .HasForeignKey(r => r.ApartmentId);
        }
    }
}
