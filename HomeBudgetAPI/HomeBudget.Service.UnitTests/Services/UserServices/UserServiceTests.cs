using Xunit;
using HomeBudget.Service.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

using HomeBudgetAPI;
using HomeBudget.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using HomeBudget.Core.Entities;
using FluentAssertions;
using HomeBudget.Core.Enums;

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