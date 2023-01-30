namespace PaymentGateway.Core.ViewModels
{
    public class CreatePaymentResponseViewModel
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public CardDetails Card { get; set; }
        public Guid TransactionId { get; set; }
        public Guid? BankTransactionId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}