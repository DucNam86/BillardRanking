using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BillardAPI.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
