using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

using ZConnector.Data;
using ZConnector.Models.JWT;
using ZConnector.Repositories;
using ZConnector.Repositories.Interfaces;
using ZConnector.Services.JWT;


WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

var commonSettings = new CommonJwtSettings
{
    Issuer = builder.Configuration["Jwt:Issuer"],
    Audience = builder.Configuration["Jwt:Audience"],
#pragma warning disable CS8604
    Secret = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]),

    ExpirationDate = Convert.ToDouble(builder.Configuration["Jwt:TimeOut"])
};

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
    }
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            RequireExpirationTime = true,

            ValidIssuer = commonSettings.Issuer,
            ValidAudience = commonSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(commonSettings.Secret),
            ClockSkew = TimeSpan.Zero
        };

        options.RequireHttpsMetadata = true;
    }
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddSingleton(commonSettings);

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<IAuthManagerService, AuthManagerService>();

builder.Services.AddTransient<IAuthManagerService, AuthManagerService>();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

WebApplication? app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions // Security headers (behind reverse proxy support)
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

// Configure the HTTP request pipeline.

app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
