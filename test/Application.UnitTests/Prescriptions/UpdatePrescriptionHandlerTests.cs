using Application.Handlers;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using Tests.Common;
using Application.Abstractions;

namespace Application.UnitTests
{
    public class UpdatePrescriptionHandlerTestsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Handle_WithInvalidCommand_ShouldDoNothing()
        {
            Mock<IPrescriptionRepository> prescriptionRepoMock = new Mock<IPrescriptionRepository>();
            Mock<IContextProvider> contextProvider = new Mock<IContextProvider>();
            var cmd = new UpdatePrescriptionCommand(Guid.Empty, "", "");

            var sut = new UpdatePrescriptionHandler(prescriptionRepoMock.Object, contextProvider.Object);            
            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);

            contextProvider.Verify(p => p.GetCurrentUserId(), Times.Never);
            prescriptionRepoMock.Verify(p => p.Update(It.IsAny<Prescription>()), Times.Never);

        }     

        [Test]
        public async Task Handle_WithValidCommand_ShouldCallCreate()
        {
            Mock<IPrescriptionRepository> prescriptionRepoMock = new Mock<IPrescriptionRepository>();
            var returnFromMock = new Prescription();
            prescriptionRepoMock.Setup(m => m.Update(It.IsAny<Prescription>())).Returns(Task.FromResult(returnFromMock));
            Mock<IContextProvider> contextProvider = new Mock<IContextProvider>();
            contextProvider.Setup(m => m.GetCurrentUserId()).Returns(1);

            var cmd = new UpdatePrescriptionCommand(Guid.NewGuid(), "dosage", "notes");

            var sut = new UpdatePrescriptionHandler(prescriptionRepoMock.Object, contextProvider.Object);
            var result = await sut.Handle(cmd);            

            result.Success.Should().BeTrue();
            result.Result.Should().Be(returnFromMock);
            result.FailureMessages.Should().BeEmpty();

            contextProvider.Verify(p => p.GetCurrentUserId(), Times.Once);
            prescriptionRepoMock.Verify(p => p.Update(It.IsAny<Prescription>()), Times.Once);

        }
    }
}
