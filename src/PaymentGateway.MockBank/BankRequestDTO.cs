namespace PaymentGateway.MockBank
{
    public class BankRequestDTO
    {
        public Guid PaymentId { get; set; }
        public decimal TransactionAmount { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string CardHolderName { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string Currency { get; set; }
    }
}