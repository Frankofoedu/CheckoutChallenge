using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Core.ViewModels
{
    //todo: add more validations for this
    public class CardDetails
    {
        [StringLength(maximumLength: 16, MinimumLength = 16)]
        public string Number { get; set; }

        public string Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string OwnerName { get; set; }
    }
}