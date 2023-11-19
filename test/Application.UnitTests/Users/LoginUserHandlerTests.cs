using Application.Handlers;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using Tests.Common;
using Application.Abstractions;

namespace Application.UnitTests
{
    public class LoginUserHandlerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Login_WithInvalidCommand_ShouldReturnFailure()
        {
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            Mock<IJwtProvider> jwtProviderMock = new Mock<IJwtProvider>();
            Mock<IPasswordHasher> passwordHasherMock = new Mock<IPasswordHasher>();

            var sut = new LoginUserHandler(userRepoMock.Object, jwtProviderMock.Object, passwordHasherMock.Object);

            var cmd = new LoginUserCommand(Constants.UserConstants.INVALID_EMAIL, Constants.UserConstants.INVALID_PASSWORD);

            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);  

            userRepoMock.Verify(p => p.GetByEmail(It.IsAny<string>()), Times.Never);
            passwordHasherMock.Verify(p => p.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            jwtProviderMock.Verify(p => p.Generate(It.IsAny<User>()), Times.Never);

        }

        [Test]
        public async Task Login_WithNonExistentUser_ShouldReturnFailure()
        {
            string testEmail = "test@test.com";
            User returnsFromMock = null;
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetByEmail(testEmail)).Returns(Task.FromResult(returnsFromMock));


            Mock<IJwtProvider> jwtProviderMock = new Mock<IJwtProvider>();
            Mock<IPasswordHasher> passwordHasherMock = new Mock<IPasswordHasher>();

            var sut = new LoginUserHandler(userRepoMock.Object, jwtProviderMock.Object, passwordHasherMock.Object);

            var cmd = new LoginUserCommand(testEmail, Constants.UserConstants.VALID_PASSWORD);

            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);
            result.FailureMessages[0].Should().Be("User doesnt exists");    
            
            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once);
            passwordHasherMock.Verify(p => p.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            jwtProviderMock.Verify(p => p.Generate(It.IsAny<User>()), Times.Never); ;

        }

        [Test]
        public async Task Login_WithExistentUserAndValidPassword_ShouldCallCreate()
        {
            string testEmail = "test@test.com";
            string newToken = "token";
            User existentUser = new User() {  Email = testEmail , PasswordHash = Constants.UserConstants.VALID_PASSWORD };
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetByEmail(testEmail)).Returns(Task.FromResult<User>(existentUser));                        

            Mock<IJwtProvider> jwtProviderMock = new Mock<IJwtProvider>();
            jwtProviderMock.Setup(m => m.Generate(existentUser)).Returns(newToken);
            Mock<IPasswordHasher> passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(m => m.VerifyHashedPassword(existentUser.PasswordHash, Constants.UserConstants.VALID_PASSWORD)).Returns(true);

            var sut = new LoginUserHandler(userRepoMock.Object, jwtProviderMock.Object, passwordHasherMock.Object);

            var cmd = new LoginUserCommand(testEmail, Constants.UserConstants.VALID_PASSWORD);

            var result = await sut.Handle(cmd);

            result.Success.Should().BeTrue();
            result.Result.Should().Be(newToken);
            result.FailureMessages.Should().BeEmpty();

            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once);
            passwordHasherMock.Verify(p => p.VerifyHashedPassword(existentUser.PasswordHash, Constants.UserConstants.VALID_PASSWORD), Times.Once);
            jwtProviderMock.Verify(p => p.Generate(existentUser), Times.Once); ;

        }
    }
}
