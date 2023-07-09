using FluentAssertions;
using HomeBudget.Core;
using HomeBudget.Core.Entities;
using HomeBudget.Core.Enums;
using HomeBudget.Service.Actions.UserActions.Login;
using HomeBudget.Service.Actions.UserActions.Register;
using HomeBudget.Service.ModelsDTO.UserModels;
using HomeBudget.Service.Services.UserServices;
using HomeBudgetAPI.IntegrationTests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HomeBudgetAPI.IntegrationTests.ControllersTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private readonly IServiceScopeFactory? _scopefactory;
        private Mock<IRequestHandler<UserLoginCommand, AppUserDTO>> _userLoginServiceMock = new();
        private readonly HomeBudgetDbContext _dbContext;

        public UserControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                         .Single(service => service.ServiceType == typeof(DbContextOptions<HomeBudgetDbContext>));

                        services.Remove(dbContextOptions);

                        services.AddSingleton(_userLoginServiceMock.Object);

                        services.AddDbContext<HomeBudgetDbContext>(options => options.UseInMemoryDatabase("HomeBudgetDbControllerTest"));
                    });
                })
                .CreateClient();

            _scopefactory = factory.Services.GetService<IServiceScopeFactory>();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<HomeBudgetDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase("HomeBudgetDbControllerTest");

            _dbContext = new HomeBudgetDbContext(dbContextOptionsBuilder.Options);

            Seed();
        }

        private void Seed()
        {
            if (!_dbContext.Users.Any())
            {
                using var scope = _scopefactory.CreateScope();

                var userService = scope?.ServiceProvider.GetService(typeof(IUserService)) as IUserService;
                var user = new User()
                {
                    Email = "test1@mail.com",
                    FirstName = "Piotr",
                    LastName = "Nowicki",
                    Role = userService.GetRole(UserRoleEnum.User)
                };
                user.PasswordHash = userService.HashPassword(user, "hashedPassword123");

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
        }

        #region RegsiterUserTests

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            // arrange
            var regiterUserCommand = new UserRegisterCommand()
            {
                NewUser = new NewUserDTO()
                {
                    Email = "test@mail.com",
                    Password = "password",
                    ConfirmPassword = "password",
                    FirstName = "Adam",
                    LastName = "Nowak",
                }
            };

            var httpContent = regiterUserCommand.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("user/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
        {
            // arrange
            var regiterUserCommand = new UserRegisterCommand()
            {
                NewUser = new NewUserDTO()
                {
                    Email = "testmail.com",
                    Password = "password",
                    ConfirmPassword = "password1",
                    FirstName = "",
                    LastName = null,
                }
            };

            var httpContent = regiterUserCommand.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("user/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RegisterUser_ForWrongModel_ReturnsInternalServerError()
        {
            // arrange

            var regiterUser = new NewUserDTO()
            {
                Email = "testmail.com",
                Password = "password",
                ConfirmPassword = "password1",
                FirstName = "",
                LastName = null,
            };

            var httpContent = regiterUser.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("user/register", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
        }

        #endregion RegsiterUserTests

        #region LoginUserTests

        [Fact]
        public async Task Login_ForRegisterUserWithValidModel_ReturnsOk()
        {
            // arrange

            _userLoginServiceMock
               .Setup(e => e.Handle(It.IsAny<UserLoginCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new AppUserDTO());

            var userLoginCommand = new UserLoginCommand()
            {
                Email = "test1@mail.com",
                Password = "hashedPassword123",
            };

            var httpContent = userLoginCommand.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("user/login", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_ForRegisterUserWithInvalidModel_ReturnsBadRequest()
        {
            // arrange

            _userLoginServiceMock
               .Setup(e => e.Handle(It.IsAny<UserLoginCommand>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new AppUserDTO());

            var userLoginCommand = new UserLoginCommand()
            {
                Email = "testmail.com",
                Password = "",
            };

            var httpContent = userLoginCommand.ToJsonHttpContent();

            // act

            var response = await _client.PostAsync("user/login", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        #endregion LoginUserTests
    }
}