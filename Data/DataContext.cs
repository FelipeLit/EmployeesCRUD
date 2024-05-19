using Microsoft.EntityFrameworkCore;
using PruebaDesempeno.Models;

namespace PruebaDesempeno.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options): base (options)
        {

        }

        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Employees> Employees { get; set; }
    }
}