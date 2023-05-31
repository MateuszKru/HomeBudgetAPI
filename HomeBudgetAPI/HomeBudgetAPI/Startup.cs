using HomeBudget.Core;
using Microsoft.EntityFrameworkCore;
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
            services.AddDbContext<HomeBudgetDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("HomeBudgetDbConnection")));
        }
        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
