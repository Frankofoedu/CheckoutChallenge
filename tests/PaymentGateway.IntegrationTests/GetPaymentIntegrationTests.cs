using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PaymentGateway.Core.ViewModels;
using System.Text;
using Xunit;

namespace PaymentGateway.IntegrationTests
{
    [Collection("SetupDatabase")]
    public class GetPaymentIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GetPaymentIntegrationTests(WebApplicationFactory<Program> factory)
        {
            var environment = "UnitTests";

            _factory = factory.WithWebHostBuilder(x => x
            .UseEnvironment(environment));
        }

        [Fact]
        public async Task Get_Transaction_Returns_OK_Data_When_Transaction_exists()
        {
            // Arrange
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("MerchantId", "17ec7b36-0863-400c-896a-841afd8a94cc");

            client.DefaultRequestHeaders.Add("x-idem-key", "123");

            var body = new CreatePaymentRequestDto
            {
                Amount = 2000,
                Currency = "dollar",
                Description = "For shoes",
                Card = new CardDetailsDto
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

            HttpResponseMessage createResponse = await client.SendAsync(request);
            string responseContent = await createResponse.Content.ReadAsStringAsync();
            //serialize string to class
            var createPaymentResponse = JsonConvert.DeserializeObject<CreatePaymentResponseDto>(responseContent);

            //act
            var response = await client.GetAsync("/Payments/" + createPaymentResponse.TransactionId);
            var json = await response.Content.ReadAsStringAsync();
            var getPaymentResponse = JsonConvert.DeserializeObject<GetPaymentResponseDto>(json);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(body.Amount, getPaymentResponse.Amount);
            Assert.Equal(body.Currency, getPaymentResponse.Currency);
            Assert.Equal(body.Description, getPaymentResponse.Description);
            Assert.Equal(body.Card.OwnerName, getPaymentResponse.Card.OwnerName);
        }

        [Fact]
        public async Task Get_Transaction_Returns_NotFound_When_Transaction_Does_Not_Exists()
        {
            // Arrange
            var client = _factory.CreateClient();

            client.DefaultRequestHeaders.Add("MerchantId", "17ec7b36-0863-400c-896a-841afd8a94cc");

            client.DefaultRequestHeaders.Add("x-idem-key", "123");

            //act
            var response = await client.GetAsync("/Payments/" + Guid.NewGuid());
            var json = await response.Content.ReadAsStringAsync();

            //assert
            Assert.True(!response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}