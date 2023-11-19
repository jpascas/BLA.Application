using Application;
using Application.Commands;
using Application.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints;
using Presentation.ViewModels;
using Tests.Common;

namespace Presentation.UnitTests
{
    public class UsersControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Create_WithInvalidInput_ShouldReturnStatus500AndError()
        {
            Mock<IUserQueryService> userQueryServiceMock = new Mock<IUserQueryService>();
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var sut = new UsersController(commandBusMock.Object, userQueryServiceMock.Object, mapperMock.Object);

            var sutInput = new ApplicationUserInsertModel() { Email = Constants.UserConstants.INVALID_EMAIL , Password = Constants.UserConstants.INVALID_PASSWORD };
            var result = (await sut.Create(sutInput)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeOfType(typeof(ErrorResult));
            ((ErrorResult)result.Value).Messages.Should().NotBeEmpty();
            commandBusMock.Verify(p => p.Send<CreateUserCommand>(It.IsAny<CreateUserCommand>()), Times.Once());
        }

        [Test]
        public async Task Create_WithValidInput_ShouldReturnStatus200AndUser()
        {
            var returnsFromMock = new User();
            Mock<IUserQueryService> userQueryServiceMock = new Mock<IUserQueryService>();
            userQueryServiceMock.Setup(m => m.FindByEmail(Constants.UserConstants.VALID_EMAIL)).Returns(Task.FromResult(returnsFromMock));

            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            var returnsFromMapperMock = new ApplicationUserIResultModel() { Email = Constants.UserConstants.VALID_EMAIL };
            mapperMock.Setup(m => m.Map<User, ApplicationUserIResultModel>(returnsFromMock)).Returns(returnsFromMapperMock);
            var sut = new UsersController(commandBusMock.Object, userQueryServiceMock.Object, mapperMock.Object);

            var sutInput = new ApplicationUserInsertModel() { Email = Constants.UserConstants.VALID_EMAIL, Password = Constants.UserConstants.VALID_PASSWORD };
            var result = (await sut.Create(sutInput)) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType(typeof(ApplicationUserIResultModel));
            ((ApplicationUserIResultModel)result.Value).Email.Should().Be(Constants.UserConstants.VALID_EMAIL);
            commandBusMock.Verify(p => p.Send<CreateUserCommand>(It.IsAny<CreateUserCommand>()), Times.Once());
            userQueryServiceMock.Verify(p => p.FindByEmail(Constants.UserConstants.VALID_EMAIL), Times.Once());
            mapperMock.Verify(p => p.Map<User, ApplicationUserIResultModel>(It.IsAny<User>()), Times.Once());
        }
    }
}