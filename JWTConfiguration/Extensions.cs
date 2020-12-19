using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mongo_JWT.Models.DataBase;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_JWT.JWTConfiguration
{
    public static class Extentions
    {
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection("ConnectionString"));
            services.AddSingleton(sp =>
            {
                var options = sp.GetService<IOptions<DatabaseSettings>>();

                return new MongoClient(options.Value.ConnectionString);
            });
            services.AddSingleton(sp =>
            {
                var options = sp.GetService<IOptions<DatabaseSettings>>();
                var client = sp.GetService<MongoClient>();

                return client.GetDatabase(options.Value.DatabaseName);
            });
        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            var section = configuration.GetSection("jwt");
            section.Bind(options);
            services.Configure<JwtOptions>(section);
            services.AddSingleton<IJwtBuilder, JwtBuilder>();
            services.AddAuthentication()
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret))
                };
            });
        }
    }
}
