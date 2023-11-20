using Application.Queries;
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
using System.Net.Mime;
using System.Text.Json;
using Presentation.ViewModels.Prescriptions;
using Application.Abstractions;

namespace WebAPI.IntegrationTests.ControllerTests
{
    public class PrescriptionsControllerTests : BaseControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _client.DefaultRequestHeaders.Clear();
        }

        public async Task<PrescriptionResultModel> CreateNewPrescription(string token)
        {
            var prescriptionModel = GenerateRandomValidCreatePrescriptionModel();
            var response = await GetResponseFromCreatePrescription(prescriptionModel, token); ;
            var requestContent = await response.Content.ReadAsStringAsync();
            var createPrescriptionResult = JsonSerializer.Deserialize<PrescriptionResultModel>(requestContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            createPrescriptionResult.Id.Should().NotBeEmpty();
            createPrescriptionResult.Dosage.Should().Be(prescriptionModel.Dosage);
            createPrescriptionResult.Drug.Should().Be(prescriptionModel.Drug);
            createPrescriptionResult.Notes.Should().Be(prescriptionModel.Notes);
            return createPrescriptionResult;
        }

        [Test]
        public async Task Create_WithInvalidToken_ShouldReturnStatus401()
        {
            string token = string.Empty;
            var response = await GetResponseFromCreatePrescription(null, token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Create_WithValidInput_ShouldReturnStatus200AndPrescription()
        {
            string token = await new UsersControllerTests().LoginUser();
            await CreateNewPrescription(token);
        }

        [Test]
        public async Task Create_WithInvalidInput_ShouldReturnStatus400AndErrors()
        {
            string token = await new UsersControllerTests().LoginUser();
            var invalidPrescriptionModel = new CreatePrescriptionRequestModel();
            var response = await GetResponseFromCreatePrescription(invalidPrescriptionModel, token);
            var requestContent = await response.Content.ReadAsStringAsync();
            var errorsResult = JsonSerializer.Deserialize<ValidationErrorResult>(requestContent); // verify


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            errorsResult.Should().NotBeNull();
            errorsResult.ValidationErrors.Should().NotBeEmpty();
        }


        [Test]
        public async Task GetAllByCurrentUser_WithInvalidToken_ShouldReturnStatus401()
        {
            string token = string.Empty;
            var response = await GetResponseFromGetAllByCurrentUserPrescription(token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetAllByCurrentUser_WithContent_ShouldReturn200AndPrescriptions()
        {
            string token = await new UsersControllerTests().LoginUser();
            var newPrescription = await CreateNewPrescription(token);
            var response = await GetResponseFromGetAllByCurrentUserPrescription(token); ;
            var requestContent = await response.Content.ReadAsStringAsync();
            var createPrescriptionResult = JsonSerializer.Deserialize<List<PrescriptionResultModel>>(requestContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            createPrescriptionResult.Should().NotBeNull();
            createPrescriptionResult.Should().HaveCount(1);
            createPrescriptionResult[0].Should().BeEquivalentTo(newPrescription);
        }

        [Test]
        public async Task GetById_WithInvalidToken_ShouldReturnStatus401()
        {
            string token = string.Empty;
            var response = await GetResponseFromGetByIdPrescription(Guid.NewGuid(), token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetById_WithContent_ShouldReturn200AndPrescriptions()
        {
            string token = await new UsersControllerTests().LoginUser();
            var newPrescription = await CreateNewPrescription(token);
            var response = await GetResponseFromGetByIdPrescription(newPrescription.Id, token); ;
            var requestContent = await response.Content.ReadAsStringAsync();
            var createPrescriptionResult = JsonSerializer.Deserialize<PrescriptionResultModel>(requestContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            createPrescriptionResult.Should().BeEquivalentTo(newPrescription, options => options.ExcludingMissingMembers());

        }

        [Test]
        public async Task GetById_WithNonExistentPrescriptionId_ShouldReturn404()
        {
            string token = await new UsersControllerTests().LoginUser();
            var response = await GetResponseFromGetByIdPrescription(Guid.NewGuid(), token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Update_WithInvalidToken_ShouldReturnStatus401()
        {
            string token = string.Empty;
            var response = await GetResponseFromUpdatePrescription(null, token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Update_WithValidInput_ShouldReturnStatus200AndPrescription()
        {
            string token = await new UsersControllerTests().LoginUser();
            var newPrescription = await CreateNewPrescription(token);
            UpdatePrescriptionRequestModel modifiedModel = new UpdatePrescriptionRequestModel();
            modifiedModel.Id = newPrescription.Id;
            modifiedModel.Dosage = newPrescription.Dosage + "_modified";
            modifiedModel.Notes = newPrescription.Notes + "_modified";
            var response = await GetResponseFromUpdatePrescription(modifiedModel, token); ;
            var requestContent = await response.Content.ReadAsStringAsync();
            var updatedPrescriptionResult = JsonSerializer.Deserialize<PrescriptionResultModel>(requestContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            updatedPrescriptionResult.Should().BeEquivalentTo(modifiedModel, options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task Update_WithAnotherUserPrescription_ShouldReturnStatus404()
        {
            string token = await new UsersControllerTests().LoginUser();
            var newPrescription = await CreateNewPrescription(token);
            UpdatePrescriptionRequestModel modifiedModel = new UpdatePrescriptionRequestModel();
            modifiedModel.Id = newPrescription.Id;
            modifiedModel.Dosage = newPrescription.Dosage + "_modified";
            modifiedModel.Notes = newPrescription.Notes + "_modified";
            string anotherUsertoken = await new UsersControllerTests().LoginUser();
            var response = await GetResponseFromUpdatePrescription(modifiedModel, anotherUsertoken); ;            

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);            
        }


        [Test]
        public async Task Update_WithNonExistantPrescription_ShouldReturnStatus404()
        {
            string token = await new UsersControllerTests().LoginUser();            
            UpdatePrescriptionRequestModel modifiedModel = new UpdatePrescriptionRequestModel();
            modifiedModel.Id = Guid.NewGuid(); // random id
            modifiedModel.Dosage = "dosage_modified";
            modifiedModel.Notes = "notes_modified";            
            var response = await GetResponseFromUpdatePrescription(modifiedModel, token); ;

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Update_WithInvalidInput_ShouldReturnStatus400AndError()
        {
            string token = await new UsersControllerTests().LoginUser();
            var invalidPrescriptionModel = new UpdatePrescriptionRequestModel();
            var response = await GetResponseFromUpdatePrescription(invalidPrescriptionModel, token);
            var requestContent = await response.Content.ReadAsStringAsync();
            var errorsResult = JsonSerializer.Deserialize<ValidationErrorResult>(requestContent); // verify


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            errorsResult.Should().NotBeNull();
            errorsResult.ValidationErrors.Should().NotBeEmpty();
        }


        [Test]
        public async Task Delete_WithInvalidToken_ShouldReturnStatus401()
        {
            string token = string.Empty;
            var response = await GetResponseFromDeletePrescription(Guid.NewGuid(), token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Delete_WithNonExistentPrescriptionId_ShouldReturn404()
        {
            string token = await new UsersControllerTests().LoginUser();
            var response = await GetResponseFromDeletePrescription(Guid.NewGuid(), token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Delete_WithExistentPrescriptionId_ShouldReturn200()
        {
            string token = await new UsersControllerTests().LoginUser();
            var newPrescription = await CreateNewPrescription(token);
            var response = await GetResponseFromDeletePrescription(newPrescription.Id, token);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Test]
        public async Task Delete_WithAnotherUserPrescription_ShouldReturn404()
        {
            string token = await new UsersControllerTests().LoginUser();
            var newPrescription = await CreateNewPrescription(token);
            string anotherUsertoken = await new UsersControllerTests().LoginUser();
            var response = await GetResponseFromDeletePrescription(newPrescription.Id, anotherUsertoken);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }


        private void AddTokenHeaderToClient(string token)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }


        public async Task<HttpResponseMessage> GetResponseFromCreatePrescription(CreatePrescriptionRequestModel model, string token)
        {
            AddTokenHeaderToClient(token);
            var httpContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _client.PostAsync("/api/prescriptions", httpContent);
            return response;
        }

        public async Task<HttpResponseMessage> GetResponseFromUpdatePrescription(UpdatePrescriptionRequestModel model, string token)
        {
            AddTokenHeaderToClient(token);
            var httpContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _client.PutAsync("/api/prescriptions", httpContent);
            return response;
        }

        public async Task<HttpResponseMessage> GetResponseFromGetAllByCurrentUserPrescription(string token)
        {
            AddTokenHeaderToClient(token);
            var response = await _client.GetAsync("/api/prescriptions");
            return response;
        }

        public async Task<HttpResponseMessage> GetResponseFromGetByIdPrescription(Guid id, string token)
        {
            AddTokenHeaderToClient(token);
            var response = await _client.GetAsync($"/api/prescriptions/{id.ToString("N")}");
            return response;
        }

        public async Task<HttpResponseMessage> GetResponseFromDeletePrescription(Guid id, string token)
        {
            AddTokenHeaderToClient(token);
            var response = await _client.DeleteAsync($"/api/prescriptions/{id.ToString("N")}");
            return response;
        }



        public static Prescription GenerateRandomValidPrescription()
        {
            Guid newId = Guid.NewGuid();
            var prescription = new Prescription()
            {
                Id = newId,
                Dosage = newId.ToString("N") + "_dosage",
                Drug = newId.ToString("N") + "_drug",
                Notes = newId.ToString("N") + "_notes",
            };
            return prescription;
        }

        public static CreatePrescriptionRequestModel GenerateRandomValidCreatePrescriptionModel()
        {
            var prescription = GenerateRandomValidPrescription();
            var model = new CreatePrescriptionRequestModel()
            {
                Dosage = prescription.Dosage,
                Drug = prescription.Drug,
                Notes = prescription.Notes,
            };
            return model;
        }


    }
}
