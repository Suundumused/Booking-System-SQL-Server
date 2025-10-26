using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ZConnector.Data;
using ZConnector.Data.Transaction;
using ZConnector.GlobalHanlders;
using ZConnector.Models.JWT;
using ZConnector.Repositories;
using ZConnector.Repositories.Interfaces;
using ZConnector.Services.Client;
using ZConnector.Services.Client.Interfaces;
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

builder.Services.AddControllers()
.ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }
);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
        options.Filters.Add(typeof(GlobalEndPointHandler));
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
            ClockSkew = TimeSpan.Zero,

            NameClaimType = JwtRegisteredClaimNames.Sub,
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";

                return context.Response.WriteAsync("Authentication failed. Please log in again.");
            }
        };

        options.RequireHttpsMetadata = true;
    }
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddSingleton(commonSettings);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(new AutoMapping());
});

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<IAuthManagerService, AuthManagerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddTransient<IAuthManagerService, AuthManagerService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IBookingService, BookingService>();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();