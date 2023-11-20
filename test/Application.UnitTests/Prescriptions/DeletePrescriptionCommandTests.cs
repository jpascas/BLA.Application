using Application.Commands;
using Tests.Common;

namespace Application.UnitTests
{
    public class DeletePrescriptionCommandTests
    {
        [SetUp]
        public void Setup()
        {
        }        

        [Test]
        public void Constructor_PassParameters_ParametersAreSetAsProperty()
        {
            var newGuid = Guid.NewGuid();
            var sut = new DeletePrescriptionCommand(newGuid);

            sut.Id.Should().Be(newGuid);                      
        }

        [Test]
        public void Constructor_PassValidParameters_IsValidIsTrue()
        {
            var newGuid = Guid.NewGuid();
            var sut = new DeletePrescriptionCommand(newGuid);

            sut.IsValid().Should().BeTrue();

        }

        [Test]
        public void Constructor_PassInvalidParameters_IsValidIsFalse()
        {
            var newGuid = Guid.Empty;
            var sut = new DeletePrescriptionCommand(newGuid);

            sut.IsValid().Should().BeFalse();
            sut.ValidationResult.Errors.Should().NotBeEmpty();
        }
    }
}