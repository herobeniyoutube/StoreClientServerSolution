using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreServer.Models;

namespace StoreServer.Services
{
    /// <summary>
    /// Contains method for configuring all services for the builder
    /// </summary>
    public static class ServiceCollectinBuilder
    {
        /// <summary>
        /// Adds services to the builder
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\net7.0-windows\\", "");
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(applicationPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
     
            string connection = $"{configurationRoot["dbConnection"]}";

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(connection));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthentificationOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthentificationOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthentificationOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });
            builder.Services.AddTransient<JWTTokenConstructor>();
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddTransient<ProductsService>();
            builder.Services.AddTransient<OrdersService>();

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
