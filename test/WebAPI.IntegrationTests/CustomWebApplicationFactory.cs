using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<BLA.Application.Program>
    {
        public CustomWebApplicationFactory() { 
            // create mocks if needed
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);            
            builder.ConfigureTestServices(services => { 
                // inject the mocks
            });   

        }
    }
}
