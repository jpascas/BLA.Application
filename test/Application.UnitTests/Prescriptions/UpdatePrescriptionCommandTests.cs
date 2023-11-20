using Application.Commands;
using Tests.Common;

namespace Application.UnitTests
{
    public class UpdatePrescriptionCommandTests
    {
        [SetUp]
        public void Setup()
        {
        }        

        [Test]
        public void Constructor_PassParameters_ParametersAreSetAsProperty()
        {
            var prescriptionGuid = Guid.NewGuid();
            var sut = new UpdatePrescriptionCommand(prescriptionGuid, "dosage", "notes");

            sut.Id.Should().Be(prescriptionGuid);
            sut.Dosage.Should().Be("dosage");
            sut.Notes.Should().Be("notes");            
        }

        [Test]
        public void Constructor_PassValidParameters_IsValidIsTrue()
        {
            var prescriptionGuid = Guid.NewGuid();
            var sut = new UpdatePrescriptionCommand(prescriptionGuid, "dosage", "notes");

            sut.IsValid().Should().BeTrue();

        }

        [Test]
        public void Constructor_PassInvalidParameters_IsValidIsFalse()
        {
            var sut = new UpdatePrescriptionCommand(Guid.Empty, "", "");

            sut.IsValid().Should().BeFalse();
            sut.ValidationResult.Errors.Should().NotBeEmpty();
        }
    }
}