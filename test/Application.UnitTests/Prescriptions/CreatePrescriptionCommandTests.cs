using Application.Commands;
using Tests.Common;

namespace Application.UnitTests
{
    public class CreatePrescriptionCommandTests
    {
        [SetUp]
        public void Setup()
        {
        }        

        [Test]
        public void Constructor_PassParameters_ParametersAreSetAsProperty()
        {
            var sut = new CreatePrescriptionCommand("drug","dosage", "notes");

            sut.Drug.Should().Be("drug");
            sut.Dosage.Should().Be("dosage");
            sut.Notes.Should().Be("notes");            
        }

        [Test]
        public void Constructor_PassValidParameters_IsValidIsTrue()
        {
            var sut = new CreatePrescriptionCommand("drug", "dosage", "notes");

            sut.IsValid().Should().BeTrue();

        }

        [Test]
        public void Constructor_PassInvalidParameters_IsValidIsFalse()
        {
            var sut = new CreatePrescriptionCommand("", "", "");

            sut.IsValid().Should().BeFalse();
            sut.ValidationResult.Errors.Should().NotBeEmpty();
        }
    }
}