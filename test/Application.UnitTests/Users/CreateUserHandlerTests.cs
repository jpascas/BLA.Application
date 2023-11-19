using Application.Handlers;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using Tests.Common;

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

            var sut = new CreateUserHandler(userRepoMock.Object);

            var cmd = new CreateUserCommand(Constants.UserConstants.INVALID_EMAIL, Constants.UserConstants.INVALID_PASSWORD);

            await sut.Handle(cmd);
            
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



            var sut = new CreateUserHandler(userRepoMock.Object);

            var cmd = new CreateUserCommand(testEmail, Constants.UserConstants.VALID_PASSWORD);

            await sut.Handle(cmd);


            cmd.Errors.Should().HaveCount(1);
            cmd.Errors[0].Message.Should().Be("User Already Exists");
            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once);
            userRepoMock.Verify(p => p.Create(It.IsAny<User>()), Times.Never);

        }

        [Test]
        public async Task Handle_WithNonExistentUser_ShouldCallCreate()
        {
            string testEmail = "test@test.com";            
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetByEmail(testEmail)).Returns(Task.FromResult<User>(null));

            var sut = new CreateUserHandler(userRepoMock.Object);

            var cmd = new CreateUserCommand(testEmail, Constants.UserConstants.VALID_PASSWORD);

            await sut.Handle(cmd);
            
            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once);
            userRepoMock.Verify(p => p.Create(It.IsAny<User>()), Times.Once);

        }
    }
}
