using Application.Abstractions;
using Application.Commands;
using Application;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Queries;
using Presentation.ViewModels.Prescriptions;
using Tests.Common;

namespace Presentation.UnitTests
{
    public class PrescriptionsControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Create_WithInvalidInput_ShouldReturnStatus500AndError()
        {
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<CreatePrescriptionCommand, Prescription>(It.IsAny<CreatePrescriptionCommand>())).Returns(Task.FromResult(OperationResult<Prescription>.FailureResult("")));
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            var sut = new PrescriptionsController(commandBusMock.Object, mapperMock.Object, null);
            var sutInput = new CreatePrescriptionRequestModel() { Dosage = "", Drug = "", Notes = "" };
            var result = (await sut.Create(sutInput)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeOfType(typeof(ErrorResult));
            ((ErrorResult)result.Value).Messages.Should().NotBeEmpty();
            commandBusMock.Verify(p => p.Send<CreatePrescriptionCommand, Prescription>(It.IsAny<CreatePrescriptionCommand>()), Times.Once());
        }

        [Test]
        public async Task Create_WithValidInput_ShouldReturnStatus200AndPrescription()
        {
            var returnsFromMock = new Prescription();
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<CreatePrescriptionCommand, Prescription>(It.IsAny<CreatePrescriptionCommand>())).Returns(Task.FromResult(OperationResult<Prescription>.SuccessResult(returnsFromMock)));

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var returnsFromMapperMock = new PrescriptionResultModel() { };
            mapperMock.Setup(m => m.Map<Prescription, PrescriptionResultModel>(returnsFromMock)).Returns(returnsFromMapperMock);

