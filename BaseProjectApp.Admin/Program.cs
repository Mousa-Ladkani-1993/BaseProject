using AutoMapper;
using BaseProjectApp.Admin.Areas.Identity.Data;
using BaseProjectApp.Admin.Areas.Identity.Model;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager _StartupConfiguration = builder.Configuration;
// Add builder.Services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<Context>(options =>
options.UseSqlServer(_StartupConfiguration.GetConnectionString("ContextConnection"))
);


//builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddHttpClient();

//builder.Services.AddDbContext<BaseProjectDBContext>();

builder.Services.AddTransient<BaseProjectDBContext>();

builder.Services.AddScoped<IUnitofWork, UnitofWork>();
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireDigit = false; 

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
    options.Lockout.MaxFailedAccessAttempts = 3;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<Context>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    // enables immediate logout, after updating the user's stat.
    options.ValidationInterval = TimeSpan.Zero;
});


builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
}).AddRazorRuntimeCompilation();

builder.Services.Configure<RazorViewEngineOptions>(o =>
{
    o.ViewLocationFormats.Add("/Pages/Shared/Components/{1}/{0}" + RazorViewEngine.ViewExtension);
});

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); 
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
 

app.UseHttpsRedirection();

//app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(""),
//                    //Path.Combine(Configuration.GetSection("MediaSettings:MEDIAFILESURL").Value, "Media", "Default")),
//    RequestPath = "/Media/Default"
//});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// app.UsePermissions();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});


//// global error handler
//app.UseMiddleware<ErrorHandler>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();