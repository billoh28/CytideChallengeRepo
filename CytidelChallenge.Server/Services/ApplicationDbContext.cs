using CytidelChallenge.Server.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CytidelChallenge.Server.Services
{
    public class ApplicationDbContext : IdentityDbContext<TaskUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskUser> TaskUsers { get; set; }
    }
}
