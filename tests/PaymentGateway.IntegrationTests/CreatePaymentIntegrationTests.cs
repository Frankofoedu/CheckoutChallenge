using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PaymentGateway.Core.ViewModels;
using System.Text;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    public class CreatePaymentIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CreatePaymentIntegrationTests(WebApplicationFactory<Program> factory)
        {
            var environment = "UnitTests";

            _factory = factory.WithWebHostBuilder(x => x
            .UseEnvironment(environment));
        }

        [Fact]
        public async Task Create_Transaction_Returns_OK_Data_When_Request_Is_Valid()
        {
            // Arrange
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("MerchantId", "17ec7b36-0863-400c-896a-841afd8a94cc");

            client.DefaultRequestHeaders.Add("x-idem-key", "123");

            var body = new CreatePaymentRequestDto
            {
                Amount = 2000,
                Currency = "$",
                Description = "For shoes",
                Card = new CardDetails
                {
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 2022,
                    Number = "2022202220222022",
                    OwnerName = "Frank"
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Payments")
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();
            //serialize string to class
            var jsondata = JsonConvert.DeserializeObject<CreatePaymentResponseViewModel>(responseContent);

            //assert
            Assert.Equal(true, response.IsSuccessStatusCode);

            Assert.True(jsondata is not null);
        }

        [Fact]
        public async Task Create_Transaction_Returns_BadRequest_When_Amount_Is_InValid()
        {
            // Arrange
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("MerchantId", "17ec7b36-0863-400c-896a-841afd8a94cc");

            client.DefaultRequestHeaders.Add("x-idem-key", "123");

            var body = new CreatePaymentRequestDto
            {
                Amount = -1,
                Currency = "$",
                Description = "For shoes",
                Card = new CardDetails
                {
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 2022,
                    Number = "2022202220222022",
                    OwnerName = "Frank"
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Payments")
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            Assert.True(!response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_Transaction_Returns_Unauthorized_When_MerchantId_Is_InValid()
        {
            // Arrange
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("MerchantId", "23ec7b36-0863-400c-896a-841afd8a94cc");

            client.DefaultRequestHeaders.Add("x-idem-key", "123");

            var body = new CreatePaymentRequestDto
            {
                Amount = 1,
                Currency = "$",
                Description = "For shoes",
                Card = new CardDetails
                {
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 2022,
                    Number = "2022202220222022",
                    OwnerName = "Frank"
                },
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/Payments")
            {
                Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            Assert.True(!response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}