            var sut = new PrescriptionsController(commandBusMock.Object, mapperMock.Object, null);
            var sutInput = new CreatePrescriptionRequestModel() { Dosage = "Dosage", Drug = "Drug", Notes = "Notes" };
            var result = (await sut.Create(sutInput)) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType(typeof(PrescriptionResultModel));
            result.Value.Should().Be(returnsFromMapperMock);
            commandBusMock.Verify(p => p.Send<CreatePrescriptionCommand, Prescription>(It.IsAny<CreatePrescriptionCommand>()), Times.Once());
            mapperMock.Verify(p => p.Map<Prescription, PrescriptionResultModel>(returnsFromMock), Times.Once());
        }

        [Test]
        public async Task GetAllByCurrentUser_WithContent_ShouldReturn200AndPrescriptions()
        {
            Mock<IPrescriptionQueryService> prescriptionQueryServiceMock = new Mock<IPrescriptionQueryService>();
            var returnsFromQueryMock = new Prescription();
            prescriptionQueryServiceMock.Setup(m => m.FindByCurrentUserId()).Returns(Task.FromResult(new List<Prescription>() { returnsFromQueryMock }));
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var returnsFromMapperMock = new PrescriptionResultModel() { };
            mapperMock.Setup(m => m.Map<Prescription, PrescriptionResultModel>(returnsFromQueryMock)).Returns(returnsFromMapperMock);

            var sut = new PrescriptionsController(null, mapperMock.Object, prescriptionQueryServiceMock.Object);
            var result = (await sut.GetAllByCurrentUser()) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType(typeof(List<PrescriptionResultModel>));
            ((List<PrescriptionResultModel>)result.Value)[0].Should().Be(returnsFromMapperMock);
            prescriptionQueryServiceMock.Verify(p=>p.FindByCurrentUserId(), Times.Once());
            mapperMock.Verify(p => p.Map<Prescription, PrescriptionResultModel>(returnsFromQueryMock), Times.Once());
        }


        [Test]
        public async Task GetById_WithContent_ShouldReturn200AndPrescriptions()
        {
            Guid prescriptionId = Guid.NewGuid();
            Mock<IPrescriptionQueryService> prescriptionQueryServiceMock = new Mock<IPrescriptionQueryService>();
            var returnsFromQueryMock = new Prescription();
            prescriptionQueryServiceMock.Setup(m => m.FindById(prescriptionId)).Returns(Task.FromResult(returnsFromQueryMock));
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var returnsFromMapperMock = new PrescriptionResultModel() { };
            mapperMock.Setup(m => m.Map<Prescription, PrescriptionResultModel>(returnsFromQueryMock)).Returns(returnsFromMapperMock);

            var sut = new PrescriptionsController(null, mapperMock.Object, prescriptionQueryServiceMock.Object);

            var result = (await sut.GetById(prescriptionId)) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType(typeof(PrescriptionResultModel));
            result.Value.Should().Be(returnsFromMapperMock);
            prescriptionQueryServiceMock.Verify(p => p.FindById(prescriptionId), Times.Once());
            mapperMock.Verify(p => p.Map<Prescription, PrescriptionResultModel>(returnsFromQueryMock), Times.Once());
        }

        [Test]
        public async Task GetById_WithNoContent_ShouldReturn400()
        {
            Guid prescriptionId = Guid.NewGuid();
            Mock<IPrescriptionQueryService> prescriptionQueryServiceMock = new Mock<IPrescriptionQueryService>();                        
            Mock<IMapper> mapperMock = new Mock<IMapper>();            

            var sut = new PrescriptionsController(null, mapperMock.Object, prescriptionQueryServiceMock.Object);

            var result = (await sut.GetById(prescriptionId)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);            
            prescriptionQueryServiceMock.Verify(p => p.FindById(prescriptionId), Times.Once());
            mapperMock.Verify(p => p.Map<Prescription, PrescriptionResultModel>(It.IsAny<Prescription>()), Times.Never());
        }

        [Test]
        public async Task Update_WithValidInput_ShouldReturnStatus200AndPrescription()
        {
            var prescriptionId = Guid.NewGuid();
            var returnsFromMock = new Prescription() { Id = prescriptionId };
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<UpdatePrescriptionCommand, Prescription>(It.IsAny<UpdatePrescriptionCommand>())).Returns(Task.FromResult(OperationResult<Prescription>.SuccessResult(returnsFromMock)));

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            var returnsFromMapperMock = new PrescriptionResultModel() { Id = prescriptionId };
            mapperMock.Setup(m => m.Map<Prescription, PrescriptionResultModel>(returnsFromMock)).Returns(returnsFromMapperMock);

            var sut = new PrescriptionsController(commandBusMock.Object, mapperMock.Object, null);
            var sutInput = new UpdatePrescriptionRequestModel() { Id = prescriptionId,  Dosage = "Dosage",Notes = "Notes" };
            var result = (await sut.Update(sutInput)) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType(typeof(PrescriptionResultModel));
            result.Value.Should().Be(returnsFromMapperMock);
            commandBusMock.Verify(p => p.Send<UpdatePrescriptionCommand, Prescription>(It.IsAny<UpdatePrescriptionCommand>()), Times.Once());
            mapperMock.Verify(p => p.Map<Prescription, PrescriptionResultModel>(returnsFromMock), Times.Once());
        }

        [Test]
        public async Task Update_WithInvalidInput_ShouldReturnStatus500AndError()
        {
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<UpdatePrescriptionCommand, Prescription>(It.IsAny<UpdatePrescriptionCommand>())).Returns(Task.FromResult(OperationResult<Prescription>.FailureResult("")));
            Mock<IMapper> mapperMock = new Mock<IMapper>();

            var sut = new PrescriptionsController(commandBusMock.Object, mapperMock.Object, null);
            var sutInput = new UpdatePrescriptionRequestModel() { Id = Guid.Empty, Dosage = "", Notes = "" };
            var result = (await sut.Update(sutInput)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeOfType(typeof(ErrorResult));
            ((ErrorResult)result.Value).Messages.Should().NotBeEmpty();
            commandBusMock.Verify(p => p.Send<UpdatePrescriptionCommand, Prescription>(It.IsAny<UpdatePrescriptionCommand>()), Times.Once());
        }

        [Test]
        public async Task Delete_WithValidInput_ShouldReturnStatus200()
        {
            var prescriptionId = Guid.NewGuid();
            var returnsFromMock = new Prescription() { Id = prescriptionId };
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<DeletePrescriptionCommand, Nothing>(It.IsAny<DeletePrescriptionCommand>())).Returns(Task.FromResult(OperationResult<Nothing>.SuccessResult(null)));
            

            var sut = new PrescriptionsController(commandBusMock.Object, null, null);            
            var result = (await sut.Delete(prescriptionId)) as OkResult;

            result.StatusCode.Should().Be(200);                        
            commandBusMock.Verify(p => p.Send<DeletePrescriptionCommand, Nothing>(It.IsAny<DeletePrescriptionCommand>()), Times.Once());            
        }


        [Test]
        public async Task Delete_WithInvalidInput_ShouldReturnStatus500AndError()
        {
            var prescriptionId = Guid.NewGuid();
            var returnsFromMock = new Prescription() { Id = prescriptionId };
            Mock<ICommandBus> commandBusMock = new Mock<ICommandBus>();
            commandBusMock.Setup(s => s.Send<DeletePrescriptionCommand, Nothing>(It.IsAny<DeletePrescriptionCommand>())).Returns(Task.FromResult(OperationResult<Nothing>.FailureResult("")));


            var sut = new PrescriptionsController(commandBusMock.Object, null, null);            
            var result = (await sut.Delete(prescriptionId)) as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(500);
            result.Value.Should().BeOfType(typeof(ErrorResult));
            ((ErrorResult)result.Value).Messages.Should().NotBeEmpty();
            commandBusMock.Verify(p => p.Send<DeletePrescriptionCommand, Nothing>(It.IsAny<DeletePrescriptionCommand>()), Times.Once());
        }
    }
}
