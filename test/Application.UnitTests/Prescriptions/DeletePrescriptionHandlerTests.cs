using Application.Handlers;
using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using Application.Commands;
using Tests.Common;
using Application.Abstractions;

namespace Application.UnitTests
{
    public class DeletePrescriptionHandlerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Handle_WithInvalidCommand_ShouldReturnFailure()
        {
            Mock<IPrescriptionRepository> prescriptionRepoMock = new Mock<IPrescriptionRepository>();
            Mock<IContextProvider> contextProvider = new Mock<IContextProvider>();
            var cmd = new DeletePrescriptionCommand(Guid.Empty);

            var sut = new DeletePrescriptionHandler(prescriptionRepoMock.Object, contextProvider.Object);            
            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);

            contextProvider.Verify(p => p.GetCurrentUserId(), Times.Never);
            prescriptionRepoMock.Verify(p => p.Delete(It.IsAny<Guid>()), Times.Never);

        }     

        [Test]
        public async Task Handle_WithValidCommand_ShouldCallDelete()
        {
            var userId = 1;
            var prescriptionId = Guid.NewGuid();
            var existentPrescription = new Prescription() { Id = prescriptionId, UserId = userId };
            Mock<IPrescriptionRepository> prescriptionRepoMock = new Mock<IPrescriptionRepository>();
            prescriptionRepoMock.Setup(m => m.GetById(prescriptionId)).Returns(Task.FromResult(existentPrescription));
            Mock<IContextProvider> contextProvider = new Mock<IContextProvider>();
            contextProvider.Setup(m => m.GetCurrentUserId()).Returns(userId);            
            var cmd = new DeletePrescriptionCommand(prescriptionId);

            var sut = new DeletePrescriptionHandler(prescriptionRepoMock.Object, contextProvider.Object);
            var result = await sut.Handle(cmd);            

            result.Success.Should().BeTrue();
            result.Result.Should().Be(null);
            result.FailureMessages.Should().BeEmpty();

            contextProvider.Verify(p => p.GetCurrentUserId(), Times.Once);
            prescriptionRepoMock.Verify(p => p.GetById(prescriptionId), Times.Once);
            prescriptionRepoMock.Verify(p => p.Delete(prescriptionId), Times.Once);

        }

        [Test]
        public async Task Handle_WithNonExistantPrescription_ShouldCallCreate()
        {
            Mock<IPrescriptionRepository> prescriptionRepoMock = new Mock<IPrescriptionRepository>();
            Mock<IContextProvider> contextProvider = new Mock<IContextProvider>();
            contextProvider.Setup(m => m.GetCurrentUserId()).Returns(1);
            var prescriptionId = Guid.NewGuid();
            var cmd = new DeletePrescriptionCommand(prescriptionId);

            var sut = new DeletePrescriptionHandler(prescriptionRepoMock.Object, contextProvider.Object);
            var result = await sut.Handle(cmd);

            result.Success.Should().BeFalse();
            result.FailureMessages.Should().HaveCount(1);

            contextProvider.Verify(p => p.GetCurrentUserId(), Times.Once);
            prescriptionRepoMock.Verify(p => p.Delete(prescriptionId), Times.Never);

        }
    }
}
