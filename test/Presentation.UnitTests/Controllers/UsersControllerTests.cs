using Application;
using Application.Abstractions;
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
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();            
            commandBusMock.Setup(s=> s.Send<CreateUserCommand, User>(It.IsAny<CreateUserCommand>())).Returns(Task.FromResult(OperationResult<User>.FailureResult("")));
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var sut = new UsersController(commandBusMock.Object, mapperMock.Object);

            var sutInput = new ApplicationUserInsertRequestModel() { Email = Constants.UserConstants.INVALID_EMAIL , Password = Constants.UserConstants.INVALID_PASSWORD };
            var result = (await sut.Create(sutInput)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeOfType(typeof(ErrorResult));
            ((ErrorResult)result.Value).Messages.Should().NotBeEmpty();
            commandBusMock.Verify(p => p.Send<CreateUserCommand,User>(It.IsAny<CreateUserCommand>()), Times.Once());
        }

        [Test]
        public async Task Create_WithValidInput_ShouldReturnStatus200AndUser()
        {
            var returnsFromMock = new User();                        
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<CreateUserCommand, User>(It.IsAny<CreateUserCommand>())).Returns(Task.FromResult(OperationResult<User>.SuccessResult(returnsFromMock)));

            Mock<IMapper> mapperMock = new Mock<IMapper>();

            var returnsFromMapperMock = new ApplicationUserResultModel() { Email = Constants.UserConstants.VALID_EMAIL };
            mapperMock.Setup(m => m.Map<User, ApplicationUserResultModel>(returnsFromMock)).Returns(returnsFromMapperMock);
            var sut = new UsersController(commandBusMock.Object, mapperMock.Object);

            var sutInput = new ApplicationUserInsertRequestModel() { Email = Constants.UserConstants.VALID_EMAIL, Password = Constants.UserConstants.VALID_PASSWORD };
            var result = (await sut.Create(sutInput)) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType(typeof(ApplicationUserResultModel));
            ((ApplicationUserResultModel)result.Value).Email.Should().Be(Constants.UserConstants.VALID_EMAIL);
            commandBusMock.Verify(p => p.Send<CreateUserCommand, User>(It.IsAny<CreateUserCommand>()), Times.Once());            
            mapperMock.Verify(p => p.Map<User, ApplicationUserResultModel>(It.IsAny<User>()), Times.Once());
        }


        [Test]
        public async Task Login_WithInvalidInput_ShouldReturnStatus500AndError()
        {
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<LoginUserCommand, string>(It.IsAny<LoginUserCommand>())).Returns(Task.FromResult(OperationResult<string>.FailureResult("")));            
            var sut = new UsersController(commandBusMock.Object, null);

            var sutInput = new LoginRequestModel() { Email = Constants.UserConstants.INVALID_EMAIL, Password = Constants.UserConstants.INVALID_PASSWORD };
            var result = (await sut.Login(sutInput)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeOfType(typeof(ErrorResult));
            ((ErrorResult)result.Value).Messages.Should().NotBeEmpty();
            commandBusMock.Verify(p => p.Send<LoginUserCommand, string>(It.IsAny<LoginUserCommand>()), Times.Once());
        }

        [Test]
        public async Task Login_WithValidInput_ShouldReturnStatus200AndUser()
        {
            var returnsFromMock = "Token";
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<LoginUserCommand, string>(It.IsAny<LoginUserCommand>())).Returns(Task.FromResult(OperationResult<string>.SuccessResult(returnsFromMock)));            
            
            var sut = new UsersController(commandBusMock.Object, null);

            var sutInput = new LoginRequestModel() { Email = Constants.UserConstants.VALID_EMAIL, Password = Constants.UserConstants.VALID_PASSWORD };
            var result = (await sut.Login(sutInput)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);            
            (result.Value).Should().Be(returnsFromMock);
            commandBusMock.Verify(p => p.Send<LoginUserCommand, string>(It.IsAny<LoginUserCommand>()), Times.Once());            
        }
    }
}