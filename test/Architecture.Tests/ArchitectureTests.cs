using NetArchTest.Rules;

namespace Architecture.Tests
{
    public class Tests
    {
        private const string DomainNamespace = "Domain";
        private const string ApplicationNamespace = "Application";
        private const string InfrastructureNamespace = "Infrastructure";
        private const string PresentationNamespace = "Presentation";
        private const string WebApiNamespace = "WebApi";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Domain_Should_Not_DependOnOtherProjects()
        {
            var assembly = typeof(Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            ApplicationNamespace,
            InfrastructureNamespace,
            PresentationNamespace,
            WebApiNamespace
            };

            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            Assert.True(testResult.IsSuccessful);
        }

        [Test]
        public void Application_Should_Not_DependOnOtherProjects()
        {            
            var assembly = typeof(Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            InfrastructureNamespace,
            PresentationNamespace,
            WebApiNamespace
            };
            
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            Assert.True(testResult.IsSuccessful);
        }

        [Test]
        public void Infrastructure_Should_Not_DependOnOtherProjectss()
        {            
            var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            PresentationNamespace,
            WebApiNamespace
            };
            
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            Assert.True(testResult.IsSuccessful);
        }

        [Test]
        public void Presentation_Should_Not_DependOnOtherProjects()
        {            
            var assembly = typeof(Infrastructure.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
            InfrastructureNamespace,
            WebApiNamespace
            };
            
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            Assert.True(testResult.IsSuccessful);
        }
    }
}