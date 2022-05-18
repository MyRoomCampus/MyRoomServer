using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyRoomServer.Entities;
using MyRoomServer.Extentions;
using MyRoomServer.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
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
    options.AddPolicy(IdentityPolicyNames.CommonUser, policy =>
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();