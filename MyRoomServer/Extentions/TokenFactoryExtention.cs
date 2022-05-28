using MyRoomServer.Entities.Contexts;
using MyRoomServer.Models;
using MyRoomServer.Services;

namespace MyRoomServer.Extentions
{
    public static class TokenFactoryExtention
    {
        public static IServiceCollection AddTokenFactory(this IServiceCollection services, Action<TokenFactoryConfiguration> options)
        {
            var config = new TokenFactoryConfiguration();
            options(config);
            if (config.Audience is null) throw new Exception(nameof(config.Audience));
            if (config.Issuer is null) throw new Exception(nameof(config.Issuer));
            if (config.SigningKey is null) throw new Exception(nameof(config.SigningKey));

            services.AddScoped<ITokenFactory, TokenFactory>(services =>
            {
                var dbContext = services.GetRequiredService<MyRoomDbContext>();
                return new TokenFactory(dbContext, config);
            });

            return services;
        }
    }
}
