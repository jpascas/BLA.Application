using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests
{
    public class BaseControllerTests : IDisposable
    {
        protected CustomWebApplicationFactory _factory;
        protected HttpClient _client;

        public BaseControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
