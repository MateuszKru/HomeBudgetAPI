using FluentValidation;
using FluentValidation.AspNetCore;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Service.Services.UserServices;
using HomeBudgetAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

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

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}