using Application.Commands;
using Tests.Common;

namespace Application.UnitTests
{
    public class LoginCommandTests
    {
        [SetUp]
        public void Setup()
        {
        }        

        [Test]
        public void Constructor_PassParameters_ParametersAreSetAsProperty()
        {
            var sut = new LoginUserCommand(Constants.UserConstants.INVALID_EMAIL, Constants.UserConstants.INVALID_PASSWORD);

            sut.Email.Should().Be(Constants.UserConstants.INVALID_EMAIL);

            sut.Password.Should().Be(Constants.UserConstants.INVALID_PASSWORD);
        }

        [Test]
        public void Constructor_PassValidParameters_IsValidIsTrue()
        {            
            var sut = new LoginUserCommand(Constants.UserConstants.VALID_EMAIL, Constants.UserConstants.VALID_PASSWORD);
            
            sut.IsValid().Should().BeTrue();

        }

        public void Constructor_PassInvalidParameters_IsValidIsFalse()
        {
            var sut = new LoginUserCommand(Constants.UserConstants.INVALID_EMAIL, Constants.UserConstants.INVALID_PASSWORD);
            
            sut.IsValid().Should().BeFalse();
            sut.ValidationResult.Errors.Should().NotBeEmpty();
        }
    }
}