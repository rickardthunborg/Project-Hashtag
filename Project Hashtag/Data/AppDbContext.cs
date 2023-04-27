using Microsoft.EntityFrameworkCore;
using Project_Hashtag.Models;
using System.Collections.Generic;

namespace Project_Hashtag.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
