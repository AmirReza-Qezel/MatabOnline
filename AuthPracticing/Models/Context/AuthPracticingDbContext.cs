using Microsoft.EntityFrameworkCore;

namespace AuthPracticing.Models.Context
{
    public class AuthPracticingDbContext : DbContext
    {
        public AuthPracticingDbContext
            (DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }
        public DbSet<Employee> Employees { get; set; }
    }
}
