using FluentValidation;
using FluentValidation.AspNetCore;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Service.Configurations;
using HomeBudget.Service.Services.UserServices;
using HomeBudgetAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace HomeBudgetAPI
{
    public class Startup
    {
        private const string _APIAssembly = "HomeBudgetAPI";
        private const string _serviceAssembly = "HomeBudget.Service";
        private const string _coreAssembly = "HomeBudget.Core";
        private const string _ConnectionStrings = "ConnectionStrings";
        private const string _dbConnection = "HomeBudgetDbConnection";

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            _configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(config =>
            {
                config.AddPolicy("AppClient", config =>
                {
                    config
                    .WithOrigins(_configuration.GetSection("AllowedOrigins").Value)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddAutoMapper(Assembly.Load(_serviceAssembly), Assembly.Load(_coreAssembly));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.Load(_APIAssembly), Assembly.Load(_serviceAssembly));
            });

            services.AddDbContext<HomeBudgetDbContext>(options =>
                options.UseSqlServer(_configuration.GetSection(_ConnectionStrings)[_dbConnection]));

            services.AddScoped<HomeBudgetDbContextSeeder>();

            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserService, UserService>();

            services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.Load(_serviceAssembly));
        }

        public async Task Configure(WebApplication app)
        {
            app.UseCors("AppClient");
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<HomeBudgetDbContextSeeder>();
            await seeder.SeedAsync();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}