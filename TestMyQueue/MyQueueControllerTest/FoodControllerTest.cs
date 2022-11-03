using System;
using Xunit;
using MyQueue.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using MyQueue;
using System.Threading.Tasks;
using System.Net;

namespace TestMyQueue
{
    public class FoodControllerTest
    {
        private readonly HttpClient _client;
        public FoodControllerTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task GetAllFoods(string method)
        {
            //Arrange
            var reques = new HttpRequestMessage(new HttpMethod(method), $"/api/Food/Food");

            //Act 
            var response = await _client.SendAsync(reques);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
