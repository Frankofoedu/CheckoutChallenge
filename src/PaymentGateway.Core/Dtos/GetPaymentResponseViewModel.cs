using PaymentGateway.Shared;

namespace PaymentGateway.Core.ViewModels
{
    public class GetPaymentResponseViewModel
    {
        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid? BankTransactionId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public CardDetails Card { get; set; }
    }
}