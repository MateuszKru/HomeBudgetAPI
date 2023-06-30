using FluentAssertions;
using HomeBudget.Core;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace HomeBudgetAPI.IntegrationTests.ControllersTests
{
    public class BudgetControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public BudgetControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                         .Single(service => service.ServiceType == typeof(DbContextOptions<HomeBudgetDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddDbContext<HomeBudgetDbContext>(options => options.UseInMemoryDatabase("HomeBudgetDb"));
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAllBudgetsList_ReturnsOkResult()
        {
            // arrange

            // act

            var response = await _client.GetAsync("/Budget/GetAllBudgetsList");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}