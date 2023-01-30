using PaymentGateway.Shared;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Core.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid? BankTransactionId { get; set; }

        [DataType("decimal(18,5)")]
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public TransactionStatus TransactionStatus { get; set; }
        public string Description { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string CardHolderName { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string? Reason { get; set; }
        public DateTime DateCreated { get; private set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; }
    }
}