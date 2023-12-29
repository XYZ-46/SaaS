using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using InterfaceProject.Middleware;
using Middleware.Database;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace XunitTest
{
    public class HomeControllerTests 
    {
        [Fact]
        public async Task ConfigureServices_RegistersDependenciesCorrectly()
        {
            //  Arrange

            //  Setting up the stuff required for Configuration.GetConnectionString("DefaultConnection")
            Mock<IConfigurationSection> configurationSectionStub = new Mock<IConfigurationSection>();
            configurationSectionStub.Setup(x => x["DefaultConnection"]).Returns("TestConnectionString");
            Mock<Microsoft.Extensions.Configuration.IConfiguration> configurationStub = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            configurationStub.Setup(x => x.GetSection("ConnectionStrings")).Returns(configurationSectionStub.Object);

        }
    }
}
