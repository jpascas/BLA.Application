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
            var httpContent = new StringContent(JsonSerializer.Serialize(userModel), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await _client.PostAsync("/api/users", httpContent);
            var requestContent = await response.Content.ReadAsStringAsync();
            var createUserResult = JsonSerializer.Deserialize<ApplicationUserIResultModel>(requestContent);


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            createUserResult.Email.Should().Be(userModel.Email);

        }

        public static ApplicationUserInsertModel GenerateRandomValidUser()
        {
            string newUserName = Guid.NewGuid().ToString("N");
            var user = new ApplicationUserInsertModel()
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
