using FluentAssertions;
using HomeBudget.Core.Entities;
using HomeBudgetAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace HomeBudget.Service.Services.UserServices.Tests
{
    public class UserServiceTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IServiceScopeFactory? _scopefactory;

        public UserServiceTests(WebApplicationFactory<Startup> factory)
        {
            _scopefactory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped<IUserService, UserService>();
                    });
                }).Services.GetService<IServiceScopeFactory>();
        }

        private IUserService? GetUserService()
        {
            using var scope = _scopefactory?.CreateScope();

            return scope?.ServiceProvider.GetService(typeof(IUserService)) as IUserService;
        }

        [Fact]
        public void GenerateTokenJWT_ForNotNullArgument_ReturnsTokenJWT()
        {
            // arrange

            var userService = GetUserService();

            // act

            var result = userService?.GenerateTokenJWT(new User());

            // assert

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GenerateTokenJWT_ForNullArgument_ThrowsNullReferenceException()
        {
            // arrange

            var userService = GetUserService();

            // act

            var result = () => userService?.GenerateTokenJWT(null);

            // assert

            result.Should().Throw<NullReferenceException>();
        }
    }
}