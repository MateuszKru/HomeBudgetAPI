using HomeBudgetAPI;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services); 

builder.Host.UseNLog();

var app = builder.Build();

startup.Configure(app);

app.Run();
