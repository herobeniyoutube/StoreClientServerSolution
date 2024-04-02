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
            string applicationPath = AppDomain.CurrentDomain.BaseDirectory; 
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(applicationPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            string connection = "";

            try
            {
                connection = configurationRoot["dbConnection"];
                if (connection == null )  throw new Exception();
            }
            catch (Exception ex) 
            { 
            Console.WriteLine(ex.ToString() + "Has no connection to db");
            }

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
            builder.Services.AddScoped<JWTTokenConstructor>();
            builder.Services.AddSingleton<UsersService>();
            builder.Services.AddSingleton<ProductsService>();
            builder.Services.AddSingleton<OrdersService>();

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }
    }
}
