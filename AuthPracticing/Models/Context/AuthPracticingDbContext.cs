using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthPracticing.Models.Context
{
    public class AuthPracticingDbContext : IdentityDbContext
    {
        public AuthPracticingDbContext
            (DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // call the base if you are using Identity service.
            // Important
            base.OnModelCreating(builder);

            // Code here ...
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
