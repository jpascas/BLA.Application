using Application.Handlers;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using Tests.Common;
using Application.Abstractions;

namespace Application.UnitTests
{
    public class CreateUserHandlerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Handle_WithInvalidCommand_ShouldDoNothing()
        {            
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();            

            var sut = new CreateUserHandler(userRepoMock.Object, null);

            var cmd = new CreateUserCommand(Constants.UserConstants.INVALID_EMAIL, Constants.UserConstants.INVALID_PASSWORD);

            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);

            userRepoMock.Verify(p => p.GetByEmail(It.IsAny<string>()), Times.Never);
            userRepoMock.Verify(p => p.Create(It.IsAny<User>()), Times.Never);

        }

        [Test]
        public async Task Handle_WithExistentUser_ShouldAddError()
        {
            string testEmail = "test@test.com";
            var returnsFromMock = new User();
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetByEmail(testEmail)).Returns(Task.FromResult(returnsFromMock));



            var sut = new CreateUserHandler(userRepoMock.Object, null);

            var cmd = new CreateUserCommand(testEmail, Constants.UserConstants.VALID_PASSWORD);

            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);
            result.FailureMessages[0].Should().Be("User Already Exists");
                 
            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once);
            userRepoMock.Verify(p => p.Create(It.IsAny<User>()), Times.Never);

        }

        [Test]
        public async Task Handle_WithNonExistentUser_ShouldCallCreate()
        {
            string testEmail = "test@test.com";
            User createdUser = new User();
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetByEmail(testEmail)).Returns(Task.FromResult<User>(null));
            userRepoMock.Setup(m => m.Create(It.IsAny<User>())).Returns(Task.FromResult<User>(createdUser));

            Mock<IPasswordHasher> hasherMock = new Mock<IPasswordHasher>();
            hasherMock.Setup(m => m.Hash(Constants.UserConstants.VALID_PASSWORD)).Returns(Constants.UserConstants.VALID_PASSWORD);
            var sut = new CreateUserHandler(userRepoMock.Object, hasherMock.Object);

            var cmd = new CreateUserCommand(testEmail, Constants.UserConstants.VALID_PASSWORD);

            var result = await sut.Handle(cmd);

            result.Success.Should().BeTrue();
            result.Result.Should().Be(createdUser);
            result.FailureMessages.Should().BeEmpty();

            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once);
            userRepoMock.Verify(p => p.Create(It.IsAny<User>()), Times.Once);

        }
    }
}
