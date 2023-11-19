using Domain.Entities;
using Application.Commands;
using Application.Queries;
using Domain.Repositories;
using Moq;

namespace Application.UnitTests
{
    public class UserQueryServiceTests
    {        
        [SetUp]
        public void Setup()
        {            
        }

        [Test]
        public async Task FindByEmail_Should_CallRepositoryGetByEmail()
        {
            string testEmail = "test@test.com";
            var returnsFromMock = new User();
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetByEmail(testEmail)).Returns(Task.FromResult(returnsFromMock));

            var sut = new UserQueryService(userRepoMock.Object);

            var result = await sut.FindByEmail(testEmail);

            result.Should().Be(returnsFromMock);
            userRepoMock.Verify(p => p.GetByEmail(testEmail), Times.Once());

        }

        [Test]
        public async Task FindById_Should_CallRepositoryGetById()
        {
            long testId = 1;
            var returnsFromMock = new User();
            Mock<IUserRepository> userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(m => m.GetById(testId)).Returns(Task.FromResult(returnsFromMock));

            var sut = new UserQueryService(userRepoMock.Object);

            var result = await sut.FindById(testId);

            result.Should().Be(returnsFromMock);
            userRepoMock.Verify(p => p.GetById(testId), Times.Once());

        }

    }
}
