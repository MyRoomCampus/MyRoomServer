using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyRoomServer.Entities.Contexts;
using MyRoomServer.Extentions;
using MyRoomServer.Hubs;
using MyRoomServer.Models;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"AllowedHosts: {builder.Configuration["AllowedHosts"]}");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {builder.Configuration["ASPNETCORE_ENVIRONMENT"]}");

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
                      {
                          policy.WithOrigins(builder.Configuration["AllowedOrigins"].Split(';'));
                          policy.AllowCredentials();
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});
builder.Services.AddDbContext<MyRoomDbContext>(opt =>
{
    opt.UseMySql(
        builder.Configuration.GetConnectionString("MyRoom"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MyRoom"))
        );
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };

        var jwtConfig = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtConfig.GetValue<string>("Audience"),
            ValidIssuer = jwtConfig.GetValue<string>("Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("SecurityKey")))
        };
    });

builder.Services.AddTokenFactory(options =>
{
    var config = builder.Configuration.GetSection("Jwt");
    options.Issuer = config.GetValue<string>("Issuer");
    options.Audience = config.GetValue<string>("Audience");
    options.SigningKey = config.GetValue<string>("SecurityKey");
    options.AccessTokenExpire = config.GetValue<int>("AccessTokenExpire");
    options.RefreshTokenExpire = config.GetValue<int>("RefreshTokenExpire");
    options.RefreshTokenBefore = config.GetValue<int>("RefreshTokenBefore");
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityPolicyNames.CommonUser, policy =>
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireClaim("TokenType", "AccessToken");
        policy.RequireClaim("UserName");
    });
    options.AddPolicy(IdentityPolicyNames.RefreshTokenOnly, policy =>
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireClaim("TokenType", "RefreshToken");
    });
});

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<ProjectHub>("/hub/project");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<MyRoomDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();
