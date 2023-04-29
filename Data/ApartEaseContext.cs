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
    }
}
