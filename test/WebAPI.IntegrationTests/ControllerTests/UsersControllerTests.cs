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

namespace WebAPI.IntegrationTests.ControllerTests
{
    public class UsersControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public UsersControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }


        [Test]
        public async Task Create_WithValidInput_ShouldReturnStatus200AndUser()
        {
            var userModel = GenerateRandomValidUser();
            var response = await GetResponseFromCreateUser(userModel); ;
            var requestContent = await response.Content.ReadAsStringAsync();
            var createUserResult = JsonSerializer.Deserialize<UserResultModel>(requestContent);


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            createUserResult.Email.Should().Be(userModel.Email);

        }

        [Test]
        public async Task Create_WithInvalidInput_ShouldReturnStatus400AndErrors()
        {
            var invalidUserModel = new CreateUserRequestModel();
            var response = await GetResponseFromCreateUser(invalidUserModel);
            var requestContent = await response.Content.ReadAsStringAsync();
            var errorsResult = JsonSerializer.Deserialize<ValidationErrorResult>(requestContent); // verify


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);            
        }

        [Test]
        public async Task CreateAndLogin_WithValidInput_ShouldReturnStatus200AndToken()
        {
            var userModel = GenerateRandomValidUser();            
            var response = await GetResponseFromCreateUser(userModel);
            var requestContent = await response.Content.ReadAsStringAsync();
            var createUserResult = JsonSerializer.Deserialize<UserResultModel>(requestContent);

            var loginRequest = new LoginUserRequestModel() { Email = createUserResult.Email, Password = userModel.Password };
            var responseLogin = await GetResponseFromLoginUser(loginRequest);
            var requestContentToken = await responseLogin.Content.ReadAsStringAsync();

            responseLogin.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            requestContentToken.Should().NotBeEmpty();
        }

        [Test]
        public async Task CreateAndLogin_WithValidButNotExistantUser_ShouldReturnStatus401()
        {
            var userModel = GenerateRandomValidUser();            
            var loginRequest = new LoginUserRequestModel() { Email = userModel.Email, Password = userModel.Password };
            var responseLogin = await GetResponseFromLoginUser(loginRequest);            

            responseLogin.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);            
        }

        [Test]
        public async Task CreateAndLogin_WithInValidInput_ShouldReturnStatus200AndToken()
        {
            var loginRequest = new LoginUserRequestModel();
            var responseLogin = await GetResponseFromLoginUser(loginRequest);

            responseLogin.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        public async Task<HttpResponseMessage> GetResponseFromCreateUser(CreateUserRequestModel model)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _client.PostAsync("/api/users", httpContent);
            return response;
        }

        public async Task<HttpResponseMessage> GetResponseFromLoginUser(LoginUserRequestModel model)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _client.PostAsync("/api/users/login", httpContent);
            return response;
        }

        public static CreateUserRequestModel GenerateRandomValidUser()
        {
            string newUserName = Guid.NewGuid().ToString("N");
            var user = new CreateUserRequestModel()
            {
                Email = newUserName + "@test.com",
                Password = newUserName.Substring(0, 8) + "aA8."
            };
            return user;
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
