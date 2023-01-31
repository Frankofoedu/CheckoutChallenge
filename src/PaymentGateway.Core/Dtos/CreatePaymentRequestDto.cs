using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentGateway.Core.ViewModels
{
    public class CreatePaymentRequestDto
    {
        [Required(ErrorMessage = "Currency is invalid")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Amount  is invalid")]
        [Range(minimum: 0, maximum: double.MaxValue)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Description is invalid")]
        public string Description { get; set; }

        public CardDetailsDto Card { get; set; }

        [JsonIgnore]
        public Guid MerchantId { get; set; }
    }
}