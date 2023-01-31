namespace PaymentGateway.Core.ViewModels
{
    public class CreatePaymentResponseDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public CardDetailsDto Card { get; set; }
        public Guid TransactionId { get; set; }
        public Guid? BankTransactionId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}