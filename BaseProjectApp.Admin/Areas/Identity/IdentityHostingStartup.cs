
[assembly: HostingStartup(typeof(BaseProjectApp.Admin.Areas.Identity.IdentityHostingStartup))]
namespace BaseProjectApp.Admin.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //services.AddDbContext<Context>(options =>
                //    options.UseSqlServer(
                //        context.Configuration.GetConnectionString("ContextConnection")));

                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<Context>();
            });
        }
    }
}
