using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudgetAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HomeBudgetAPI
{
    public class Startup
    {
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

            services.AddAutoMapper(Assembly.Load("HomeBudget.Service"), Assembly.Load("HomeBudget.Core"));
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.Load("HomeBudgetAPI"), Assembly.Load("HomeBudget.Service"));
            });

            services.AddDbContext<HomeBudgetDbContext>(options =>
                options.UseSqlServer(_configuration.GetSection("ConnectionStrings")["HomeBudgetDbConnection"]));
            services.AddScoped<HomeBudgetDbContextSeeder>();

            services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
      .AddEntityFrameworkStores<HomeBudgetDbContext>();

            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
        }
        public void Configure(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<HomeBudgetDbContextSeeder>();
            seeder.Seed();

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
