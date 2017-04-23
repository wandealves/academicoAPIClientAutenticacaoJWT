using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AcademicoAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Autenticação
            var key = Encoding.UTF8.GetBytes(Configuration["Auth:SecurityKey"]);
            services.AddSingleton<JwtSettings>(new JwtSettings(new SymmetricSecurityKey(key)));
            services.AddSingleton<AuthenticationHandler>();
            services.AddTransient<JwtSecurityTokenHandler>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,AuthenticationHandler handler, JwtSettings jwtSettings)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Autenticação
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = jwtSettings.SecurityKey,
                ValidAudience = jwtSettings.Audience,
                ValidIssuer = jwtSettings.Issuer
            };

            var options = new JwtBearerOptions
            {
                Events = handler,
                TokenValidationParameters = validationParameters
            };

            app.UseJwtBearerAuthentication(options);

            // Add framework services.
            app.UseMvc();
        }
    }
}
