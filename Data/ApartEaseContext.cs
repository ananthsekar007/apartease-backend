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

        public DbSet<Vendor> Vendor { get; set; } = default!;

        public DbSet<Company> Company { get; set; } = default!;

        public DbSet<Category> Category { get; set; } = default!;

        public DbSet<Amenity> Amenity { get; set; } = default!;

        public DbSet<AmenityBooking> AmenityBooking { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apartment>()
                .HasOne(a => a.Manager)
                .WithOne()
                .HasForeignKey<Apartment>(p => p.ManagerId);

            modelBuilder.Entity<Resident>()
                .HasOne(r => r.Apartment)
                .WithOne()
                .HasForeignKey<Resident>(p => p.ApartmentId);

            modelBuilder.Entity<Amenity>()
                .HasOne(a => a.Apartment)
                .WithOne()
                .HasForeignKey<Amenity>(a => a.ApartmentId);

            modelBuilder.Entity<Vendor>()
                .HasOne(v => v.Company)
                .WithOne(c => c.Vendor)
                .HasForeignKey<Vendor>(v => v.CompanyId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Companies)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<AmenityBooking>()
                .HasOne(a => a.Amenity)
                .WithOne()
                .HasForeignKey<AmenityBooking>(a => a.AmenityId);

            modelBuilder.Entity<AmenityBooking>()
                .HasOne(a => a.Manager)
                .WithOne()
                .HasForeignKey<AmenityBooking>(a => a.ManagerId);

            modelBuilder.Entity<AmenityBooking>()
                .HasOne(a => a.Resident)
                .WithOne()
                .HasForeignKey<AmenityBooking>(a => a.ResidentId);

        }
    }
}
