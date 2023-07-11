using AutoMapper;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.API.Helpers;
using BaseProjectApp.API.Middlewares.ExceptionHandler;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
ConfigurationManager _StartupConfiguration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Base Project APIs documentation 1.0.0", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Add 'Bearer ' into textbox before token value => (Bearer #GivenToken)",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                {
                new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                }
                },
                new string[] {}
                }
                });
});

builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseSqlServer(
      _StartupConfiguration.GetConnectionString("ContextConnection"))
 );

builder.Services.AddDbContext<BaseProjectDBContext>();
builder.Services.AddTransient<IUnitofWork, UnitofWork>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddTokenProvider("BaseProjectApp.API", typeof(DataProtectorTokenProvider<IdentityUser>))
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();


//_ = builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
//    options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
//})
//.AddApiKeySupport(options => { });


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}) 
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = _StartupConfiguration["JWT:ValidAudience"],
        ValidIssuer = _StartupConfiguration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_StartupConfiguration["JWT:Secret"]))
    };
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddScoped<IAuthorizationHandler, PermissionsAuthorizationHandler>();

//builder.Services.AddMemoryCache();x`

builder.Services.AddAuthorization(options =>
{

    var PermessionsClasses = typeof(Auth_Permissions).GetNestedTypes();

    foreach (var TypeClass in PermessionsClasses)
    {
        var properties = TypeClass.GetProperties();

        foreach (PropertyInfo property in properties)
        {

            var propertyName = property.Name;
            object ob = Activator.CreateInstance(TypeClass);
            var propertyObject = ob.GetType().GetProperty(propertyName).GetValue(ob, null);

            string propertyValue = propertyObject.ToString();

            options.AddPolicy(propertyValue, builder =>
            {
                builder.AddRequirements(new PermissionsRequirement(propertyValue));
            });
        }
    }

});

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        //new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});


builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });


//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
//    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
//});


var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    //app.UseSwaggerUI();

    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            /// 
            //options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            //options.res

            string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
            options.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", description.GroupName.ToUpperInvariant());

        }
    });
}
//else if (app.Environment.IsProduction())
//{
//    app.UseSwaggerUI(options =>
//    {
//        foreach (var description in provider.ApiVersionDescriptions)
//        {
//            //string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
//            //options.SwaggerEndpoint($"{swaggerJsonBasePath}/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
//            options.SwaggerEndpoint($"http://BaseProject.cloudsystems.tech/api/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
//        }
//    });
//}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
