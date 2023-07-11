using BaseProjectApp.Admin.Areas.Identity.Model;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseProjectApp.Admin.Areas.Identity.Data
{
    public class Context : IdentityDbContext<IdentityUser, ApplicationRole, string>
    {
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ConnectionString = Configuration.GetConfigurationString();
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 
        }
    }
}
