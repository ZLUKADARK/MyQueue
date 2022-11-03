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
    public class OrdersControllerTest
    {
        private readonly HttpClient _client;
        public OrdersControllerTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET", 1)]
        public async Task GetAllOrders(string method, int? id = null)
        {
            //Arrange
            var reques = new HttpRequestMessage(new HttpMethod(method), $"/api/Orders/{id}");

            //Act 
            var response = await _client.SendAsync(reques);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
